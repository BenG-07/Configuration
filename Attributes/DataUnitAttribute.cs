using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataUnitAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DataUnitType DataUnitType { get; set; }

        public Type InputDataType { get; set; }

        public string InputDataDescription { get; set; }

        public Type OutputDataType { get; set; }

        public string OutputDataDescription { get; set; }

        public DataUnitAttribute(string name, string description, DataUnitType dataUnitType, Type inputDataType, string inputDataDescription, Type outputDataType, string outputDataDescription)
        {
            this.Name = name;
            this.Description = description;
            this.DataUnitType = dataUnitType;
            this.InputDataType = inputDataType;
            this.InputDataDescription = inputDataDescription;
            this.OutputDataType = outputDataType;
            this.OutputDataDescription = outputDataDescription;
        }
    }
}
