using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models.Instances
{
    public class Person
    {
        public delegate void NameChangedEventHandler(object sender, string s);

        public event NameChangedEventHandler NameChanged;

        private string name;

        public void SetName(string n)
        {
            name = n;
            NameChanged(this, n);
        }

        public Person(NameChangedEventHandler nameChangedEventHandler)
        {
            NameChanged += nameChangedEventHandler;
        }
    }
}
