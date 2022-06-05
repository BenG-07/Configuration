using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUAttributes.DataUnitType;

namespace ConfigurationManager
{
    public class ActiveDataUnit
    {
        public string Name { get; set; }

        public DataUnit DataUnitType { get; set; }

        public object Instance { get; set; }

        public DataUnitState State { get; set; }

        public ActiveDataUnit(string name, DataUnit dataUnit, object instance, DataUnitState state)
        {
            this.Name = name;
            this.DataUnitType = dataUnit;
            this.Instance = instance;
            this.State = state;
        }

        public override string ToString()
        {
            return $"Name: {this.Name}" +
                $"\nType: {this.DataUnitType}" +
                $"\nState: {this.State}";
        }
    }
}
