using BSOA.IO;
using BSOA.Test.Components;
using BSOA.Test.Model.Log;

using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            Result result = new Result(run);
            run.Results.Add(result);

            for (int i = 0; i < 5; ++i)
            {
                rules.Add(new Rule(run) { Id = i.ToString() });
            }

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

            // TODO: GC should update object instance automatically
            //six = run.Database.Rule.Where((r) => r.Id == "6").First();

            // Verify object model instance is pointing to correct data (Collect will have swapped it, so OM object index must be changed)
            Assert.Equal("6", six.Id);

            // Make a Rule reachable from a different table only
            result.Rule = six;
            rules.RemoveAt(0);

            // Verify removed Rule collected and new rule still kept (reachable via Result)
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4, 6", TableRules(run));

            // TODO: GC should fix object instance index
            //six = run.Database.Rule.Where((r) => r.Id == "6").First();

            // Verify Result and OM instance are still pointing to the right data (index of '6' will have been updated again)
            Assert.Equal("6", result.Rule.Id);
            Assert.Equal("6", six.Id);

            // Make Rule unreachable from Result; verify now unreachable Rule removed
            result.Rule = null;
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4", TableRules(run));

            // Verify OM object moved to temporary database with data intact
            // TODO: GC not copying unreachable but still used object references yet
            Assert.Equal("6", six.Id);

            // Make a Rule self-referential; verify Collect doesn't hang
            run.Rules[0].RelatedRules = new List<Rule>() { run.Rules[0] };
            run.DB.Collect();
            Assert.Equal("2, 3, 4", RunRules(run));
            Assert.Equal("2, 3, 4", TableRules(run));

            //// Remove self-referencing row; verify removed
            //run.Rules.RemoveAt(0);
            //run.DB.Collect();
            //Assert.Equal("3, 4", RunRules(run));
            //Assert.Equal("3, 4", TableRules(run));

        }

        private void RoundTrip(Run run)
        {
            // RoundTrip a single Database instance so that the root object from it is still valid (same Table instance, same index).

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(stream, new TreeSerializationSettings(leaveStreamOpen: true)))
                {
                    run.DB.Write(writer);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryTreeReader reader = new BinaryTreeReader(stream))
                {
                    run.DB.Read(reader);
                }
            }
        }
    }
}