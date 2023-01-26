namespace Translator.LexicalAnalyzer
{
    public class Token
    {
        private string _text;
        public string Text => _text;

        private TokenType _type;
        public TokenType Type => _type;

        private int _startIndex;
        public int StartIndex => _startIndex;

        public Token(string text, TokenType type, int start)
        {
            _text = text;
            _type = type;
            _startIndex = start;
        }

    }
}