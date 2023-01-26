using Translator.Collections;
using Translator.Errors;
using Translator.Generator;
using Translator.LexicalAnalyzer;
using Translator.SyntaxParser;

namespace Translator
{
    public class Program
    {
        public const ConsoleColor DEBUG_COLOR = ConsoleColor.Cyan;

        public static void PrintDebugInfo(string debugTitle, string debugInfo, ConsoleColor color, bool emptyLineInTheEnd = false)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(debugTitle);
            Console.ResetColor();

            Console.WriteLine(debugInfo);

            Console.ForegroundColor = color;
            Console.WriteLine($"END OF {debugTitle}");
            Console.ResetColor();

            if (emptyLineInTheEnd)
            {
                Console.WriteLine();
            }
        }

        public static List<Token> GetTokensFromLexer(string input)
        {
            CSharpTokenType[] tokenTypes =
            {
                new CSharpTokenType("SYSTEM_METHOD", "\\b(?:Console\\.WriteLine)\\b"),
                new CSharpTokenType("KEYWORD", "\\b(?:if|else|true|false|return|while|for|foreach|do)\\b"),
                new CSharpTokenType("DATATYPE", "\\b(?:int|float|bool|void|string|char)\\b"),
                new CSharpTokenType("ID", "[A-Za-z][A-Za-z0-9_]*"),
                new CSharpTokenType("REAL_NUMBER", "[0-9]+\\.[0-9]*"),
                new CSharpTokenType("NUMBER", "[0-9]+"),
                new CSharpTokenType("STRING", "\"[^\"]*\""),
                new CSharpTokenType("TAB", "\\t"),
                new CSharpTokenType("LINEFEED", "\\r\\n+"),
                new CSharpTokenType("SPACE", "\\s"),
                new CSharpTokenType("HATCH", "\\'"),
                new CSharpTokenType("COMMA", ","),
                new CSharpTokenType("COMMENT", "//[\\n\\r]*"),
                new CSharpTokenType("OPERATION", "[+\\-\\*/.=\\(\\)\\{\\}<>\\!]"),
                new CSharpTokenType("END_OF_INSTRUCTION", ";"),
            };

            RegexTokenizer tokenizer = new RegexTokenizer(input, tokenTypes);

            return tokenizer.Tokens;
        }

        public static string GetTokensAsString(List<Token> tokens)
        {
            string result = string.Empty;

            foreach (var token in tokens)
            {
                result += $"{token.Type.Type} : '{token.Text}'\n";
            }

            return result;
        }

        public static List<string> GetSyntaxParserData(List<Token> tokens)
        {
            List<string> output = new List<string>();

            List<string> fullnameTypes = new List<string>() { "KEYWORD", "DATATYPE" };
            List<string> skipTypes = new List<string>() { "SPACE", "TAB", "LINEFEED" };

            foreach (var token in tokens)
            {
                if (fullnameTypes.Contains(token.Type.Type))
                {
                    output.Add(token.Text);
                }
                else if (skipTypes.Contains(token.Type.Type))
                {
                    continue;
                }
                else
                {
                    foreach (var ch in token.Text)
                    {
                        output.Add(ch.ToString());
                    }
                }
            }

            return output;
        }

        public static string GetSyntaxParserInputAsString(List<string> data)
        {
            string result = string.Empty;

            foreach (var item in data)
            {
                result += $"\"{item}\", ";
            }

            return result;
        }

        public static List<string> GetTerms()
        {
            return new List<string>
            {
                "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
                "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л",
                "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш",
                "щ", "ъ", "ы", "ь", "э", "ю", "я",  "А", "Б", "В", "Г", "Д", "Е",
                "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С",
                "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю",
                "Я", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "(", ")",
                "{", "}", "[", "]", "+", "-", "*", "/", "%", "=", "!", ">", ",",
                "&", "|", ";", "\'", "\"", "_", "#", "@", "$", "^", "~", "№", ".", "<",
                ":", "?", "bool", "char", "int", "float", "string", "if", "else",
                "==", "!=", ">=", "<=", "true", "false", "||", "&&", " ", "'", "void",
                "return", "for", "while", "do",
            };
        }

