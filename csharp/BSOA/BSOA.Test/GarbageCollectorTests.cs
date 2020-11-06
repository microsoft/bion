using BSOA.Column;
using BSOA.GC;
using BSOA.Model;
using BSOA.Test.Model.Log;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xunit;

namespace BSOA.Test
{
    public class GarbageCollectorTests
    {
        // Return Rule IDs in the Run.Rules collection (in found order)
        // Used to verify Collect() hasn't altered the Run.Rules collection, even if underlying rows were swapped around.
        private string RunRules(Run run)
        {
            return string.Join(", ", run.Rules.Select((r) => r.Id));
        }

        // Return Rule IDs present in the underlying BSOA Rules Table (all reachable rules, in *sorted* order)
        // Used to verify Collect() has removed and kept all reachable rows.
        private string TableRules(Run run)
        {
            RunDatabase db = (RunDatabase)run.DB;
            IEnumerable<Rule> tableRules = db.Rule;
            return string.Join(", ", tableRules.Select((r) => r.Id).OrderBy((id) => id));
        }

        [Fact]
        public void GarbageCollector_Basics()
        {
            Run run = new Run() { Rules = new List<Rule>(), Results = new List<Result>() };
            IList<Rule> rules = run.Rules;

            for (int i = 0; i < 5; ++i)
            {
                run.Results.Add(new Result(run) { StartLine = i });
            }

            for (int i = 0; i < 5; ++i)
            {
                rules.Add(new Rule(run) { Id = i.ToString() });
            }

            Rule two = run.Rules[2];
            Result result = run.Results[0];

            // Verify all rules present in Run and Table
            Assert.Equal("0, 1, 2, 3, 4", RunRules(run));
            Assert.Equal("0, 1, 2, 3, 4", TableRules(run));

            // Verify nothing removed by Garbage Collection (all Rules directly reachable from root)            
            run.DB.Collect();
            Assert.Equal("0, 1, 2, 3, 4", RunRules(run));
            Assert.Equal("0, 1, 2, 3, 4", TableRules(run));

            // Add a new Rule without adding to a collection
            new Rule(run) { Id = "5" };

            // Verify new Rule in Table only
            Assert.Equal("0, 1, 2, 3, 4", RunRules(run));
            Assert.Equal("0, 1, 2, 3, 4, 5", TableRules(run));

            // Verify unreachable instance removed by Garbage Collection
            run.DB.Collect();
            Assert.Equal("0, 1, 2, 3, 4", RunRules(run));
            Assert.Equal("0, 1, 2, 3, 4", TableRules(run));

            // Add a new Rule; confirm ID from old Rule wasn't "left behind" on collected instance
            Rule six = new Rule(run);
            Assert.Null(six.Id);

            // Make Rule reachable only from another Rule
            six.Id = "6";
            rules[0].RelatedRules = new List<Rule>() { six };

            // Verify indirectly reachable instance kept
            run.DB.Collect();
            Assert.Equal("0, 1, 2, 3, 4", RunRules(run));
            Assert.Equal("0, 1, 2, 3, 4, 6", TableRules(run));

            // Add another link to the new Rule
            rules[1].RelatedRules = new List<Rule>() { six };

            // Remove the original rule (so the new one is reachable via '1' but not '0' anymore)
            rules.RemoveAt(0);

            // Verify removed Rule collected and new rule still kept (reachable via '1')
            run.DB.Collect();
            Assert.Equal("1, 2, 3, 4", RunRules(run));
            Assert.Equal("1, 2, 3, 4, 6", TableRules(run));

            // Verify object model instance is pointing to correct data (Collect will have swapped it, so OM object index must be changed)
            Assert.Equal("6", six.Id);

            // Make a Rule reachable from a different table only
            result.Rule = six;
            rules.RemoveAt(0);

            // Verify removed Rule collected and new rule still kept (reachable via Result)
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4, 6", TableRules(run));

            // Verify Result and OM instance are still pointing to the right data (index of '6' will have been updated again)
            Assert.Equal("6", result.Rule.Id);
            Assert.Equal("6", six.Id);

            // Make Rule unreachable from Result; verify now unreachable Rule removed
            result.Rule = null;
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4", TableRules(run));

            // Verify OM object moved to temporary database with data intact
            Assert.Equal("6", six.Id);

            // Make a Rule self-referential; verify Collect doesn't hang (walking reachable graph)
            run.Rules[0].RelatedRules = new List<Rule>() { run.Rules[0] };
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4", TableRules(run));

            // Remove self-referencing row; verify removed and Collect doesn't hang (copying unreachable graph)
            run.Rules.RemoveAt(0);
            run.DB.Collect();
            Assert.Equal("3, 4", RunRules(run));
            Assert.Equal("3, 4", TableRules(run));

            // Verify an old object model instance (swapped, then later moved to temp) is fully updated
            Assert.Equal("2", two.Id);

            // Make a rule needed by both reachable (Run.Rules) and unreachable objects (Result).
            result.Rule = run.Rules[0];
            run.Results.RemoveAt(0);
            run.DB.Collect();

            // Verify one copy of Rule kept in main DB
            Assert.Equal("3, 4", RunRules(run));
            Assert.Equal("3, 4", TableRules(run));
            Assert.Equal("3", run.Rules[0].Id);

            // Verify Rule copied; one copy left in main DB and one referenced by Result now in temp
            result.Rule.Guid = "New";
            Assert.Equal("New", result.Rule.Guid);
            Assert.Null(run.Rules[0].Guid);

            // Verify moved Result still has correct data
            Assert.Equal(0, result.StartLine);
            Assert.Equal("3", result.Rule.Id);
        }

