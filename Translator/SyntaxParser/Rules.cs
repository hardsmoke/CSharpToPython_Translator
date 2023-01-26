using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Translator.SyntaxParser
{
    public class Rules : IEnumerable<Rule>
    {
        private List<Rule> _list = new List<Rule>();

        public Rule this[int i]
        {
            get { return _list[i]; }
            private set { _list[i] = value; }
        }

        public Rules(List<Rule> list)
        {
            Add(list);
        }

        public void Add(Rule rule)
        {
            if (!_list.Contains(rule) && GetRule(rule.LeftPart, rule.RightPart) == null)
            {
                _list.Add(rule);
            }
        }

        public void Add(List<Rule> rules)
        {
            foreach (Rule rule in rules)
            {
                Add(rule);
            }
        }

        public Rule GetRule(string leftPart, List<string> rightPart)
        {
            bool IsRightPartsEqual(List<string> p1, List<string> p2)
            {
                if (p1.Count != p2.Count)
                {
                    return false;
                }

                for (int i = 0; i < p1.Count; i++)
                {
                    if (p1[i] != p2[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            foreach (Rule rule in _list)
            {
                if (rule.LeftPart == leftPart && IsRightPartsEqual(rule.RightPart, rightPart))
                {
                    return rule;
                }
            }

            return null;
        }

        public Rule GetRule(string leftPart, int serialNumber)
        {
            List<Rule> rules = GetRules(leftPart);
            if (serialNumber < rules.Count)
            {
                return rules[serialNumber];
            }

            return null;
        }

        public bool IsCorrect(List<string> terms, List<string> nonterms, bool needThrowException = false)
        {
            foreach (Rule rule in _list)
            {
                if (!rule.IsCorrect(terms, nonterms))
                {
                    if (needThrowException)
                    {
                        throw new Exception($"AHTUNG: Правило {rule} некорректно.");
                    }
                    return false;
                }
            }

            return true;
        }

        public List<Rule> GetRules(string leftPart)
        {
            List<Rule> rules = new List<Rule>();
            foreach (Rule rule in this)
            {
                if (rule.LeftPart == leftPart)
                {
                    rules.Add(rule);
                }
            }

            return rules;
        }

        public int GetRuleSerialNumber(Rule rule)
        {
            int counter = -1;
            foreach (var _rule in this)
            {
                if (_rule.LeftPart == rule.LeftPart)
                {
                    counter++;

                    if (_rule.RightPart == rule.RightPart)
                    {
                        return counter;
                    }
                }              
            }
            
            throw new Exception("Введенного правила не существует");
        }

        public int GetRuleIndex(Rule rule)
        {
            int counter = -1;
            foreach (var _rule in this)
            {
                counter++;

                if (_rule.LeftPart == rule.LeftPart && _rule.RightPart == rule.RightPart)
                {
                    return counter;
                }
            }

            throw new Exception("Введенного правила не существует");
        }

        public IEnumerator<Rule> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
