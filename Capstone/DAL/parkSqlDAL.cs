using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ParkSqlDAL
    {
        private string connectionString;
        private const string SQL_GetParks = @"SELECT * FROM park";

        // Single Parameter Constructor // 
        public ParkSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Park> GetParks()
        {
            List<Park> output = new List<Park>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(SQL_GetParks, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Park park = new Park();
                        park.park_id = Convert.ToInt32(reader["park_id"]);
                        park.name = Convert.ToString(reader["name"]);
                        park.location = Convert.ToString(reader["location"]);
                        park.establish_date = Convert.ToDateTime(reader["establish_date"]);
                        park.area = Convert.ToInt32(reader["area"]);
                        park.visitors = Convert.ToInt32(reader["visitors"]);
                        park.description = Convert.ToString(reader["description"]);
                        
                        output.Add(park);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An error occurred reading the database: " + ex.Message);
            }
            return output;
        }       
    }
}

