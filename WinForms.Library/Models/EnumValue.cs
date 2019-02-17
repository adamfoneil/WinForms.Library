namespace WinForms.Library.Models
{
	/// <summary>
	/// Used with combo boxes filled from enum values
	/// </summary>
	public class EnumValue<TEnum>
	{
		public EnumValue(TEnum value, string name)
		{
			Value = value;
			Name = name;
		}

		public string Name { get; }
		public TEnum Value { get; }

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			EnumValue<TEnum> test = obj as EnumValue<TEnum>;
			return (test != null) ? test.Value.Equals(Value) : false;			
		}

		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
	}
}