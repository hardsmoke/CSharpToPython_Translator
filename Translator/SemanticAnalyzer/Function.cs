using System.Linq;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Translator.SemanticAnalyzer
{
    public class Function : VariableBase
    {
        public readonly List<Variable> Parameters;

        public Function(DataType dataType, string identificator, List<Variable> parameters) : base(dataType, identificator)
        {
            Parameters = parameters;
        }

        public static bool IsParametersEquals(List<Variable> parametersA, List<Variable> parametersB)
        {
            foreach (var paramA in parametersA)
            {
                if (parametersB.FindIndex(paramB => paramA == paramB) < 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator ==(Function a, Function b)
        {
            if (a is null ^ b is null)
            {
                return false;
            }
            else if (a is null && b is null)
            {
                return true;
            }

            bool isIdentificatorsEquals = a?.Identificator == b?.Identificator;
            //bool isDataTypesEquals = a?.DataType == b?.DataType;

            return isIdentificatorsEquals && IsParametersEquals(a.Parameters, b.Parameters);
        }

        public static bool operator !=(Function a, Function b) => !(a == b);

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public string GetParametersString()
        {
            string result = "";

            foreach (var param in Parameters)
            {
                result += param + ", ";
            }

            return result.Count() > 0 ? result.Substring(0, result.Length - 2) : result;
        }

        public override string ToString()
        {
            return $"{(DataType == DataType.NONE ? string.Empty : DataType.ToString() + " ")}{Identificator} ({GetParametersString()})";
        }
    }
}
