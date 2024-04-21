using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class OpeningAcronymTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        
        if (!ParseUtils.IsChar(in input, '<', ref i))
            return false;
        
        StringBuilder sb = new StringBuilder();
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'a', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'c', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'r', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'o', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'n', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'y', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'm', ref i))
            return false;
        sb.Append(input[i++]);
        
        List<Attribute> attributes = Tokenizer.ParseOptionalAttributes(sb, input, ref i);
        
        result = new TokenParseResult()
        {
            Token = new Token()
            {
                Type = TokenType.OpeningAcronymTag,
                Value = sb.ToString(),
                Attributes = attributes
            },
            Index = i
        };
        
        return true;
    }
}
