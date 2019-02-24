using System.Collections.Generic;

namespace WinForms.Library
{
	/// <summary>
	/// Most recently used list
	/// </summary>	
	public class MruList<T> : List<T>
	{
		public MruList(int maxItems)
		{
			MaxItems = maxItems;
		}

		public int MaxItems { get; }

		public new void Add(T item)
		{
			if (Contains(item)) return;
			Insert(0, item);
			while (Count > MaxItems) RemoveAt(Count + 1);
		}		

		/// <summary>
		/// Inserting on an Mru list always goes to front of list (index arg is ignored)
		/// </summary>
		public new void Insert(int index, T item)
		{
			Add(item);
		}

		public new void AddRange(IEnumerable<T> items)
		{
			foreach (var item in items) Add(item);
		}
	}
}