using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class DocTypeTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        StringBuilder sb = new StringBuilder();
        
        if (!ParseUtils.IsChar(in input, '<', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, '!', ref i))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'd', ref i, false))
            return false;
        sb.Append(input[i++]);

        if (!ParseUtils.IsChar(in input, 'o', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'c', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 't', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'y', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'p', ref i, false))
            return false;
        sb.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'e', ref i, false))
            return false;
        sb.Append(input[i++]);

        List<Attribute> attributes = Tokenizer.ParseOptionalAttributes(sb, input, ref i);

        result = new TokenParseResult()
        {
            Token = new Parser.Token()
            {
                Type = TokenType.DocTypeTag,
                Value = sb.ToString(),
                Attributes = attributes
            },
            Index = i
        };
        
        return true;
    }
}
