using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class FunctionNotDeclaredError : NotDeclaredError
    {
        public readonly List<DataType> ParameterDataTypes;

        public FunctionNotDeclaredError(string identificator, List<DataType> parameterDataTypes, string message = null) : base(identificator, message)
        {
            ParameterDataTypes = parameterDataTypes;
        }

        public FunctionNotDeclaredError(string identificator, string message = null) : base(identificator, message)
        {
            ParameterDataTypes = new List<DataType>();
        }

        public override string GetErrorDescription()
        {
            string result = $"Attempt To Use Not Declared Function \"{Identificator}\"";
            if (ParameterDataTypes.Count > 0)
            {
                string ParameterDataTypesString = string.Empty;
                for (int i = 0; i < ParameterDataTypes.Count - 1; i++)
                {
                    ParameterDataTypesString += ParameterDataTypes[i] + ", ";
                }
                ParameterDataTypesString += ParameterDataTypes.Last();
                result += $" with parameters data types {{{ParameterDataTypesString}}}";
            }

            return result;
        }
    }
}
