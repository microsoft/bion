namespace BSOA.IO
{
    public class TreeSerializationSettings
    {
        public byte[] Buffer;
        public bool LeaveStreamOpen;

        public TreeSerializationSettings(bool leaveStreamOpen = false, byte[] buffer = null)
        {
            LeaveStreamOpen = leaveStreamOpen;
            Buffer = buffer;
        }
    }
}
