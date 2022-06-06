//-----------------------------------------------------------------------
// <copyright file="IsDataSourceUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a type checker for a data unit.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes.DataUnitComparison
{
    using DUAttributes.DataUnitType;

    /// <summary>
    /// A type checker for a data unit.
    /// </summary>
    public class IsDataSourceUnit : IsDataUnit
    {
        /// <summary>
        /// Returns true.
        /// </summary>
        /// <param name="dataSourceUnit">The data unit.</param>
        /// <returns>The boolean value true.</returns>
        public override bool Visit(DataSourceUnit dataSourceUnit) => true;
    }
}
