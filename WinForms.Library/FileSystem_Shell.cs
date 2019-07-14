using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace WinForms.Library
{
	public static partial class FileSystem
	{
		public static void OpenDocument(string fileName)
		{
			ProcessStartInfo psi = new ProcessStartInfo(fileName);
			psi.UseShellExecute = true;
			Process.Start(psi);
		}

		public static void RevealInExplorer(string fileName)
		{
			ProcessStartInfo psi = new ProcessStartInfo("explorer.exe");
			psi.Arguments = $"/select,\"{fileName}\"";
			Process.Start(psi);
		}

        [DllImport("Shlwapi.dll", CharSet = CharSet.Auto)]
        private static extern long StrFormatByteSize(long fileSize, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder buffer, int bufferSize);

        public static string GetFileSize(string fileName)
        {
            return GetFileSize(new FileInfo(fileName).Length);
        }

        /// <summary>
        /// Returns a file size as human-readable strong
        /// Thanks to https://stackoverflow.com/a/281716/2023653
        /// </summary>
        public static string GetFileSize(long filesize)
        {
            StringBuilder sb = new StringBuilder(11);
            StrFormatByteSize(filesize, sb, sb.Capacity);
            return sb.ToString();
        }
    }
}