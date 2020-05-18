using System;

namespace BSOA.Converter
{
    // Provide converters for small number types up to int.
    // These enable easy use of small numbers as array indices when the array size is known to be limited.

    public class ByteConverter : IConverter<byte, int>, IConverter<int, byte>
    {
        public static ByteConverter Instance = new ByteConverter();

        private ByteConverter()
        { }

        public int Convert(byte value)
        {
            return value;
        }

        public byte Convert(int value)
        {
            if (value < byte.MinValue || value > byte.MaxValue) { throw new ArgumentOutOfRangeException(nameof(value)); }
            return (byte)value;
        }
    }

    public class SByteConverter : IConverter<sbyte, int>, IConverter<int, sbyte>
    {
        public static SByteConverter Instance = new SByteConverter();

        private SByteConverter()
        { }

        public int Convert(sbyte value)
        {
            return value;
        }

        public sbyte Convert(int value)
        {
            if (value < sbyte.MinValue || value > sbyte.MaxValue) { throw new ArgumentOutOfRangeException(nameof(value)); }
            return (sbyte)value;
        }
    }

    public class ShortConverter : IConverter<short, int>, IConverter<int, short>
    {
        public static ShortConverter Instance = new ShortConverter();

        private ShortConverter()
        { }

        public int Convert(short value)
        {
            return value;
        }

        public short Convert(int value)
        {
            if (value < short.MinValue || value > short.MaxValue) { throw new ArgumentOutOfRangeException(nameof(value)); }
            return (short)value;
        }
    }

    public class UShortConverter : IConverter<ushort, int>, IConverter<int, ushort>
    {
        public static UShortConverter Instance = new UShortConverter();

        private UShortConverter()
        { }

        public int Convert(ushort value)
        {
            return value;
        }

        public ushort Convert(int value)
        {
            if (value < ushort.MinValue || value > ushort.MaxValue) { throw new ArgumentOutOfRangeException(nameof(value)); }
            return (ushort)value;
        }
    }
}
