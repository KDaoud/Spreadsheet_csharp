using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadSheetEngine
{
    public class VariableNode : Node
    {
        private string name;
        private Dictionary<string, double> variables;

        public VariableNode(string name, ref Dictionary<string, double> variables)
        {
            this.name = name;
            this.variables = variables;
        }

        public override double Evaluate()
        {
            if (variables.ContainsKey(this.name))
            {
                return variables[name];
            }
            else
            {
                return 0.0;
            }
            
        }
    }
}
