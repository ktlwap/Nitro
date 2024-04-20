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
}