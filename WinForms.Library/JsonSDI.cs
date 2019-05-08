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
	/// Json Single Document Interface - handles form commands and events
	/// related to saving, loading, and prompting json documents
	/// </summary>
	public class JsonSDI<TDocument> where TDocument : new()
	{
		public event EventHandler FileOpened;

		public event EventHandler FileSaved;

		public event EventHandler FilenameChanged;

		private string _fileName;

		public Action<JsonSerializerSettings> UpdateSerializerSettingsOnSave { get; set; }

		public JsonSDI(string defaultExtension, string fileOpenFilter, string formClosingMessage)
		{
			DefaultExtension = defaultExtension;
			FileDialogFilter = fileOpenFilter;
			FormClosingMessage = formClosingMessage;
			Controls = new ControlBinder<TDocument>();
		}

		public ControlBinder<TDocument> Controls { get; } = null;

		/// <summary>
		/// Use this to open a different file than what was selected in File Open dialog based on extension.
		/// This is used in SMM to open an .smm file from a given .sln
		/// </summary>
		public Dictionary<string, Func<string, string>> FileHandlers { get; } = new Dictionary<string, Func<string, string>>();

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

		private TDocument _doc;
		public TDocument Document
		{
			get { return _doc; }
			set { _doc = value; Controls.Document = value; Controls.LoadValues(); }
		}

		public string DefaultExtension { get; }
		public string FileDialogFilter { get; }
		public string FormClosingMessage { get; }
		public bool HasFilename { get { return !string.IsNullOrEmpty(Filename); } }

		public async Task<bool> OpenAsync(string fileName, Func<TDocument> ifNotExists = null)
		{
			if (!await SaveIfDirtyAsync()) return false;
			await OpenInnerAsync(fileName, ifNotExists);
			return true;
		}

		public async Task<bool> PromptOpenAsync(Func<TDocument> ifNotExists = null)
		{
			if (!await SaveIfDirtyAsync()) return false;

			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = FileDialogFilter;
			if (dlg.ShowDialog() == DialogResult.OK)
			{
				await OpenInnerAsync(dlg.FileName, ifNotExists);
				return true;
			}

			return false;
		}

		private async Task OpenInnerAsync(string fileName, Func<TDocument> ifNotExists = null)
		{
			string ext = Path.GetExtension(fileName);

			string openFile = fileName;
			if (FileHandlers.ContainsKey(ext))
			{
				openFile = FileHandlers[ext].Invoke(fileName);
			}			
			
			Document = await JsonFile.LoadAsync(openFile, ifNotExists);
			Filename = openFile;
			FileOpened?.Invoke(this, new EventArgs());
		}

		/// <summary>
		/// Returns true if 
		/// - document is unchanged, 
		/// - document has changes and user wants to save,
		/// - user discards changes
		/// </summary>		
		private async Task<bool> SaveIfDirtyAsync()
		{
			// this is a single document interface, so opening or newing a doc
			// requires taking care of the current doc before continuing
			if (Controls.IsDirty)
			{
				var response = PromptClosing();
				if (response == DialogResult.Yes)
				{
					return await SaveAsync();
				}
				else
				{
					// if user clicks No, it means we discard changes
					return (response == DialogResult.No);
				}
			}

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
				if (Controls.IsDirty)
				{
					var response = PromptClosing();
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
			}
			catch (Exception exc)
			{
				MessageBox.Show($"There was an error saving the file: {exc.Message}");
				e.Cancel = true;
			}
		}

		private DialogResult PromptClosing()
		{
			return MessageBox.Show(FormClosingMessage, "Unsaved Changes", MessageBoxButtons.YesNoCancel);
		}

		private async Task SaveInnerAsync()
		{
			await JsonFile.SaveAsync(Filename, Document, UpdateSerializerSettingsOnSave);
			Controls.IsDirty = false;
			FileSaved?.Invoke(this, new EventArgs());
		}

		public async Task<bool> NewAsync()
		{
			if (!await SaveIfDirtyAsync()) return false;

			Filename = null;
			Document = new TDocument();
			Controls.ClearValues();
			return true;
		}
	}
}