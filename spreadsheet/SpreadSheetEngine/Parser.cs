using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class Parser
    {
        private static string pattern = @"([a-zA-Z]+(\d[a-zA-Z]*)*|\d+\.\d+|\d+|[+-/*()])";
        private static string vpattern = @"([a-zA-Z]+(\d[a-zA-Z]*)*|\d+(\.\d+)?)";

        public Parser()
        {
            
        }
        public List<string> ConvertToRPN(string inOrderExp)
        {

            var matches = Regex.Matches(inOrderExp, pattern);
            string[] tokens = matches.Cast<Match>().Select(m => m.Value).ToArray();
            // The output list of tokens in RPN order
            var output = new List<string>();

            // The stack for holding operators and parentheses
            var stack = new Stack<string>();

            // For each token in the input list of tokens
            foreach (string token in tokens)
            {
                // If the token is a number or variable, add it to the output list
                if (double.TryParse(token, out _) || Regex.IsMatch(token, vpattern))
                {
                    output.Add(token);
                }
                // If the token is an opening parenthesis, push it onto the stack
                else if (token == "(")
                {
                    stack.Push(token);
                }
                // If the token is a closing parenthesis, pop all operators from the stack onto the output list until we find the matching opening parenthesis
                else if (token == ")")
                {
                    while (stack.Peek() != "(")
                    {
                        output.Add(stack.Pop());
                    }

                    stack.Pop(); // Remove the opening parenthesis from the stack
                }
                // If the token is an operator, pop all operators from the stack onto the output list that have higher or equal precedence, then push the operator onto the stack
                else
                {
                    while (stack.Count > 0 && GetPrecedence(token) <= GetPrecedence(stack.Peek()))
                    {
                        output.Add(stack.Pop());
                    }

                    stack.Push(token);
                }
            }

            // Pop all remaining operators from the stack onto the output list
            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

        // A helper function for getting the precedence of an operator
        // possible to move to the Node class for better code
        private static int GetPrecedence(string token)
        {
            if (token == "+" || token == "-")
            {
                return 1;
            }
            else if (token == "*" || token == "/")
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

    }
}