        public static List<string> GetNonterms()
        {
            return new List<string>
            {
                "программа",
                "блок кода",
                "тело",
                "объявление переменной",
                "значение",
                "объявление функции",
                "параметр функции",
                "параметры функции",
                "func param",
                "значимый тип данных",
                "тип данных функции",
                "модификатор типа данных",
                "тип данных",
                "буква",
                "цифра",
                "целое число",
                "вещественное число",
                "число",
                "прочие символы",
                "символ идентификатора",
                "идентификатор",
                "ид",
                "тело функции",
                "возврат значения",
                "цикл",
                "оператор цикла",
                "ветвление",
                "символьное значение",
                "строковое значение",
                "подстрока",
                "оператор сравнения",
                "знак унарной операции",
                "выражение",
                "лог выражение",
                "LOGICAL EXPRESSION",
                "мат выражение",
                "мат подвыражение",
                "мат оператор",
                "логическое значение",
                "инструкция",
                "мат знак типа сложения",
                "мат знак типа умножения",
                "лог знак типа сложения",
                "лог знак типа умножения",
                "вызов функции",
                "параметры вызова функции",
                "параметр вызова функции",
                "оператор присваивания",
                "присваивание",
                "главная функция",
                "E1",
                "T1",
                "оператор инкремента",
            };
        }

