namespace Translator.SemanticAnalyzer
{
    public abstract class VariableBase
    {
        public readonly DataType DataType;
        public readonly string Identificator;

        public VariableBase(DataType dataType, string identificator)
        {
            DataType = dataType;
            Identificator = identificator;
        }
    }

    public enum DataType
    {
        NONE,
        VOID,
        CHAR,
        STRING,
        INTEGER,
        FLOAT,
        BOOL,
    }
}
