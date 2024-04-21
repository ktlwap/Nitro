using System.Text;

namespace Nitro.Parser.Html.Parsing.Attributes;

internal class HtmlAttributeParser : AttributeParser
{
    internal override bool Parse(StringBuilder sb, ref char[] input, int i, out AttributeParseResult? result)
    {
        result = null;
        StringBuilder tempBuilder = new StringBuilder();
        
        if (!ParseUtils.IsChar(in input, 'h', ref i))
            return false;
        tempBuilder.Append(input[i++]);

        if (!ParseUtils.IsChar(in input, 't', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'm', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);
        
        if (!ParseUtils.IsChar(in input, 'l', ref i, false))
            return false;
        tempBuilder.Append(input[i++]);

        // Check if not a different html attribute is meant.
        if (ParseUtils.IsChar(in input, '=', ref i))
            return false;

        sb.Append(tempBuilder.ToString());
        
        result = new AttributeParseResult()
        {
            Attribute = new Attribute()
            {
                Type = AttributeType.Html,
                Value = null
            },
            Index = i
        };

        return true;
    }
}
