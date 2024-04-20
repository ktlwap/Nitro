using System.Runtime.CompilerServices;
using System.Text;

namespace Nitro.Parser;

internal static class Tokenizer
{
    internal static List<Token> Execute(char[] input)
    {
        List<Token> tokens = new List<Token>();
        
        int i = 0;
        while (i < input.Length - 1)
        {
            if (IsAnyWhitespaceSequence(input, i) is (Token anyWhitespaceSeqToken, int anyWhitespaceSeqNewIndex))
            {
                tokens.Add(anyWhitespaceSeqToken);
                i = anyWhitespaceSeqNewIndex;
            }
            else if (ParseDocTypeTag(input, i) is (Token docTypeTagToken, int docTypeTagNewIndex))
            {
                tokens.Add(docTypeTagToken);
                i = docTypeTagNewIndex;
            }
            else if (ParseOpeningHtmlTag(input, i) is (Token openingHtmlTagToken, int openingHtmlTagNewIndex))
            {
                tokens.Add(openingHtmlTagToken);
                i = openingHtmlTagNewIndex;
            }
            else if (ParseClosingHtmlTag(input, i) is (Token closingHtmlTagToken, int closingHtmlTagNewIndex))
            {
                tokens.Add(closingHtmlTagToken);
                i = closingHtmlTagNewIndex;
            }
            else if (ParseOpeningHeadTag(input, i) is (Token openingHeadTagToken, int openingHeadTagNewIndex))
            {
                tokens.Add(openingHeadTagToken);
                i = openingHeadTagNewIndex;
            }
            else if (ParseClosingHeadTag(input, i) is (Token closingHeadTagToken, int closingHeadTagNewIndex))
            {
                tokens.Add(closingHeadTagToken);
                i = closingHeadTagNewIndex;
            }
            else if (ParseOpeningBodyTag(input, i) is (Token openingBodyTagToken, int openingBodyTagNewIndex))
            {
                tokens.Add(openingBodyTagToken);
                i = openingBodyTagNewIndex;
            }
            else if (ParseClosingBodyTag(input, i) is (Token closingBodyTagToken, int closingBodyTagNewIndex))
            {
                tokens.Add(closingBodyTagToken);
                i = closingBodyTagNewIndex;
            }
            else if (ParseInnerHtml(input, i) is (Token innerHtmlToken, int innerHtmlNewIndex))
            {
                tokens.Add(innerHtmlToken);
                i = innerHtmlNewIndex;
            }
            else
            {
                throw new Exception("Reached invalid state.");
            }
        }

        return tokens;
    }

    private static (Token, int)? IsAnyWhitespaceSequence(char[] input, int i)
    {
        if (!char.IsWhiteSpace(input[i]))
            return null;

        StringBuilder sb = new StringBuilder();
        while (i < input.Length)
        {
            char c = input[i];
            if (!char.IsWhiteSpace(c))
                break;
            
            sb.Append(c);
            ++i;
        }
        
        return (
            new Token()
            {
                Type = TokenType.WhitespaceSequence,
                Value = sb.ToString(),
                Attributes = null,
            }, i
        );
    }

