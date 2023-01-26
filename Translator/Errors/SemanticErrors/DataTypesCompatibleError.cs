using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class DataTypesCompatibleError : SemanticError
    {
        public readonly List<DataType> DataTypes;

        public DataTypesCompatibleError(List<DataType> dataTypes, string message = default) : base(message)
        {
            DataTypes = dataTypes;
        }

        public override string GetErrorDescription()
        {
            string dataTypesStr = string.Empty;
            for (int i = 0; i < DataTypes.Count - 1; i++)
            {
                dataTypesStr += DataTypes[i] + ", ";
            }
            dataTypesStr += DataTypes.Last();

            return $"Data types are incompatible ({dataTypesStr})";
        }
    }
}

