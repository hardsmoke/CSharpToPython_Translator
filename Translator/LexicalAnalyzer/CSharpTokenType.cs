namespace Translator.LexicalAnalyzer
{
    public class CSharpTokenType : TokenType
    {
        public CSharpTokenType(string type, string regexPattern) : base(type, regexPattern)
        {
        }
    }
}