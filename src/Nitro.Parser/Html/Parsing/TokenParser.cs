namespace Nitro.Parser.Html.Parsing;

internal abstract class TokenParser
{
    internal abstract bool Parse(char[] input, int i, out TokenParseResult? result);
}
