public static class StringExtensions
{
    public static bool IsEmail(this string input)
    {
        return !string.IsNullOrWhiteSpace(input) && input.Contains('@');
    }
}