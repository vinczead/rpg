using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models
{
    public class VariableAlreadyDeclaredException : Exception
    {
        public VariableAlreadyDeclaredException(string varName) : base($"Variable already declared: {varName}")
        {

        }
    }
}
