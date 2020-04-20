using BSOA.Test.Components;
using System;
using Xunit;

namespace BSOA.Test.IO
{
    public class TreeReaderWriterTests
    {
        [Fact]
        public void TreeReaderWriter_ExtensionMethods_Tests()
        {
            // DateTime and Guid serialization covered in TreeSerializable.Basics

            Random r = new Random();
            Sample sample = new Sample(r);
            Sample sample2 = new Sample(r);

            // List and Dictionary serialization built-ins
            CollectionContainer<Sample> samples = new CollectionContainer<Sample>();
            samples.Add(sample);
            samples.Add(sample2);

            samples.AssertEqual(TreeSerializable.RoundTripJson(samples));
            samples.AssertEqual(TreeSerializable.RoundTripBinary(samples));
        }
    }
}
