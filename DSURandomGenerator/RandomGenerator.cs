﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DUAttributes;
using DUAttributes.DataUnitType;

namespace DSURandomGenerator
{
    [DataUnitInfo("Random value generator", 
        "Generates a random number between 1 and 10 every second.", 
        DataUnitEnum.DataSourceUnit, 
        null, 
        "", 
        typeof(int), 
        "A random value between 1 and 10.")]
    public class RandomGenerator
    {
        private Thread generatorThread;

        private GeneratorThreadArguments generatorThreadArguments;

        public delegate void NewValueAvailableEventHandler(object sender, dynamic e);

        [DataSource]
        public event NewValueAvailableEventHandler NewValueAvailable;

        public void Start()
        {
            if (this.generatorThread != null && this.generatorThread.IsAlive)
            {
                return;
            }

            this.generatorThread = new Thread(this.GeneratorWorker);
            this.generatorThreadArguments = new GeneratorThreadArguments() { Exit = false };
            this.generatorThread.Start(this.generatorThreadArguments);
        }

        public void Stop()
        {
            if (this.generatorThread == null || !this.generatorThread.IsAlive)
            {
                return;
            }

            this.generatorThreadArguments.Exit = true;
        }

        private void GeneratorWorker(object data)
        {
            GeneratorThreadArguments args;
            Random random = new Random();

            try
            {
                args = (GeneratorThreadArguments)data;
            }
            catch (Exception)
            {
                throw;
            }

            while (!args.Exit)
            {
                this.FireNewNumberAvailable(random.Next(1, 11));

                Thread.Sleep(1000);
            }
        }

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
