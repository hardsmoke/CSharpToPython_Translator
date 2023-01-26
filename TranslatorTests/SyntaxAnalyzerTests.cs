using Translator.SyntaxParser;

namespace TranslatorTests
{
    public class SyntaxAnalyzerTests
    {
        [Fact]
        public void VariableDeclaration_DeclaringVariableWithoutInitialization_ShouldNotReturnAnyErrors()
        {
            string input = "int a;";
            SyntaxParser syntaxParser = Translator.Program.GetSyntaxParser(input);

            int actual = syntaxParser.Errors.Count;
            Assert.Equal(0, actual);
        }

        [Fact]
        public void VariableDeclaration_TryToDeclareVariableWithoutEndOfLineSymbol_ShouldReturnError()
        {
            string input = "int a";
            SyntaxParser syntaxParser = Translator.Program.GetSyntaxParser(input);

            int actual = syntaxParser.Errors.Count;
            Assert.Equal(1, actual);
        }

        [Fact]
        public void VariableDeclaration_DeclareVariableWithMonsterShitExpression_ShouldntReturnSyntaxError()
        {
            string input = ";;;int a = b - foo(c - 52.5, testfunc(10, \"abc\"));";
            SyntaxParser syntaxParser = Translator.Program.GetSyntaxParser(input);

            int actual = syntaxParser.Errors.Count;
            Assert.Equal(0, actual);
        }
    }
}
