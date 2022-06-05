//-----------------------------------------------------------------------
// <copyright file="DataSourceAttribute.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a attribute that indicates the source for new values.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes
{
    using System;

    /// <summary>
    /// Represents a attribute that indicates  the source for new values.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
    public class DataSourceAttribute : Attribute
    {
    }
}
