using System;
using System.IO;
using System.Windows.Forms;
using WinForms.Library;
using WinForms.SampleApp.Models;

namespace WinForms.SampleApp
{
    public partial class frmDataGridViewBinder : Form
    {
        private JsonSDI<SampleDoc> _binder = new JsonSDI<SampleDoc>("*.json", "Json Files|*.json", "Save changes?");

        public frmDataGridViewBinder()
        {
            InitializeComponent();
        }

        private string GetFilename() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "sample-grid-binder.json");

        private async void frmDataGridViewBinder_Load(object sender, EventArgs e)
        {
            await _binder.OpenAsync(GetFilename(), () => new SampleDoc());

            _binder.Controls.Add(dataGridView1, doc => doc.Items);            
        }

        private async void frmDataGridViewBinder_FormClosing(object sender, FormClosingEventArgs e)
        {
            await _binder.SaveAsync();
        }
    }
}
