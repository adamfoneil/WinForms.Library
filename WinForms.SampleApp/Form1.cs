using System;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.SampleApp.Models;

namespace WinForms.SampleApp
{
	public partial class Form1 : Form
	{
		private JsonSDI<AppDocument> _docManager = null;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_docManager = new JsonSDI<AppDocument>(".json", "Json Files|*.json", "Save changes?");
			_docManager.Document = new AppDocument();
			_docManager.Controls.Add(tbFirstName, doc => doc.FirstName);
			_docManager.Controls.Add(tbLastName, doc => doc.LastName);
			_docManager.Controls.Add(chkIsActive, doc => doc.IsActive);
			_docManager.Controls.Add(cbBelt, doc => doc.Belt);
			_docManager.Controls.Add(dateTimePicker1, doc => doc.EffectiveDate);
			_docManager.Controls.Add(builderTextBox1, doc => doc.BuilderText);
			_docManager.Controls.Add(numericUpDown1, doc => doc.Level);
			_docManager.Controls.Add(cbItem, doc => doc.Item, AppDocument.SelectableItems);
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