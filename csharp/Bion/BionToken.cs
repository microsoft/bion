namespace Bion
{
    public enum BionToken : byte
    {
        StartObject = 0xFF,
        StartArray = 0xFE,
        EndObject = 0xFD,
        EndArray = 0xFC,

        String = 0xFA,              // Encoding must equal longest (5b) String

        PropertyName = 0xF6,        // Encoding must equal longest (5b) PropertyName

        Null = 0xF2,
        True = 0xF1,
        False = 0xF0,

        Float = 0xE0,

        Integer = 0xB0,

        None = 0x00,                // End of File or Read not called yet.
    }

    internal enum BionMarker : byte
    {
        StartObject = 0xFF,
        StartArray = 0xFE,
        EndObject = 0xFD,
        EndArray = 0xFC,
        EndValue = 0xFB,

        StringLength5b = 0xFA,
        StringLength2b = 0xF9,
        StringLength1b = 0xF8,
        StringCompressedTerminated = 0xF7,
        String = 0xF7,

        PropertyNameLength5b = 0xF6,
        PropertyNameLength2b = 0xF5,
        PropertyNameLength1b = 0xF4,
        PropertyNameCompressedTerminated = 0xF3,
        PropertyName = 0xF3,

        Null = 0xF2,
        True = 0xF1,
        False = 0xF0,

        InlineInteger = 0xE0,
        Integer = 0xD0,
        NegativeInteger = 0xC0,
        Float = 0xB0
    }
}
