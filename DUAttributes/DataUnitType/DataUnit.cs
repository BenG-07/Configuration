//-----------------------------------------------------------------------
// <copyright file="DataUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents the base of a data unit.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes.DataUnitType
{
    /// <summary>
    /// Represents the base of a data unit.
    /// </summary>
    public abstract class DataUnit : IDataUnitVisitable
    {
        /// <summary>
        /// Accepts a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IDataUnitVisitor visitor);

        /// <summary>
        /// Accepts a visitor.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The result.</returns>
        public abstract T Accept<T>(IDataUnitVisitor<T> visitor);

        /// <summary>
        /// Converts the data unit type to a string.
        /// </summary>
        /// <returns>The data unit type as a string.</returns>
        public override string ToString() => this.GetType().Name;
    }
}
