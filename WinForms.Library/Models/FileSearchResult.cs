using System;

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
    }
}
