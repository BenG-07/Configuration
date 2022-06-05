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

        private Dictionary<string, DUInformation> DUInformation;

        private List<ActiveDataUnit> activeDataUnits;

        public ConfigurationManager()
        {
            this.DUInformation = new Dictionary<string, DUInformation>();
            this.activeDataUnits = new List<ActiveDataUnit>();

            foreach (var item in this.GetAllAvailableExtensions())
            {
                this.LoadAssembly(item);
                this.DUInformation.Add(item.Type.FullName, item);
            }

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please enter the identifier of the dataunit you want to activate! To Start the unit enter \"Start\"");
                Console.WriteLine("Following modules were found:\n");

                foreach (var item in DUInformation)
                {
                    Console.WriteLine($"Identifier: {item.Key}");
                    Console.WriteLine(item.Value.Attribute);
                    Console.WriteLine($"Active intances: {item.Value.ActiveInstances}\n");
                }

                string input = Console.ReadLine().Trim();
                if (input == "Start")
                {
                    break;
                }

                if (DUInformation.ContainsKey(input))
                {
                    DUInformation[input].ActiveInstances++;
                    this.activeDataUnits.Add(this.CreateDataUnitInstance(input));
                }
            }
            Console.Clear();

            while (true)
            {
                Console.Clear();
                Console.Write("Enter the index of the DataUnit you want to start: ");
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                Console.WriteLine();
                for (int i = 0; i < this.activeDataUnits.Count; i++)
                {
                    Console.WriteLine($"index: {i}");
                    Console.WriteLine(activeDataUnits[i]);
                    Console.WriteLine();
                }

                Console.SetCursorPosition(x, y);
                string input = Console.ReadLine();
                if (!int.TryParse(input, out int choice) || choice >= this.activeDataUnits.Count)
                {
                    continue;
                }

                this.activeDataUnits[choice].State = DataUnitState.Running;
                this.DUInformation[this.activeDataUnits[choice].Name].Start.Invoke(this.activeDataUnits[choice].Instance, new object[] { });
            }


            // Get extension
            //var assembly = Assembly.LoadFrom($"{this.ExtensionPaths[0]}\\DSURandomGenerator.dll");
            //var assembly2 = Assembly.LoadFrom($"{this.ExtensionPaths[1]}\\DPUAddNumbers.dll");

            //// Get the class with the DataUnit Attribute and the DataSourceUnit as Type
            //Type type = this.DUInformation["DSURandomGenerator.RandomGenerator"].Type;
            //Type type2 = this.DUInformation["DPUAddNumbers.AddNumbers"].Type;

            //// Get the DataSourceAttribute
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource = (EventInfo)this.GetDUByAttribute(type, typeof(DataSourceAttribute));
            //this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource = (EventInfo)this.GetDUByAttribute(type2, typeof(DataSourceAttribute));
            //this.DUInformation["DPUAddNumbers.AddNumbers"].callBack = (MethodInfo)this.GetDUByAttribute(type2, typeof(DataDestinationAttribute));

            //// Create instance of the DSU
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance = Activator.CreateInstance(type);
            //this.DUInformation["DPUAddNumbers.AddNumbers"].Instance = Activator.CreateInstance(type2);

            //// Get the start method for the DSU
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].Start = type.GetMethod("Start");
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].Stop = type.GetMethod("Stop");

            //// Get the handler
            //Delegate handler = Delegate.CreateDelegate(this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));
            //Delegate handler2 = Delegate.CreateDelegate(this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));

            //// Subscribe to the EmittingEvent (the handler(Callbakc)) 
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].DataSource.AddEventHandler(this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance, handler);
            //this.DUInformation["DPUAddNumbers.AddNumbers"].DataSource.AddEventHandler(this.DUInformation["DPUAddNumbers.AddNumbers"].Instance, handler2);

            //// Start the DSU
            //this.DUInformation["DSURandomGenerator.RandomGenerator"].Start.Invoke(this.DUInformation["DSURandomGenerator.RandomGenerator"].Instance, new object[] { });

            Console.ReadLine();
        }

        private void LoadAssembly(DUInformation info)
        {
            string path = string.Empty;

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataSourceUnit())))
            {
                path = $"{this.ExtensionPaths[0]}\\{info.Type.Module.Name}";
            }

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataProcessingUnit())))
            {
                path = $"{this.ExtensionPaths[1]}\\{info.Type.Module.Name}";
            }

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataVisualizationUnit())))
            {
                path = $"{this.ExtensionPaths[2]}\\{info.Type.Module.Name}";
            }

            //var assembly = Assembly.LoadFrom(path);

            if (info.Attribute.OutputDataType != null)
            {
                info.DataSource = (EventInfo)this.GetDUByAttribute(info.Type, typeof(DataSourceAttribute));
            }

            if (info.Attribute.InputDataType != null)
            {
                info.CallBack = (MethodInfo)this.GetDUByAttribute(info.Type, typeof(DataDestinationAttribute));
            }

            info.Start = info.Type.GetMethod("Start");
            info.Stop = info.Type.GetMethod("Stop");
        }

        private ActiveDataUnit CreateDataUnitInstance(string identifier)
        {
            var dataUnit = this.DUInformation[identifier];

            object instance = Activator.CreateInstance(dataUnit.Type);
            Delegate handler = Delegate.CreateDelegate(dataUnit.DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));
            dataUnit.DataSource.AddEventHandler(instance, handler);

            var state = DataUnitState.Active;

            return new ActiveDataUnit(identifier, this.DUInformation[identifier].Attribute.DataUnit, instance, state);
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
            //this.DUInformation["DPUAddNumbers.AddNumbers"].callBack.Invoke(this.DUInformation["DPUAddNumbers.AddNumbers"].Instance, new object[] { args.Value });
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
