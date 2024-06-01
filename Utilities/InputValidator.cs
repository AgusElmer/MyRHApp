using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.Utilities
{
    public static class InputValidator
    {
        public static int GetValidatedInt(string prompt)
        {
            int result;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                Console.Write(prompt);
            }
            return result;
        }

        public static DateTime GetValidatedDate(string prompt)
        {
            DateTime result;
            Console.Write(prompt);
            while (!DateTime.TryParse(Console.ReadLine(), out result))
            {
                Console.WriteLine("Invalid input. Please enter a valid date (yyyy-mm-dd).");
                Console.Write(prompt);
            }
            return result;
        }

        public static string GetNonEmptyString(string prompt)
        {
            string result;
            Console.Write(prompt);
            while (string.IsNullOrWhiteSpace(result = Console.ReadLine()))
            {
                Console.WriteLine("Input cannot be empty. Please enter a valid string.");
                Console.Write(prompt);
            }
            return result;
        }
    }
}
