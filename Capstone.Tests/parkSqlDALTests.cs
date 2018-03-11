using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
	public class parkSqlDALTests
	{

		private string connectionString = @"Data Source=.\SQLEXPRESS;Database=campground-tiny;Trusted_Connection=True;";
		
		[TestMethod]
		public void TestParkList()
		{
			TransactionScope testParkList;
			testParkList = new TransactionScope();

			ParkSqlDAL parkDAL = new ParkSqlDAL(connectionString);
			List<Park> park = parkDAL.GetParks();
			bool output = park.Count > 0;

			Assert.IsTrue(output);
			testParkList.Dispose();
		}

		[TestMethod]
		public void TestParkNames()
		{
			TransactionScope testParkNames;
			testParkNames = new TransactionScope();
			ParkSqlDAL parkDAL = new ParkSqlDAL(connectionString);
			List<Park> park = parkDAL.GetParks();

			string park_1_Name = "Acadia";
			string park_2_Name = "Arches";
			string park_3_Name = "Cuyahoga Valley";

			bool output_Name_Matches1 = park[0].name == park_1_Name;
			Assert.IsTrue(output_Name_Matches1);

			bool output_Name_Matches2 = park[1].name == park_2_Name;
			Assert.IsTrue(output_Name_Matches2);

			bool output_Name_Matches3 = park[2].name == park_3_Name;
			Assert.IsTrue(output_Name_Matches3);
			
			testParkNames.Dispose();
		}
	}
}
