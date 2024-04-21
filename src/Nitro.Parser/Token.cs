namespace Nitro.Parser;

public sealed class Token
{
    public required TokenType Type { get; set; }
    public required string Value { get; set; }
    public required List<Attribute>? Attributes { get; init; } // TODO: Remove nullability
}
