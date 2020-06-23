using BSOA.Diagnostics;
using BSOA.Test.Model.V1;

using System;
using System.Threading;

using Xunit;

namespace BSOA.Test.Diagnostics
{
    public class MeasureTests
    {
        [Fact]
        public void Measure_Basics()
        {
            TimeSpan average;

            // Single iteration
            average = Measure.Time("Thread.Sleep", () => Thread.Sleep(10));
            Assert.True(average.TotalMilliseconds > 8);
            Assert.True(average.TotalMilliseconds < 12);

            // Multiple iterations
            average = Measure.Time("Thread.Sleep", () => Thread.Sleep(10), 3);
            Assert.True(average.TotalMilliseconds > 8);
            Assert.True(average.TotalMilliseconds < 12);

            // LoadPerformance
            string filePath = "Measure.bsoa";
            Community db = new Community();
            db.People.Add(new Person() { Name = "Scott", Age = 39 });
            db.WriteBsoa(filePath);

            Community roundTripped = Measure.LoadPerformance<Community>(Community.ReadBsoa, filePath, "Loading Community from BSOA");
            CollectionReadVerifier.VerifySame(db.People, roundTripped.People);

            // Can't verify Console output, but exercising to ensure no exceptions and show usage
        }
    }
}
