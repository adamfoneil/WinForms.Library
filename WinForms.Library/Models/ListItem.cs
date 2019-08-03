namespace WinForms.Library.Models
{
    /// <summary>
    /// General purpose item for lists, used with BuilderTextBox Suggestions
    /// </summary>	
    public class ListItem<T>
    {
        public ListItem(T value, string text)
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
            var test = obj as ListItem<T>;
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