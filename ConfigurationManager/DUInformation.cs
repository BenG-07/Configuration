//-----------------------------------------------------------------------
// <copyright file="DUInformation.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents all necessary information of a data unit.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager
{
    using System;
    using System.Reflection;
    using DUAttributes;

    /// <summary>
    /// Represents all necessary information of a data unit.
    /// </summary>
    public class DUInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DUInformation"/> class.
        /// </summary>
        /// <param name="type">The type extracted from an assembly.</param>
        /// <param name="attribute">The attributes from the data unit info attribute.</param>
        public DUInformation(Type type, DataUnitInfoAttribute attribute)
        {
            this.Type = type;
            this.Attribute = attribute;
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type of the data unit.</value>
        public Type Type { get; set; }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>The attribute.</value>
        public DataUnitInfoAttribute Attribute { get; set; }

        /// <summary>
        /// Gets or sets the source of the data.
        /// </summary>
        /// <value>The source of the data.</value>
        public EventInfo DataSource { get; set; }

        /// <summary>
        /// Gets or sets the callback for new data.
        /// </summary>
        /// <value>The callback for new data.</value>
        public MethodInfo CallBack { get; set; }

        /// <summary>
        /// Gets or sets the start method.
        /// </summary>
        /// <value>The start method.</value>
        public MethodInfo Start { get; set; }

        /// <summary>
        /// Gets or sets the stop method.
        /// </summary>
        /// <value>The stop method.</value>
        public MethodInfo Stop { get; set; }

        /// <summary>
        /// Gets or sets the amount of instances from this data unit.
        /// </summary>
        /// <value>The amount of instances.</value>
        public int ActiveInstances { get; set; }
    }
}
