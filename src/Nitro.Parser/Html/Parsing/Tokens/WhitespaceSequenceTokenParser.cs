using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class WhitespaceSequenceTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        
        if (!char.IsWhiteSpace(input[i]))
            return false;

        StringBuilder sb = new StringBuilder();
        while (i < input.Length)
        {
            char c = input[i];
            if (!char.IsWhiteSpace(c))
                break;
            
            sb.Append(c);
            ++i;
        }
        
        result = new TokenParseResult()
        {
            Token = new Parser.Token()
            {
                Type = TokenType.WhitespaceSequence,
                Value = sb.ToString(),
                Attributes = []
            },
            Index = i
        };
        
        return true;
    }
}
