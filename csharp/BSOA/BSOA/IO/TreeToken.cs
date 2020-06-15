// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.IO
{
    /// <summary>
    ///  TreeToken identifies the type of the current token an ITreeReader
    ///  has read. 
    ///  
    ///  It mirrors Newtonsoft JsonToken names and values for easy
    ///  conversion to and from JsonToken.
    ///  
    ///  Values must be under 16, allowing high bits to be used for other data.
    /// </summary>
    public enum TreeToken : byte
    {
        None = 0,

        StartObject = 1,
        StartArray = 2,
        PropertyName = 4,
        Integer = 7,
        Float = 8,
        String = 9,
        Boolean = 10,
        Null = 11,
        EndObject = 13,
        EndArray = 14,

        BlockArray = 15
    }
}
