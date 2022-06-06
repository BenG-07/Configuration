//-----------------------------------------------------------------------
// <copyright file="DataUnitTypeComparer.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents the comparison of two data units.</summary>
//-----------------------------------------------------------------------


namespace DUAttributes.DataUnitComparison
{
    using DUAttributes.DataUnitType;

    /// <summary>
    /// A comparer of two data units.
    /// </summary>
    public class DataUnitTypeComparer : IDataUnitVisitor<bool>
    {
        /// <summary>
        /// The data unit for comparison.
        /// </summary>
        private DataUnit dataUnit;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataUnitTypeComparer"/> class.
        /// </summary>
        /// <param name="dataUnit">The data unit for comparison.</param>
        public DataUnitTypeComparer(DataUnit dataUnit)
        {
            this.dataUnit = dataUnit;
        }

        /// <summary>
        /// Visits as a <see cref="DataSourceUnit"/>.
        /// </summary>
        /// <param name="dataSourceUnit">The <see cref="DataSourceUnit"/>.</param>
        /// <returns>Whether the visitor and visited are from the same type or not.</returns>
        public bool Visit(DataSourceUnit dataSourceUnit) => this.dataUnit.Accept(new IsDataSourceUnit());

        /// <summary>
        /// Visits as a <see cref="DataProcessingUnit"/>.
        /// </summary>
        /// <param name="dataProcessingUnit">The <see cref="DataProcessingUnit"/>.</param>
        /// <returns>Whether the visitor and visited are from the same type or not.</returns>
        public bool Visit(DataProcessingUnit dataProcessingUnit) => this.dataUnit.Accept(new IsDataProcessingUnit());

        /// <summary>
        /// Visits as a <see cref="DataVisualizationUnit"/>.
        /// </summary>
        /// <param name="dataVisualizationUnit">The <see cref="DataVisualizationUnit"/>.</param>
        /// <returns>Whether the visitor and visited are from the same type or not.</returns>
        public bool Visit(DataVisualizationUnit dataVisualizationUnit) => this.dataUnit.Accept(new IsDataVisualizationeUnit());
    }
}
