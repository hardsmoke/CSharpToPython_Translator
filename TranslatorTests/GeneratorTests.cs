using Translator.LexicalAnalyzer;
using Translator;
using Translator.Generator;

namespace TranslatorTests
{
    public class GeneratorTests
    {
        private string GetPythonCode(string input)
        {
            List<Token> tokens = Program.GetTokensFromLexer(input);
            string conversionTableFilename = "conversion_table.txt";
            List<string> conversionsRaw = Program.GetConversionsRaw(conversionTableFilename);
            Conversions conversions = new Conversions(conversionsRaw, '*', "-");
            Generator generator = new Generator(tokens, conversions);
            List<string> outputList = generator.Generate();
            return generator.GetGeneratedString(outputList);
        }

        [Fact]
        public void Generator_Test1_ShouldReturnCodeOnPython()
        {
            string input = "int b = 4; if (4 > 2) { float a = 6.1 * b; } else { float c = 3.14; }";
            string output = GetPythonCode(input);

            Assert.Equal("b = 4\r\nif (4 > 2):\r\n\ta = 6.1 * b\r\nelse:\r\n\tc = 3.14\r\n", output);
        }

        [Fact]
        public void Generator_Test2_ShouldReturnCodeOnPython()
        {
            string input = "int b = 4; while (true == false) { b++; }";
            string output = GetPythonCode(input);

            Assert.Equal("b = 4\r\nwhile (True == False):\r\n\tb++\r\n", output);
        }

        [Fact]
        public void Generator_Test3_ShouldReturnCodeOnPython()
        {
            string input = "int foo(int b, float c) { if (b > c) { return b - 1; } else { return b + 1; } } Console.WriteLine(foo(1, 3.14));";
            string output = GetPythonCode(input);

            Assert.Equal("foo(b, c):\r\n\tif (b > c):\r\n\t\treturn b - 1\r\n\telse:\r\n\t\treturn b + 1\r\nprint(foo(1, 3.14))\r\n", output);
        }
    }
}
