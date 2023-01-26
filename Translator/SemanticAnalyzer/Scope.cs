using System;
using System.Collections.Generic;

namespace Translator.SemanticAnalyzer
{
    public class Scope
    {
        private List<Variable> _variables = new List<Variable>();
        private List<Function> _functions = new List<Function>();

        public void Add(Variable variable)
        {
            _variables.Add(variable);
        }

        public void Add(Function function)
        {
            _functions.Add(function);
        }

        public List<Function> FindFunctions(string identificator)
        {
            List<Function> functions = new List<Function>();
            foreach (var func in _functions)
            {
                if (func.Identificator == identificator)
                {
                    functions.Add(func);
                }
            }

            return functions;
        }

        public Function FindFunction(string identificator, List<DataType> dataTypes)
        {
            foreach (var function in _functions)
            {
                if (function.Identificator == identificator)
                {
                    if (function.Parameters.Count != dataTypes.Count)
                    {
                        continue;
                    }

                    for (int i = 0; i < function.Parameters.Count; i++)
                    {
                        if (function.Parameters[i].DataType != dataTypes[i])
                        {
                            continue;
                        }
                    }
                    return function;
                }
            }

            return null;
        }

        public Variable FindVariable(string identificator)
        {
            foreach (var variable in _variables)
            {
                if (variable.Identificator == identificator)
                {
                    return variable;
                }
            }

            return null;
        }

        public bool Contains(string identificator)
        {
            foreach (var variable in _variables)
            {
                if (variable.Identificator == identificator)
                {
                    return true;
                }
            }

            foreach (var function in _functions)
            {
                if (function.Identificator == identificator)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(Variable variable, bool searchByRef = true)
        {
            if (searchByRef)
            {
                return _variables.Contains(variable);
            }
            else
            {
                foreach (var variable_ in _variables)
                {
                    if (variable == variable_)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool Contains(Function function, bool searchByRef = true)
        {
            if (searchByRef)
            {
                return _functions.Contains(function);
            }
            else
            {
                foreach (var function_ in _functions)
                {
                    if (function == function_)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public Variable GetVariable(string identificator)
        {
            foreach (var variable in _variables)
            {
                if (variable.Identificator == identificator)
                {
                    return variable;
                }
            }

            return null;
        }

        public void Print()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"VARIABLES: ");
            Console.ResetColor();
            foreach (var variable in _variables)
            {
                Console.Write($"{variable.DataType} {variable.Identificator}, ");
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"FUNCTIONS: ");
            Console.ResetColor();
            foreach (var function in _functions)
            {
                string parameters = string.Empty;

                foreach (var parameter in function.Parameters)
                {
                    parameters += $"{parameter.DataType} {parameter.Identificator}";
                    parameters += ", ";
                }

                Console.Write($"{function.DataType} {function.Identificator} ({parameters})");
            }
        }
    }
}
