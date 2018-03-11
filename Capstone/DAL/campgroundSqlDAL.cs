using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class CampgroundSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAllCampgroundFromPark = @"SELECT * FROM campground WHERE campground.park_id = @campgroundparkid";
        
        // Single Parameter Constructor
        public CampgroundSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Campground> GetAllCampgroundFromPark(Park customerParkSelection)
        {
            List<Campground> output = new List<Campground>();
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand(SQL_GetAllCampgroundFromPark, conn);
                        cmd.Parameters.AddWithValue("@campgroundparkid", customerParkSelection.park_id);
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            Campground campground = new Campground();
                            campground.campground_id = Convert.ToInt32(reader["campground_id"]);
                            campground.park_id = Convert.ToInt32(reader["park_id"]);
                            campground.name = Convert.ToString(reader["name"]);
                            campground.open_from_mm = Convert.ToInt32(reader["open_from_mm"]);
                            campground.open_to_mm = Convert.ToInt32(reader["open_to_mm"]);
                            campground.daily_fee = Convert.ToDecimal(reader["daily_fee"]);
                            output.Add(campground);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("An error occurred reading the database: " + ex.Message);
                }
            }
            return output;
        }
    }
}
