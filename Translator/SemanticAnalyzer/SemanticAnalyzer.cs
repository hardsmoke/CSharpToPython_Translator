using Translator.Errors.SemanticErrors;
using Translator.Collections;
using Translator.Errors;

namespace Translator.SemanticAnalyzer
{
    public class SemanticAnalyzer
    {
        public List<Error> GetErrors(Tree syntaxTree)
        {
            List<Error> errors = new List<Error>();

            Scopes scopes = new Scopes();
            scopes.Add(new Scope());

            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { }));
            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { new Variable(DataType.CHAR, "var", true) }));
            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { new Variable(DataType.BOOL, "var", true) }));
            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { new Variable(DataType.FLOAT, "var", true) }));
            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { new Variable(DataType.INTEGER, "var", true) }));
            scopes.Last().Add(new Function(DataType.VOID, "Console.WriteLine", new List<Variable> { new Variable(DataType.STRING, "var", true) }));

            syntaxTree.Root.Traverse((node) =>
            {
                if (node.Data == "тело" || node.Data == "параметры функции")
                {
                    scopes.Add(new Scope());
                }
                else
                {
                    errors.AddRange(Analyze(node, scopes));
                }
            }, (node) =>
            {
                if (node.Data == "тело" || node.Data == "объявление функции")
                {
                    scopes.Remove(scopes.Last());
                }
            });

            return errors;
        }

        private List<Error> Analyze(Node node, in Scopes scopes)
        {
            List<Error> errors = new List<Error>();

            if (node.Data == "идентификатор")
            {
                string identificator = node.Children[0].Data;

                errors.AddRange(GetVariablesDeclarationErrors(node, scopes, identificator));
                errors.AddRange(GetFunctionsDeclarationErrors(node, scopes, identificator));
                errors.AddRange(GetVariablesDataTypeErrors(node, scopes, identificator));
            }

            return errors;
        }

        private List<Error> GetVariablesDataTypeErrors(Node node, in Scopes scopes, string identificator)
        {
            List<Error> errors = new List<Error>();

            Variable variable = scopes.GetVariable(identificator);
            if (variable is null)
            {
                return errors;
            }

            if (!(node.Parent.Data == "объявление переменной" || node.Parent.Data == "присваивание"))
            {
                return errors;
            }

            List<Node> expressionNodes = node.Parent.GetChildren("выражение");
            if (expressionNodes.Count == 0)
            {
                return errors;
            }

            Node expressionNode = expressionNodes[0];
            List<DataType> dataTypeList = GetDataTypesFromExpressionNode(expressionNode, scopes);

            if (dataTypeList.Count != 0)
            {
                if (!IsDataTypesCompatible(dataTypeList))
                {
                    errors.Add(new DataTypesCompatibleError(dataTypeList));
                }
                else if (!IsAssignCorrect(variable.DataType, IsExpressionLogical(expressionNode) ? new List<DataType> { DataType.BOOL } : dataTypeList))
                {
                    errors.Add(new VariableIncorrectDataTypeError(variable.DataType,
                        IsExpressionLogical(expressionNode) ? DataType.BOOL : dataTypeList.Max()));
                }
            }

            return errors;
        }

        private List<DataType> GetDataTypesFromExpressionNode(Node expressionNode, Scopes scopes)
        {
            List<Node> dataTypeNodes = new List<Node>();
            expressionNode.GetChildren("значение").ForEach(dataTypeNode => dataTypeNodes.Add(dataTypeNode.Children[0]));

            List<Node> identificatorNodes = new List<Node>();
            expressionNode.GetChildren("идентификатор").ForEach(identificatorNode => identificatorNodes.Add(identificatorNode.Children[0]));

            List<DataType> dataTypeList = new List<DataType>();
            foreach (var dataTypeNode in dataTypeNodes)
            {
                dataTypeList.Add(GetDataTypeByNonterm(dataTypeNode.Data));
            }

            if (expressionNode.GetChildren("логическое значение").Count != 0)
            {
                dataTypeList.Add(DataType.BOOL);
            }

            foreach (var identificatorNode in identificatorNodes)
            {
                Variable variableTemp = scopes.GetVariable(identificatorNode.Data);
                if (variableTemp is not null)
                {
                    dataTypeList.Add(variableTemp.DataType);
                    continue;
                }

                List<DataType> parameterDataTypeList = new List<DataType>();
                foreach (var parameterNode in identificatorNode.Parent.GetChildren("параметр вызова функции"))
                {
                    Node expNode = parameterNode.GetChildren("выражение")[0];
                    parameterDataTypeList.Add(GetDataTypesFromExpressionNode(expNode, scopes).Max());
                }
                Function functionTemp = scopes.FindFunction(identificatorNode.Data, parameterDataTypeList);

                if (functionTemp != null)
                {
                    dataTypeList.Add(functionTemp.DataType);
                    continue;
                }
            }

            return dataTypeList;
        }

        private bool IsExpressionLogical(Node expressionNode)
        {
            return expressionNode.Children[0].Data == "лог выражение";
        }

        private bool IsDataTypesCompatible(List<DataType> dataTypeList)
        {
            if (dataTypeList.Count == 0) return true;

            if (!dataTypeList.Contains(DataType.FLOAT) && !dataTypeList.Contains(DataType.INTEGER))
            {
                if (dataTypeList.Min() != dataTypeList.Max())
                {
                    return false;
                }
            }
            else
            {
                if (dataTypeList.Contains(DataType.BOOL) || dataTypeList.Contains(DataType.CHAR) || dataTypeList.Contains(DataType.STRING))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsAssignCorrect(DataType VariableDataType, List<DataType> dataTypeList)
        {
            if (!dataTypeList.Any())
            {
                return false;
            }

            DataType minDataType = dataTypeList.Min();
            DataType maxDataType = dataTypeList.Max();

            if (!dataTypeList.Contains(DataType.FLOAT) && !dataTypeList.Contains(DataType.INTEGER))
            {
                if (VariableDataType != minDataType)
                {
                    return false;
                }
            }
            else
            {
                if (VariableDataType != DataType.INTEGER && VariableDataType != DataType.FLOAT)
                {
                    return false;
                }
                else if (VariableDataType < maxDataType)
                {
                    return false;
                }
            }

            return true;
        }

        private List<Error> GetVariablesDeclarationErrors(Node node, in Scopes scopes, string identificator)
        {
            List<Error> errors = new List<Error>();

            if (node.Parent.Data == "объявление переменной" || node.Parent.Data == "параметр функции")
            {
                string dataTypeStr = node.Parent.GetChildren("значимый тип данных")[0].Children[0].Data;
                DataType dataType = GetDataType(dataTypeStr);
                bool hasValue = false;

                if (node.Parent.Data == "параметр функции" || node.Parent.GetChildren("выражение").Any())
                {
                    hasValue = true;
                }

                Variable variable = new Variable(dataType, identificator, hasValue);

                if (scopes.Contains(identificator))
                {
                    errors.Add(new VariableIsAlreadyDeclaredError(identificator));
                }
                else
                {
                    scopes.Last().Add(variable);
                }
            }
            else if (node.Parent.Data != "вызов функции" && node.Parent.Data != "объявление функции")
            {
                Variable variable = scopes.FindVariable(identificator);
                if (variable is null)
                {
                    errors.Add(new VariableNotDeclaredError(identificator));
                }
                else if (node.Parent.Data == "присваивание" && !node.Parent.GetChildren("оператор инкремента").Any())
                {
                    variable.HasValue = true;
                }
                else if (!variable.HasValue)
                {
                    errors.Add(new VariableNotInitializedError(identificator));
                }
            }

            return errors;
        }

        private List<Error> GetFunctionsDeclarationErrors(Node node, in Scopes scopes, string identificator)
        {
            List<Error> errors = new List<Error>();

            if (node.Parent.Data == "объявление функции")
            {
                Node tempNode = node.Parent.GetChildren("тип данных функции")[0];
                while (tempNode.Children.Any())
                {
                    tempNode = tempNode.Children[0];
                }

                string dataTypeStr = tempNode.Data;
                DataType dataType = GetDataType(dataTypeStr);

                if (dataType != DataType.VOID && !node.Parent.GetChildren("возврат значения").Any())
                {
                    errors.Add(new FunctionDeclarationWithoutReturn(identificator, dataType));
                }

                List<Variable> parameters = new List<Variable>();
                foreach (var parameterNode in node.Parent.GetChildren("параметр функции"))
                {
                    string parameterIdentificator = parameterNode.GetChildren("идентификатор")[0].Children[0].Data;
                    string parameterDataTypeStr = parameterNode.GetChildren("значимый тип данных")[0].Children[0].Data;
                    DataType parameterDataType = GetDataType(parameterDataTypeStr);

                    parameters.Add(new Variable(parameterDataType, parameterIdentificator));
                }

                Function function = new Function(dataType, identificator, parameters);

                if (scopes.Contains(function, false))
                {
                    errors.Add(new FunctionIsAlreadyDeclaredError(function));
                }
                else
                {
                    scopes.Last().Add(function);
                }

                List<Node> returnNodes = node.Parent.GetChildren("возврат значения");
                foreach (var returnNode in returnNodes)
                {
                    Node expressionNode = returnNode.GetChildren("выражение")[0];

                    scopes.Add(new Scope());
                    foreach (var param in parameters)
                    {
                        scopes.Last().Add(param);
                    }
                    List<DataType> returnExpressionDataTypes = GetDataTypesFromExpressionNode(expressionNode, scopes);
                    scopes.Remove(scopes.Last());

                    if (!IsDataTypesCompatible(returnExpressionDataTypes))
                    {
                        errors.Add(new DataTypesCompatibleError(returnExpressionDataTypes));
                        continue;
                    }

                    if (!IsAssignCorrect(dataType, returnExpressionDataTypes))
                    {
                        errors.Add(new ReturnIncorrectDataTypeError(dataType, returnExpressionDataTypes.Max(), identificator, null));
                        continue;
                    }
                }
            }
            else if (node.Parent.Data == "вызов функции")
            {
                Node parameterersNode = node.Parent.GetChildren("параметры вызова функции")[0];

                List<List<DataType>> parameterDataTypeLists = new List<List<DataType>>();
                foreach (var parameterNode in parameterersNode.Children)
                {
                    if (parameterNode.Data != "параметр вызова функции") continue;
                    Node expressionNode = parameterNode.GetChildren("выражение")[0];
                    parameterDataTypeLists.Add(GetDataTypesFromExpressionNode(expressionNode, scopes));
                }

                List<DataType> parameterDataTypeList = new List<DataType>();
                bool dataTypesCompatible = true;
                foreach (var dataTypes in parameterDataTypeLists)
                {
                    if (dataTypes.Count == 0)
                    {
                        dataTypesCompatible = false;
                        break;
                    }

                    if (IsDataTypesCompatible(dataTypes))
                    {
                        parameterDataTypeList.Add(dataTypes.Max());
                    }
                    else
                    {
                        errors.Add(new DataTypesCompatibleError(dataTypes));
                        dataTypesCompatible = false;
                        break;
                    }
                }

                if (dataTypesCompatible)
                {
                    List<Function> functions = scopes.FindFunctions(identificator);

                    bool functionExists = false;
                    foreach (Function function in functions)
                    {
                        if (function.Parameters.Count != parameterDataTypeList.Count)
                        {
                            continue;
                        }

                        List<DataType> functionParamDataTypeList = new List<DataType>();
                        foreach (var functionParam in function.Parameters)
                        {
                            functionParamDataTypeList.Add(functionParam.DataType);
                        }

                        if (functionParamDataTypeList.SequenceEqual(parameterDataTypeList))
                        {
                            functionExists = true;
                            break;
                        }
                    }

                    if (!functionExists)
                    {
                        errors.Add(new FunctionNotDeclaredError(identificator, parameterDataTypeList));
                    }
                }
            }

            return errors;
        }

        public DataType GetDataType(string str)
        {
            switch (str)
            {
                case "void":
                    return DataType.VOID;
                case "bool":
                    return DataType.BOOL;
                case "char":
                    return DataType.CHAR;
                case "string":
                    return DataType.STRING;
                case "int":
                    return DataType.INTEGER;
                case "float":
                    return DataType.FLOAT;
                default:
                    return DataType.NONE;
            }
        }

        public DataType GetDataTypeByNonterm(string nonterm)
        {
            switch (nonterm)
            {
                case "\'":
                    return DataType.CHAR;
                case "\"":
                    return DataType.STRING;
                case "логическое значение":
                    return DataType.BOOL;
                case "целое число":
                    return DataType.INTEGER;
                case "вещественное число":
                    return DataType.FLOAT;
                default:
                    return DataType.NONE;
            }
        }
    }
}