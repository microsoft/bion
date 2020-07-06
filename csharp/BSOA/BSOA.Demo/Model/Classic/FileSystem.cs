using BSOA.Json;

using System.Collections.Generic;

namespace BSOA.Demo.Model.Classic
{
    public class FileSystem
    {
        public IList<Folder> Folders { get; set; }
        public IList<File> Files { get; set; }

        public FileSystem()
        {
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public void Save(string filePath)
        {
            AsJson.Save(filePath, this);
        }

        public static FileSystem Load(string filePath)
        {
            return AsJson.Load<FileSystem>(filePath);
        }
    }
}
