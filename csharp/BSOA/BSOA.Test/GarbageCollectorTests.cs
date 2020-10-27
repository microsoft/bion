using BSOA.IO;
using BSOA.Test.Components;
using BSOA.Test.Model.Log;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;

using Xunit;

namespace BSOA.Test
{
    public class GarbageCollectorTests
    {
        private const int RuleCount = 10;
        private const int ResultCount = 100;

        [Fact]
        public void GarbageCollector_NoDeletion()
        {
            Run run = Generator.Build(RuleCount, ResultCount);
            List<int> startLines = run.Results.Select((r) => r.StartLine).ToList();

            // Verify all created objects are found in Tables
            RunDatabase db = (RunDatabase)run.DB;
            Assert.Equal(RuleCount, db.Tables["Rule"].Count);
            Assert.Equal(ResultCount, db.Tables["Result"].Count);

            // Round-trip and verify collect doesn't remove anything
            RoundTrip(run);
            Assert.Equal(RuleCount, db.Tables["Rule"].Count);
            Assert.Equal(ResultCount, db.Tables["Result"].Count);

            // Remove rules (leaving some at the beginning and end)
            int remainingResultCount = 90;
            run.Results = run.Results.Skip(5).Take(remainingResultCount).ToList();
            startLines = startLines.Skip(5).Take(remainingResultCount).ToList();

            // Collection is updated, but Table still has previously created items
            Assert.Equal(remainingResultCount, run.Results.Count);
            Assert.Equal(ResultCount, db.Tables["Result"].Count);

            // Round-trip and verify unreferenced items collected
            RoundTrip(run);
            db = TreeSerializer.RoundTrip<RunDatabase>(db, TreeFormat.Binary);
            Assert.Equal(RuleCount, db.Tables["Rule"].Count);
            Assert.Equal(remainingResultCount, db.Tables["Result"].Count);

            // Verify that remapping worked properly (Results in list still point to the right underlying values)
            Assert.Equal(startLines, run.Results.Select((r) => r.StartLine));
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

        // TODO:
        //  - Verify RemoveFromEnd happening (ensure new instances don't initialize with values from old ones)
        //  - Need Ref AND RefList from one table to another. (Result.Rule?)
        //    - Instances reachable only indirectly are kept. (Rule on Result, not in root collection)
        //    - Instances cut off from root are removed.
        //    - Instances with one path to root cut but another left are still kept.
        //  - Cycle in references, self-references (verify no hang)
        //  - Remap optimization? (Avoid swap and remap when
        //  - Performance (GC with nothing to do, and GC with work)
        //  - Hold OM instances; verify removed from Table and put into a new DB

        // Do I want a separate object model for GC tests, or no?
        // When should Trim be happening?
        // What is performance impact here?
    }
}