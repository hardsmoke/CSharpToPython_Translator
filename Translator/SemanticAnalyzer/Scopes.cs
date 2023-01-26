using System;
using System.Collections.Generic;
using System.Linq;

namespace Translator.SemanticAnalyzer
{
    public class Scopes
    {
        private List<Scope> _scopes = new List<Scope>();

        public void Add(Scope scope)
        {
            _scopes.Add(scope);
        }

        public Scope Last()
        {
            return _scopes.Last();
        }

        public void Remove(Scope scope)
        {
            _scopes.Remove(scope);
        }

        public List<Function> FindFunctions(string identificator)
        {
            List<Function> functions = new List<Function>();

            foreach (var scope in _scopes)
            {
                functions.AddRange(scope.FindFunctions(identificator));
            }

            return functions;
        }

        public Function FindFunction(string identificator, List<DataType> dataTypes)
        {
            foreach (var scope in _scopes)
            {
                Function function = scope.FindFunction(identificator, dataTypes);
                if (function != null)
                {
                    return function;
                }
            }

            return null;
        }

        public Variable FindVariable(string identificator)
        {
            foreach (var scope in _scopes)
            {
                Variable variable = scope.FindVariable(identificator);
                if (variable != null)
                {
                    return variable;
                }
            }

            return null;
        }

        public bool Contains(string identificator)
        {
            foreach (var scope in _scopes)
            {
                if (scope.Contains(identificator))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(Variable variable, bool searchByRef = true)
        {
            foreach (var scope in _scopes)
            {
                if (scope.Contains(variable, searchByRef))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(Function function, bool searchByRef = true)
        {
            foreach (var scope in _scopes)
            {
                if (scope.Contains(function, searchByRef))
                {
                    return true;
                }
            }

            return false;
        }

        public Variable GetVariable(string identificator)
        {
            foreach (var scope in _scopes)
            {
                if (scope.Contains(identificator))
                {
                    return scope.GetVariable(identificator);
                }
            }

            return null;
        }

        public void Print()
        {
            foreach (var scope in _scopes)
            {
                scope.Print();
                Console.WriteLine("\n----------------");
            }
        }
    }
}
