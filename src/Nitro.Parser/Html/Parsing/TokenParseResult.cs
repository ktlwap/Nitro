namespace Nitro.Parser.Html.Parsing;

internal class TokenParseResult
{
    public required Parser.Token Token { get; init; }
    public required int Index { get; init; }
}
