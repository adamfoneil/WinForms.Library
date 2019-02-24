using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

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

		public new void Add(string item)
		{
			if (Contains(item)) return;
			base.Insert(0, item);
			while (Count > MaxItems) RemoveAt(Count - 1);
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
			result.AddRange(jo[ItemsProperty].ToObject<string[]>(serializer));
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