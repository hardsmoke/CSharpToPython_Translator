using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public abstract class IncorrectDataTypeError : SemanticError
    {
        public readonly string SourceIdentificator;
        public readonly DataType CorrectDataType;
        public readonly DataType IncorrectDataType;

        public IncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string message = default) : base(message)
        {
            CorrectDataType = correctDataType;
            IncorrectDataType = incorrectDataType;
        }

        public IncorrectDataTypeError(DataType correctDataType, DataType incorrectDataType, string identificator, string message = default) : base(message)
        {
            CorrectDataType = correctDataType;
            IncorrectDataType = incorrectDataType;
            SourceIdentificator = identificator;
        }

        public virtual string GetErrorSource()
        {
            return $"(Error Source: {SourceIdentificator})";
        }

        public override string GetErrorDescription()
        {
            return $"Attempt To Use {IncorrectDataType} Instead {CorrectDataType} {(SourceIdentificator == default ? string.Empty : GetErrorSource())}";
        }
    }
}
