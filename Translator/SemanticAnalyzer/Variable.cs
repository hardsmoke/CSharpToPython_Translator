namespace Translator.SemanticAnalyzer
{
    public class Variable : VariableBase
    {
        public bool HasValue = false;

        public Variable(DataType dataType, string identificator, bool hasValue = false) : base(dataType, identificator)
        {
            if (dataType == DataType.VOID)
            {
                throw new System.Exception($"INVALID DATATYPE: {dataType} {identificator}");
            }
            HasValue = hasValue;
        }

        public static bool operator ==(Variable a, Variable b)
        {
            bool isIdentificatorsEquals = a?.Identificator == b?.Identificator;
            bool isDataTypesEquals = a?.DataType == b?.DataType;

            return isIdentificatorsEquals && isDataTypesEquals;
        }

        public static bool operator !=(Variable a, Variable b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"{(DataType == DataType.NONE ? string.Empty : DataType.ToString() + " ")}{Identificator}";
        }
    }
}
