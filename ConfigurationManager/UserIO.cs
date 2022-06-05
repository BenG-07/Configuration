//-----------------------------------------------------------------------
// <copyright file="UserIO.cs" company="FHWN">
//     Copyright (c) FHWN. All rights reserved.
// </copyright>
// <author>Benjamin Weirer</author>
// <summary>Represents all user interactions of the ConfigurationManager for data units.</summary>
//-----------------------------------------------------------------------

namespace ConfigurationManager
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// All user interactions of the ConfigurationManager for data units.
    /// </summary>
    public static class UserIO
    {
        /// <summary>
        /// Asks the user from which data unit to create instances and adds the to the activeDataUnits list.
        /// </summary>
        /// <param name="dataUnitInformation">The data unit information.</param>
        /// <param name="activeDataUnits">The active data units.</param>
        /// <param name="createInstance">The function to create an instance of a data unit.</param>
        public static void GetCreateDataUnitInstances(Dictionary<string, DUInformation> dataUnitInformation, List<ActiveDataUnit> activeDataUnits, Func<string, ActiveDataUnit> createInstance)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please enter the identifier of the dataunit you want to activate! To Start the unit enter \"Start\"");
                Console.WriteLine("Following modules were found:\n");

                foreach (var item in dataUnitInformation)
                {
                    Console.WriteLine($"Identifier: {item.Key}");
                    Console.WriteLine(item.Value.Attribute);
                    Console.WriteLine($"Active intances: {item.Value.ActiveInstances}\n");
                }

                string input = Console.ReadLine().Trim();
                if (input.ToLower() == "start")
                {
                    break;
                }

                if (dataUnitInformation.ContainsKey(input))
                {
                    dataUnitInformation[input].ActiveInstances++;
                    activeDataUnits.Add(createInstance(input));
                }
            }

            Console.Clear();
        }

        /// <summary>
        /// Asks the user which instance of a data unit shall be connected with another and adds them to the corresponding <see cref="ActiveDataUnit"/> ConnectedDataUnitInstances list.
        /// </summary>
        /// <param name="dataUnitInformation">The data unit information.</param>
        /// <param name="activeDataUnits">The active data units.</param>
        public static void ConnectDataUnits(Dictionary<string, DUInformation> dataUnitInformation, List<ActiveDataUnit> activeDataUnits)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Enter the index of the DataUnit you want to connect: ");
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                Console.WriteLine();
                Console.Write("Enter the index of the DataUnit you want to connect to: ");
                int x2 = Console.CursorLeft;
                int y2 = Console.CursorTop;
                Console.WriteLine();

                PrintActiveDataUnits(activeDataUnits);

                Console.SetCursorPosition(x, y);
                string input = Console.ReadLine().Trim();
                if (input.ToLower() == "start")
                {
                    break;
                }

                if (!int.TryParse(input, out int source) || source >= activeDataUnits.Count)
                {
                    continue;
                }

                Console.SetCursorPosition(x2, y2);
                input = Console.ReadLine().Trim();
                if (input.ToLower() == "start")
                {
                    break;
                }

                if (!int.TryParse(input, out int desination) || desination >= activeDataUnits.Count)
                {
                    continue;
                }

                if (dataUnitInformation[activeDataUnits[source].Name].Attribute.OutputDataType != dataUnitInformation[activeDataUnits[desination].Name].Attribute.InputDataType)
                {
                    continue;
                }

                activeDataUnits[source].ConnectedDataUnitInstances.Add(activeDataUnits[desination].Instance);
            }
        }

        /// <summary>
        /// Asks the user which DSU shall be started.
        /// </summary>
        /// <param name="dataUnitInformation">The data unit information.</param>
        /// <param name="activeDataUnits">The active data units.</param>
        /// <returns>A list of actions to start the data units.</returns>
        public static List<Action> GetStartDataUnits(Dictionary<string, DUInformation> dataUnitInformation, List<ActiveDataUnit> activeDataUnits)
        {
            List<Action> tasks = new List<Action>();

            while (true)
            {
                Console.Clear();
                Console.Write("Enter the index of the DataUnit you want to start: ");
                int x = Console.CursorLeft;
                int y = Console.CursorTop;
                Console.WriteLine();

                PrintActiveDataUnits(activeDataUnits);

                Console.SetCursorPosition(x, y);
                string input = Console.ReadLine().Trim();
                if (input.ToLower() == "start")
                {
                    break;
                }

                if (!int.TryParse(input, out int choice) || choice >= activeDataUnits.Count)
                {
                    continue;
                }

                activeDataUnits[choice].State = DataUnitState.Running;
                tasks.Add(new Action(() => { dataUnitInformation[activeDataUnits[choice].Name].Start.Invoke(activeDataUnits[choice].Instance, new object[] { }); }));
            }

            Console.Clear();

            return tasks;
        }

        /// <summary>
        /// Prints the active data units with all relevant information.
        /// </summary>
        /// <param name="activeDataUnits">The active data units.</param>
        private static void PrintActiveDataUnits(List<ActiveDataUnit> activeDataUnits)
        {
            for (int i = 0; i < activeDataUnits.Count; i++)
            {
                Console.WriteLine($"index: {i}");
                Console.WriteLine(activeDataUnits[i]);
                Console.Write("Connected DataUnits: ");
                var connectedDataUnits = activeDataUnits[i].ConnectedDataUnitInstances;

                for (int j = 0; j < connectedDataUnits.Count; j++)
                {
                    Console.Write(activeDataUnits.IndexOf(activeDataUnits.Find((du) => { return du.Instance == connectedDataUnits[j]; })));

                    if (j != connectedDataUnits.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine();

                Console.WriteLine();
            }
        }
    }
}
