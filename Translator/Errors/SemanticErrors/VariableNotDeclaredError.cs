namespace Translator.Errors.SemanticErrors
{
    public class VariableNotDeclaredError : NotDeclaredError
    {
        public VariableNotDeclaredError(string identificator, string message = null) : base(identificator, message)
        {
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Use Not Declared Variable \"{Identificator}\"";
        }
    }
}
