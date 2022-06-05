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

            UserIO.GetCreateDataUnitInstances(this.DUInformation, this.activeDataUnits, this.CreateDataUnitInstance);
            UserIO.ConnectDataUnits(this.DUInformation, this.activeDataUnits);
            UserIO.GetStartDataUnits(this.DUInformation, this.activeDataUnits).ForEach(a => a());

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
            if (dataUnit.DataSource != null)
            {
                Delegate handler = Delegate.CreateDelegate(dataUnit.DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));
                dataUnit.DataSource.AddEventHandler(instance, handler);
            }

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
            var method = this.GetType().GetMethod($"{nameof(SendData)}", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(targetType);
            method.Invoke(this, new object[] { sender, e });
        }

        private void SendData<T>(object sender, dynamic e)
        {
            NewValueAvailableEventArgs<T> args = (NewValueAvailableEventArgs<T>)e;

            this.activeDataUnits.Find(u => u.Instance == sender).ConnectedDataUnitInstances.ForEach(i => this.DUInformation[this.activeDataUnits.Find(u => u.Instance == i).Name].CallBack.Invoke(i, new object[] { args.Value }));
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
