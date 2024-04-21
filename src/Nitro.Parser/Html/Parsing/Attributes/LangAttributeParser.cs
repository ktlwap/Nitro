using System.Text;

namespace Nitro.Parser.Html.Parsing.Attributes;

internal class LangAttributeParser : AttributeParser
{
    internal override bool Parse(StringBuilder sb, ref char[] input, int i, out AttributeParseResult? attributeParseResult)
    {
        attributeParseResult = null;
        StringBuilder tempBuilder = new StringBuilder();
        
        if (!ParseUtils.IsChar(in input, 'l', ref i))
            return false;
        tempBuilder.Append(input[i++]);

        if (!ParseUtils.IsChar(in input, 'a', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'n', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'g', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, '=', ref i))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, '"', ref i))
            return false;
        tempBuilder.Append(input[i++]);

        StringBuilder valueBuilder = new StringBuilder();
        do
        {
            if (input[i] == '>')
                break;
            
            valueBuilder.Append(input[i++]);
        } while (input[i] != '"');
        
        tempBuilder.Append(valueBuilder.ToString());
        
        // Append closing quote.
        tempBuilder.Append(input[i++]);

        // We do not want to append attribute directly into parent StringBuilder to handle abort operations.
        sb.Append(tempBuilder.ToString());

        attributeParseResult = new AttributeParseResult()
        {
            Attribute = new Attribute()
            {
                Type = AttributeType.Lang,
                Value = valueBuilder.ToString()
            },
            Index = i
        };

        return true;
    }
}