    private static (Token, int)? ParseDocTypeTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();
        
        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, '!', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'd', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'o', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'c', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 't', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'y', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'p', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'e', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.DocTypeTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }

    private static (Token, int)? ParseOpeningHtmlTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'h', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 't', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'm', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'l', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.OpeningHtmlTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }
    
    private static (Token, int)? ParseClosingHtmlTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, '/', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'h', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 't', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'm', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'l', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.ClosingHtmlTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }
    
    private static (Token, int)? ParseOpeningHeadTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'h', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'e', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'a', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'd', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.OpeningHeadTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }
    
    private static (Token, int)? ParseClosingHeadTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, '/', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'h', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'e', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'a', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'd', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.ClosingHeadTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }
    
    private static (Token, int)? ParseOpeningBodyTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'b', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'o', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'd', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'y', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.OpeningBodyTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }
    
    private static (Token, int)? ParseClosingBodyTag(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();

        if (!IsChar(in input, '<', ref i))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, '/', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'b', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'o', ref i, false))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'd', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'y', ref i, false))
            return null;
        sb.Append(input[i++]);

        (List<Attribute>, int) result = ParseAttributes(sb, input, i);
        
        return (
            new Token()
            {
                Type = TokenType.ClosingBodyTag,
                Value = sb.ToString(),
                Attributes = result.Item1,
            }, result.Item2
        );
    }

    private static (Token, int)? ParseInnerHtml(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();
        
        while (i < input.Length - 1 && input[i] != '<' && input[i] != '>')
        {
            sb.Append(input[i]);
            ++i;
        }
        
        if (sb.Length == 0)
            return null;
        
        return (
            new Token()
            {
                Type = TokenType.InnerHtml,
                Value = sb.ToString(),
                Attributes = null,
            }, i
        );
    }

    #region ATTRIBUTES_PARSING
    private static (List<Attribute>, int) ParseAttributes(StringBuilder sb, char[] input, int i)
    {
        // Return directly in case of no additional attributes available.
        if (IsChar(in input, '>', ref i))
        {
            sb.Append(input[i]);
            return ([], ++i);
        }
        
        // Append blank space before attributes start.
        sb.Append(' ');
        
        List<Attribute> attributes = new List<Attribute>();
        while (i < input.Length)
        {
            if (IsChar(in input, '>', ref i))
            {
                sb.Append(input[i]);
                return (attributes, ++i);
            }
            else if (ParseHtmlAttribute(sb, input, i) is (Attribute htmlAttribute, int htmlNewIndex))
            {
                attributes.Add(htmlAttribute);
                i = htmlNewIndex;
            }
            else if (ParseLangAttribute(sb, input, i) is (Attribute langAttribute, int langNewIndex))
            {
                attributes.Add(langAttribute);
                i = langNewIndex;
            }
            else
            {
                throw new Exception();
            }
        }

        throw new Exception();
    }
    
    private static (Attribute, int)? ParseHtmlAttribute(StringBuilder sb, char[] input, int i)
    {
        if (!IsChar(in input, 'h', ref i))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 't', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'm', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'l', ref i, false))
            return null;
        sb.Append(input[i++]);

        // Check if not a different html attribute is meant.
        if (IsChar(in input, '=', ref i))
            return null;
        
        return (
            new Attribute()
            {
                Type = AttributeType.Html,
                Value = null,
            }, i
        );
    }

    private static (Attribute, int)? ParseLangAttribute(StringBuilder sb, char[] input, int i)
    {
        if (!IsChar(in input, 'l', ref i))
            return null;
        sb.Append(input[i++]);

        if (!IsChar(in input, 'a', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'n', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, 'g', ref i, false))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, '=', ref i))
            return null;
        sb.Append(input[i++]);
        
        if (!IsChar(in input, '"', ref i))
            return null;
        sb.Append(input[i++]);

        StringBuilder attrValueBuilder = new StringBuilder();
        do
        {
            if (input[i] == '>')
                break;
            
            attrValueBuilder.Append(input[i++]);
        } while (input[i] != '"');
        
        sb.Append(attrValueBuilder.ToString());
        
        // Append closing quote.
        sb.Append(input[i++]);

        return (
            new Attribute()
            {
                Type = AttributeType.Lang,
                Value = attrValueBuilder.ToString()
            }, i
        );
    }
    #endregion

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    private static bool IsChar(in char[] input, char expected, ref int index, bool skipWhitespace = true)
    {
        char c = input[index];

        if (skipWhitespace && char.IsWhiteSpace(c))
        {
            ++index;
            return IsChar(in input, expected, ref index);
        }

        return char.ToLower(c) == char.ToLower(expected);
    }
}