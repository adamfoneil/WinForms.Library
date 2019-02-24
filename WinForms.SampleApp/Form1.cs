using JsonSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.Library.Controls;
using WinForms.SampleApp.Models;

namespace WinForms.SampleApp
{
	public partial class Form1 : Form
	{
		private JsonSDI<AppDocument> _docManager = null;
		private ListViewItem _selectedItem = null;
		private MruFileList _recentFiles = null;

		private const string mruFile = @"c:\users\adam\desktop\mruList.json";

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			_docManager = new JsonSDI<AppDocument>(".json", "Json Files|*.json", "Save changes?");
			_docManager.FileOpened += UpdateMruList;
			_docManager.FileSaved += UpdateMruList;

			_docManager.Document = new AppDocument();
			_docManager.Controls.Add(tbFirstName, doc => doc.FirstName);
			_docManager.Controls.Add(tbLastName, doc => doc.LastName);
			_docManager.Controls.Add(chkIsActive, doc => doc.IsActive);
			_docManager.Controls.AddEnum(cbBelt, doc => doc.Belt);
			_docManager.Controls.Add(dateTimePicker1, doc => doc.EffectiveDate);
			_docManager.Controls.Add(builderTextBox1, doc => doc.BuilderText);
			_docManager.Controls.Add(numericUpDown1, doc => doc.Level);
			_docManager.Controls.AddItems(cbItem, doc => doc.Item, AppDocument.SelectableItems);
			_docManager.Controls.AddItems(cbKeyedItem, doc => doc.Key, AppDocument.KeyedItems);

			_recentFiles = JsonFile.Load(mruFile, () =>
			{
				return new MruFileList(4);
			});

			LoadMruToolbar(_recentFiles);
		}

		private void UpdateMruList(object sender, EventArgs e)
		{
			_recentFiles.Add(_docManager.Filename);
			LoadMruToolbar(_recentFiles);
		}

		private void LoadMruToolbar(MruFileList files)
		{
			var buttons = toolStrip1.Items.OfType<OpenFileButton>().ToArray();
			foreach (var btn in buttons) toolStrip1.Items.Remove(btn);

			foreach (string fileName in files)
			{
				var btn = new OpenFileButton(fileName, async (f) =>
				{
					await _docManager.OpenAsync(f);
				});
				toolStrip1.Items.Add(btn);
			}
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
			if (_recentFiles != null) JsonFile.Save(mruFile, _recentFiles);

			await _docManager.FormClosingAsync(e);
		}

		private async void bldPath_BuilderClicked(object sender, BuilderEventArgs e)
		{
			if (bldPath.SelectFolder(e))
			{
				var files = await FileSystem.SearchAsync(e.Result);
				listView1.Items.Clear();
				var list = files.ToArray().Take(100);
				foreach (var file in list)
				{					
					var listItem = new ListViewItem(file.FullPath)
					{
						ImageKey = FileSystem.AddIcon(imageList1, file.FullPath, FileSystem.IconSize.Small)
					};
					listView1.Items.Add(listItem);
				}
			}
		}

		private void listView1_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				var hti = listView1.HitTest(e.Location);
				_selectedItem = hti.Item;
			}
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_selectedItem != null)
			{
				FileSystem.OpenDocument(_selectedItem.Text);
			}
		}

		private void revealInFileExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (_selectedItem != null)
			{
				FileSystem.RevealInExplorer(_selectedItem.Text);
			}
		}
	}
}