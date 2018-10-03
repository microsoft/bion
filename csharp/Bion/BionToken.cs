using System;
using System.Collections.Generic;
using System.Text;

namespace Bion
{
    public enum BionToken : byte
    {
        StartObject = 0xFF,
        StartArray = 0xFE,
        EndObject = 0xFD,
        EndArray = 0xFC,

        String = 0xFB,

        PropertyName = 0xF7,

        Null = 0xF2,
        True = 0xF1,
        False = 0xF0,

        Float = 0xE0,

        Integer = 0xB0,
    }

    internal enum BionMarker : byte
    {
        StartObject = 0xFF,
        StartArray = 0xFE,
        EndObject = 0xFD,
        EndArray = 0xFC,

        StringLength5b = 0xFB,
        StringLength2b = 0xFA,
        StringLength1b = 0xF9,
        StringLookup1b = 0xF8,
        String = 0xF8,

        PropertyNameLength5b = 0xF7,
        PropertyNameLength2b = 0xF6,
        PropertyNameLength1b = 0xF5,
        PropertyNameLookup2b = 0xF4,
        PropertyName = 0xF4,

        Null = 0xF2,
        True = 0xF1,
        False = 0xF0,

        InlineInteger = 0xE0,
        Integer = 0xD0,
        NegativeInteger = 0xC0,
        Float = 0xB0
    }
}
