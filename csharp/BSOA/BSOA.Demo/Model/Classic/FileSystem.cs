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
    }
}