        public static Rules GetRules()
        {
            List<Rule> rulesList = new List<Rule>
            {
                new Rule("программа", new List<string> {"блок кода"}),

                new Rule("тело", new List<string> {"блок кода"}),
                new Rule("блок кода", new List<string> {"инструкция", "блок кода"}),
                new Rule("блок кода", new List<string> {"инструкция"}),

                new Rule("инструкция", new List<string> {"объявление переменной", ";"}),
                new Rule("инструкция", new List<string> {"объявление функции"}),
                new Rule("инструкция", new List<string> {"присваивание", ";"}),
                new Rule("инструкция", new List<string> {"ветвление"}),
                new Rule("инструкция", new List<string> {"цикл"}),
                new Rule("инструкция", new List<string> {"вызов функции", ";"}),
                new Rule("инструкция", new List<string> {"возврат значения"}),
                new Rule("инструкция", new List<string> {";"}),

                new Rule("объявление переменной", new List<string> {"тип данных", "идентификатор", "=", "выражение"}),
                new Rule("объявление переменной", new List<string> {"тип данных", "идентификатор"}),

                new Rule("тип данных", new List<string> {"значимый тип данных"}),
                new Rule("значимый тип данных", new List<string> {"int"}),
                new Rule("значимый тип данных", new List<string> {"float"}),
                new Rule("значимый тип данных", new List<string> {"bool"}),
                new Rule("значимый тип данных", new List<string> {"char"}),
                new Rule("значимый тип данных", new List<string> {"string"}),

                new Rule("идентификатор" , new List<string> { "символ идентификатора" }),
                new Rule("идентификатор" , new List<string> { "символ идентификатора", "ид" }),
                new Rule("ид" , new List<string> { "символ идентификатора" }),
                new Rule("ид" , new List<string> { "символ идентификатора", "ид" }),
                new Rule("ид" , new List<string> { "цифра", "ид" }),
                new Rule("ид" , new List<string> { "цифра" }),
                new Rule("символ идентификатора" , new List<string> { "буква" }),
                new Rule("символ идентификатора" , new List<string> { "_" }),
                new Rule("символ идентификатора" , new List<string> { "." }),

                // Значения
                new Rule("значение", new List<string> {"целое число"}),
                new Rule("значение", new List<string> {"вещественное число"}),
                new Rule("значение", new List<string> {"\'", "символьное значение", "\'"}),
                new Rule("значение", new List<string> {"\"", "строковое значение", "\""}),

                new Rule("логическое значение", new List<string> {"true"}),
                new Rule("логическое значение", new List<string> {"false"}),

                new Rule("символьное значение", new List<string> {"буква"}),
                new Rule("символьное значение", new List<string> {"цифра"}),
                new Rule("символьное значение", new List<string> {"прочие символы"}),

                new Rule("строковое значение", new List<string> {"подстрока"}),
                new Rule("подстрока", new List<string> {}),
                new Rule("подстрока", new List<string> {"символьное значение"}),
                new Rule("подстрока", new List<string> {"символьное значение", "подстрока"}),
                
                // Ветвления, циклы
                new Rule("ветвление", new List<string> {"if", "(", "лог выражение", ")", "{", "тело", "}", "else", "{", "тело", "}"}),
                new Rule("ветвление", new List<string> {"if", "(", "лог выражение", ")", "{", "тело", "}", "else", "ветвление"}),
                new Rule("ветвление", new List<string> {"if", "(", "лог выражение", ")", "{", "тело", "}"}),

                new Rule("цикл", new List<string> {"while", "(", "лог выражение", ")", "{", "тело", "}"}),
                new Rule("цикл", new List<string> {"do", "{", "тело", "}", "while", "(", "лог выражение", ")", ";"}),
                //new Rule("цикл", new List<string> {"for", "(", "инструкция", "лог выражение", "инструкция", ")", "{", "тело", "}"}),

                new Rule("выражение", new List<string> {"лог выражение"}),
                new Rule("выражение", new List<string> {"E1"}),

                new Rule("лог выражение", new List<string> {"LOGICAL EXPRESSION"}),
                new Rule("LOGICAL EXPRESSION", new List<string> {"(", "LOGICAL EXPRESSION", ")"}),
                new Rule("LOGICAL EXPRESSION", new List<string> {"E1", "оператор сравнения", "E1"}),
                new Rule("LOGICAL EXPRESSION", new List<string> {"логическое значение", "=", "=", "логическое значение"}),
                new Rule("LOGICAL EXPRESSION", new List<string> {"символьное значение", "оператор сравнения", "символьное значение"}),
                new Rule("LOGICAL EXPRESSION", new List<string> {"логическое значение"}),

                new Rule("E1", new List<string> {"мат выражение"}),
                new Rule("E1", new List<string> {"мат выражение", "T1"}),
                new Rule("T1", new List<string> {"мат оператор", "E1"}),

                new Rule("мат выражение", new List<string> {"мат знак типа сложения", "мат выражение"}),
                new Rule("мат выражение", new List<string> {"(", "мат подвыражение", ")"}),
                new Rule("мат подвыражение", new List<string> {"E1"}),
                new Rule("мат выражение", new List<string> {"идентификатор"}),
                new Rule("мат выражение", new List<string> {"значение"}),
                new Rule("мат выражение", new List<string> {"вызов функции"}),

                new Rule("целое число", new List<string> {"цифра"}),
                new Rule("целое число", new List<string> {"цифра", "целое число"}),
                new Rule("вещественное число", new List<string> {"целое число", ".", "целое число"}),
                new Rule("число", new List<string> {"целое число"}),
                new Rule("число", new List<string> {"вещественное число"}),

                new Rule("мат оператор", new List<string> {"мат знак типа сложения"}),
                new Rule("мат оператор", new List<string> {"мат знак типа умножения"}),

                new Rule("мат знак типа сложения", new List<string> {"+"}),
                new Rule("мат знак типа сложения", new List<string> {"-"}),
                new Rule("мат знак типа умножения", new List<string> {"*"}),
                new Rule("мат знак типа умножения", new List<string> {"/"}),
                new Rule("мат знак типа умножения", new List<string> {"%"}),

                new Rule("оператор сравнения", new List<string> {"=", "="}),
                new Rule("оператор сравнения", new List<string> {"!", "="}),
                new Rule("оператор сравнения", new List<string> {">"}),
                new Rule("оператор сравнения", new List<string> {"<"}),
                new Rule("оператор сравнения", new List<string> {">", "="}),
                new Rule("оператор сравнения", new List<string> {"<", "="}),
                new Rule("оператор присваивания", new List<string> {"="}),
                new Rule("оператор инкремента", new List<string> {"+", "+"}),
                new Rule("оператор инкремента", new List<string> {"-", "-"}),

                new Rule("присваивание", new List<string> {"идентификатор", "оператор присваивания", "выражение"}),
                new Rule("присваивание", new List<string> {"идентификатор", "оператор инкремента"}),
                new Rule("присваивание", new List<string> {"идентификатор", "оператор инкремента"}),

                new Rule("объявление функции", new List<string> {"тип данных функции", "идентификатор", "(", "параметры функции", ")", "{", "тело функции", "}"}),

                new Rule("параметр функции", new List<string> {"тип данных", "идентификатор"}),
                new Rule("параметр функции", new List<string> {"тип данных", "идентификатор", ",", "параметр функции"}),

                new Rule("параметры функции", new List<string> {"параметр функции"}),
                new Rule("параметры функции", new List<string> {}),

                new Rule("тело функции", new List<string> {}),
                new Rule("тело функции", new List<string> {"блок кода"}),

                new Rule("тип данных функции", new List<string> {"тип данных"}),
                new Rule("тип данных функции", new List<string> {"void"}),

                new Rule("возврат значения", new List<string> {"return", "выражение", ";"}),

                new Rule("вызов функции", new List<string> {"идентификатор", "(", ")"}),
                new Rule("вызов функции", new List<string> {"идентификатор", "(", "параметры вызова функции", ")"}),
                new Rule("параметры вызова функции", new List<string> {"func param"}),
                new Rule("func param", new List<string> {"параметр вызова функции"}),
                new Rule("func param", new List<string> {"параметр вызова функции", ",", "func param"}),
                new Rule("параметр вызова функции", new List<string> {"выражение"}),
            };

            // Буква
            rulesList.AddRange(new List<Rule>
            {
                new Rule("буква", new List<string> {"a"}),
                new Rule("буква", new List<string> {"b"}),
                new Rule("буква", new List<string> {"c"}),
                new Rule("буква", new List<string> {"d"}),
                new Rule("буква", new List<string> {"e"}),
                new Rule("буква", new List<string> {"f"}),
                new Rule("буква", new List<string> {"g"}),
                new Rule("буква", new List<string> {"h"}),
                new Rule("буква", new List<string> {"i"}),
                new Rule("буква", new List<string> {"j"}),
                new Rule("буква", new List<string> {"k"}),
                new Rule("буква", new List<string> {"l"}),
                new Rule("буква", new List<string> {"m"}),
                new Rule("буква", new List<string> {"n"}),
                new Rule("буква", new List<string> {"o"}),
                new Rule("буква", new List<string> {"p"}),
                new Rule("буква", new List<string> {"q"}),
                new Rule("буква", new List<string> {"r"}),
                new Rule("буква", new List<string> {"s"}),
                new Rule("буква", new List<string> {"t"}),
                new Rule("буква", new List<string> {"u"}),
                new Rule("буква", new List<string> {"v"}),
                new Rule("буква", new List<string> {"w"}),
                new Rule("буква", new List<string> {"x"}),
                new Rule("буква", new List<string> {"y"}),
                new Rule("буква", new List<string> {"z"}),
                new Rule("буква", new List<string> {"A"}),
                new Rule("буква", new List<string> {"B"}),
                new Rule("буква", new List<string> {"C"}),
                new Rule("буква", new List<string> {"D"}),
                new Rule("буква", new List<string> {"E"}),
                new Rule("буква", new List<string> {"F"}),
                new Rule("буква", new List<string> {"G"}),
                new Rule("буква", new List<string> {"H"}),
                new Rule("буква", new List<string> {"I"}),
                new Rule("буква", new List<string> {"J"}),
                new Rule("буква", new List<string> {"K"}),
                new Rule("буква", new List<string> {"L"}),
                new Rule("буква", new List<string> {"M"}),
                new Rule("буква", new List<string> {"N"}),
                new Rule("буква", new List<string> {"O"}),
                new Rule("буква", new List<string> {"P"}),
                new Rule("буква", new List<string> {"Q"}),
                new Rule("буква", new List<string> {"R"}),
                new Rule("буква", new List<string> {"S"}),
                new Rule("буква", new List<string> {"T"}),
                new Rule("буква", new List<string> {"U"}),
                new Rule("буква", new List<string> {"V"}),
                new Rule("буква", new List<string> {"W"}),
                new Rule("буква", new List<string> {"X"}),
                new Rule("буква", new List<string> {"Y"}),
                new Rule("буква", new List<string> {"Z"}),
            });

            // Цифра
            rulesList.AddRange(new List<Rule>
            {
                new Rule("цифра", new List<string> {"1"}),
                new Rule("цифра", new List<string> {"2"}),
                new Rule("цифра", new List<string> {"3"}),
                new Rule("цифра", new List<string> {"4"}),
                new Rule("цифра", new List<string> {"5"}),
                new Rule("цифра", new List<string> {"6"}),
                new Rule("цифра", new List<string> {"7"}),
                new Rule("цифра", new List<string> {"8"}),
                new Rule("цифра", new List<string> {"9"}),
                new Rule("цифра", new List<string> {"0"}),
            });

            // Прочие символы
            rulesList.AddRange(new List<Rule>
            {
                new Rule("прочие символы" , new List<string> { "а" }),
                new Rule("прочие символы" , new List<string> { "б" }),
                new Rule("прочие символы" , new List<string> { "в" }),
                new Rule("прочие символы" , new List<string> { "г" }),
                new Rule("прочие символы" , new List<string> { "д" }),
                new Rule("прочие символы" , new List<string> { "е" }),
                new Rule("прочие символы" , new List<string> { "ё" }),
                new Rule("прочие символы" , new List<string> { "ж" }),
                new Rule("прочие символы" , new List<string> { "з" }),
                new Rule("прочие символы" , new List<string> { "и" }),
                new Rule("прочие символы" , new List<string> { "й" }),
                new Rule("прочие символы" , new List<string> { "к" }),
                new Rule("прочие символы" , new List<string> { "л" }),
                new Rule("прочие символы" , new List<string> { "м" }),
                new Rule("прочие символы" , new List<string> { "н" }),
                new Rule("прочие символы" , new List<string> { "о" }),
                new Rule("прочие символы" , new List<string> { "п" }),
                new Rule("прочие символы" , new List<string> { "р" }),
                new Rule("прочие символы" , new List<string> { "с" }),
                new Rule("прочие символы" , new List<string> { "т" }),
                new Rule("прочие символы" , new List<string> { "у" }),
                new Rule("прочие символы" , new List<string> { "ф" }),
                new Rule("прочие символы" , new List<string> { "х" }),
                new Rule("прочие символы" , new List<string> { "ц" }),
                new Rule("прочие символы" , new List<string> { "ч" }),
                new Rule("прочие символы" , new List<string> { "ш" }),
                new Rule("прочие символы" , new List<string> { "щ" }),
                new Rule("прочие символы" , new List<string> { "ъ" }),
                new Rule("прочие символы" , new List<string> { "ы" }),
                new Rule("прочие символы" , new List<string> { "ь" }),
                new Rule("прочие символы" , new List<string> { "э" }),
                new Rule("прочие символы" , new List<string> { "ю" }),
                new Rule("прочие символы" , new List<string> { "я" }),
                new Rule("прочие символы" , new List<string> { "А" }),
                new Rule("прочие символы" , new List<string> { "Б" }),
                new Rule("прочие символы" , new List<string> { "В" }),
                new Rule("прочие символы" , new List<string> { "Г" }),
                new Rule("прочие символы" , new List<string> { "Д" }),
                new Rule("прочие символы" , new List<string> { "Е" }),
                new Rule("прочие символы" , new List<string> { "Ё" }),
                new Rule("прочие символы" , new List<string> { "Ж" }),
                new Rule("прочие символы" , new List<string> { "З" }),
                new Rule("прочие символы" , new List<string> { "И" }),
                new Rule("прочие символы" , new List<string> { "Й" }),
                new Rule("прочие символы" , new List<string> { "К" }),
                new Rule("прочие символы" , new List<string> { "Л" }),
                new Rule("прочие символы" , new List<string> { "М" }),
                new Rule("прочие символы" , new List<string> { "Н" }),
                new Rule("прочие символы" , new List<string> { "О" }),
                new Rule("прочие символы" , new List<string> { "П" }),
                new Rule("прочие символы" , new List<string> { "Р" }),
                new Rule("прочие символы" , new List<string> { "С" }),
                new Rule("прочие символы" , new List<string> { "Т" }),
                new Rule("прочие символы" , new List<string> { "У" }),
                new Rule("прочие символы" , new List<string> { "Ф" }),
                new Rule("прочие символы" , new List<string> { "Х" }),
                new Rule("прочие символы" , new List<string> { "Ц" }),
                new Rule("прочие символы" , new List<string> { "Ч" }),
                new Rule("прочие символы" , new List<string> { "Ш" }),
                new Rule("прочие символы" , new List<string> { "Щ" }),
                new Rule("прочие символы" , new List<string> { "Ъ" }),
                new Rule("прочие символы" , new List<string> { "Ы" }),
                new Rule("прочие символы" , new List<string> { "Ь" }),
                new Rule("прочие символы" , new List<string> { "Э" }),
                new Rule("прочие символы" , new List<string> { "Ю" }),
                new Rule("прочие символы" , new List<string> { "Я" }),
                new Rule("прочие символы" , new List<string> { "_" }),
                new Rule("прочие символы" , new List<string> { "!" }),
                new Rule("прочие символы" , new List<string> { "@" }),
                new Rule("прочие символы" , new List<string> { "#" }),
                new Rule("прочие символы" , new List<string> { "$" }),
                new Rule("прочие символы" , new List<string> { "%" }),
                new Rule("прочие символы" , new List<string> { "^" }),
                new Rule("прочие символы" , new List<string> { "&" }),
                new Rule("прочие символы" , new List<string> { "*" }),
                new Rule("прочие символы" , new List<string> { "(" }),
                new Rule("прочие символы" , new List<string> { ")" }),
                new Rule("прочие символы" , new List<string> { "-" }),
                new Rule("прочие символы" , new List<string> { "+" }),
                new Rule("прочие символы" , new List<string> { "[" }),
                new Rule("прочие символы" , new List<string> { "]" }),
                new Rule("прочие символы" , new List<string> { "{" }),
                new Rule("прочие символы" , new List<string> { "}" }),
                new Rule("прочие символы" , new List<string> { "~" }),
                new Rule("прочие символы" , new List<string> { "№" }),
                new Rule("прочие символы" , new List<string> { ";" }),
                new Rule("прочие символы" , new List<string> { ":" }),
                new Rule("прочие символы" , new List<string> { "?" }),
                new Rule("прочие символы" , new List<string> { "=" }),
                new Rule("прочие символы" , new List<string> { "<" }),
                new Rule("прочие символы" , new List<string> { ">" }),
                new Rule("прочие символы" , new List<string> { "." }),
                new Rule("прочие символы" , new List<string> { "," }),
                new Rule("прочие символы" , new List<string> { " " }),
            });

            return new Rules(rulesList);
        }

