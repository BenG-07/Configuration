//-----------------------------------------------------------------------
// <copyright file="ActiveDataUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents an active data unit with an instance.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager
{
    using System.Collections.Generic;
    using DUAttributes.DataUnitType;

    /// <summary>
    /// A active data unit with its instance.
    /// </summary>
    public class ActiveDataUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActiveDataUnit"/> class.
        /// </summary>
        /// <param name="name">The name of the data unit.</param>
        /// <param name="dataUnit">The data unit.</param>
        /// <param name="instance">The instance of the data unit.</param>
        /// <param name="state">The current state of the data unit.</param>
        public ActiveDataUnit(string name, DataUnit dataUnit, object instance, DataUnitState state)
        {
            this.Name = name;
            this.DataUnitType = dataUnit;
            this.Instance = instance;
            this.State = state;
            this.ConnectedDataUnitInstances = new List<object>();
        }

        /// <summary>
        /// Gets or sets the name of the data unit.
        /// </summary>
        /// <value>The data unit name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data unit.
        /// </summary>
        /// <value>The data unit.</value>
        public DataUnit DataUnitType { get; set; }

        /// <summary>
        /// Gets or sets the instance of the data unit.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance { get; set; }

        /// <summary>
        /// Gets or sets the state of the data unit.
        /// </summary>
        /// <value>The state.</value>
        public DataUnitState State { get; set; }

        /// <summary>
        /// Gets or sets the list of the references to the instances of the data unit subscribers.
        /// </summary>
        /// <value>The list of the instances.</value>
        public List<object> ConnectedDataUnitInstances { get; set; }

        /// <summary>
        /// Converts the active data unit in a string format.
        /// </summary>
        /// <returns>The string format.</returns>
        public override string ToString()
        {
            return $"Name: {this.Name}" +
                $"\nType: {this.DataUnitType}" +
                $"\nState: {this.State}";
        }
    }
}
