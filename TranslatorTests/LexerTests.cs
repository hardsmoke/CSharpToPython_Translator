using Translator;
using Translator.Errors;
using Translator.LexicalAnalyzer;

namespace TranslatorTests
{
    public class LexerTests
    {
        private List<Token> GetTokens(string input)
        {
            return Program.GetTokensFromLexer(input);
        }

        [Fact]
        public void Keyword_ParseKeyword_ShouldReturnListOnlyOfKeywords()
        {
            string input = "ifelsetruefalsereturnwhileforforeachdo";
            List<Token> tokens = GetTokens(input);

            //Assert.Collection(tokens, token => Assert.Equal(new TokenType(), token.Type));
        }
    }
}
