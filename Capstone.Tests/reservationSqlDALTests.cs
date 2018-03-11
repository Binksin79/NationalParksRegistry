using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Capstone.Models;
using Capstone.DAL;
using System.Transactions;


namespace Capstone.Tests
{
    [TestClass]
    public class reservationSqlDALTests
    {

		private string connectionString = @"Data Source=.\SQLEXPRESS;Database=campground-tiny;Trusted_Connection=True;";

		[TestMethod]
        public void TestFakeReservation()
        {
			int id_of_site = 1;
			string resName = "Fake Reservation Name";
			DateTime reqStart = new DateTime(2016, 08, 01);
			DateTime reqEnd = new DateTime(2016, 08, 03);
			DateTime presentDay = DateTime.Now;

			List<Reservation> reservationWithNewReservation = new List<Reservation>();

			reservationWithNewReservation = CreateFakeReservation(id_of_site, resName, reqStart, reqEnd, presentDay);
			
			Assert.AreEqual(45, reservationWithNewReservation.Count);
			Assert.AreEqual("Fake Reservation Name", reservationWithNewReservation[45].name);
			Assert.AreEqual(id_of_site, reservationWithNewReservation[45].site_id);

		}


		public List<Reservation> CreateFakeReservation(int id_of_site, string reservation_name, DateTime requestedStartDate, DateTime requested_end_date, DateTime today)
		{
			List<Reservation> output = new List<Reservation>();
			try
			{
				using (SqlConnection conn = new SqlConnection(connectionString))
				{

					conn.Open();
					SqlCommand cmd = new SqlCommand($"INSERT INTO reservation VALUES ('{id_of_site}', '{reservation_name}', '{requestedStartDate}', '{requested_end_date}', '{today}')", conn);
					cmd.ExecuteNonQuery();
					SqlCommand readercmd = new SqlCommand(@"SELECT * from reservation", conn);
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
