using Translator.Errors.SemanticErrors;
using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class ReturnIncorrectDataTypeError : IncorrectDataTypeError
    {
        public ReturnIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string message = null) : 
            base(correctDataType, incorrectDataType, message)
        {

        }

        public ReturnIncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string identificator, string message = null) : 
            base(correctDataType, incorrectDataType, identificator, message)
        {
            
        }

        public override string GetErrorSource()
        {
            return $"(Function: {SourceIdentificator})";
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Use {IncorrectDataType} Instead {CorrectDataType} in Return {(SourceIdentificator == default ? string.Empty : GetErrorSource())}";
        }
    }
}
