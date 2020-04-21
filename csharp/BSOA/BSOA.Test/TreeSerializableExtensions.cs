using BSOA.IO;
using BSOA.Test.Components;
using System.IO;

namespace BSOA.Test
{
    // Helpers for easy Stream and File I/O; not possible in base library because not all formats implemented there
    public static class TreeSerializableExtensions
    {
        public static void Save(this ITreeSerializable item, string filePath, TreeFormat format, TreeSerializationSettings settings = null)
        {
            Save(item, File.Create(filePath), format, settings);
        }

        public static void Save(this ITreeSerializable item, Stream stream, TreeFormat format, TreeSerializationSettings settings = null)
        {
            using (ITreeWriter writer = TreeSerializable.Writer(format, stream, settings))
            {
                item.Write(writer);
            }
        }

        public static void Load(this ITreeSerializable item, string filePath, TreeFormat format, TreeSerializationSettings settings = null)
        {
            Load(item, File.OpenRead(filePath), format, settings);
        }

        public static void Load(this ITreeSerializable item, Stream stream, TreeFormat format, TreeSerializationSettings settings = null)
        {
            using (ITreeReader reader = TreeSerializable.Reader(format, stream, settings))
            {
                item.Read(reader);
            }
        }
    }
}
