using System.IO;

namespace BSOA.Demo.Model.BSOA
{
    public partial class File
    {
        public string Description(FileSystem db)
        {
            return $"{db.Folders[ParentFolderIndex].FullPath(db)}\\{Name} | {Length:n0} | {LastModifiedUtc:u}";
        }
    }
}
