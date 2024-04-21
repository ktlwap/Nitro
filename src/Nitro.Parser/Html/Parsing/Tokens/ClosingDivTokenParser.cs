using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class ClosingDivTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        
        if (!ParseUtils.IsChar(in input, '<', ref i))
            return false;
        
        StringBuilder sb = new StringBuilder();
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, '/', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'd', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'i', ref i, false))
            return false;
        sb.Append(input[i++]);

        if (!ParseUtils.IsChar(in input, 'v', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        List<Attribute> attributes = Tokenizer.ParseOptionalAttributes(sb, input, ref i);
        
        result = new TokenParseResult()
        {
            Token = new Parser.Token()
            {
                Type = TokenType.ClosingDivTag,
                Value = sb.ToString(),
                Attributes = attributes
            },
            Index = i
        };
        
        return true;
    }
}
