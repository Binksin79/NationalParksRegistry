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

namespace Capstone.CLIs
{
    public class ReservationInterface
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;
        int customerSelection = -1;
        int milliseconds = 1000;

        // Constructor and variable to bring in a list of campgrounds for park //       
        private List<Campground> campgroundsForCurrentPark;
        public ReservationInterface(List<Campground> campgroundsForCurrentPark)
        {
            this.campgroundsForCurrentPark = campgroundsForCurrentPark;
        }

        public void Display()
        {
            // displays a list of campgrounds for current reso //
            // prompts user to choose a campground or cancel //            
            PrintHeader();
            int chosenCampground = CLIHelper.GetInteger("Which campground (enter 0 to cancel)?");

            // user has opted to cancel and return to main menu //
            if (chosenCampground == 0)
            {
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();
            }
            
            // user has selected a campground that does not exist //
            else if (chosenCampground > campgroundsForCurrentPark.Count)
            {
                Console.WriteLine("That campground does not exist, please try again!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                ViewCampgrounds();
                Display();
            }

            // user has properly selected a campground
            else
            {
                Campground userChoice = campgroundsForCurrentPark[chosenCampground - 1];
                DateTime arrivalDate = CLIHelper.GetDateTime("What is the arrival date? ");
                DateTime departureDate = CLIHelper.GetDateTime("What is the departure date? ");
                Console.Clear();
                // calls method that will display available reservations based on the 3 above constructors //
                List<Site> tempSiteList = ViewAvailableSites(userChoice, arrivalDate, departureDate);

                // brings up options for user to make reservation and makes the reservation
                MakeReservation(userChoice, arrivalDate, departureDate);
            }
        }

        // method that displays sites availabe during the selected date range //
        private List<Site> ViewAvailableSites(Campground campground, DateTime arrival, DateTime departure)
        {
            SiteSqlDAL dal = new SiteSqlDAL(connectionString);
            List<Site> sites = dal.GetAvailableSites(campground, arrival, departure);
            // following 3 lines calculate the rate for the chosen span of dates //
            decimal rate = campground.daily_fee;
            int days = (arrival - departure).Days;
            decimal cost = rate * days;
            Console.WriteLine();
            Console.WriteLine("Results Matching Your Search Criteria:");
            Console.WriteLine();
            Console.WriteLine("------------------------------------------------------------------------------------------");
            Console.WriteLine("| {0,2} | {1,13} | {2,20} | {3,14} | {4,13} | {5,9} |", $"ID", $"Max Occupancy", $"Handicap Accessible", $"Max RV Length", $"Has Utilities", $"Cost");
            Console.WriteLine("------------------------------------------------------------------------------------------");

            if (sites.Count > 0)
            {
                for (int i = 0; i < sites.Count; i++)
                {
                    Console.WriteLine("| {0,2} | {1,13} | {2,20} | {3,14} | {4,13} | {5,9} |", $"{i + 1}", $"{sites[i].max_occupancy.ToString()}", $"{sites[i].acessible.ToString()}", $"{sites[i].max_rv_length.ToString()}", $"{sites[i].utilities.ToString()}", $"{cost.ToString("C")}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine("------------------------------------------------------------------------------------------");
            return sites;
        }

        // method that inserts a reservation //
        private List<Site> MakeReservation(Campground campground, DateTime arrival, DateTime departure)
        {
            // opens a connection to provide a list of sites for current campground //
            SiteSqlDAL dal = new SiteSqlDAL(connectionString);
            List<Site> sites = dal.GetAvailableSites(campground, arrival, departure);
            // customer chooses which site they'd like to make a reservation for OR cancels //
            int customerSelection = CLIHelper.GetInteger("Which site should be reserved (enter 0 to cancel)?");
            // customer chose to return to main menu (probably choked on total cost of trip) //
            if (customerSelection == 0)
            {
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();
            }
            else if (customerSelection > sites.Count)
            {
                Console.WriteLine("That site does not exist, returning to main menu!");
                Thread.Sleep(milliseconds);
                Console.Clear();
                ParksInterface mainmenu = new ParksInterface();
                mainmenu.RunCLI();

            }
            // customer opted to make a reservation // 
            // returns a list of all current resos, then gives customer confirmation number using last reservation_id (newest created) //
            else
            {
                string reservationName = CLIHelper.GetString("What name should the reservation be made under?");
                int selectedSite = customerSelection - 1;
                DateTime now = DateTime.Now;
                ReservationSqlDAL resodal = new ReservationSqlDAL(connectionString);
                List<Reservation> result = resodal.CreateReservation(sites[selectedSite].site_id, reservationName, arrival, departure, now);
                Console.WriteLine($"The reservation has been made and the confirmation id is {result[result.Count - 1].reservation_id.ToString()}");
            }
            return sites;
        }

        // simple method to be used for customer display of campgrounds //
        private void ViewCampgrounds()
        {
            Console.WriteLine();
            Console.WriteLine($"Park Campgrounds");
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("| {0,-3}| {1,-35}|{2,6} |{3,6} | {4,10} |", $"ID", $"Name", $"Open", $"Close", $"Fee   ");
            Console.WriteLine("------------------------------------------------------------------------");
            if (campgroundsForCurrentPark.Count > 0)
            {
                for (int i = 0; i < campgroundsForCurrentPark.Count; i++)
                {
                    Console.WriteLine("| {0,-3}| {1,-35}|{2,6} |{3,6} | {4,10} |", $"{i + 1}", $"{campgroundsForCurrentPark[i].name}", $"{campgroundsForCurrentPark[i].open_from_mm.ToString()}", $"{campgroundsForCurrentPark[i].open_to_mm.ToString()}", $"{campgroundsForCurrentPark[i].daily_fee.ToString("C")}");
                }
            }
            else
            {
                Console.WriteLine("**** NO RESULTS ****");
            }
            Console.WriteLine("------------------------------------------------------------------------");
        }

        private void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine("Search for Campground Reservation");
        }

        private void PrintReservatioMenu()
        {
            Console.WriteLine("Please select a Park from the list below: ");
        }


    }
}
