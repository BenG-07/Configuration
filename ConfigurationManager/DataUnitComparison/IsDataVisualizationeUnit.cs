//-----------------------------------------------------------------------
// <copyright file="IsDataVisualizationeUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a type checker for a data unit.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager.DataUnitComparison
{
    using DUAttributes.DataUnitType;

    /// <summary>
    /// A type checker for a data unit.
    /// </summary>
    public class IsDataVisualizationeUnit : IsDataUnit
    {
        /// <summary>
        /// Returns true.
        /// </summary>
        /// <param name="dataVisualizationUnit">The data unit.</param>
        /// <returns>The boolean value true.</returns>
        public override bool Visit(DataVisualizationUnit dataVisualizationUnit) => true;
    }
}
