namespace Translator.Errors.SemanticErrors
{
    public class AlreadyDeclaredError : SemanticError
    {
        public readonly string Identificator;

        public AlreadyDeclaredError(string identificator, string message = default) : base(message)
        {
            Identificator = identificator;
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Declare Against The Identificator \"{Identificator}\"";
        }
    }
}
