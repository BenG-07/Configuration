//-----------------------------------------------------------------------
// <copyright file="DataUnitInfoAttribute.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents an attribute indicating the sources of a data unit.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes
{
    using System;
    using DUAttributes.DataUnitType;

    /// <summary>
    /// Represents an attribute indicating the sources of a data unit.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DataUnitInfoAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataUnitInfoAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the data unit.</param>
        /// <param name="description">The description of the data unit.</param>
        /// <param name="dataUnitType">The type of the data unit.</param>
        /// <param name="inputDataType">The input data type.</param>
        /// <param name="inputDataDescription">The description of the input data.</param>
        /// <param name="outputDataType">The output data type.</param>
        /// <param name="outputDataDescription">The description´of the output data.</param>
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

                case DataUnitEnum.DataVisualiztaionUnit:
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

        /// <summary>
        /// Gets or sets the name of the data unit.
        /// </summary>
        /// <value>The name of the data unit.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the data unit.
        /// </summary>
        /// <value>The description of the data unit.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the data unit.
        /// </summary>
        /// <value>The type of the data unit.</value>
        public DataUnit DataUnit { get; set; }

        /// <summary>
        /// Gets or sets the type of the input data.
        /// </summary>
        /// <value>The name of the type of the input data.</value>
        public Type InputDataType { get; set; }

        /// <summary>
        /// Gets or sets the description of the input data.
        /// </summary>
        /// <value>The name of the description of the input data.</value>
        public string InputDataDescription { get; set; }

        /// <summary>
        /// Gets or sets the type of the output data.
        /// </summary>
        /// <value>The name of the type of the output data.</value>
        public Type OutputDataType { get; set; }

        /// <summary>
        /// Gets or sets the description of the output data.
        /// </summary>
        /// <value>The name of the description of the output data.</value>
        public string OutputDataDescription { get; set; }

        /// <summary>
        /// Converts the attribute to a string.
        /// </summary>
        /// <returns>A string with all relevant information.</returns>
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
