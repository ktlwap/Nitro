using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class OpeningAbbrTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        
        if (!ParseUtils.IsChar(in input, '<', ref i, false))
            return false;
        
        StringBuilder sb = new StringBuilder();
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'a', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'b', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'b', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'r', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        List<Attribute> attributes = Tokenizer.ParseOptionalAttributes(sb, input, ref i);
        
        result = new TokenParseResult()
        {
            Token = new Token()
            {
                Type = TokenType.OpeningAbbrTag,
                Value = sb.ToString(),
                Attributes = attributes
            },
            Index = i
        };
        
        return true;
    }
}
