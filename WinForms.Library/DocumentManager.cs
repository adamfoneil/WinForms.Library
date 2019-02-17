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
		private bool _suspend = false;
		private List<Action<TDocument>> _setControls = new List<Action<TDocument>>();

		public event EventHandler FileOpened;

		public event EventHandler FileSaved;

		public event EventHandler FilenameChanged;

		public event EventHandler IsDirtyChanged;

		private bool _dirty = false;
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
			get { return _dirty; }
			set
			{
				if (_dirty != value)
				{
					_dirty = value;
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
		public bool HasFilename { get { return !string.IsNullOrEmpty(Filename); } }

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
			if (!HasFilename)
			{				
				return await PromptSaveAsync();				
			}

			await SaveInnerAsync();
			return true;
		}

		public async Task FormClosingAsync(FormClosingEventArgs e)
		{
			try
			{
				if (IsDirty)
				{
					var response = MessageBox.Show(FormClosingMessage, "Form Closing", MessageBoxButtons.YesNoCancel);
					switch (response)
					{
						case DialogResult.Yes:
							e.Cancel = !await SaveAsync();
							break;

						case DialogResult.No: // close without saving	
							e.Cancel = false;
							break;

						case DialogResult.Cancel: // keep form open
							e.Cancel = true;
							break;
					}
				}
				else
				{
					e.Cancel = !await SaveAsync();
				}
			}
			catch (Exception exc)
			{
				MessageBox.Show($"There was an error saving the file: {exc.Message}");
				e.Cancel = true;
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