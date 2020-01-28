using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Carpool
{
    public static class Helper
    {
        public static void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        public static void ValidateMobileNumber(string readInput, out string input)
        {
            bool isValid = false;
            input = readInput;
            while (isValid != true)
            {
                string validInput = @"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9] 
                {2}[0-9]{8}$)|(^[0-9]{3}-[0-9]{4}-[0-9]{4}$)";
                Regex regex = new Regex(validInput);
                isValid = regex.IsMatch(input);
                if(isValid == false)
                {
                    Print($"Please enter a valid in format");
                    input = Console.ReadLine();
                }
            }
        }
    }
}
