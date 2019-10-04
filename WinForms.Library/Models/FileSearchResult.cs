using System;
using System.IO;

namespace WinForms.Library.Models
{
    public class FileSearchResult
    {
        public string FullPath { get; set; }
        public string BaseName { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public DateTime LastModified { get; set; }
        public long Length { get; set; }

        public override string ToString()
        {
            return $"{BaseName} - {Length} - {LastModified}";
        }

        internal static FileSearchResult FromFileInfo(FileInfo fi, string searchPath)
        {
            return new FileSearchResult()
            {
                FullPath = fi.FullName,
                BaseName = fi.FullName.Substring(searchPath.Length + 1),
                Name = Path.GetFileName(fi.FullName),
                Extension = Path.GetExtension(fi.FullName),
                LastModified = fi.LastWriteTimeUtc,
                Length = fi.Length
            };
        }
    }
}
