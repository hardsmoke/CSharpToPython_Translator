using Translator;
using Translator.Collections;
using Translator.Errors;
using Translator.Errors.SemanticErrors;
using Translator.LexicalAnalyzer;
using Translator.SemanticAnalyzer;
using Translator.SyntaxParser;

namespace TranslatorTests
{
    public class SemanticAnalyzerTests
    {
        private List<Error> GetSemanticErrors(string input)
        {
            //Lex
            List<Token> tokens = Program.GetTokensFromLexer(input);

            //Syntax
            List<string> syntaxParserInput = Program.GetSyntaxParserData(tokens);
            Tree syntaxTree = Program.GetSyntaxTree(syntaxParserInput, DebugMode.DISABLED);

            //Semantic
            return Program.GetErrorsFromSemanticAnalyzer(syntaxTree);
        }

        [Fact]
        public void VariableDeclaration_DeclaringVariableWithoutInitialization_ShouldNotReturnAnyErrors()
        {
            string input = "int a;";
            List<Error> semanticErrors = GetSemanticErrors(input);
            
            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableDeclaration_DeclaringAlreadyDeclaredVariable_ShouldReturnSingleError()
        {
            string input = "int a; int a;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIsAlreadyDeclaredError("a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableDeclaration_DeclaringVariableWithInitialization_ShouldNotReturnAnyErrors()
        {
            string input = "int a = 50;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableDeclaration_DeclaringVariableWithInitializationWrongDataType_ShouldReturnSingleError()
        {
            string input = "int a = 1.54;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.INTEGER, DataType.FLOAT, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableDeclaration_UsingNotDeclaredVariable_ShouldReturnSingleError()
        {
            string input = "a = 1.54;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableNotDeclaredError("a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableDeclaration_DeclareVariableWithoutInitialization_ShouldNotReturnAnyErrors()
        {
            string input = "int a;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableInitialization_UsingNotInitializedVariable_ShouldReturnSingleError()
        {
            string input = "int a; int b = a;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableNotInitializedError("a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableInitialization_InitializeVariableAfterDeclaringAndUse_ShouldNotReturnAnyErrors()
        {
            string input = "int a; a = 10; int b = a;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableInitialization_IncrementNotInitializedVariable_ShouldReturnSingleError()
        {
            string input = "int a; a++;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableNotInitializedError("a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableInitialization_DecrementNotInitializedVariable_ShouldReturnSingleError()
        {
            string input = "int a; a--;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableNotInitializedError("a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableAssign_AssignToVariableIncorrectDataType_ShouldReturnSingleError()
        {
            string input = "int a; a = 1.5;";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.INTEGER, DataType.FLOAT, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableAssign_AssignToIntVariableExpressionWithIntValues_ShouldNotReturnAnyErrors()
        {
            string input = "int a; int b = 4; a = 1 + 5 * 10 - 12 + (15 - (53));";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableAssign_AssignToIntVariableExpressionWithIntAndFloatValues_ShouldReturnSingleError()
        {
            string input = "int a; int b = 4; a = 1 + 5.5 * 10 - 12 + (15 - (53));";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.INTEGER, DataType.FLOAT, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableAssign_AssignToIntVariableExpressionWithIntAndFloatValues2_ShouldReturnSingleError()
        {
            string input = "int a; int b = 4.10; a = 1 + 5 * 10 - 12 + (15 - (53));";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.INTEGER, DataType.FLOAT, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableAssign_AssignToBoolVariableIncorrectLogicalExpression_ShouldReturnSingleError()
        {
            string input = "bool a; a = 1 + 5 * 10 - 122.2 + (15 - (53));";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.BOOL, DataType.FLOAT, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void VariableAssign_AssignToBoolVariableCorrectLogicalExpression_ShouldNotReturnAnyErrors()
        {
            string input = "bool a; a = 1 + 5 * 10 > 122.2 + (15 - (53));";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableAssign_AssignToVariableFunctionResultWithCorrectDataTytpe_ShouldNotReturnAnyErrors()
        {
            string input = "int foo() { return 0; } int a = foo();";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void VariableAssign_AssignToVariableFunctionResultWithInorrectDataTytpe_ShouldReturnSingleError()
        {
            string input = "string foo() { return \"srt\"; } int a = foo();";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new VariableIncorrectDataTypeError(DataType.INTEGER, DataType.STRING, "a").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_DeclaringFunctionWithoutParameters_ShouldNotReturnAnyErrors()
        {
            string input = "void foo() { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void FunctionDeclaration_DeclaringAlreadyDeclaredFunction_ShouldReturnSingleError()
        {
            string input = "void foo() { int v; } void foo() { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionIsAlreadyDeclaredError(new Function(DataType.VOID, "foo", 
                new List<Variable>())).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_DeclaringAlreadyDeclaredFunctionWithOtherParameters_ShouldNotReturnAnyErrors()
        {
            string input = "void foo(int a) { int v; } void foo(float a) { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void FunctionDeclaration_DeclaringAlreadyDeclaredFunctionWithOtherDataType_ShouldReturnSingleError()
        {
            string input = "int foo() { return 0; } void foo() { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionIsAlreadyDeclaredError(new Function(DataType.VOID, "foo",
                new List<Variable>())).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_DeclaringFunctionWithParameters_ShouldNotReturnAnyErrors()
        {
            string input = "void foo(int a, float b, bool c) { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void FunctionDeclaration_CallingNotDeclaredFunction_ShouldReturnSingleError()
        {
            string input = "foo();";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionNotDeclaredError("foo").GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_CallingDeclaredFunctionWithoutParameters_ShouldNotReturnAnyErrors()
        {
            string input = "void foo() { int v; } foo();";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Empty(semanticErrors);
        }

        [Fact]
        public void FunctionDeclaration_CallingDeclaredFunctionWithIncorrectParameters_ShouldReturnSingleError()
        {
            string input = "void foo(int a) { int v; } foo(1.2);";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionNotDeclaredError("foo", new List<DataType> { DataType.FLOAT }).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_CallingDeclaredFunctionWithIncorrectParametersCount_ShouldReturnSingleError()
        {
            string input = "void foo(int a) { int v; } foo(1, 1);";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionNotDeclaredError("foo", new List<DataType> { DataType.INTEGER, DataType.INTEGER }).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_DeclarationIntFunctionWithoutReturn_ShouldReturnSingleError()
        {
            string input = "int foo(int a) { int v; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new FunctionDeclarationWithoutReturn("foo", DataType.INTEGER).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }

        [Fact]
        public void FunctionDeclaration_DeclarationIntFunctionWithIncorrectDataTypeReturn_ShouldReturnSingleError()
        {
            string input = "int foo() { return 1.54; }";
            List<Error> semanticErrors = GetSemanticErrors(input);

            Assert.Single(semanticErrors);
            Assert.Equal(new ReturnIncorrectDataTypeError(DataType.INTEGER, DataType.FLOAT, "foo", null).GetErrorDescription(),
                semanticErrors[0].GetErrorDescription());
        }
    }
}