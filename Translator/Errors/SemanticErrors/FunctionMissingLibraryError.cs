namespace Translator.Errors.SemanticErrors
{
    public class FunctionMissingLibraryError : SemanticError
    {
        public readonly string Identificator;

        public FunctionMissingLibraryError(string identificator, string message = default) : base(message)
        {
            Identificator = identificator;
        }

        public override string GetErrorDescription()
        {
            return $"Function \"{Identificator}\" Not Found. Try \"using System;\"";
        }
    }
}
