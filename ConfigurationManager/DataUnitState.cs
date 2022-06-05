//-----------------------------------------------------------------------
// <copyright file="DataUnitState.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents all possible state of a data unit.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager
{
    /// <summary>
    /// Represents all possible state of a data unit.
    /// </summary>
    public enum DataUnitState
    {
        /// <summary>
        /// Indicating the data unit is loaded but not active.
        /// </summary>
        Inactive,

        /// <summary>
        /// Indicating the data unit is loaded but won't produce data.
        /// </summary>
        Active,

        /// <summary>
        /// Indicating the data unit is loaded and active.
        /// </summary>
        Running
    }
}
