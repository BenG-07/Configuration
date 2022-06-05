using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManager
{
    public static class UserIO
    {
        public static void GetCreateDataUnitInstances(Dictionary<string, DUInformation> dUInformation, List<ActiveDataUnit> activeDataUnits, Func<string, ActiveDataUnit> createInstance)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Please enter the identifier of the dataunit you want to activate! To Start the unit enter \"Start\"");
                Console.WriteLine("Following modules were found:\n");

                foreach (var item in dUInformation)
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

                if (dUInformation.ContainsKey(input))
                {
                    dUInformation[input].ActiveInstances++;
                    activeDataUnits.Add(createInstance(input));
                }
            }

            Console.Clear();
        }

        public static void ConnectDataUnits(Dictionary<string, DUInformation> dUInformation, List<ActiveDataUnit> activeDataUnits)
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

                if (dUInformation[activeDataUnits[source].Name].Attribute.OutputDataType != dUInformation[activeDataUnits[desination].Name].Attribute.InputDataType)
                {
                    continue;
                }

                activeDataUnits[source].ConnectedDataUnitInstances.Add(activeDataUnits[desination].Instance);
            }
        }

        public static List<Action> GetStartDataUnits(Dictionary<string, DUInformation> dUInformation, List<ActiveDataUnit> activeDataUnits)
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
                tasks.Add(new Action(() => { dUInformation[activeDataUnits[choice].Name].Start.Invoke(activeDataUnits[choice].Instance, new object[] { }); }));
            }

            Console.Clear();

            return tasks;
        }

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
