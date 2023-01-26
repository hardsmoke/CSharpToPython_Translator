namespace Translator.Errors.SemanticErrors
{
    public abstract class SemanticError : Error
    {
        public SemanticError(string message) : base(message)
        {
        }
    }
}
