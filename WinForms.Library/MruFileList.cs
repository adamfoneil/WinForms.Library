using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using WinForms.Library.Controls;

namespace WinForms.Library
{
	/// <summary>
	/// Most recently used list
	/// </summary>		
	[JsonConverter(typeof(MruFileListConverter))]
	public class MruFileList : List<string>
	{
		public MruFileList()
		{
		}

		public MruFileList(int maxItems)
		{
			MaxItems = maxItems;
		}

		public int MaxItems { get; set; }

		public new void Add(string fileName)
		{
			if (Contains(fileName)) return;
			base.Insert(0, fileName);

			while (Count > MaxItems) RemoveAt(Count - 1);

			if (ToolStripItemCollection != null)
			{
				RefreshMenuItems(ToolStripItemCollection, FileSelectedHandler);
			}			
		}

		public void BindToMenu(ToolStripItemCollection itemCollection, FileSelectedHandler handler)
		{
			ToolStripItemCollection = itemCollection ?? throw new ArgumentNullException(nameof(itemCollection));
			FileSelectedHandler = handler ?? throw new ArgumentNullException(nameof(handler));
			RefreshMenuItems(itemCollection, handler);
		}

		private void RefreshMenuItems(ToolStripItemCollection itemCollection, FileSelectedHandler handler)
		{
			var buttons = itemCollection.OfType<OpenFileButton>().ToArray();
			foreach (var btn in buttons) itemCollection.Remove(btn);

			foreach (string fileName in this)
			{
				var btn = new OpenFileButton(fileName, handler);
				btn.ToolTipText = fileName;
				itemCollection.Add(btn);
			}
		}

		/// <summary>
		/// Inserting on an Mru list always goes to front of list (index arg is ignored)
		/// </summary>
		public new void Insert(int index, string item)
		{
			Add(item);
		}

		public new void AddRange(IEnumerable<string> items)
		{
			foreach (var item in items)
			{
				if (File.Exists(item)) Add(item);
			}
		}

		public ToolStripItemCollection ToolStripItemCollection { get; private set; }

		public FileSelectedHandler FileSelectedHandler { get; private set; }
	}

	// thanks to https://stackoverflow.com/a/21272042/2023653
	public class MruFileListConverter : JsonConverter
	{
		private const string ItemsProperty = "Items";

		public override bool CanConvert(Type objectType)
		{
			return (objectType.Equals(typeof(MruFileList)));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JObject jo = JObject.Load(reader);
			MruFileList result = new MruFileList();
			result.MaxItems = (int)jo[nameof(MruFileList.MaxItems)];
			// since added items are actually inserted, I reverse them here to preserve the persisted order
			var items = jo[ItemsProperty].ToObject<string[]>(serializer).Reverse(); 
			result.AddRange(items);
			return result;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			MruFileList result = (MruFileList)value;
			JObject jo = new JObject();
			jo.Add(nameof(MruFileList.MaxItems), result.MaxItems);
			jo.Add(ItemsProperty, JArray.FromObject(result.ToArray(), serializer));
			jo.WriteTo(writer);
		}
	}
}