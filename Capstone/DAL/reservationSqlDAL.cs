using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.DAL
{
    public class ReservationSqlDAL
    {
        private string connectionString;
        private const string SQL_GetResoInfo = @"SELECT * from reservation";

        // Single Parameter Constructor // 
        public ReservationSqlDAL(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public List<Reservation> CreateReservation(int id_of_site, string reservation_name, DateTime requestedStartDate, DateTime requested_end_date, DateTime today)
        {
            List<Reservation> output = new List<Reservation>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {

                    conn.Open();
                    SqlCommand cmd = new SqlCommand($"INSERT INTO reservation VALUES ('{id_of_site}', '{reservation_name}', '{requestedStartDate}', '{requested_end_date}', '{today}')", conn);
                    cmd.ExecuteNonQuery();
                    SqlCommand readercmd = new SqlCommand(SQL_GetResoInfo, conn);                  
                    SqlDataReader reader = readercmd.ExecuteReader();
                    Reservation tempStorage = new Reservation();
                    while (reader.Read())
                    {
                        tempStorage.reservation_id = Convert.ToInt32(reader["reservation_id"]);
                        tempStorage.site_id = Convert.ToInt32(reader["site_id"]);
                        tempStorage.name = Convert.ToString(reader["name"]);
                        tempStorage.from_date = Convert.ToDateTime(reader["from_date"]);
                        tempStorage.to_date = Convert.ToDateTime(reader["to_date"]);
                        tempStorage.create_date = Convert.ToDateTime(reader["create_date"]);
                        output.Add(tempStorage);
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
