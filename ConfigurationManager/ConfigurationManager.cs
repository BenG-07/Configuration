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
using System.IO;
using System.Threading;

namespace ConfigurationManager
{
    public class ConfigurationManager
    {
        private readonly string[] ExtensionPaths = { @"Extensions\DSU", @"Extensions\DPU", @"Extensions\DVU" };

        private Dictionary<string, Type> DUs = new Dictionary<string, Type>();

        private List<DUInformation> DUInformation;

        public ConfigurationManager()
        {
            foreach (var item in this.DUInformation = this.GetAllAvailableExtensions().ToList())
            {
                DUs.Add(item.Type.FullName, item.Attribute.OutputDataType);
            }

            DUInformation.ForEach(x => Console.WriteLine($"{x.Attribute}\n"));

            // Get extension
            var assembly = Assembly.LoadFrom(@"Extensions\DSU\DSURandomGenerator.dll");

            // Get the class with the DataUnit Attribute and the DataSourceUnit as Type
            Type type = this.GetDUInformation(assembly).Where(info => (info.Attribute).DataUnit.Accept(new DataUnitTypeComparer(new DataSourceUnit()))).First().Type;

            // Get the DataSourceAttribute
            MemberInfo m = this.GetDUDataEmitter(type, typeof(DataSourceAttribute));

            // Create instance of the DSU
            object DSU = Activator.CreateInstance(type);
            
            // Get the start method for the DSU
            var startMethod = type.GetMethod("Start");

            // Get the Emitter of the values
            var eventInfo = (EventInfo)m;

            // Get the handler
            Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));

            // Subscribe to the EmittingEvent (the handler(Callbakc)) 
            eventInfo.AddEventHandler(DSU, handler);

            // Start the DSU
            startMethod.Invoke(DSU, new object[] { });

            Console.ReadLine();
        }

        private IEnumerable<DUInformation> GetDUInformation(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                foreach (var attribute in type.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(DataUnitInfoAttribute))
                    {
                        yield return new DUInformation(type, (DataUnitInfoAttribute)attribute);
                    }
                }
            }

            yield break;
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

        public void DSUCallback(object sender, dynamic e)
        {
            Type targetType;
            if (!this.DUs.TryGetValue(sender.ToString(), out targetType))
            {
                throw new Exception($"Unknown Callback from {sender.ToString()} class.");
            }

            var method = this.GetType().GetMethod("ProcessData", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(targetType);
            method.Invoke(this, new object[] { e });
        }

        private void ProcessData<T>(dynamic e)
        {
            NewValueAvailableEventArgs<T> args = (NewValueAvailableEventArgs<T>)e;
            Console.WriteLine(args.Value);
        }

        private IEnumerable<DUInformation> GetAllAvailableExtensions()
        {
            foreach (var path in ExtensionPaths)
            {
                foreach (var libarary in Directory.GetFiles(path, "*.dll"))
                {
                    foreach (var item in this.GetDUInformation(Assembly.LoadFrom(libarary)))
                    {
                        yield return item;
                    }
                }

                foreach (var executable in Directory.GetFiles(path, "*.exe"))
                {
                    foreach (var item in this.GetDUInformation(Assembly.LoadFrom(executable)))
                    {
                        yield return item;
                    }
                }
            }

            yield break;
        }
    }
}
