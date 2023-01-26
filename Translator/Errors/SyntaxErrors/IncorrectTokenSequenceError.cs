namespace Translator.Errors.SyntaxErrors
{
    internal class IncorrectTokenSequenceError : SyntaxError
    {
        public readonly string Symbol;

        public IncorrectTokenSequenceError(string symbol)
        {
            Symbol = symbol;
        }

        public override string GetErrorDescription()
        {
            return $"Incorrect token sequence entered! Error near symbol: ({Symbol})";
        }
    }
}
