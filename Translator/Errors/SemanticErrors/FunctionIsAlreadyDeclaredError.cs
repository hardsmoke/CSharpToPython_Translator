using Translator.SemanticAnalyzer;

namespace Translator.Errors.SemanticErrors
{
    public class FunctionIsAlreadyDeclaredError : AlreadyDeclaredError
    {
        public readonly List<Variable> Parameters;

        public FunctionIsAlreadyDeclaredError(string identificator, List<Variable> parameters, string message = null) : base(identificator, message)
        {
            Parameters = parameters is null ? new List<Variable>() : parameters;
        }

        public FunctionIsAlreadyDeclaredError(Function function, string message = null) : base(function.Identificator, message)
        {
            Parameters = function.Parameters;
        }

        public override string GetErrorDescription()
        {
            string parametersString = string.Empty;
            if (Parameters.Any())
            {
                for (int i = 0; i < Parameters.Count - 1; i++)
                {
                    parametersString += Parameters[i] + ", ";
                }
                parametersString += Parameters.Last();
            }

            return $"Attempt To Declare Against a Function \"{Identificator}\" with parameters {{{parametersString}}}";
        }
    }
}
