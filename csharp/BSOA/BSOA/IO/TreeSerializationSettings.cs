namespace BSOA.IO
{
    public class TreeSerializationSettings
    {
        public byte[] Buffer;
        public bool LeaveStreamOpen { get; set; }
        public bool Compact { get; set; }

        public static TreeSerializationSettings DefaultSettings = new TreeSerializationSettings();

        public TreeSerializationSettings(bool leaveStreamOpen = false, byte[] buffer = null)
        {
            LeaveStreamOpen = leaveStreamOpen;
            Buffer = buffer;
            Compact = true;
        }
    }
}
