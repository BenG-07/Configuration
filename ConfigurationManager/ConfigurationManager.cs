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

        private Dictionary<string, DUInformation> DUInformation = new Dictionary<string, DUInformation>();

        public ConfigurationManager()
        {
            foreach (var item in this.GetAllAvailableExtensions().ToList())
            {
                this.DUInformation.Add(item.Type.FullName, item);
            }

            DUInformation.ToList().ForEach(x => Console.WriteLine($"{x.Value.Attribute}\n"));

            // Get extension
            var assembly = Assembly.LoadFrom($"{this.ExtensionPaths[0]}\\DSURandomGenerator.dll");
            var assembly2 = Assembly.LoadFrom($"{this.ExtensionPaths[1]}\\DPUAddNumbers.dll");

            // Get the class with the DataUnit Attribute and the DataSourceUnit as Type
            Type type = this.DUInformation["DSURandomGenerator.RandomGenerator"].Type;
            Type type2 = this.DUInformation["DPUAddNumbers.AddNumbers"].Type;

            // Get the DataSourceAttribute
            this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource = (EventInfo)this.GetDUByAttribute(type, typeof(DataSourceAttribute));
            this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource = (EventInfo)this.GetDUByAttribute(type2, typeof(DataSourceAttribute));
            this.DUInformation["DPUAddNumbers.AddNumbers"].callBack = (MethodInfo)this.GetDUByAttribute(type2, typeof(DataDestinationAttribute));

            // Create instance of the DSU
            this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance = Activator.CreateInstance(type);
            this.DUInformation["DPUAddNumbers.AddNumbers"].Instance = Activator.CreateInstance(type2);

            // Get the start method for the DSU
            this.DUInformation["DSURandomGenerator.RandomGenerator"].Start = type.GetMethod("Start");
            this.DUInformation["DSURandomGenerator.RandomGenerator"].Stop = type.GetMethod("Stop");

            // Get the handler
            Delegate handler = Delegate.CreateDelegate(this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));
            Delegate handler2 = Delegate.CreateDelegate(this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));

            // Subscribe to the EmittingEvent (the handler(Callbakc)) 
            this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource.AddEventHandler(this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance, handler);
            this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource.AddEventHandler(this.DUInformation["DPUAddNumbers.AddNumbers"].Instance, handler2);

            // Start the DSU
            this.DUInformation["DSURandomGenerator.RandomGenerator"].Start.Invoke(this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance, new object[] { });

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

        private MemberInfo GetDUByAttribute(Type type, Type targetAttributeType)
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
            DUInformation dUInformation = null;
            if (!this.DUInformation.TryGetValue(sender.ToString(), out dUInformation))
            {
                throw new Exception($"Unknown Callback from {sender.ToString()} class.");
            }

            Type targetType = dUInformation.Attribute.OutputDataType;
            var method = this.GetType().GetMethod("ProcessData", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(targetType);
            method.Invoke(this, new object[] { e });
        }

        private void ProcessData<T>(dynamic e)
        {
            NewValueAvailableEventArgs<T> args = (NewValueAvailableEventArgs<T>)e;

            Console.WriteLine(args.Value);
            this.DUInformation["DPUAddNumbers.AddNumbers"].callBack.Invoke(this.DUInformation["DPUAddNumbers.AddNumbers"].Instance, new object[] { args.Value });
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
