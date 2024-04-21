using System.Text;
using Nitro.Parser.Html.Parsing;
using Nitro.Parser.Html.Parsing.Attributes;
using Nitro.Parser.Html.Parsing.Tokens;

namespace Nitro.Parser;

internal static class Tokenizer
{
    private static List<TokenParser> _tokenParsers =
    [
        new WhitespaceSequenceTokenParser(),
        new DocTypeTokenParser(),
        new OpeningHtmlTokenParser(),
        new ClosingHtmlTokenParser(),
        new OpeningHeadTokenParser(),
        new ClosingHeadTokenParser(),
        new OpeningBodyTokenParser(),
        new ClosingBodyTokenParser(),
        new OpeningDivTokenParser(),
        new ClosingDivTokenParser(),
        new OpeningButtonTokenParser(),
        new ClosingButtonTokenParser(),
        new InnerHtmlTokenParser()
    ];
    
    private static List<AttributeParser> _attributeParsers =
    [
        new HtmlAttributeParser(),
        new LangAttributeParser()
    ];
    
    internal static List<Token> Parse(char[] input)
    {
        List<Token> tokens = new List<Token>();
        
        int index = 0;
        while (index < input.Length - 1)
        {
            bool parserFound = false;
            
            foreach (TokenParser parser in _tokenParsers)
            {
                if (parser.Parse(input, index, out TokenParseResult? result) && result is not null)
                {
                    index = result.Index;
                    tokens.Add(result.Token);
                    
                    parserFound = true;
                    break;
                }
            }
            
            if (!parserFound)
            {
                Console.WriteLine($"No parser found for input at index {index}.");
            }
        }

        return tokens;
    }
    
    internal static List<Attribute> ParseOptionalAttributes(StringBuilder sb, char[] input, ref int index)
    {
        // Return directly in case of no additional attributes available.
        if (ParseUtils.IsChar(in input, '>', ref index))
        {
            sb.Append(input[index++]);
            return [];
        }
        
        // Append blank space before attributes start.
        sb.Append(' ');
        
        List<Attribute> attributes = new List<Attribute>();
        while (index < input.Length)
        {
            if (ParseUtils.IsChar(in input, '>', ref index))
            {
                sb.Append(input[index++]);
                return attributes;
            }
            
            bool parserFound = false;

            foreach (AttributeParser parser in _attributeParsers)
            {
                if (parser.Parse(sb, ref input, index, out AttributeParseResult? result) && result is not null)
                {
                    index = result.Index;
                    attributes.Add(result.Attribute);
                    
                    parserFound = true;
                    break;
                }
            }
            
            if (!parserFound)
            {
                Console.WriteLine($"No parser found for input at index {index}.");
            }
        }

        throw new Exception("Invalid state reached.");
    }
}
