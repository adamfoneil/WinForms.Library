using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WinForms.Library
{
    public static partial class FileSystem
    {
        // thanks to http://pinvoke.net/default.aspx/kernel32/FindFirstFile.html

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct WIN32_FIND_DATA
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public uint nFileSizeHigh;
            public uint nFileSizeLow;
            public uint dwReserved0;
            public uint dwReserved1;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        private const int MAX_PATH = 260;
        private const int MAX_ALTERNATE = 14;

        [StructLayout(LayoutKind.Sequential)]
        private struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        };

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FindClose(IntPtr hFindFile);

        public static IEnumerable<FileSearchResult> Search(string path, string[] excludeItems = null, string[] includeItems = null, IProgress<string> progress = null)
        {
            return SearchR(path, path, excludeItems, includeItems, progress).ToArray();
        }

        public static async Task<IEnumerable<FileSearchResult>> SearchAsync(string path, string[] excludeItems = null, string[] includeItems = null, IProgress<string> progress = null)
        {
            return await Task.Run(() =>
            {
                return SearchR(path, path, excludeItems, includeItems, progress).ToArray();
            });
        }

        private static List<FileSearchResult> SearchR(string originalPath, string path, string[] excludeItems = null, string[] includeItems = null, IProgress<string> progress = null)
        {
            IntPtr invalidHandle = new IntPtr(-1);

            IntPtr handle;
            WIN32_FIND_DATA findData;
            List<FileSearchResult> results = new List<FileSearchResult>();

            handle = FindFirstFile($@"{path}\*", out findData);
            if (handle != invalidHandle)
            {
                do
                {
                    if (IsDirectory(findData))
                    {
                        string subdir = Path.Combine(path, findData.cFileName);
                        if (IncludeItem(subdir, true, excludeItems, includeItems))
                        {
                            results.AddRange(SearchR(originalPath, subdir, excludeItems, includeItems, progress));
                        }
                    }
                    else if (IsValidFile(findData))
                    {
                        string fullPath = Path.Combine(path, findData.cFileName);
                        if (IncludeItem(fullPath, false, excludeItems, includeItems))
                        {
                            string baseName = fullPath.Substring(originalPath.Length + 1);
                            progress?.Report(baseName);
                            int extPeriod = baseName.LastIndexOf(".");

                            results.Add(new FileSearchResult()
                            {
                                FullPath = fullPath,
                                BaseName = baseName,
                                Name = Path.GetFileName(fullPath),
                                LastModified = ToDateTime(findData.ftLastWriteTime),
                                Extension = (extPeriod > -1) ? baseName.Substring(extPeriod) : string.Empty,
                                Length = (long)findData.nFileSizeLow + (long)findData.nFileSizeHigh * uint.MaxValue
                            });
                        }
                    }
                }
                while (FindNextFile(handle, out findData));
            }
            FindClose(handle);

            return results;
        }

        private static bool IncludeItem(string path, bool isDirectory, string[] excludeItems = null, string[] includeItems = null)
        {
            if (excludeItems == null && includeItems == null) return true;

            bool include = !includeItems?.Any() ?? true;
            bool exclude = !excludeItems?.Any() ?? false;

            if (includeItems?.Any() ?? false)
            {
                foreach (var item in includeItems)
                {
                    if (path.ToLower().Contains(item.ToLower()))
                    {
                        include = true;
                        break;
                    }
                }
                if (isDirectory) include = true;
            }

            if (excludeItems?.Any() ?? false)
            {
                foreach (var item in excludeItems)
                {
                    if (path.ToLower().Contains(item.ToLower()))
                    {
                        exclude = true;
                        break;
                    }
                }
            }

            return include && !exclude;
        }

        private static bool IsValidFile(WIN32_FIND_DATA findData)
        {
            return (!findData.cFileName.Equals(".") && !findData.cFileName.Equals(".."));
        }

        private static bool IsDirectory(WIN32_FIND_DATA findData)
        {
            return
                ((findData.dwFileAttributes & FileAttributes.Directory) == FileAttributes.Directory) && IsValidFile(findData);
        }

        private static DateTime ToDateTime(FILETIME fileTime)
        {
            // thanks to http://stackoverflow.com/questions/6083733/not-being-able-to-convert-from-filetime-windows-time-to-datetime-i-get-a-dif
            // adapted from answer down at bottom

            long longValue;

            byte[] highBytes = BitConverter.GetBytes(fileTime.dwHighDateTime);
            Array.Resize(ref highBytes, 8);

            longValue = BitConverter.ToInt64(highBytes, 0);
            longValue = longValue << 32;
            longValue = longValue | fileTime.dwLowDateTime;

            return DateTime.FromFileTime(longValue);
        }

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
}