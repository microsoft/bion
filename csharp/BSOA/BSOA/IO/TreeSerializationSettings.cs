using System.IO;

namespace BSOA.IO
{
    public class TreeSerializationSettings
    {
        public byte[] Buffer;
        public bool LeaveStreamOpen { get; set; }
        public bool Verbose { get; set; }
        public bool Strict { get; set; }
        public TextWriter Diagnostics { get; set; }

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
