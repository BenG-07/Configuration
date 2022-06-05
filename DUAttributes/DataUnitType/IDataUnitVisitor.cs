//-----------------------------------------------------------------------
// <copyright file="IDataUnitVisitor.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a visitor for a data unit.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes.DataUnitType
{
    /// <summary>
    /// A visitor for a data unit.
    /// </summary>
    public interface IDataUnitVisitor
    {
        /// <summary>
        /// Visits as a <see cref="DataSourceUnit"/>.
        /// </summary>
        /// <param name="dataSourceUnit">The <see cref="DataSourceUnit"/>.</param>
        void Visit(DataSourceUnit dataSourceUnit);

        /// <summary>
        /// Visits as a <see cref="DataProcessingUnit"/>.
        /// </summary>
        /// <param name="dataProcessingUnit">The <see cref="DataProcessingUnit"/>.</param>
        void Visit(DataProcessingUnit dataProcessingUnit);

        /// <summary>
        /// Visits as a <see cref="DataVisualizationUnit"/>.
        /// </summary>
        /// <param name="dataVisualizationUnit">The <see cref="DataVisualizationUnit"/>.</param>
        void Visit(DataVisualizationUnit dataVisualizationUnit);
    }

    /// <summary>
    /// A visitor for a data unit.
    /// </summary>
    /// <typeparam name="T">The return type.</typeparam>
    public interface IDataUnitVisitor<T>
    {
        /// <summary>
        /// Visits as a <see cref="DataSourceUnit"/>.
        /// </summary>
        /// <param name="dataSourceUnit">The <see cref="DataSourceUnit"/>.</param>
        /// <returns>A value of the call.</returns>
        T Visit(DataSourceUnit dataSourceUnit);

        /// <summary>
        /// Visits as a <see cref="DataProcessingUnit"/>.
        /// </summary>
        /// <param name="dataProcessingUnit">The <see cref="DataProcessingUnit"/>.</param>
        /// <returns>A value of the call.</returns>
        T Visit(DataProcessingUnit dataProcessingUnit);

        /// <summary>
        /// Visits as a <see cref="DataVisualizationUnit"/>.
        /// </summary>
        /// <param name="dataVisualizationUnit">The <see cref="DataVisualizationUnit"/>.</param>
        /// <returns>A value of the call.</returns>
        T Visit(DataVisualizationUnit dataVisualizationUnit);
    }
}
