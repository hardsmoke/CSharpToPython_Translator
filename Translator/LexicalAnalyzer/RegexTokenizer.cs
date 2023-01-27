using System.Collections;
using System.Text.RegularExpressions;
using Translator.Errors;
using Translator.Errors.LexicalErrors;

namespace Translator.LexicalAnalyzer
{
    public class RegexTokenizer : IEnumerator<Token>
    {
        private string _input;

        private TokenType[] _tokenTypes;

        private Match _currentMatch;

        private int _currentPosition = 0;

        private Token _currentToken;
        public Token Current => _currentToken;

        private List<Token> _tokens = new List<Token>();
        public List<Token> Tokens => _tokens;

        private List<Error> _errors = new List<Error>();
        public List<Error> Errors => _errors;

        public RegexTokenizer(string content, TokenType[] tokenTypes)
        {
            _input = content;
            _tokenTypes = tokenTypes;

            string regexPattern = GetPattern(tokenTypes);

            Tokenize(regexPattern);
        }

        public string GetPattern(TokenType[] tokenTypes)
        {
            List<string> regexList = new List<string>();
            for (int i = 0; i < tokenTypes.Length; i++)
            {
                TokenType tokenType = tokenTypes[i];
                regexList.Add("(?<g" + i + ">" + tokenType.RegexPattern + ")");
            }

            string regexPattern = "";
            foreach (string reg in regexList)
            {
                regexPattern += reg + "|";
            }

            regexPattern = regexPattern.Remove(regexPattern.Length - 1);

            return regexPattern;
        }

        public void Tokenize(string pattern)
        {
            _errors = new List<Error>();
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

            if (_input == null)
            {
                _errors.Add(new DetectedEmptyInputError());
            }
            else
            {
                _currentMatch = regex.Match(_input);
                _currentToken = GetToken(_currentMatch);

                while (HasMoreElements())
                {
                    if (_currentToken == null)
                    {
                        break;
                    }

                    MoveNext();
                }
            }
        }

        public Token GetToken(Match match)
        {
            int start = match.Success ? match.Index : _input.Length;
            int end = match.Success ? match.Index + match.Length - 1 : _input.Length;

            if (match.Success && _currentPosition == start)
            {
                _currentPosition = end + 1;
                for (int i = 0; i < _tokenTypes.Length; i++)
                {
                    string si = "g" + i;
                    if (match.Groups[si].Index == start && match.Groups[si].Length + match.Groups[si].Index - 1 == end)
                    {
                        return CreateToken(_input, _tokenTypes[i], start, end);
                    }
                }
            }

            if (_input.Length > 0)
            {
                _errors.Add(new LexemNotFoundError(_input[_currentPosition], _currentPosition));
            }

            return null;
        }

        protected Token CreateToken(string content, TokenType tokenType, int startIndex, int endIndex)
        {
            Token token = new Token(content.Substring(startIndex, endIndex - startIndex + 1), tokenType, startIndex);
            _tokens.Add(token);
            return token;
        }

        public bool HasMoreElements()
        {
            return _currentPosition < _input.Length;
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            _currentMatch = _currentMatch.NextMatch();
            _currentToken = GetToken(_currentMatch);
            return _currentMatch.Success;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
