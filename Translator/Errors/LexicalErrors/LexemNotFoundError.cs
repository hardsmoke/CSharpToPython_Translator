namespace Translator.Errors.LexicalErrors
{
    internal class LexemNotFoundError : LexicalError
    {
        public readonly char Lexem;
        public readonly int LexemPosition;

        public LexemNotFoundError(char lexem, int lexemPosition)
        {
            Lexem = lexem;
            LexemPosition = lexemPosition;
        }

        public override string GetErrorDescription()
        {
            return $"Не удается определить лексему {Lexem} в позиции {LexemPosition}";
        }
    }
}
