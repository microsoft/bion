// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;

namespace BSOA.IO
{
    public class TreeSerializationSettings
    {
        public byte[] Buffer;
        public bool LeaveStreamOpen { get; set; }
        public bool Verbose { get; set; }
        public bool Strict { get; set; }

        public static TreeSerializationSettings DefaultSettings = new TreeSerializationSettings();

        public TreeSerializationSettings(bool leaveStreamOpen = false, byte[] buffer = null)
        {
            LeaveStreamOpen = leaveStreamOpen;
            Buffer = buffer;
            Verbose = false;
            Strict = false;
        }
    }
}
