using System.Text;

namespace Nitro.Parser.Html.Parsing.Tokens;

internal class InnerHtmlTokenParser : TokenParser
{
    internal override bool Parse(char[] input, int i, out TokenParseResult? result)
    {
        result = null;
        
        StringBuilder sb = new StringBuilder();
        while (i < input.Length - 1 && input[i] != '<' && input[i] != '>')
        {
            sb.Append(input[i]);
            ++i;
        }
        
        if (sb.Length == 0)
            return false;
        
        result = new TokenParseResult()
        {
            Token = new Token()
            {
                Type = TokenType.InnerHtml,
                Value = sb.ToString(),
                Attributes = []
            },
            Index = i
        };
        
        return true;
    }
}
