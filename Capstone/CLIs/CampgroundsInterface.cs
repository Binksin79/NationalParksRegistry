using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Capstone.CLIs;
using System.Threading;

namespace Capstone
{
    public class CampgroundsInterface
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        int parkInfoSelection = -1;
        int parkCampgroundSelection = -1;
        int milliseconds = 1000;

        // Constructor that carries selected park from main menu //
        private Park selectedPark;
        public CampgroundsInterface(Park selectedPark)
        {
            this.selectedPark = selectedPark;
        }

        public void Display()

        {
            // prompts user to view available campgrounds or return to previous screen //           
            PrintMenu();
            parkInfoSelection = CLIHelper.GetInteger("Select a command: ");
            Console.WriteLine();
            if (parkInfoSelection != 1 && parkInfoSelection !=2 && parkInfoSelection != 3)
            {
                Console.WriteLine("Invalid Input, returning to main menu!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();
            }
            // user selected to view available campgrounds //
            if (parkInfoSelection == 1)
            {
                // Clears screen and calls method that shows menu of campgrounds at the selected park //
                // prompts user to search for available reservations or return to previous menu //
                Console.Clear();
                ViewCampgrounds();
                Console.WriteLine();
                PrintMenu2();

                // Checks if user entered 1 or 2, if not, sends back to previous menu display //
                if (parkCampgroundSelection != 1 && parkCampgroundSelection != 2)
                {
                    Console.WriteLine("Invalid Input, Campground Menu!");
                    Thread.Sleep(milliseconds);
                    Console.Clear();
                    Display();                                       
                }

                // user selected to search for available reservations //
                if (parkCampgroundSelection == 1)
                {
                    // clears screen and displays a list of campgrounds for selected park //
                    // saves selected campground and carries it to reservation subMenu //
                    Console.Clear();
                    List<Campground> campgroundsForCurrentPark = ViewCampgrounds();
                    ReservationInterface reservationSubmenu = new ReservationInterface(campgroundsForCurrentPark);
                    reservationSubmenu.Display();
                }

                // user opted to return to previous menu //
                // the constructor keeps the selectedPark //
                // the method ensures the park info is redisplayed for the user //
                else if (parkCampgroundSelection == 2)
                {
                    Console.Clear();
                    DisplayParkInfo();
                    CampgroundsInterface reuturnToPreviousMenu = new CampgroundsInterface(selectedPark);
                    reuturnToPreviousMenu.Display();
                }
            }

            // THIS IS ONE OF THE BONUS SELECTIONS FOR PARKWIDE RESOS (CURRENTLY NOT IMPLEMENTED) //
            else if (parkInfoSelection == 2)
            {
                Console.WriteLine("This option is currently not available, returning to mainmenu!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();
            }

            // user opted to return to the main menu //
            else if (parkInfoSelection == 3)
            {
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();
            }
        }

        // this method displays the campgrounds for the currently selected park //
        private List<Campground> ViewCampgrounds()
        {
            CampgroundSqlDAL dal = new CampgroundSqlDAL(connectionString);
            List<Campground> campgrounds = dal.GetAllCampgroundFromPark(selectedPark);
            Console.WriteLine();
            Console.WriteLine($"{selectedPark.name} Park Campgrounds");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("| {0,-3}| {1,-35}|{2,6} |{3,6} | {4,10} |", $"ID", $"Name", $"Open", $"Close", $"Fee   ");
            Console.WriteLine("------------------------------------------------------------------------");
            if (campgrounds.Count > 0)
            {
                for (int i = 0; i < campgrounds.Count; i++)
                {
                    Console.WriteLine("| {0,-3}| {1,-35}|{2,6} |{3,6} | {4,10} |", $"{i + 1}", $"{campgrounds[i].name}", $"{campgrounds[i].open_from_mm.ToString()}", $"{campgrounds[i].open_to_mm.ToString()}", $"{campgrounds[i].daily_fee.ToString("C")}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine("------------------------------------------------------------------------");
            return campgrounds;
        }

        // simple method used to display information for one park, if a user goes back to previous menu //
        private void DisplayParkInfo()
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("              PARK INFORMATION");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("{0, -20}{1, 25}", $"Name: ", $"{selectedPark.name.ToString().ToUpper()}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("{0, -20}{1, 25}", $"Location:", $"{selectedPark.location}");
            Console.WriteLine("{0, -20}{1, 25}", $"Established:", $"{selectedPark.establish_date.ToString()}");
            Console.WriteLine("{0, -20}{1, 25}", $"Area: ", $"{selectedPark.area.ToString()}");
            Console.WriteLine("{0, -20}{1, 25}", $"Annual Visitors:", $"{selectedPark.visitors.ToString()}");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine();
            Console.WriteLine(selectedPark.description);
        }

        public void PrintHeader()
        {
            Console.WriteLine("Park Information Screen");
        }

        public void PrintMenu()
        {

            Console.WriteLine("1) View Campgrounds");
            Console.WriteLine("2) Search for ParkWide Reservation (currently disabled)");
            Console.WriteLine("3) Return to Previous Screen");
        }

        public void PrintMenu2()
        {

            Console.WriteLine("1) Search for Available Reservation");
            Console.WriteLine("2) Return to previous screen");
            parkCampgroundSelection = CLIHelper.GetInteger("Select a command: ");
        }
    }
}
