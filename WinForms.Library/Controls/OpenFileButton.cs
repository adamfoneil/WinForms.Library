using System;
using System.IO;
using System.Windows.Forms;

namespace WinForms.Library.Controls
{
	internal delegate void FileSelectedHandler(string fileName);

	internal class OpenFileButton : ToolStripButton
	{
		public event FileSelectedHandler FileSelected;

		public OpenFileButton(string fileName) : base(Path.GetFileNameWithoutExtension(fileName))
		{
			Filename = fileName;
			DisplayStyle = ToolStripItemDisplayStyle.Text;
		}

		public string Filename { get; }

		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			FileSelected?.Invoke(Filename);
		}
	}
}