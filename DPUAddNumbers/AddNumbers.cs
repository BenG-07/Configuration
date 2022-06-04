using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DUAttributes;

namespace DPUAddNumbers
{
    [DataUnitInfo("Add numbers",
        "Add 2 numbers together.",
        DataUnitEnum.DataProcessingUnit,
        typeof(int),
        "A number",
        typeof(int),
        "The result of two numbers.")]
    public class AddNumbers
    {
        const int numberToAdd = 2;

        private int counter;

        private int sum;

        public delegate void NewValueAvailableEventHandler(object sender, dynamic e);

        [DataSource]
        public event NewValueAvailableEventHandler NewValueAvailable;

        private void FireNewNumberAvailable(int number)
        {
            var assembly = Assembly.LoadFrom(@"DUEvents.dll");
            var eventType = assembly.GetType("DUEvents.NewValueAvailableEventArgs`1").MakeGenericType(typeof(int));
            var constructor = eventType.GetConstructor(new Type[] { typeof(int) });
            var eventArgs = constructor.Invoke(new object[] { number });

            this.NewValueAvailable?.Invoke(this, eventArgs);
        }

        [DataDestination]
        public void GetNumber(int number)
        {
            sum += number;

            if (++counter % numberToAdd == 0)
            {
                this.FireNewNumberAvailable(this.sum);
                this.counter = 0;
                this.sum = 0;
            }
        }
    }
}
