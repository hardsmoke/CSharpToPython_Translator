using System.Collections.Generic;

namespace Translator.SyntaxParser
{
    public class Rule
    {
        public readonly string LeftPart;
        public readonly List<string> RightPart;

        public Rule(string leftPart, List<string> rightPart)
        {
            LeftPart = leftPart;
            RightPart = rightPart;
        }

        public override string ToString()
        {
            string resultString = base.ToString() + $": <{LeftPart}> -> ";
            foreach (string symbol in RightPart)
            {
                resultString += $"<{symbol}> ";
            }
            return resultString;
        }

        public bool IsCorrect(List<string> terms, List<string> nonterms)
        {
            if (!terms.Contains(LeftPart) && !nonterms.Contains(LeftPart))
            {
                return false;
            }

            foreach (string str in RightPart)
            {
                if (!terms.Contains(str) && !nonterms.Contains(str))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
