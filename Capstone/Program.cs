using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CapstoneDatabase"].ConnectionString;

            try
            {
                ParksInterface ui = new ParksInterface();
                ui.RunCLI();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error with the Park Reservation System. Out of service.");
            }
        }
    }
}
