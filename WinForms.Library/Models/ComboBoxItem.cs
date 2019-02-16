namespace WinForms.Library.Models
{
	public class ComboBoxItem<T>
	{
		public ComboBoxItem(T value, string text)
		{
			Value = value;
			Text = text;
		}

		public T Value { get; }
		public string Text { get; }

		public override string ToString()
		{
			return Text;
		}

		public override bool Equals(object obj)
		{
			var test = obj as ComboBoxItem<T>;
			if (test != null)
			{
				return test.Value.Equals(Value);
			}

			return false;
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}