        static Grammar CreateGrammar()
        {
            return new Grammar(GetTerms(), GetNonterms(), GetRules());
        }

        public static Tree GetSyntaxTree(List<string> input, DebugMode debugMode = DebugMode.DISABLED)
        {
            SyntaxParser.SyntaxParser syntaxParser = new SyntaxParser.SyntaxParser(CreateGrammar(), input);

            return syntaxParser.SyntaxTree;
        }

        public static List<Error> GetErrorsFromSemanticAnalyzer(Tree syntaxTree)
        {
            SemanticAnalyzer.SemanticAnalyzer semanticAnalyzer = new SemanticAnalyzer.SemanticAnalyzer();

            List<Error> semanticErrors = semanticAnalyzer.GetErrors(syntaxTree);

            return semanticErrors;
        }

        static string GetErrorsString(List<Error> errors)
        {
            string errorsString = default;

            if (errors.Any())
            {
                for (int i = 0; i < errors.Count - 1; i++)
                {
                    errorsString += errors[i] + "\n";
                }
                errorsString += errors[^1];
            }

            return errorsString;
        }

        public static List<string> GetConversionsRaw(string filename)
        {
            List<string> convertionsRow = new List<string>();

            foreach (string line in File.ReadLines(filename))
            {
                if (line != string.Empty)
                {
                    convertionsRow.Add(line);
                }
            }

            return convertionsRow;
        }

