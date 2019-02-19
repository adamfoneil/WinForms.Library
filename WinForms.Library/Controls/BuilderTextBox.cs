using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Interfaces;
using WinForms.Library.Models;

namespace WinForms.Library.Controls
{
	public delegate void BuilderEventHandler(object sender, BuilderEventArgs e);

	public partial class BuilderTextBox : UserControl, IBoundControl<string>
	{
		private IEnumerable<ListItem<string>> _suggestions = null;

		public event BuilderEventHandler BuilderClicked;
		public new event EventHandler TextChanged;
		public event EventHandler ValueChanged;

		public BuilderTextBox()
		{
			InitializeComponent();			
		}

		public new bool Enabled
		{
			get { return textBox1.Enabled; }
			set
			{
				textBox1.Enabled = value;
				button1.Enabled = value;
			}
		}

		public new string Text
		{
			get { return textBox1.Text; }
			set { textBox1.Text = value; }
		}

		private void button1_Click(object sender, EventArgs e)
		{			
			if (Suggestions?.Any() ?? false)
			{				
				cmSuggestions.Show(button1, new Point(0, button1.Height));
			}
			else
			{
				InvokeBuilder(sender);
			}
		}

		private void InvokeBuilder(object sender)
		{
			BuilderEventArgs args = new BuilderEventArgs();
			args.Result = Text;
			BuilderClicked?.Invoke(sender, args);
			if (args.IsAccepted) Text = args.Result;
		}

		private void LoadSuggestions()
		{
			var removeItems = cmSuggestions.Items.OfType<BuilderSuggestionMenuItem>().ToArray();
			foreach (var item in removeItems) cmSuggestions.Items.Remove(item);

			if (Suggestions != null)
			{
				foreach (var item in Suggestions)
				{
					BuilderSuggestionMenuItem menuItem = new BuilderSuggestionMenuItem(item);
					menuItem.Click += delegate (object sender, EventArgs e) { textBox1.Text = item.Value; };
					cmSuggestions.Items.Insert(0, menuItem);
				}
			}
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			TextChanged?.Invoke(textBox1, e);
			ValueChanged?.Invoke(textBox1, e);
		}

		public IEnumerable<ListItem<string>> Suggestions
		{
			get { return _suggestions; }
			set
			{
				_suggestions = value;
				LoadSuggestions();
			}
		}

		public bool HasText { get { return !string.IsNullOrEmpty(textBox1.Text); } }

		public string Value { get { return textBox1.Text; } set { textBox1.Text = value; } }

		private void selectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			InvokeBuilder(sender);
		}
	}

	public class BuilderEventArgs
	{
		public bool IsAccepted { get; set; }
		public string Result { get; set; }
	}

	public class BuilderSuggestionMenuItem : ToolStripMenuItem
	{
		public BuilderSuggestionMenuItem(ListItem<string> comboBoxItem) 
		{
			Text = comboBoxItem.Text;
			Value = comboBoxItem.Value;
		}

		public string Value { get; }
	}
}