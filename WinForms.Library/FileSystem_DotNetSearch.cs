using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WinForms.Library
{
    public enum EnumFileResult
    {
        /// <summary>
        /// Return the next file or directory
        /// </summary>
        Continue,
        /// <summary>
        /// Move to the next folder
        /// </summary>
        NextFolder,
        /// <summary>
        /// Stop the enumeration entirely
        /// </summary>
        Stop
    }

    public static partial class FileSystem
    {
        public static void EnumFiles(string path, string searchPattern, Func<DirectoryInfo, EnumFileResult> directoryFound = null, Func<FileInfo, EnumFileResult> fileFound = null)
        {
            if (fileFound != null)
            {
                if (TryGetFiles(path, searchPattern, out IEnumerable<string> fileNames))
                {
                    foreach (var file in fileNames)
                    {
                        var fi = new FileInfo(file);
                        var result = fileFound.Invoke(fi);
                        if (result == EnumFileResult.NextFolder) break;
                        if (result == EnumFileResult.Stop) return;
                    }
                }

                if (TryGetDirectories(path, out IEnumerable<string> folderNames))
                {
                    foreach (var subFolder in folderNames)
                    {
                        EnumFiles(subFolder, searchPattern, null, fileFound);
                    }
                }
            }

            if (directoryFound != null)
            {
                if (TryGetDirectories(path, out IEnumerable<string> folderNames))
                {
                    foreach (var dir in folderNames)
                    {
                        var di = new DirectoryInfo(dir);
                        var result = directoryFound.Invoke(di);
                        if (result == EnumFileResult.NextFolder) continue;
                        if (result == EnumFileResult.Stop) return;
                        EnumFiles(dir, searchPattern, directoryFound, null);
                    }
                }
            }
        }

        public static async Task<IEnumerable<FileInfo>> FindFilesAsync(string path, string searchPattern, Func<FileInfo, bool> filter = null)
        {
            List<FileInfo> results = new List<FileInfo>();

            await Task.Run(() =>
            {
                EnumFiles(path, searchPattern, fileFound: (fi) =>
                {
                    if (filter?.Invoke(fi) ?? true) results.Add(fi);
                    return EnumFileResult.Continue;
                });
            });

            return results;
        }

        private static bool TryGetFiles(string path, string pattern, out IEnumerable<string> fileNames)
        {
            try
            {
                fileNames = Directory.GetFiles(path, pattern);
                return true;
            }
            catch
            {
                fileNames = Enumerable.Empty<string>();
                return false;
            }
        }

        private static bool TryGetDirectories(string path, out IEnumerable<string> folderNames)
        {
            try
            {
                folderNames = Directory.GetDirectories(path);
                return true;
            }
            catch
            {
                folderNames = Enumerable.Empty<string>();
                return false;
            }
        }
    }
}