        public static string GetPythonCode(string input)
        {
            List<Token> tokens = GetTokensFromLexer(input);

            string conversionTableFilename = "conversion_table.txt";
            List<string> conversionsRaw = GetConversionsRaw(conversionTableFilename);
            Conversions conversions = new Conversions(conversionsRaw, '*', "-");
            Generator.Generator generator = new Generator.Generator(tokens, conversions);

            List<string> outputList = generator.Generate();
            return generator.GetGeneratedString(outputList);
        }

        public static List<Error> GetSemanticErrors(string input)
        {
            List<Token> tokens = GetTokensFromLexer(input);

            List<string> syntaxParserInput = GetSyntaxParserData(tokens);
            Tree syntaxTree = GetSyntaxTree(syntaxParserInput, DebugMode.RESULT);

            List<Error> semanticErrors = GetErrorsFromSemanticAnalyzer(syntaxTree);

            return semanticErrors;
        }

        public static string GetErrorsAsString(List<Error> errors)
        {
            string result = string.Empty;

            foreach (var error in errors)
            {
                result += error + "\n";
            }

            return result;
        }

        public static List<Error> GetErrors(string input)
        {
            SyntaxParser.SyntaxParser parser = GetSyntaxParser(input);

            List<Error> errors = new List<Error>();

            errors.AddRange(parser.Errors);
            errors.AddRange(GetSemanticErrors(input));

            return errors;
        }

