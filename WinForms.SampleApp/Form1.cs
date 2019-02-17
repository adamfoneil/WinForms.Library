using System;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.SampleApp.Models;

namespace WinForms.SampleApp
{
	public partial class Form1 : Form
	{
		private DocumentManager<AppDocument> _docManager = null;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_docManager = new DocumentManager<AppDocument>(".json", "Json Files|*.json", "Save changes?");
			_docManager.Document = new AppDocument();
			_docManager.AddControl(tbFirstName, doc => doc.FirstName);
			_docManager.AddControl(tbLastName, doc => doc.LastName);
			_docManager.AddControl(chkIsActive, doc => doc.IsActive);
			_docManager.AddControl(cbBelt, doc => doc.Belt);
			_docManager.AddControl(dateTimePicker1, doc => doc.EffectiveDate);
			_docManager.AddControl(builderTextBox1, doc => doc.BuilderText);
		}

		private async void btnNew_Click(object sender, EventArgs e)
		{
			await _docManager.NewAsync();
		}

		private async void toolStripButton1_Click(object sender, EventArgs e)
		{
			if (await _docManager.PromptOpenAsync())
			{
				Text = _docManager.Filename;
			}
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			await _docManager.SaveAsync();
		}

		private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			await _docManager.FormClosingAsync(e);
		}
	}
}