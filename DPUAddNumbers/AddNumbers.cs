using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUAttributes;
using DUEvents;

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

        public delegate void NewValueAvailableEventHandler(object sender, NewValueAvailableEventArgs<int> e);

        [DataSource]
        public event NewValueAvailableEventHandler NewValueAvailable;

        private void FireNewNumberAvailable(int number)
        {
            this.NewValueAvailable?.Invoke(this, new NewValueAvailableEventArgs<int>(number));
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
