using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SpreadSheetEngine
{
    public class ExpressionTree
    {
        private Node _root;
        private Dictionary<string, double> variables = new();

        public Dictionary<string, double> Variables 
        { 
            get => variables; 
            set => variables = value;
        }

        //parser
        Parser parser = new Parser();

        //factory
        BinaryOperatorFactory binaryOperatorFactory = new BinaryOperatorFactory();

        //constructor to construct the tree from the specific expression
        public ExpressionTree(Node r)
        {
            this._root = r;
        }

        public ExpressionTree(string infix)
        {
            this._root = BuildTree(infix);
        }

        //Sets the specified variable within the ExpressionTree variables dictionary
        public void SetVariable(string variableName, double variableValue)
        {
            this.variables[variableName] = variableValue;
        }

        public double Evaluate()
        {
            return _root.Evaluate();
        }

        public Node BuildTree(string infix)
        {
            List<string> postfix = parser.ConvertToRPN(infix);
            // Stack for building the tree
            Stack<Node> operands = new Stack<Node>();

            foreach(string token in postfix)
            {
                if (token.Length == 1 && isOperator(token[0]))
                {
                    Node right = operands.Pop();
                    Node left = operands.Pop();
                    BinaryOperatorNode node = BinaryOperatorFactory.CreateBinaryNode(left, right, token[0]);
                   
                    operands.Push(node);
                }
                else
                {
                    if (double.TryParse(token, out var num))
                    {
                        operands.Push(new ConstantNode(num));
                    }
                    else
                    {
                        operands.Push(new VariableNode(token, ref this.variables));
                        this.variables.Add(token, 0.0);
                    }
                }
            }

            return operands.Pop();

        }

        public bool isOperator(char c)
        {
            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                    return true;
                default:
                    return false;
            }
        }
    }
}
