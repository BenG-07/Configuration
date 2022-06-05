﻿//-----------------------------------------------------------------------
// <copyright file="DataProcessingUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a data processing unit.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes.DataUnitType
{
    /// <summary>
    /// A data processing unit.
    /// </summary>
    public class DataProcessingUnit : DataUnit
    {
        /// <summary>
        /// Calls back a visitor with itself as parameter.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public override void Accept(IDataUnitVisitor visitor)
        {
            visitor.Visit(this);
        }

        /// <summary>
        /// Calls back a visitor with itself as parameter.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The result.</returns>
        public override T Accept<T>(IDataUnitVisitor<T> visitor)
        {
            return visitor.Visit(this);
        }
    }
}
