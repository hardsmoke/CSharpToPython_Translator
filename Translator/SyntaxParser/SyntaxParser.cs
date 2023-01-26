using Translator.Collections;
using Translator.Errors;
using Translator.Errors.SyntaxErrors;

namespace Translator.SyntaxParser
{
    public enum DebugMode
    {
        DISABLED,
        RESULT,
        FULLY,
    }

    public class SyntaxParser
    {
        private const string INPUT_END_SYMBOL = "INPUT END SYMBOL";

        private enum States { q, b, t }

        private Stack<StoryChainElement> _storyChain;
        private Stack<string> _currentChain;

        private Grammar _grammar;

        private int _carriagePos;
        private States _state;

        private List<string> _input;
        private string _resultString;
        private List<Rule> _resultRules;

        private Tree _parseTree;
        public Tree ParseTree => _parseTree;

        private Tree _syntaxTree;
        public Tree SyntaxTree => _syntaxTree;

        private List<Error> _errors = new List<Error>();
        public List<Error> Errors => _errors;

        public SyntaxParser(Grammar grammar, List<string> input)
        {
            _grammar = grammar;

            Initialize();

            Parse(input);
        }

        private void Initialize()
        {
            _errors = new List<Error>();
            _state = States.q;
            _carriagePos = 0;

            _storyChain = new Stack<StoryChainElement>();

            _currentChain = new Stack<string>();

            _currentChain.Push(INPUT_END_SYMBOL);
            _currentChain.Push(_grammar.Nonterms[0]);

            _resultRules = new List<Rule>();
        }

        private string GetTerminalStringFromStoryChain(bool needReverse = true)
        {
            StoryChainElement[] stackString = new StoryChainElement[_storyChain.Count];
            _storyChain.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            string newStr = "";
            foreach (var symbol in stackString)
            {
                if (_grammar.Terms.Contains(symbol.Symbol))
                    newStr += symbol.Symbol;
            }

            return newStr;
        }

        public void Parse(List<string> input, DebugMode debugMode = DebugMode.DISABLED)
        {
            _input = input;

            foreach (string symbol in input)
            {
                if (!_grammar.IsTerminal(symbol))
                {
                    _errors.Add(new UnexpectedNonterminalError(symbol));
                }
            }

            while (_state != States.t)
            {
                if (debugMode == DebugMode.FULLY)
                {
                    PrintDebugInfo();
                }

                switch (_state)
                {
                    case States.q:
                        {
                            if (_currentChain.Peek() == INPUT_END_SYMBOL)
                            {
                                if (string.Join("", _input.ToArray()) == GetTerminalStringFromStoryChain())
                                {
                                    SuccessFinish();
                                }
                                else
                                {
                                    FailedFinish();
                                }

                                continue;
                            }

                            if (!_grammar.IsTerminal(_currentChain.Peek()))
                            {
                                Overgrowth();
                                continue;
                            }

                            if (CompareInputCharWithCurChar())
                            {
                                SuccessCompare();
                            }
                            else
                            {
                                FailedCompare();
                                continue;
                            }
                            break;
                        }

                    case States.b:
                        {
                            if (_grammar.IsTerminal(_storyChain.Peek().Symbol))
                            {
                                BackTrack();
                                continue;
                            }

                            CheckNextAlternative();

                            break;
                        }

                    case States.t:
                        break;

                    default:
                        break;
                }
            }

            if (debugMode == DebugMode.RESULT)
            {
                PrintDebugInfo();
            }

            foreach (var symbol in _storyChain)
            {
                if (_grammar.IsNonTerminal(symbol.Symbol))
                {
                    Rule rule = _grammar.Rules.GetRule(symbol.Symbol, symbol.SerialNumber);
                    if (rule != null)
                    {
                        _resultRules.Add(rule);
                        _resultString += $"{_grammar.Rules.GetRuleIndex(rule) + 1} | ";
                    }
                }
            }

            _resultRules.Reverse();

            if (debugMode == DebugMode.RESULT || debugMode == DebugMode.FULLY)
            {
                Console.WriteLine(ReverseString(_resultString));
            }

            CreateParseTree();
            CreateSyntaxTree();
        }

