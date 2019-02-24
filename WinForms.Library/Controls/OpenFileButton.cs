using System;
using System.IO;
using System.Windows.Forms;

namespace WinForms.Library.Controls
{
	public delegate void FileSelectedHandler(string fileName);

	public class OpenFileButton : ToolStripButton
	{
		public event FileSelectedHandler FileSelected;

		public OpenFileButton(string fileName, FileSelectedHandler handler = null) : base(Path.GetFileNameWithoutExtension(fileName))
		{
			Filename = fileName;
			DisplayStyle = ToolStripItemDisplayStyle.Text;
			if (handler != null) FileSelected = handler;
		}

		public string Filename { get; }

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			FileSelected?.Invoke(Filename);
		}
	}
}