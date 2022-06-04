using DUAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManager
{
    public class DUInformation
    {
        public Type Type { get; set; }

        public DataUnitInfoAttribute Attribute { get; set; }

        public DUInformation(Type type, DataUnitInfoAttribute attribute)
        {
            this.Type = type;
            this.Attribute = attribute;
        }
    }
}
