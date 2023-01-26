using Translator.Errors.SemanticErrors;

namespace Translator.Errors.SemanticErrors
{
    public class VariableNotInitializedError : SemanticError
    {
        public readonly string Identificator;

        public VariableNotInitializedError(string identificator, string message = default) : base(message)
        {
            Identificator = identificator;
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Use Not Initialized Variable \"{Identificator}\"";
        }
    }
}
