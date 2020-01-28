using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carpool
{
    class Program
    {
        public static bool Enter = true;
        static void Main(string[] args)
        {
            Homepage Homepage = new Homepage();
            try
            {
                while (Program.Enter)
                {
                    Helper.Print("\n\t----------Welcome to Carpool----------\n");
                    Helper.Print("Please Enter Your Option:");
                    Helper.Print("\t1)Login\n\t2)New User-Register\n\t3)Exit");
                    int choosedOption = Convert.ToInt16(Console.ReadLine());
                    if (choosedOption >= 1 && choosedOption <= 3)
                    {
                        switch (choosedOption)
                        {
                            case 1:
                                Homepage.LogIn();
                                break;
                            case 2:
                                Homepage.SignUp();
                                break;
                            case 3:
                                Program.Enter = false;
                                break;
                        }
                    }
                    else if (choosedOption < 1 || choosedOption > 3)
                    {
                        Helper.Print("\t*Please input a valid key to enter*");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
