//-----------------------------------------------------------------------
// <copyright file="IsDataUnit.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents the base for a type checker for a data unit.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager.DataUnitComparison
{
    using DUAttributes.DataUnitType;

    /// <summary>
    /// The base for a data unit type checker.
    /// </summary>
    public abstract class IsDataUnit : IDataUnitVisitor<bool>
    {
        /// <summary>
        /// Returns false.
        /// </summary>
        /// <param name="dataSourceUnit">The data unit.</param>
        /// <returns>The boolean value false.</returns>
        public virtual bool Visit(DataSourceUnit dataSourceUnit) => false;

        /// <summary>
        /// Returns false.
        /// </summary>
        /// <param name="dataProcessingUnit">The data unit.</param>
        /// <returns>The boolean value false.</returns>
        public virtual bool Visit(DataProcessingUnit dataProcessingUnit) => false;

        /// <summary>
        /// Returns false.
        /// </summary>
        /// <param name="dataVisualizationUnit">The data unit.</param>
        /// <returns>The boolean value false.</returns>
        public virtual bool Visit(DataVisualizationUnit dataVisualizationUnit) => false;
    }
}
