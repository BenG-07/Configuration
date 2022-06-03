using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUAttributes
{
    [AttributeUsage(AttributeTargets.Event)]
    public class DataSourceAttribute : Attribute
    {
    }
}
