using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
        public static void EnumFiles(string path, string searchPattern, Func<FileInfo, EnumFileResult> fileFound)
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
                    EnumFiles(subFolder, searchPattern, fileFound);
                }
            }
        }

        public static void EnumDirectories(string path, Func<DirectoryInfo, EnumFileResult> starting = null, Func<DirectoryInfo, EnumFileResult> ending = null)
        {
            var di = new DirectoryInfo(path);

            var result = starting?.Invoke(di) ?? EnumFileResult.Continue;            
            if (result == EnumFileResult.Stop) return;

            if (TryGetDirectories(path, out IEnumerable<string> folderNames))
            {
                foreach (var dir in folderNames)
                {
                    EnumDirectories(dir, starting, ending);
                }
            }

            result = ending?.Invoke(di) ?? EnumFileResult.Continue;            
            if (result == EnumFileResult.Stop) return;
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

        public static IEnumerable<string> FindFolders(string rootPath, string query, int minQueryLength = 3, int maxResults = 0, IProgress<string> progress = null, CancellationTokenSource cancellationTokenSource = null)
        {
            string[] queryParts = query
                .Split(new char[] { ' ', '/', '\\' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.ToLower().Trim())
                .Where(part => part.Length >= minQueryLength)
                .ToArray();

            if (!queryParts.Any()) return Enumerable.Empty<string>();

            List<string> results = new List<string>();
            addFolders(rootPath);

            return results
                .Where(path => queryParts.All(part => path.ToLower().Contains(part)))
                .ToArray();

            void addFolders(string searchPath)
            {
                progress?.Report(searchPath);
                if (cancellationTokenSource?.IsCancellationRequested ?? false) return;

                if (TryGetDirectories(searchPath, out IEnumerable<string> folders))
                {
                    results.AddRange(folders);                    

                    if (maxResults > 0 && results.Count > maxResults)
                    {
                        results = results.Take(maxResults).ToList();
                        return;
                    }

                    foreach (var folder in folders) addFolders(folder);
                }
            }            
        }

        public static async Task<IEnumerable<string>> FindFoldersAsync(string rootPath, string query, int minQueryLength = 3, int maxResults = 0, IProgress<string> progress = null, CancellationTokenSource cancellationTokenSource = null)
        {
            IEnumerable<string> results = null;
            
            await Task.Run(() =>
            {
                results = FindFolders(rootPath, query, minQueryLength, maxResults, progress, cancellationTokenSource);
            });

            return results;
        }
    }
}
