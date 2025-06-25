using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseApp
{
    internal class Helper
    {
        public static string MakeAChoice()
        {
            Console.WriteLine(Constants.Menu);
            Console.WriteLine(Constants.Separator);
            var choice = Console.ReadLine();
            while (string.IsNullOrEmpty(choice) || !Constants.AllowedChoices.Contains(choice))
            {
                Console.WriteLine("Выберите корректный вариант:");
                choice = Console.ReadLine();
            }
            return choice;
        }
    }
}
