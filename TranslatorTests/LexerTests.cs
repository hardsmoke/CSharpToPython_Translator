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
        public void Keyword_ParseKeyword_ShouldReturnListOnlyOfKeywordsOrSpaces()
        {
            string input = "if else true false return while for foreach do";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("KEYWORD" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void Datatype_ParseDatatype_ShouldReturnListOnlyOfDatatypesOrSpaces()
        {
            string input = "int float bool void string char";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("DATATYPE" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void Id_ParseIdentificators_ShouldReturnListOnlyOfIdentificatorsOrSpaces()
        {
            string input = "something foo func myVar Hello World_";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("ID" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void RealNumber_ParseRealNumbers_ShouldReturnListOnlyOfRealNumbersOrSpaces()
        {
            string input = "3.14 3.213 7.555435 53515.3";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("REAL_NUMBER" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void Number_ParseNumbers_ShouldReturnListOnlyOfNumbersOrSpaces()
        {
            string input = "3 33 7515";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("NUMBER" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void String_ParseStrings_ShouldReturnListOnlyOfStringsOrSpaces()
        {
            string input = "\"some string\" \"and more\" \"and...\"";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("STRING" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void Operation_ParseOperations_ShouldReturnListOnlyOfOperationsOrSpaces()
        {
            string input = "+ - * / . = ( ) { } < >";
            List<Token> tokens = GetTokens(input);

            foreach (var token in tokens)
            {
                Assert.True("OPERATION" == token.Type.Type || "SPACE" == token.Type.Type);
            }
        }

        [Fact]
        public void EndOfInstruction_ParseEndOfInstructionSymbol_ShouldReturnListOnlyOfEndOfInstructionSymbol()
        {
            string input = ";";
            List<Token> tokens = GetTokens(input);

            Assert.Single(tokens);
            Assert.Equal("END_OF_INSTRUCTION", tokens[0].Type.Type);
        }
    }
}
