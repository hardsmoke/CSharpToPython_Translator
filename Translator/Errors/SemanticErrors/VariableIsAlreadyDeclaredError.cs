namespace Translator.Errors.SemanticErrors
{
    public class VariableIsAlreadyDeclaredError : AlreadyDeclaredError
    {
        public VariableIsAlreadyDeclaredError(string identificator, string message = null) : base(identificator, message)
        {
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Declare Against a Variable \"{Identificator}\"";
        }
    }
}
