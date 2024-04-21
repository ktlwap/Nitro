using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class ClosingButtonTokenParser : TokenParser
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
        
        if (!ParseUtils.IsChar(in input, 'b', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'u', ref i, false))
            return false;
        sb.Append(input[i++]);

        if (!ParseUtils.IsChar(in input, 't', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 't', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'o', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'n', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        List<Attribute> attributes = Tokenizer.ParseOptionalAttributes(sb, input, ref i);
        
        result = new TokenParseResult()
        {
            Token = new Token()
            {
                Type = TokenType.ClosingButtonTag,
                Value = sb.ToString(),
                Attributes = attributes
            },
            Index = i
        };
        
        return true;
    }
}
