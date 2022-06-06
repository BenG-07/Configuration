//-----------------------------------------------------------------------
// <copyright file="GeneratorThreadArguments.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>The thread arguments for the random number generator.</summary>
//-----------------------------------------------------------------------

namespace DSUTimeGenerator
{
    using System;

    /// <summary>
    /// The thread arguments for the random number generator.
    /// </summary>
    public class GeneratorThreadArguments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorThreadArguments"/> class.
        /// </summary>
        /// <param name="random">The <see cref="System.Random"/> object.</param>
        public GeneratorThreadArguments(Random random)
        {
            this.Exit = true;
            this.Random = random;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the thread shall stop or not.
        /// </summary>
        /// <value>The exit flag.</value>
        public bool Exit { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Random"/> object for the number generation.
        /// </summary>
        /// <value>The <see cref="System.Random"/> object.</value>
        public Random Random { get; set; }
    }
}
