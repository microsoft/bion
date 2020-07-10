using System.Text;

namespace BSOA.Demo.Model.Classic
{
    public class Folder
    {
        public int ParentIndex { get; set; }
        public string Name { get; set; }

        public string FullPath(FileSystem fileSystem)
        {
            StringBuilder path = new StringBuilder();
            FullPath(fileSystem, path);
            return path.ToString();
        }

        private void FullPath(FileSystem fileSystem, StringBuilder path)
        {
            if (ParentIndex != -1)
            {
                fileSystem.Folders[ParentIndex].FullPath(fileSystem, path);
                path.Append("\\");
            }

            path.Append(Name);
        }
    }
}
