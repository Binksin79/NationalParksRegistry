using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Capstone.DAL
{
    public class SiteSqlDAL
    {
        private string connectionString;
        private const string SQL_GetAvailableSites = @"SELECT * FROM site WHERE site_id NOT IN 
	    (SELECT site_id FROM reservation WHERE (@arrival <= reservation.from_date AND @departure >= reservation.to_date)
										  OR (@departure >= reservation.from_date AND @departure <= reservation.to_date)
										  OR (@arrival >= reservation.from_date AND @arrival <= reservation.to_date)
        )
	    AND site.campground_id = @selectedcampground";

        // Single Parameter Constructor
        public SiteSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Site> GetAvailableSites(Campground customerCampgroundSelection, DateTime arrival, DateTime departure)
        {
            List<Site> output = new List<Site>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetAvailableSites, conn);
                    cmd.Parameters.AddWithValue("@selectedcampground", customerCampgroundSelection.campground_id);
                    cmd.Parameters.AddWithValue("@arrival", arrival);
                    cmd.Parameters.AddWithValue("@departure", departure);                    
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Site site = new Site();
                        site.site_id = Convert.ToInt32(reader["site_id"]);
                        site.campground_id = Convert.ToInt32(reader["campground_id"]);
                        site.site_number = Convert.ToInt32(reader["site_number"]);
                        site.max_occupancy = Convert.ToInt32(reader["max_occupancy"]);
                        site.acessible = Convert.ToBoolean(reader["accessible"]);
                        site.max_rv_length = Convert.ToInt32(reader["max_rv_length"]);
                        site.utilities = Convert.ToBoolean(reader["utilities"]);
                        output.Add(site);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading the database: " + ex.Message);
                throw;
            }
            return output;
        }
    }
}
