using System;

static class Helper
{
    public static string GetUntilOrEmpty(this string text, string stopAt = "-")
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return string.Empty;
    }
}