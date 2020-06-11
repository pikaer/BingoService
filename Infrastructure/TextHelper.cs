namespace Infrastructure
{
    public static class TextHelper
    {
        public static string CutText(this string text,int maxLenght)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLenght)
            {
                return text;
            }
            return string.Format("{0}...",text.Substring(0, maxLenght - 3));
        }
    }
}
