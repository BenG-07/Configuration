//-----------------------------------------------------------------------
// <copyright file="DataDestinationAttribute.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a attribute that indicates the callback.</summary>
//-----------------------------------------------------------------------

namespace DUAttributes
{
    using System;

    /// <summary>
    /// Represents a attribute that indicates the callback.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DataDestinationAttribute : Attribute
    {
    }
}
