using System.Runtime.CompilerServices;
using System.Text;

namespace Nitro.Parser;

internal static class Tokenizer
{
    internal static List<Token> Execute(char[] input)
    {
        List<Token> tokens = new List<Token>();
        
        int i = 0;
        while (i < input.Length)
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
                Value = sb.ToString()
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

        while (i < input.Length)
        {
            char c = input[i++];
            sb.Append(c);
            if (c == '>')
                break;
        }
        
        return (
            new Token()
            {
                Type = TokenType.DocTypeTag,
                Value = sb.ToString()
            }, i
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

        while (i < input.Length)
        {
            char c = input[i++];
            sb.Append(c);
            if (c == '>')
                break;
        }

        return (
            new Token()
            {
                Type = TokenType.OpeningHtmlTag,
                Value = sb.ToString()
            }, i
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

        while (i < input.Length)
        {
            char c = input[i++];
            sb.Append(c);
            if (c == '>')
                break;
        }

        return (
            new Token()
            {
                Type = TokenType.ClosingHtmlTag,
                Value = sb.ToString()
            }, i
        );
    }

    private static (Token, int)? ParseInnerHtml(char[] input, int i)
    {
        StringBuilder sb = new StringBuilder();
        
        while (input[i] != '<' && input[i] != '>')
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
                Value = sb.ToString()
            }, i
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool IsChar(in char[] input, char expected, ref int index, bool skipWhitespace = true)
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