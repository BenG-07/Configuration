//-----------------------------------------------------------------------
// <copyright file="NewValueAvailableEventArgs.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Defines the EventArgs to broadcast a single value of a generic type.</summary>
//-----------------------------------------------------------------------

namespace DUEvents
{
    using System;

    /// <summary>
    /// The EventArgs to broadcast a single value of a generic type.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class NewValueAvailableEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewValueAvailableEventArgs{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public NewValueAvailableEventArgs(T value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value { get; set; }
    }
}
