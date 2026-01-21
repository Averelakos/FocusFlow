
public static class StringExtensions
{
    /// <summary>
    /// Checks if the string appears to be an email address (contains '@' symbol)
    /// </summary>
    /// <param name="input">The string to validate</param>
    /// <returns>True if the string is not empty and contains an '@' symbol</returns>
    public static bool IsEmail(this string input)
    {
        return !string.IsNullOrWhiteSpace(input) && input.Contains('@');
    }
}