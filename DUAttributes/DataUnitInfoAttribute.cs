using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUAttributes.DataUnitType;

namespace DUAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataUnitInfoAttribute : Attribute
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public DataUnit DataUnit { get; set; }

        public Type InputDataType { get; set; }

        public string InputDataDescription { get; set; }

        public Type OutputDataType { get; set; }

        public string OutputDataDescription { get; set; }

        public DataUnitInfoAttribute(string name, string description, DataUnitEnum dataUnitType, Type inputDataType, string inputDataDescription, Type outputDataType, string outputDataDescription)
        {
            this.Name = name;
            this.Description = description;
            switch (dataUnitType)
            {
                case DataUnitEnum.DataSourceUnit:
                    this.DataUnit = new DataSourceUnit();
                    break;

                case DataUnitEnum.DataProcessingUnit:
                    this.DataUnit = new DataProcessingUnit();
                    break;

                case DataUnitEnum.DateVisualiztaionUnit:
                    this.DataUnit = new DataVisualizationUnit();
                    break;

                default:
                    throw new NotImplementedException();
            }

            this.InputDataType = inputDataType;
            this.InputDataDescription = inputDataDescription;
            this.OutputDataType = outputDataType;
            this.OutputDataDescription = outputDataDescription;
        }

        public override string ToString()
        {
            string info = $"{nameof(this.Name)}: {this.Name}" +
                $"\n{nameof(this.Description)}: {this.Description}" +
                $"\n{nameof(this.InputDataType)}: {this.InputDataType}" +
                $"\n{nameof(this.InputDataDescription)}: {this.InputDataDescription}" +
                $"\n{nameof(this.OutputDataType)}: {this.OutputDataType}" +
                $"\n{nameof(this.OutputDataDescription)}: {this.OutputDataDescription}";
            return info;
        }
    }
}
