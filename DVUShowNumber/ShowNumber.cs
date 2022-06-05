//-----------------------------------------------------------------------
// <copyright file="ShowNumber.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>A library extension for the Configuration-program representing a DVU that prints a number to the console.</summary>
//-----------------------------------------------------------------------

namespace DVUShowNumber
{
    using System;
    using DUAttributes;

    /// <summary>
    /// A class for the Configuration-program representing a DVU that prints a number to the console.
    /// </summary>
    [DataUnitInfo("Show Number",
        "Print a given number to the console.",
        DataUnitEnum.DataVisualiztaionUnit,
        typeof(int),
        "The number.",
        null,
        "")]
    public class ShowNumber
    {
        /// <summary>
        /// The callback for numbers, that prints the number to the console.
        /// </summary>
        /// <param name="number">The number.</param>
        [DataDestination]
        public void PrintNumber(int number)
        {
            Console.WriteLine(number);
        }
    }
}
