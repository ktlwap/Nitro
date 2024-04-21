namespace Nitro.Parser.Html.Parsing;

internal static class ParseUtils
{
    internal static bool IsChar(in char[] input, char expected, ref int index, bool skipWhitespace = true)
    {
        if (index >= input.Length)
            return false;
        
        char c = input[index];

        if (skipWhitespace && char.IsWhiteSpace(c))
        {
            ++index;
            return IsChar(in input, expected, ref index);
        }

        return char.ToLower(c) == char.ToLower(expected);
    }
}