        public static string GetErrorsAsString(string input)
        {
            List<Error> errors = GetErrors(input);

            return GetErrorsAsString(errors);
        }

        public static SyntaxParser.SyntaxParser GetSyntaxParser(string input)
        {
            List<Token> tokens = GetTokensFromLexer(input);
            List<string> syntaxParserInput = GetSyntaxParserData(tokens);
            SyntaxParser.SyntaxParser parser = new SyntaxParser.SyntaxParser(CreateGrammar(), syntaxParserInput);

            return parser;
        }

        static void Main(string[] args)
        {
            DebugMode debugMode = DebugMode.RESULT;

            //Input
            string filename = "input.txt";
            string input = File.ReadAllText(filename);

            //Lex
            List<Token> tokens = GetTokensFromLexer(input);

            //Syntax
            List<string> syntaxParserInput = GetSyntaxParserData(tokens);
            Tree syntaxTree = GetSyntaxTree(syntaxParserInput, DebugMode.RESULT);
            syntaxTree.Print();


            switch (debugMode)
            {
                case DebugMode.DISABLED:
                    break;
                case DebugMode.RESULT:
                    PrintDebugInfo("INPUT", input, ConsoleColor.Red, true);
                    PrintDebugInfo("ERRORS", GetErrorsAsString(input), ConsoleColor.Red, true);
                    PrintDebugInfo("OUTPUT", GetPythonCode(input), ConsoleColor.Red, true);
                    break;
                case DebugMode.FULLY:
                    PrintDebugInfo("TOKENS", GetTokensAsString(tokens), DEBUG_COLOR, true);
                    PrintDebugInfo("INPUT TO SYNTAXER", GetSyntaxParserInputAsString(syntaxParserInput), DEBUG_COLOR, true);
                    syntaxTree.Print("SYNTAX");
                    Console.WriteLine();
                    PrintDebugInfo("INPUT", input, ConsoleColor.Red, true); 
                    PrintDebugInfo("ERRORS", GetErrorsAsString(input), ConsoleColor.Red, true);
                    PrintDebugInfo("OUTPUT", GetPythonCode(input), ConsoleColor.Red, true);
                    break;
                default:
                    break;
            }
        }
    }
}