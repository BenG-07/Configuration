//-----------------------------------------------------------------------
// <copyright file="ConfigurationManager.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents a manager for multiple data units.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using DataUnitComparison;
    using DUAttributes;
    using DUAttributes.DataUnitType;
    using DUEvents;

    /// <summary>
    /// Manages multiple data units.
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// The paths to the extensions.
        /// </summary>
        private readonly string[] extensionPaths = { @"Extensions\DSU", @"Extensions\DPU", @"Extensions\DVU" };

        /// <summary>
        /// The information for a data unit with the name as key.
        /// </summary>
        private Dictionary<string, DUInformation> dataUnitInformation;

        /// <summary>
        /// The activated data units.
        /// </summary>
        private List<ActiveDataUnit> activeDataUnits;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationManager"/> class.
        /// </summary>
        public ConfigurationManager()
        {
            this.dataUnitInformation = new Dictionary<string, DUInformation>();
            this.activeDataUnits = new List<ActiveDataUnit>();

            foreach (var item in this.GetAllAvailableExtensions())
            {
                this.LoadAssembly(item);
                this.dataUnitInformation.Add(item.Type.FullName, item);
            }

            UserIO.GetCreateDataUnitInstances(this.dataUnitInformation, this.activeDataUnits, this.CreateDataUnitInstance);
            UserIO.ConnectDataUnits(this.dataUnitInformation, this.activeDataUnits);
            UserIO.GetStartDataUnits(this.dataUnitInformation, this.activeDataUnits).ForEach(a => a());

            Console.ReadLine();
        }

        /// <summary>
        /// The callback for all data units.
        /// </summary>
        /// <param name="sender">The instance.</param>
        /// <param name="e">The eventArgs.</param>
        public void DSUCallback(object sender, dynamic e)
        {
            DUInformation dataUnitInformation = null;
            if (!this.dataUnitInformation.TryGetValue(sender.ToString(), out dataUnitInformation))
            {
                throw new Exception($"Unknown Callback from {sender.ToString()} class.");
            }

            Type targetType = dataUnitInformation.Attribute.OutputDataType;
            var method = this.GetType().GetMethod($"{nameof(SendData)}", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(targetType);
            method.Invoke(this, new object[] { sender, e });
        }

        /// <summary>
        /// Sends the data to all subscribers of the data unit.
        /// </summary>
        /// <typeparam name="T">The type of the value sent.</typeparam>
        /// <param name="sender">The instance of the sender.</param>
        /// <param name="e">The eventArgs.</param>
        private void SendData<T>(object sender, dynamic e)
        {
            NewValueAvailableEventArgs<T> args = (NewValueAvailableEventArgs<T>)e;

            this.activeDataUnits.Find(u => u.Instance == sender).ConnectedDataUnitInstances.ForEach(i => this.dataUnitInformation[this.activeDataUnits.Find(u => u.Instance == i).Name].CallBack.Invoke(i, new object[] { args.Value }));
        }

        /// <summary>
        /// Loads all assembly information, relevant for the <see cref="ConfigurationManager"/>.
        /// </summary>
        /// <param name="info">The data unit info object to populate.</param>
        private void LoadAssembly(DUInformation info)
        {
            string path = string.Empty;

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataSourceUnit())))
            {
                path = $"{this.extensionPaths[0]}\\{info.Type.Module.Name}";
            }

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataProcessingUnit())))
            {
                path = $"{this.extensionPaths[1]}\\{info.Type.Module.Name}";
            }

            if (info.Attribute.DataUnit.Accept(new DataUnitTypeComparer(new DataVisualizationUnit())))
            {
                path = $"{this.extensionPaths[2]}\\{info.Type.Module.Name}";
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

        /// <summary>
        /// Creates a new instance of a data unit.
        /// </summary>
        /// <param name="identifier">The name of the data unit to create.</param>
        /// <returns>The instance.</returns>
        private ActiveDataUnit CreateDataUnitInstance(string identifier)
        {
            var dataUnit = this.dataUnitInformation[identifier];

            object instance = Activator.CreateInstance(dataUnit.Type);
            if (dataUnit.DataSource != null)
            {
                Delegate handler = Delegate.CreateDelegate(dataUnit.DataSource.EventHandlerType, this, this.GetType().GetMethod("DSUCallback"));
                dataUnit.DataSource.AddEventHandler(instance, handler);
            }

            var state = DataUnitState.Active;

            return new ActiveDataUnit(identifier, this.dataUnitInformation[identifier].Attribute.DataUnit, instance, state);
        }

        /// <summary>
        /// Gets all information about class with the <see cref="DataUnitInfoAttribute"/> of an assembly .
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The information.</returns>
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

        /// <summary>
        /// Gets the member info of an attribute of a type from an assembly.
        /// </summary>
        /// <param name="type">The type form the assembly.</param>
        /// <param name="targetAttributeType">The target type.</param>
        /// <returns>The member info.</returns>
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

        /// <summary>
        /// Gets all available extensions from the defined folders.
        /// </summary>
        /// <returns>The extensions.</returns>
        private IEnumerable<DUInformation> GetAllAvailableExtensions()
        {
            foreach (var path in this.extensionPaths)
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
