using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.Library.Controls
{
	public class DropDownButton : Button
	{
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			if (ContextMenuStrip != null) ContextMenuStrip.Show(this, new Point(0, this.Height));
		}

		protected override void OnPaint(PaintEventArgs pevent)
		{
			base.OnPaint(pevent);

			int left = this.Width - 20;
			int top = 10;
			int width = 10;
			int height = 5;

			pevent.Graphics.FillPolygon(new SolidBrush(Color.Black), new Point[] {
				new Point(left, top), new Point(left + width, top), new Point(left + (width / 2), height + top)
			});
		}
	}
}
