using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.Library.Extensions;
using WinForms.Library.Extensions.ToolStrip;
using WinForms.SampleApp.Models;

namespace WinForms.SampleApp
{
    public partial class Form1 : Form
    {
        private JsonSDI<AppDocument> _docManager = null;
        private ListViewItem _selectedItem = null;

        public Form1()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _docManager = new JsonSDI<AppDocument>(".json", "Json Files|*.json", "Save changes?");
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

            _docManager.Controls.Add(tbToolStrip, doc => doc.ToolStripText);
            _docManager.Controls.AddItems(cbToolStrip, 
                (doc) => doc.DocumentItemValue = ToolStripComboBoxExtensions.GetItem<DocumentItem>(cbToolStrip).Value,
                (doc) => cbToolStrip.SelectedItem = AppDocument.SelectableItems.SingleOrDefault(item => item.Value == doc.DocumentItemValue),
                AppDocument.SelectableItems);
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

        private async void bldPath_BuilderClicked(object sender, Library.Controls.BuilderEventArgs e)
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

        private void BtnFileType_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All Files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string type = FileSystem.GetFileType(dlg.FileName);
                string size = FileSystem.GetFileSize(dlg.FileName);
                MessageBox.Show(type + " - " + size);
            }
        }

        private void builderTextBox2_BuilderClicked(object sender, Library.Controls.BuilderEventArgs e)
        {
            if (builderTextBox2.SelectFolder(e))
            {
                listView1.BeginUpdate();
                FileSystem.EnumFiles(e.Result, "*", directoryFound: (di) =>
                {
                    listView1.Items.Add(di.FullName);
                    return (listView1.Items.Count < 100) ? EnumFileResult.Continue : EnumFileResult.Stop;
                });
                listView1.EndUpdate();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var items = new SampleItem[]
            {
                new SampleItem() { FirstName = "Yoda", LastName = "Manginold", BirthDate = DateTime.Today },
                new SampleItem() { FirstName = "Jimminy", LastName = "Hambone", BirthDate = DateTime.Today.AddDays(-720) },
                new SampleItem() { FirstName = "Orzo", LastName = "Oopsiedaisey", BirthDate = DateTime.Today.AddDays(-1000) }
            };

            dataGridView1.Fill(items);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var items = dataGridView1.GetItems<SampleItem>();
            MessageBox.Show(string.Join("\r\n", items.Select(item => $"{item.FirstName} {item.LastName} {item.BirthDate}")));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Fill<SampleItem>(null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(PathUtil.EnvironmentPath(Environment.SpecialFolder.LocalApplicationData, "this", "that", "hello.json"));
        }
    }
}