        private string ReverseString(string str)
        {
            string newString = string.Empty;

            char[] cArray = str.ToCharArray();
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                newString += cArray[i];
            }

            return newString;
        }

        private void CreateSyntaxTree()
        {
            _syntaxTree = _parseTree.CopyWithoutNode("блок кода");
            _syntaxTree = _syntaxTree.CopyWithoutNode("инструкция");
            _syntaxTree = _syntaxTree.CopyWithoutNode("E1");
            _syntaxTree = _syntaxTree.CopyWithoutNode("T1");
            _syntaxTree = _syntaxTree.CopyWithoutNode("LOGICAL EXPRESSION");
            _syntaxTree = _syntaxTree.CopyWithoutNode("мат выражение");
            _syntaxTree = _syntaxTree.CopyWithoutNode("func param");

            ReplaceChildrenWithString(_syntaxTree.GetNodes("идентификатор"));
            ReplaceChildrenWithString(_syntaxTree.GetNodes("строковое значение"));

            List<Node> nodes = new List<Node>();
            _syntaxTree.GetNodes("значение").ForEach((node) => nodes.Add(node.Children[0]));
            ReplaceChildrenWithString(nodes);
        }

        private void PrintDebugInfo()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("State: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_state}\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Каретка position: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{_carriagePos}\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("StoryChain:\n");
            PrintStoryChain(_storyChain);
            Console.Write($"\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("CurrentChain:\n");
            PrintCurrentChain(_currentChain, false);
            Console.Write("\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Current result: ");
            PrintTermsFromStoryChain(_storyChain);
            Console.Write("\n\n");
            Console.ResetColor();
        }

        private void ReplaceChildrenWithString(List<Node> nodes)
        {
            foreach (var node in nodes)
            {
                string id = string.Empty;
                _grammar.GetTermsStartsWith(node).ForEach((term) => id += term);
                Node nodeToAdd = new Node(id);
                node.ReplaceChildrenWith(new List<Node>() { nodeToAdd });
            }
        }

        private void CreateParseTree()
        {
            if (_resultRules.Count == 0)
            {
                return;
            }

            int index = 0;

            Rule curRule = _resultRules[index];
            Node rootNode = new Node(curRule.LeftPart, null);
            _parseTree = new Tree(rootNode);

            CreateParseTree(rootNode, ref index);
        }

        private void CreateParseTree(Node parentNode, ref int index)
        {
            if (index >= _resultRules.Count)
            {
                return;
            }

            Rule curRule = _resultRules[index];
            index++;
            foreach (var str in curRule.RightPart)
            {
                Node newNode = new Node(str);

                parentNode.AddChild(newNode);


                if (_grammar.IsTerminal(newNode.Data))
                {
                    continue;
                }

                CreateParseTree(newNode, ref index);
            }
        }

        // 1 punk
        private void Overgrowth()
        {
            string curNonterm = _currentChain.Pop();
            Rule curRule = _grammar.Rules.GetRules(curNonterm)[0];

            _storyChain.Push(new StoryChainElement(curNonterm, _grammar.Rules.GetRuleSerialNumber(curRule)));

            for (int i = curRule.RightPart.Count - 1; i >= 0; i--)
            {
                _currentChain.Push(curRule.RightPart[i]);
            }
        }

        // 2, 4 if
        private bool CompareInputCharWithCurChar()
        {
            if (_carriagePos >= _input.Count) return false;

            return _input[_carriagePos] == _currentChain.Peek();
        }

        // 2
        private void SuccessCompare()
        {
            if (_carriagePos < _input.Count)
            {
                _storyChain.Push(new StoryChainElement(_currentChain.Pop(), 0));
            }
            _carriagePos++;
        }

        // 3
        private void SuccessFinish()
        {
            _state = States.t;
            _currentChain.Pop();
        }
        private void FailedFinish(string symbol = "")
        {
            _errors.Add(new IncorrectTokenSequenceError(symbol));
            _state = States.t;
        }

        // 4
        private void FailedCompare()
        {
            _state = States.b;
        }

        // 5
        private void BackTrack()
        {
            _carriagePos--;
            _currentChain.Push(_storyChain.Pop().Symbol);
        }

        private Rule GetNextAlternative()
        {
            List<Rule> rules = _grammar.Rules.GetRules(_storyChain.Peek().Symbol);
            int curRuleIndex = _storyChain.Peek().SerialNumber;

            if (rules.Count > curRuleIndex + 1)
            {
                return rules[curRuleIndex + 1];
            }
            return null;
        }

        private void RemoveLastRuleFromChains()
        {
            Rule curRule = _grammar.Rules.GetRule(_storyChain.Peek().Symbol, _storyChain.Peek().SerialNumber);

            _storyChain.Pop();
            for (var i = 0; i < curRule.RightPart.Count; i++)
            {
                _currentChain.Pop();
            }
        }

        private bool TryReplaceWithAlternativeRule()
        {
            Rule alternativeRule = GetNextAlternative();
            if (alternativeRule == null)
            {
                return false;
            }

            int curRuleSerialNumber = _storyChain.Peek().SerialNumber;

            RemoveLastRuleFromChains();
            _storyChain.Push(new StoryChainElement(alternativeRule.LeftPart, curRuleSerialNumber + 1));

            for (var i = alternativeRule.RightPart.Count - 1; i >= 0; i--)
            {
                _currentChain.Push(alternativeRule.RightPart[i]);
            }

            return true;
        }

        // 6
        private void CheckNextAlternative()
        {
            //a
            if (TryReplaceWithAlternativeRule())
            {
                _state = States.q;
            }

            // b
            else if (_carriagePos == 0 && _storyChain.Peek().Symbol == _grammar.Nonterms[0])
            {
                FailedFinish(_storyChain.Peek().Symbol);
            }

            // c
            else
            {
                string currentLeftPart = _storyChain.Peek().Symbol;
                RemoveLastRuleFromChains();
                _currentChain.Push(currentLeftPart);
            }
        }

        string GetStackAsString(Stack<Tuple<string, int>> stack, bool needReverse = true)
        {
            Tuple<string, int>[] stackString = new Tuple<string, int>[stack.Count];
            stack.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            string resultString = "";
            foreach (var str in stackString)
            {
                if (_grammar.IsTerminal(str.Item1))
                {
                    resultString += $"[{str.Item1}]";

                }
                else
                {
                    resultString += $"<{str.Item1}({(str.Item2 >= 0 ? str.Item2.ToString() : string.Empty)})>";
                }
            }

            return resultString;
        }

        string GetStackAsString(Stack<string> stack, bool needReverse = true)
        {
            string[] stackString = new string[stack.Count];
            stack.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            string resultString = "";
            foreach (var str in stackString)
            {
                resultString += str;
            }

            return resultString;
        }

        void PrintStoryChain(Stack<StoryChainElement> stack, bool needReverse = true)
        {
            StoryChainElement[] stackString = new StoryChainElement[stack.Count];
            stack.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            foreach (var str in stackString)
            {
                if (_grammar.IsTerminal(str.Symbol))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write($"[{str.Symbol}]");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"<");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{str.Symbol}");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"({(str.SerialNumber >= 0 ? str.SerialNumber.ToString() : string.Empty)})");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($">");
                }
                Console.ResetColor();
            }
        }

        void PrintCurrentChain(Stack<string> stack, bool needReverse = true)
        {
            string[] stackString = new string[stack.Count];
            stack.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            foreach (var str in stackString)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("<");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{str}");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(">");
            }
            Console.ResetColor();
        }

        void PrintTermsFromStoryChain(Stack<StoryChainElement> stack, bool needReverse = true)
        {
            StoryChainElement[] stackString = new StoryChainElement[stack.Count];
            stack.CopyTo(stackString, 0);

            if (needReverse)
                stackString = stackString.Reverse().ToArray();

            foreach (var str in stackString)
            {
                if (_grammar.IsTerminal(str.Symbol))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    //Console.Write("<");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{str.Symbol} ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("|");
                }
            }
            Console.ResetColor();
        }
    }
}
