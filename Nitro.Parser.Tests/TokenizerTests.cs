namespace Nitro.Parser.Tests;

public class TokenizerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SimpleDocTypeWithHtmlTag()
    {
        string source = "<!DOCTYPE html><html></html>";

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.OpeningHtmlTag));
        Assert.That(result[1].Value, Is.EqualTo("<html>"));
        
        Assert.That(result[2].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[2].Value, Is.EqualTo("</html>"));
    }
    
    [Test]
    public void SimpleDocTypeWithHtmlTagDirectlyClosing()
    {
        string source = "<!DOCTYPE html></html>";

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[1].Value, Is.EqualTo("</html>"));
    }
    
    [Test]
    public void SimpleDocTypeWithHtmlTagWithInnerHtml()
    {
        string source = "<!DOCTYPE html><html>Hello, World!</html>";

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(4));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.OpeningHtmlTag));
        Assert.That(result[1].Value, Is.EqualTo("<html>"));
        
        Assert.That(result[2].Type, Is.EqualTo(TokenType.InnerHtml));
        Assert.That(result[2].Value, Is.EqualTo("Hello, World!"));
        
        Assert.That(result[3].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[3].Value, Is.EqualTo("</html>"));
    }
    
    [Test]
    public void SimpleDocTypeWithHtmlTagWithWhitespacesInTags()
    {
        string source = "<!DOCTYPE html>\n< html>\n</ html>";

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(5));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        Assert.That(result[1].Value, Is.EqualTo("\n"));
        
        Assert.That(result[2].Type, Is.EqualTo(TokenType.OpeningHtmlTag));
        Assert.That(result[2].Value, Is.EqualTo("<html>"));
        
        Assert.That(result[3].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        Assert.That(result[3].Value, Is.EqualTo("\n"));
        
        Assert.That(result[4].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[4].Value, Is.EqualTo("</html>"));
    }
    
    [Test]
    public void SimpleDocTypeWithHtmlTagWithLangAttribute()
    {
        string source = "<!DOCTYPE html>< html lang=\"en\"></ html>";

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(3));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        Assert.That(result[0].Attributes, Has.Count.EqualTo(1));
        Assert.That(result[0].Attributes[0].Type, Is.EqualTo(AttributeType.Html));
        Assert.That(result[0].Attributes[0].Value, Is.Null);
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.OpeningHtmlTag));
        Assert.That(result[1].Value, Is.EqualTo("<html lang=\"en\">"));
        Assert.That(result[1].Attributes, Has.Count.EqualTo(1));
        Assert.That(result[1].Attributes[0].Type, Is.EqualTo(AttributeType.Lang));
        Assert.That(result[1].Attributes[0].Value, Is.EqualTo("en"));
        
        Assert.That(result[2].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[2].Value, Is.EqualTo("</html>"));
    }
    
    [Test]
    public void ComplexHtml()
    {
        string source = """
                        <!DOCTYPE html>
                        <html lang = "en">
                        </head>
                        <body>
                        
                        < /body>
                        </ html>
                        """;

        List<Token> result = Tokenizer.Execute(source.ToArray());
        
        Assert.That(result.Count, Is.EqualTo(11));
        Assert.That(result[0].Type, Is.EqualTo(TokenType.DocTypeTag));
        Assert.That(result[0].Value, Is.EqualTo("<!DOCTYPE html>"));
        
        Assert.That(result[1].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        
        Assert.That(result[2].Type, Is.EqualTo(TokenType.OpeningHtmlTag));
        Assert.That(result[2].Value, Is.EqualTo("<html lang=\"en\">"));
        
        Assert.That(result[3].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        
        Assert.That(result[4].Type, Is.EqualTo(TokenType.ClosingHeadTag));
        Assert.That(result[4].Value, Is.EqualTo("</head>"));
        
        Assert.That(result[5].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        
        Assert.That(result[6].Type, Is.EqualTo(TokenType.OpeningBodyTag));
        Assert.That(result[6].Value, Is.EqualTo("<body>"));
        
        Assert.That(result[7].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        
        Assert.That(result[8].Type, Is.EqualTo(TokenType.ClosingBodyTag));
        Assert.That(result[8].Value, Is.EqualTo("</body>"));
        
        Assert.That(result[9].Type, Is.EqualTo(TokenType.WhitespaceSequence));
        
        Assert.That(result[10].Type, Is.EqualTo(TokenType.ClosingHtmlTag));
        Assert.That(result[10].Value, Is.EqualTo("</html>"));
    }
}