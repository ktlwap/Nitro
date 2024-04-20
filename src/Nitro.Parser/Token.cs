namespace Nitro.Parser;

public sealed class Token
{
    public required TokenType Type { get; set; }
    public required string Value { get; set; }
}