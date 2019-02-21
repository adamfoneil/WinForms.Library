using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForms.Library.Models
{
	public class FormPosition
	{
		public bool IsMaximized { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }

		/// <summary>
		/// Gets a FormPosition object from a Form
		/// </summary>
		public static FormPosition FromForm(Form form)
		{
			FormPosition result = new FormPosition();
			result.IsMaximized = (form.WindowState == FormWindowState.Maximized);
			result.Location = form.Location;
			result.Size = form.Size;
			return result;
		}

		/// <summary>
		/// Apply's the position to a Form
		/// </summary>		
		public void Apply(Form form)
		{
			form.SuspendLayout();
			try
			{
				if (IsMaximized)
				{
					form.WindowState = FormWindowState.Maximized;
				}
				else
				{
					form.WindowState = FormWindowState.Normal;
					form.Location = Location;
					form.Size = Size;
				}
			}
			finally
			{
				form.ResumeLayout();
			}			
		}

		/// <summary>
		/// Persists a form's position as long as it's not minimized
		/// </summary>
		public static void Save(Form form, Action<FormPosition> saveAction)
		{
			if (form.WindowState != FormWindowState.Minimized)
			{
				var position = FromForm(form);
				saveAction.Invoke(position);
			}
		}
	}
}