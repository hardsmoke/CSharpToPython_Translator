namespace Translator.LexicalAnalyzer
{
    public abstract class TokenType
    {
        private string _type;
        private string _regexPattern;

        public string Type => _type;
        public string RegexPattern => _regexPattern;

        public TokenType(string type, string regexPattern)
        {
            _type = type;
            _regexPattern = regexPattern;
        }
    }
}
