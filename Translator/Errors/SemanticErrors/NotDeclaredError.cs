namespace Translator.Errors.SemanticErrors
{
    public class NotDeclaredError : SemanticError
    {
        public readonly string Identificator;

        public NotDeclaredError(string identificator, string message = default) : base(message)
        {
            Identificator = identificator;
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Use Not Declared Identificator \"{Identificator}\"";
        }
    }
}
