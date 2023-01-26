namespace Translator.Errors.SyntaxErrors
{
    internal class UnexpectedNonterminalError : SyntaxError
    {
        public readonly string Symbol;

        public UnexpectedNonterminalError(string symbol)
        {
            Symbol = symbol;
        }

        public override string GetErrorDescription()
        {
            return $"A non-terminal \"{Symbol}\" was found in the entered input";
        }
    }
}
