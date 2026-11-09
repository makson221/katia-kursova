namespace PrivateSchool.Utils
{
    /// <summary>
    /// Елемент випадаючого списку: значення + текст для відображення.
    /// </summary>
    public class ComboItem
    {
        public ComboItem(object value, string text)
        {
            Value = value;
            Text = text;
        }

        public object Value { get; private set; }

        public string Text { get; private set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
