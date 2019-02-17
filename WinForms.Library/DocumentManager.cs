using JsonSettings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinForms.Library
{
	public delegate Task<TDocument> DocumentLoadHandler<TDocument>(string fileName);

	/// <summary>
	/// Handles form commands and events related to saving, loading, and prompting,
	/// but not a complete data binding solution
	/// </summary>
	public partial class DocumentManager<TDocument> where TDocument : new()
	{
		public event EventHandler FileOpened;

		public event EventHandler FileSaved;

		public event EventHandler FilenameChanged;

		public event EventHandler IsDirtyChanged;

		private bool _isDirty = false;
		private string _fileName;

		public Action<JsonSerializerSettings> UpdateSerializerSettingsOnSave { get; set; }

		public DocumentManager(string defaultExtension, string fileOpenFilter, string formClosingMessage)
		{
			DefaultExtension = defaultExtension;
			FileDialogFilter = fileOpenFilter;
			FormClosingMessage = formClosingMessage;
		}

		public Dictionary<string, DocumentLoadHandler<TDocument>> FileHandlers { get; } = new Dictionary<string, DocumentLoadHandler<TDocument>>();

		public bool IsDirty
		{
			get { return _isDirty; }
			set
			{
				if (_isDirty != value)
				{
					_isDirty = value;
					IsDirtyChanged?.Invoke(this, new EventArgs());
				}
			}
		}

		public string Filename
		{
			get { return _fileName; }
			set
			{
				if (!_fileName?.Equals(value) ?? true)
				{
					_fileName = value;
					FilenameChanged?.Invoke(this, new EventArgs());
				}
			}
		}

		public TDocument Document { get; set; }

		public string DefaultExtension { get; }
		public string FileDialogFilter { get; }
		public string FormClosingMessage { get; }

		public async Task<bool> OpenAsync(string fileName)
		{
			if (!await SaveIfDirtyAsync()) return false;
			await OpenInnerAsync(fileName);
			return true;
		}

		public async Task<bool> PromptOpenAsync()
		{
			if (!await SaveIfDirtyAsync()) return false;

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = FileDialogFilter;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				await OpenInnerAsync(dlg.FileName);
				return true;
			}

			return false;
		}

		private async Task OpenInnerAsync(string fileName)
		{
			string ext = Path.GetExtension(fileName);
			if (FileHandlers.ContainsKey(ext))
			{
				Document = await FileHandlers[ext].Invoke(fileName);
			}
			else
			{
				Document = await JsonFile.LoadAsync<TDocument>(fileName);
			}
			
			Filename = fileName;
			SetControls(); 
			FileOpened?.Invoke(this, new EventArgs());
		}

		private void SetControls()
		{
			_suspend = true;
			foreach (var setter in _setControls) setter.Invoke(Document);
			_suspend = false;			
		}

		private async Task<bool> SaveIfDirtyAsync()
		{
			// this is a single document interface, so opening or newing a doc
			// requires taking care of the current doc before continuing
			if (IsDirty) return await SaveAsync();

			return true;
		}

		public async Task<bool> PromptSaveAsync(string initialPath = null)
		{
			SaveFileDialog dlg = new SaveFileDialog();

			if (!string.IsNullOrEmpty(initialPath)) dlg.InitialDirectory = initialPath;
			dlg.DefaultExt = DefaultExtension;
			dlg.Filter = FileDialogFilter;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				Filename = dlg.FileName;
				await SaveInnerAsync();
				return true;
			}

			return false;
		}

		public async Task<bool> SaveAsync(string initialPath = null)
		{
			if (string.IsNullOrEmpty(Filename))
			{				
				return await PromptSaveAsync();				
			}

			await SaveInnerAsync();
			return true;
		}

		public async Task FormClosingAsync(FormClosingEventArgs e)
		{
			if (IsDirty && !await SaveAsync())
			{
				e.Cancel = (MessageBox.Show(FormClosingMessage, "Form Closing", MessageBoxButtons.OKCancel) == DialogResult.Cancel);
			}
		}

		private async Task SaveInnerAsync()
		{
			await JsonFile.SaveAsync(Filename, Document, UpdateSerializerSettingsOnSave);
			IsDirty = false;
			FileSaved?.Invoke(this, new EventArgs());
		}

		public async Task<bool> NewAsync()
		{
			if (!await SaveIfDirtyAsync()) return false;

			IsDirty = false;
			Filename = null;
			Document = new TDocument();
			return true;
		}
	}
}