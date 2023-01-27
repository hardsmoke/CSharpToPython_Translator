using System.Text.RegularExpressions;
using Translator.LexicalAnalyzer;

namespace Translator.Generator
{
    public class Generator
    {
        public readonly List<Token> Tokens;
        public readonly Conversions Conversions;

        public Generator(List<Token> tokens, Conversions conversions)
        {
            Tokens = tokens;
            Conversions = conversions;
        }

        public List<string> Generate()
        {
            List<string> output = new List<string>();

            int depth = 0;

            string outputLine = string.Empty;

            List<string> typesNeedSpaceAfter = new List<string>() { "TAB", "SPACE", "LINEFEED" };

            for (int i = 0; i < Tokens.Count; i++)
            {
                Conversion<string> converted = Conversions.GetConversion(Tokens[i].Text);

                if (Tokens[i].Type.Type == "SPACE")
                {
                    List<string> unsignedChars = new List<string> { " ", "\r", "\n", "\t" };
                    string outputLineCopy = outputLine;

                    foreach (var item in unsignedChars)
                    {
                        outputLineCopy = outputLineCopy.Replace(item, "");
                    }

                    if (outputLineCopy.Length == 0)
                    {
                        continue;
                    }

                    if (i > 0 && Tokens[i - 1].Type.Type == "DATATYPE")
                    {
                        continue;
                    }
                }


                if (i > 1 && Tokens[i - 2].Text == "using")
                {
                    continue;
                }

                if (converted != null)
                {
                    outputLine += converted.Converted;
                }
                else
                {
                    outputLine += Tokens[i].Text;
                }

                HandleToken(Tokens, i, output, ref outputLine, ref depth);
            }

            output.Add(outputLine.Trim(' '));

            for (int i = 0; i < output.Count; i++)
            {
                output[i] = output[i].TrimStart(new char[] { '\r', '\n' });
                output[i] = Regex.Replace(output[i], " {2,}", " ");
            }

            output.RemoveAll(string.IsNullOrWhiteSpace);

            return output;
        }

        private void HandleToken(List<Token> tokens, int tokenId, List<string> output, ref string outputLine, ref int depth)
        {
            switch (tokens[tokenId].Text)
            {
                case ";":
                    AddNewCodeLine(output, ref outputLine, depth);
                    break;
                case "{":
                    depth++;
                    AddNewCodeLine(output, ref outputLine, depth);
                    List<char> unsignedChars = new List<char> { ' ', '\r', '\n', '\t' };
                    int previousSignedCharPosition = GetPreviousSignedCharPosition(outputLine, outputLine.Length - 1, unsignedChars);
                    if (previousSignedCharPosition == -1)
                    {
                        previousSignedCharPosition = GetPreviousSignedCharPosition(output[^1], output[^1].Length - 1, unsignedChars);
                        output[^1] = output[^1].Insert(previousSignedCharPosition + 1, ":\r\n");
                    }
                    else
                    {
                        outputLine = outputLine.Insert(previousSignedCharPosition + 1, ":\r\n");
                    }
                    break;
                case "}":
                    depth--;
                    AddNewCodeLine(output, ref outputLine, depth);
                    break;
                case "if":
                    if (tokenId > 1 && tokens[tokenId - 2].Text == "else")
                    {
                        outputLine = outputLine.Replace("if", "");
                        outputLine = outputLine.Replace("else", "elif");
                    }
                    break;
                default:
                    break;
            }
        }

        private void AddNewCodeLine(List<string> output, ref string outputLine, int depth)
        {
            if (outputLine != string.Empty && outputLine != "\r\n")
            {
                outputLine = outputLine.Trim(' ');
                output.Add(outputLine);
                outputLine = string.Empty;
                for (int i = 0; i < depth; i++)
                {
                    outputLine += '\t';
                }
            }
        }

        private int GetPreviousSignedCharPosition(string str, int currentPosition, List<char> unsignedChars)
        {
            int position = -1;

            for (int i = currentPosition; i >= 0; i--)
            {
                if (!unsignedChars.Contains(str[i]))
                {
                    position = i;
                    Console.WriteLine($"pos: {position} | char: \'{str[i]}\'");
                    break;
                }
            }

            return position;
        }

        public string GetGeneratedString(List<string> generated)
        {
            string generatedString = string.Empty;

            foreach (var str in generated)
            {
                generatedString += str;
            }

            return generatedString;
        }
    }
}
