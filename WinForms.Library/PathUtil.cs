using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WinForms.Library
{
    public static class PathUtil
    {
        /// <summary>
        /// Finds the common base path of a list of files
        /// </summary>
        /// <param name="samePathReturnsParent">if true, then the parent folder is returned if all the files in the list have same folder.
        /// Use this when you need to distinguish between folders and files in a drag-drop operation, for example.</param>
        public static string GetCommonPath(IEnumerable<string> fileNames, char separator = '\\', bool samePathReturnsParent = false)
        {
            // convert file list to array of folder name arrays
            var explodedFiles = fileNames.Select(s => s.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries).ToArray());

            int depth = 0;

            bool AllFilesHaveSameFolderAtDepth(IEnumerable<string[]> files, int atDepth)
            {
                try
                {
                    var groups = files.GroupBy(item => item[atDepth]);
                    return groups.Count() == 1;
                }
                catch
                {
                    return false;
                }
            }

            while (AllFilesHaveSameFolderAtDepth(explodedFiles, depth))
            {
                depth++;
            }

            if (samePathReturnsParent && explodedFiles.All(folders => folders.Length == depth + 1))
            {
                depth--;
            }

            return string.Join(separator.ToString(), explodedFiles.First().Take(depth));
        }

        /// <summary>
        /// Converts a list of files to a dictionary where the key is unique portion of path and the value is the full path
        /// </summary>
        public static Dictionary<string, string> UniquifyFiles(IEnumerable<string> fileNames)
        {
            // make sure we're starting with a unique list of files
            var uniqueFiles = fileNames.GroupBy(path => path).Select(grp => grp.Key).ToArray();

            var results = uniqueFiles.Select((path, index) => new FileItem()
            {
                Index = index,
                DisplayPath = Path.GetFileName(path),
                FullPath = path
            }).ToDictionary(item => item.Index);

            while (true)
            {
                var dups = results
                    .GroupBy(item => item.Value.DisplayPath)
                    .Where(grp => grp.Count() > 1);

                foreach (var dupGrp in dups)
                {
                    var items = dupGrp.ToArray();
                    for (int i = 0; i < items.Length; i++)
                    {
                        var item = items[i];
                        results[item.Key].Depth++;
                        results[item.Key].DisplayPath = item.Value.GetFilenameAtDepth();
                    }
                }

                if (!dups.Any()) break;
            }

            return results.ToDictionary(item => item.Value.DisplayPath, item => item.Value.FullPath);
        }

        /// <summary>
        /// Gets the containing folder of the specified path
        /// </summary>
        public static string GetParentFolder(string path)
        {
            string folder = Path.GetDirectoryName(path);
            string[] parts = folder.Split('\\');
            return string.Join("\\", parts.Take(parts.Length - 1));
        }

        public static string ReplaceExtension(string fileName, string newExtension)
        {
            string path = Path.GetDirectoryName(fileName);
            string baseName = Path.GetFileNameWithoutExtension(fileName);
            return Path.Combine(path, baseName + EnsureStartsWith(newExtension, "."));
        }

        private static string EnsureStartsWith(string input, string mustStartsWith)
        {
            return (!input.StartsWith(mustStartsWith)) ? mustStartsWith + input : input;
        }

        internal class FileItem
        {
            public int Index { get; set; }
            public string DisplayPath { get; set; }
            public string FullPath { get; set; }
            public int Depth { get; set; }

            public string GetFilenameAtDepth()
            {
                string[] folders = FullPath.Split('\\');
                return string.Join("\\", folders.Skip(folders.Length - Depth - 1));
            }
        }
    }
}
