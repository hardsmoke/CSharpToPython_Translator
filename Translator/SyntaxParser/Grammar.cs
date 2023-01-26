using Translator.Collections;

namespace Translator.SyntaxParser
{
    public class Grammar
    {
        public readonly List<string> Terms;
        public readonly List<string> Nonterms;
        public readonly Rules Rules;

        public Grammar(List<string> terms, List<string> nonterms, Rules rules)
        {
            if (rules.IsCorrect(terms, nonterms, true))
            {
                Terms = terms;
                Nonterms = nonterms;
                Rules = rules;
            }
        }

        public bool IsTerminal(string symbol)
        {
            return Terms.Contains(symbol);
        }

        public bool IsNonTerminal(string symbol)
        {
            return Nonterms.Contains(symbol);
        }

        public List<string> GetTermsStartsWith(Node node)
        {
            List<string> terms = new List<string>();

            node.Traverse((traversedNode) =>
            {
                string data = traversedNode.Data;
                if (IsTerminal(data))
                {
                    terms.Add(data);
                }
            });

            return terms;
        }
    }
}
