using DUAttributes;
using DUAttributes.DataUnitType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigurationManager.DataUnitComparison;

namespace ConfigurationManager
{
    public class ConfigurationManager
    {
        public ConfigurationManager()
        {
            var assembly = Assembly.LoadFrom(@"Extensions\DSU\DSURandomGenerator.dll");
            Type type = this.GetDUClass(assembly, new DataSourceUnit());
            MemberInfo m = this.GetDUDataEmitter(type, typeof(DataSourceAttribute));

            object DSU = Activator.CreateInstance(type);
            
            var startMethod = type.GetMethod("Start");
            var eventInfo = (EventInfo)m;
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, typeof(ConfigurationManager).GetMethod("DSUCallback"));
            eventInfo.AddEventHandler(DSU, handler);
            startMethod.Invoke(DSU, new object[] { });
            Console.WriteLine(m.Name);

            Console.ReadLine();
        }

        private Type GetDUClass(Assembly assembly, DataUnit targetUnit)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var attribute in type.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(DataUnitInfoAttribute) && ((DataUnitInfoAttribute)attribute).DataUnit.Accept(new DataUnitTypeComparer(targetUnit)))
                    {
                        return type;
                    }
                }
            }

            throw new Exception($"No Class with Attribute {nameof(DataUnitInfoAttribute)} found!");
        }

        private MemberInfo GetDUDataEmitter(Type type, Type targetAttributeType)
        {
            foreach (var member in type.GetMembers())
            {
                foreach (var attribute in member.GetCustomAttributes())
                {
                    if (attribute.GetType() == targetAttributeType)
                    {
                        return member;
                    }
                }
            }

            throw new Exception($"No Event with Attribute {nameof(targetAttributeType)} found!");
        }

        public void DSUCallback(object sender, EventArgs e)
        {
            Console.WriteLine(e);
        }
    }
}
