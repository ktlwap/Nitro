using System.Text;
using Nitro.Parser.Html.Parsing;
using Nitro.Parser.Html.Parsing.Attributes;
using Nitro.Parser.Html.Parsing.Tokens;

namespace Nitro.Parser;

internal static class Tokenizer
{
    private static readonly List<TokenParser> TokenParsers =
    [
        new WhitespaceSequenceTokenParser(),
        
        new OpeningATokenParser(),
        new ClosingATokenParser(),
        new OpeningAbbrTokenParser(),
        new ClosingAbbrTokenParser(),
        new OpeningAcronymTokenParser(),
        new ClosingAcronymTokenParser(),
        new OpeningAddressTokenParser(),
        new ClosingAddressTokenParser(),
        new OpeningAreaTokenParser(),
        new ClosingAreaTokenParser(),
        new OpeningArticleTokenParser(),
        new ClosingArticleTokenParser(),
        new OpeningAsideTokenParser(),
        new ClosingAsideTokenParser(),
        new OpeningAudioTokenParser(),
        new ClosingAudioTokenParser(),
        new OpeningBTokenParser(),
        new ClosingBTokenParser(),
        new OpeningBaseTokenParser(),
        new ClosingBaseTokenParser(),
        
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
    
    private static readonly List<AttributeParser> AttributeParsers =
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
            
            foreach (TokenParser parser in TokenParsers)
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

            foreach (AttributeParser parser in AttributeParsers)
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
                // TODO: Find a way to handle invalid HTML.
                Console.WriteLine($"No parser found for input at index {index}.");
            }
        }

        throw new Exception("Invalid state reached.");
    }
}
