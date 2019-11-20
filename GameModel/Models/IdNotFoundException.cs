using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameModel.Models
{
    public class IdNotFoundException : Exception
    {
        public IdNotFoundException(string id)
            : base($"Id not found: #{id}")
        //: base($"At line #{context.start.Line}, column #{context.start.Column}: Id not found: #{id}")
        {
        }
    }
}
