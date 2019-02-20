using System.Diagnostics;

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
	}
}