//-----------------------------------------------------------------------
// <copyright file="IDataUnitVisitable.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a data unit that can be visited.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes.DataUnitType
{
    /// <summary>
    /// Represents a data unit that can be visited.
    /// </summary>
    public interface IDataUnitVisitable
    {
        /// <summary>
        /// Accepts a visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        void Accept(IDataUnitVisitor visitor);

        /// <summary>
        /// Accepts a visitor.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="visitor">The visitor.</param>
        /// <returns>The result.</returns>
        T Accept<T>(IDataUnitVisitor<T> visitor);
    }
}
