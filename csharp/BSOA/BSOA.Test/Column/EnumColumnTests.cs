using BSOA.Column;
using System;
using System.IO;
using Xunit;

namespace BSOA.Test
{
    public class EnumColumnTests
    {
        [Fact]
        public void EnumColumn_Basics()
        {
            // Normal Enum
            Column.Basics<DayOfWeek>(
                () => new EnumColumn<DayOfWeek, int>(DayOfWeek.Sunday, (v) => (int)v, (v) => (DayOfWeek)v),
                DayOfWeek.Sunday,
                DayOfWeek.Wednesday,
                (i) => ((DayOfWeek)(i % 7))
            );

            // Flags enum
            FileAttributes[] samples = new[]
            {
                default(FileAttributes),
                FileAttributes.Normal,
                FileAttributes.ReadOnly | FileAttributes.Hidden,
                FileAttributes.Directory,
                FileAttributes.ReadOnly | FileAttributes.Directory,
                FileAttributes.Compressed | FileAttributes.Hidden,
                FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.Compressed
            };

            Column.Basics<FileAttributes>(
                () => new EnumColumn<FileAttributes, int>(default(FileAttributes), (v) => (int)v, (v) => (FileAttributes)v),
                default(FileAttributes),
                FileAttributes.ReadOnly | FileAttributes.System,
                (i) => samples[i % samples.Length]
            );
        }
    }
}
