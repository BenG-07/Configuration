using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationManager
{
    public static class UserIO
    {
        public static T GetUserChoice<T>(T[] options)
        {
            int choice = 0;
            while (true)
            {
                Console.Clear();
                for (int i = 0; i < options.Length; i++)
                {
                    string pre;
                    pre = i == choice ? "> " : "  ";
                    Console.WriteLine(pre + options[i]);
                }

                switch (Console.ReadKey(false).Key)
                {
                    case ConsoleKey.UpArrow:
                        if (choice > 0)
                        {
                            choice--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (choice < options.Length - 1)
                        {
                            choice++;
                        }
                        break;

                    case ConsoleKey.Enter:
                        return options[choice];

                    default:
                        break;
                }
            }
        }
    }
}
