using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class FunctionIncorrectDataTypeError : IncorrectDataTypeError
    {
        public FunctionIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string message = null) : base(correctDataType, incorrectDataType, message)
        {
        }

        public FunctionIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string identificator, string message = null) : base(correctDataType, incorrectDataType, identificator, message)
        {
        }

        public override string GetErrorSource()
        {
            return $"(Function: {SourceIdentificator})";
        }
    }
}
