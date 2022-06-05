//-----------------------------------------------------------------------
// <copyright file="AddNumbers.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>A library extension for the Configuration-program representing a DPU that adds numbers up and broadcasts the sum of the numbers.</summary>
//-----------------------------------------------------------------------

namespace DPUAddNumbers
{
    using System;
    using System.Reflection;
    using DUAttributes;

    /// <summary>
    /// A class for the Configuration-program representing a DPU that adds numbers up and broadcasts the sum of the numbers.
    /// </summary>
    [DataUnitInfo("Add numbers",
        "Add 2 numbers together.",
        DataUnitEnum.DataProcessingUnit,
        typeof(int),
        "A number",
        typeof(int),
        "The result of two numbers.")]
    public class AddNumbers
    {
        /// <summary>
        /// The amount of number to add together until the new number is broadcasted.
        /// </summary>
        private const int NumberToAdd = 2;

        /// <summary>
        /// The amount of numbers taken in.
        /// </summary>
        private int counter;

        /// <summary>
        /// The current sum of all numbers.
        /// </summary>
        private int sum;

        /// <summary>
        /// The locker object to prevent multiple threads from intervening each other.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// The delegate for the <see cref="NewValueAvailable"/> event.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The eventArgs.</param>
        public delegate void NewValueAvailableEventHandler(object sender, dynamic e);

        /// <summary>
        /// The event to broadcast the new number.
        /// </summary>
        [DataSource]
        public event NewValueAvailableEventHandler NewValueAvailable;

        /// <summary>
        /// The callback for new numbers.
        /// </summary>
        /// <param name="number">The new number.</param>
        [DataDestination]
        public void GetNumber(int number)
        {
            lock (this.locker)
            {
                this.sum += number;

                if (++this.counter % NumberToAdd == 0)
                {
                    this.FireNewNumberAvailable(this.sum);
                    this.counter = 0;
                    this.sum = 0;
                }
            }
        }

        /// <summary>
        /// Fires the <see cref="NewValueAvailable"/> event.
        /// </summary>
        /// <param name="number">The number to broadcast.</param>
        private void FireNewNumberAvailable(int number)
        {
            // Getting the eventArgs from the same dll the ConfigurationManager is using.
            var assembly = Assembly.LoadFrom("DUEvents.dll");
            var eventType = assembly.GetType("DUEvents.NewValueAvailableEventArgs`1").MakeGenericType(typeof(int));
            var constructor = eventType.GetConstructor(new Type[] { typeof(int) });
            var eventArgs = constructor.Invoke(new object[] { number });

            this.NewValueAvailable?.Invoke(this, eventArgs);
        }
    }
}
