using System.Text;

namespace Nitro.Parser.Html.Parsing;

internal abstract class AttributeParser
{
    internal abstract bool Parse(StringBuilder sb, ref char[] input, int i, out AttributeParseResult? result);
}
