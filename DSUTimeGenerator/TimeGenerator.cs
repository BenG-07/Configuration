﻿//-----------------------------------------------------------------------
// <copyright file="TimeGenerator.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>A library extension for the Configuration-program representing a DSU that broadcasts the current unix timestamp every few seconds.</summary>
//-----------------------------------------------------------------------

namespace DSUTimeGenerator
{
    using System;
    using System.Reflection;
    using System.Threading;
    using DUAttributes;

    /// <summary>
    /// A class for the Configuration-program representing a DSU that broadcasts the current unix timestamp every few seconds.
    /// </summary>
    [DataUnitInfo("Time generator",
        "Broadcasts the current unix timestamp every few seconds.",
        DataUnitEnum.DataSourceUnit,
        null,
        "",
        typeof(int),
        "The current unix timestamp.")]
    public class TimeGenerator
    {
        /// <summary>
        /// The thread to run the worker that generates random numbers.
        /// </summary>
        private Thread generatorThread;

        /// <summary>
        /// The thread arguments for the <see cref="generatorThread"/>.
        /// </summary>
        private GeneratorThreadArguments generatorThreadArguments;

        /// <summary>
        /// The random object to generate random numbers.
        /// </summary>
        private Random random;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeGenerator"/> class.
        /// </summary>
        public TimeGenerator()
        {
            this.random = new Random();
        }

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
        /// Starts the <see cref="generatorThread"/>.
        /// </summary>
        public void Start()
        {
            if (this.generatorThread != null && this.generatorThread.IsAlive)
            {
                return;
            }

            this.generatorThread = new Thread(this.GeneratorWorker);
            this.generatorThreadArguments = new GeneratorThreadArguments(this.random) { Exit = false };
            this.generatorThread.Start(this.generatorThreadArguments);
        }

        /// <summary>
        /// Sends a stop signal to the <see cref="generatorThread"/>.
        /// </summary>
        public void Stop()
        {
            if (this.generatorThread == null || !this.generatorThread.IsAlive)
            {
                return;
            }

            this.generatorThreadArguments.Exit = true;
        }

        /// <summary>
        /// Generates a new number each second and fires the <see cref="NewValueAvailable"/>.
        /// </summary>
        /// <param name="data">The arguments for the worker.</param>
        private void GeneratorWorker(object data)
        {
            GeneratorThreadArguments args;

            try
            {
                args = (GeneratorThreadArguments)data;
            }
            catch (Exception)
            {
                throw;
            }

            Random random = args.Random;

            while (!args.Exit)
            {
                this.FireNewNumberAvailable((int)DateTimeOffset.Now.ToUnixTimeSeconds());

                Thread.Sleep(random.Next(1000, 5000));
            }
        }

        /// <summary>
        /// Fires the <see cref="NewValueAvailable"/>.
        /// </summary>
        /// <param name="number">The generated number.</param>
        private void FireNewNumberAvailable(int number)
        {
            var assembly = Assembly.LoadFrom(@"DUEvents.dll");
            var eventType = assembly.GetType("DUEvents.NewValueAvailableEventArgs`1").MakeGenericType(typeof(int));
            var constructor = eventType.GetConstructor(new Type[] { typeof(int) });
            var eventArgs = constructor.Invoke(new object[] { number });

            this.NewValueAvailable?.Invoke(this, eventArgs);
        }
    }
}
