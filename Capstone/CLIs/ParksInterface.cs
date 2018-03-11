using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;

namespace Capstone
{
    public class ParksInterface
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        int customerSelection = -1;
        int inputAsAnInt = -1;
        int milliseconds = 1000;

        public void RunCLI()
        {
            // Prints Opening menu sequence, calls method to list the available parks and prompts user to enter a number to select a park //
            // customerSelection subtracts 1 from user input to set it equal to the selected park's index # in the list //
            PrintHeader();
            Console.WriteLine();
            PrintParksMenu();
            Console.WriteLine();
            List<Park> listOfAllParks = ListAllParks();
            Console.WriteLine();
            string input = CLIHelper.GetString("Enter a number: ").ToUpper();  
            
            if (input == "Q")
            {
                Console.WriteLine("Exiting Program, please wait!");
                return;
            }
            else if (!int.TryParse(input, out int x))
            {
                Console.WriteLine("Invalid input, retry!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                RunCLI();
            }
            else if (int.Parse(input) > listOfAllParks.Count)
            {
                Console.WriteLine("That park does not exist, retry!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                RunCLI();
            }
            else
            {
                int inputAsAnInt = int.Parse(input);
                customerSelection = inputAsAnInt - 1;
                Console.Clear();
                // calls the method DisplayParkInfo to display the information for the selected park //
                List<Park> customerParkSelection = DisplayParkInfo();
                Console.WriteLine();
                // stores the selected park as a constructor and carries it to the campgrounds submenu //
                CampgroundsInterface campgroundsSubmenu = new CampgroundsInterface(customerParkSelection[customerSelection]);
                campgroundsSubmenu.Display();
            }
        }

        // this method connects to the DAL class to store (in a list) and display the info on the selected park //
        private List<Park> DisplayParkInfo()
        {
            ParkSqlDAL dal = new ParkSqlDAL(connectionString);
            List<Park> parks = dal.GetParks();

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("              PARK INFORMATION"               );            
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("{0, -20}{1, 25}", $"Name: ", $"{parks[customerSelection].name.ToString().ToUpper()}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("{0, -20}{1, 25}", $"Location:", $"{parks[customerSelection].location}");
            Console.WriteLine("{0, -20}{1, 25}", $"Established:", $"{parks[customerSelection].establish_date.ToString()}");
            Console.WriteLine("{0, -20}{1, 25}", $"Area: ", $"{parks[customerSelection].area.ToString()}");
            Console.WriteLine("{0, -20}{1, 25}", $"Annual Visitors:", $"{parks[customerSelection].visitors.ToString()}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();           
            Console.WriteLine(parks[customerSelection].description);
            return parks;
        }

        // this method connects to the DAL class to store (in a list) and display all parks for the user //
        private List<Park> ListAllParks()
        {
            ParkSqlDAL dal = new ParkSqlDAL(connectionString);
            List<Park> parks = dal.GetParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine("{0, -3}{1, -20}", $"{park.park_id.ToString()})", $"{park.name}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine("{0, -3}{1, -20}", $"Q)", $"quit");
            return parks;
        }

        private void PrintHeader()
        {
            Console.WriteLine(@" _______                      __                        _______   _______  ");
            Console.WriteLine(@"/       \                    /  |                      /      /\ /       \ ");
            Console.WriteLine(@"$$$$$$$  | ______    ______  $$ |   __   _______       $$$$$$$  |$$$$$$$  |");
            Console.WriteLine(@"$$ |__$$ |/      \  /      \ $$ |  /  | /       |      $$ |  $$ |$$ |__$$ |");
            Console.WriteLine(@"$$    $$/ $$$$$$  |/$$$$$$  |$$ |_/$$/ /$$$$$$$/       $$ |  $$ |$$    $$< ");
            Console.WriteLine(@"$$$$$$$/   /   $$ |$$ |  $$/ $$   $$<  $$      \       $$ |  $$ |$$$$$$$  |");   
            Console.WriteLine(@"$$ |      $$$$$$$ |$$ |      $$$$$$  \  $$$$$$  |      $$ |__$$ |$$ |__$$ |");
            Console.WriteLine(@"$$ |      $$   $$ |$$ |      $$ | $$  |/     $$/       $$    $$/ $$    $$/ ");
            Console.WriteLine(@"$$/       $$$$$$$/ $$/       $$/   $$/ $$$$$$$/        $$$$$$$/  $$$$$$$/  ");           
        }

        private void PrintParksMenu()
        {
            Console.WriteLine("Please select a Park from the list below: ");
        }                   
    }
}




