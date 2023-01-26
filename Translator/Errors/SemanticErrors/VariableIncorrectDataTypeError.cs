using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class VariableIncorrectDataTypeError : IncorrectDataTypeError
    {
        public VariableIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string message = null) : base(correctDataType, incorrectDataType, message)
        {
        }

        public VariableIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string identificator, string message = null) : base(correctDataType, incorrectDataType, identificator, message)
        {
        }

        public override string GetErrorSource()
        {
            return $"(Variable: {SourceIdentificator})";
        }
    }
}
