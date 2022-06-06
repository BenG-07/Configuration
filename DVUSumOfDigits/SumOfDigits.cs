//-----------------------------------------------------------------------
// <copyright file="SumOfDigits.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>A library extension for the Configuration-program representing a DVU that prints the sum of digits to the console.</summary>
//-----------------------------------------------------------------------

namespace DVUSumOfDigits
{
    using System;
    using DUAttributes;

    /// <summary>
    /// A class for the Configuration-program representing a DVU that prints the sum of digits to the console.
    /// </summary>
    [DataUnitInfo("Sum of digits.",
        "Print the sum of digits to the console.",
        DataUnitEnum.DataVisualiztaionUnit,
        typeof(int),
        "The number.",
        null,
        "")]
    public class SumOfDigits
    {
        /// <summary>
        /// The callback for numbers, that prints the number to the console.
        /// </summary>
        /// <param name="number">The number.</param>
        [DataDestination]
        public void PrintNumber(int number)
        {
            int sum = 0;
            while (number != 0)
            {
                sum += number % 10;
                number /= 10;
            }

            Console.WriteLine(sum);
        }
    }
}
