using DUAttributes;
using DUAttributes.DataUnitType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConfigurationManager.DataUnitComparison;
using DUEvents;

namespace ConfigurationManager
{
    public class ConfigurationManager
    {
        private Dictionary<string, Type> DUs = new Dictionary<string, Type>();

        public ConfigurationManager()
        {
            // Get extension
            var assembly = Assembly.LoadFrom(@"Extensions\DSU\DSURandomGenerator.dll");

            // Get the class with the DataUnit Attribute and the DataSourceUnit as Type
            Type type = this.GetDUClass(assembly, new DataSourceUnit());

            // Get the DataSourceAttribute
            MemberInfo m = this.GetDUDataEmitter(type, typeof(DataSourceAttribute));

            // Create instance of the DSU
            object DSU = Activator.CreateInstance(type);
            
            // Get the start method for the DSU
            var startMethod = type.GetMethod("Start");

            // Get the Emitter of the values
            var eventInfo = (EventInfo)m;

            // Get the handler
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, typeof(ConfigurationManager).GetMethod("DSUCallback"));

            // After finding all necessary parts => add them to the dictionary with the output type
            DUs.Add(type.FullName, ((DataUnitInfoAttribute)type.GetCustomAttribute(typeof(DataUnitInfoAttribute))).OutputDataType);

            // Subscribe to the EmittingEvent (the handler(Callbakc)) 
            eventInfo.AddEventHandler(DSU, handler);

            // Start the DSU
            startMethod.Invoke(DSU, new object[] { });

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
            Type targetType;
            if (!this.DUs.TryGetValue(sender.ToString(), out targetType))
            {
                throw new Exception($"Unknown Callback from {sender.ToString()} class.");
            }

            var method = typeof(ConfigurationManager).GetMethod("UseData").MakeGenericMethod(targetType);
            method.Invoke(this, new object[] { e });
        }

        public void UseData<T>(EventArgs e)
        {
            NewValueAvailableEventArgs<T> args = (NewValueAvailableEventArgs<T>)e;
            Console.WriteLine(args.Value);
        }
    }
}