        [Fact]
        public void GarbageCollector_RandomVariations()
        {
            // Test keeping everything
            Test(new bool[] { true, true, true, true, true });

            // Test keeping nothing
            Test(new bool[] { false, false, false, false, false });

            // Test keeping a single row only
            for (int keepIndex = 0; keepIndex < 5; ++keepIndex)
            {
                Test(Enumerable.Range(0, 5).Select((i) => (i == keepIndex)).ToArray());
            }

            // Test removing a single row only
            for (int removeIndex = 0; removeIndex < 5; ++removeIndex)
            {
                Test(Enumerable.Range(0, 5).Select((i) => (i != removeIndex)).ToArray());
            }

            // Try repeatable randomized scenarios; tricky to get Collect
            // to swap a given row only once, correctly count removed rows, and
            // track every remapping.
            Random r = new Random(5);
            for (int i = 0; i < 100; ++i)
            {
                Test(RandomRowsToKeep(r, 10));
            }
        }

        private bool[] RandomRowsToKeep(Random r, int count)
        {
            bool[] rowsToKeep = new bool[count];

            // Randomly choose rows to remove
            int leftToRemove = r.Next(count);
            for (int i = 0; i < count; ++i)
            {
                bool keep = r.NextDouble() < (double)(leftToRemove / (count - i));
                rowsToKeep[i] = keep;
                if (!keep) { leftToRemove--; }
            }

            return rowsToKeep;
        }

        private void Test(bool[] rowsToKeep)
        {
            int count = rowsToKeep.Length;

            // Build an identity column
            NumberColumn<int> column = new NumberColumn<int>(-1);
            for (int i = 0; i < rowsToKeep.Length; ++i)
            {
                column[i] = i;
            }

            // Build a RowUpdater and fake mapping to temp to check those results
            TableStub table = new TableStub();
            RowUpdater updater = new RowUpdater(table, table);
            int[] rowsToTemp = Enumerable.Range(10, count).ToArray();

            // Request Garbage Collection
            GarbageCollector.Collect<int>(column, null, rowsToKeep, updater, rowsToTemp);

            // Verify correct values were kept
            StringBuilder expected = new StringBuilder();
            int expectedCount = 0;

            for (int i = 0; i < count; ++i)
            {
                if (rowsToKeep[i])
                {
                    expectedCount++;

                    if (expected.Length > 0) { expected.Append(", "); }
                    expected.Append(i);
                }
            }

            Assert.Equal(expectedCount, column.Count);
            Assert.Equal(expected.ToString(), String.Join(", ", column.OrderBy((i) => i)));

            // Verify rows removed are reported in the correct indices in temp or swapped in original column
            RowStub stub = new RowStub(table, 0);

            for (int i = 0; i < count; ++i)
            {
                stub.Index = i;
                updater.Update(stub);

                if (!rowsToKeep[i])
                {
                    Assert.Equal(10 + i, stub.Index);
                }
                else
                {
                    Assert.Equal(i, column[stub.Index]);
                }
            }
        }

        private class TableStub : Table<RowStub>
        {
            public TableStub() : base(null)
            { }

            public override RowStub Get(int index)
            {
                return (index == -1 ? null : new RowStub(this, index));
            }

            public override void GetOrBuildColumns()
            { }
        }

        private class RowStub : IRow<RowStub>
        {
            public ITable Table { get; set; }
            public int Index { get; set; }

            public RowStub(ITable table, int index)
            {
                this.Table = table;
                this.Index = index;
            }

            public void Remap(ITable table, int index)
            {
                Table = table;
                Index = index;
            }

            public void CopyFrom(RowStub other)
            { }
        }
    }
}