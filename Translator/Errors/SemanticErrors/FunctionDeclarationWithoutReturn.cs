using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class FunctionDeclarationWithoutReturn : SemanticError
    {
        public readonly string Identificator;
        public readonly DataType DataType;

        public FunctionDeclarationWithoutReturn(string identificator, DataType dataType, string message = default) : base(message)
        {
            Identificator = identificator;
            DataType = dataType;
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Declare Function \"{Identificator}\" With \"{DataType}\" DataType Without \"Return\" Instruction";
        }
    }
}
