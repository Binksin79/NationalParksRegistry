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
    public class campgroundSqlDALTests
    {
		private string connectionString = @"Data Source=.\SQLEXPRESS;Database=campground-tiny;Trusted_Connection=True;";



		[TestMethod]
		public void TestCampgroundList()
		{
			TransactionScope test = new TransactionScope();

			ParkSqlDAL parkDAL = new ParkSqlDAL(connectionString);
			List<Park> park = parkDAL.GetParks();

			CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL(connectionString);

			List<Campground> testNumberOfCampsAcadia = campgroundDAL.GetAllCampgroundFromPark(park[0]);
			List<Campground> testNumberOfCampsArches = campgroundDAL.GetAllCampgroundFromPark(park[1]);
			List<Campground> testNumberOfCampsCuyahogaValley = campgroundDAL.GetAllCampgroundFromPark(park[2]);
			
			//assert
			bool outputAcadia_doesCampCountNumberMatch = testNumberOfCampsAcadia.Count == 3;
			Assert.IsTrue(outputAcadia_doesCampCountNumberMatch);

			bool outputArches_doesCampCountNumberMatch = testNumberOfCampsArches.Count == 3;
			Assert.IsTrue(outputArches_doesCampCountNumberMatch);

			bool outputCuyahogaValley_doesCampCountNumberMatch = testNumberOfCampsCuyahogaValley.Count == 1;
			Assert.IsTrue(outputCuyahogaValley_doesCampCountNumberMatch);

			test.Dispose();
		}

		[TestMethod]
		public void TestCampgroundName()
		{
			TransactionScope testNames = new TransactionScope();

			ParkSqlDAL parkDAL = new ParkSqlDAL(connectionString);
			List<Park> park = parkDAL.GetParks();

			CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL(connectionString);

			List<Campground> testNamesOfCampsAcadia = campgroundDAL.GetAllCampgroundFromPark(park[0]);

			List<Campground> testNamesOfCampsArches = campgroundDAL.GetAllCampgroundFromPark(park[1]);

			List<Campground> testNamesOfCampsCuyahoga = campgroundDAL.GetAllCampgroundFromPark(park[2]);

			//assert
			bool outputArcadia_isCampNameCorrect1 = testNamesOfCampsAcadia[0].name == "Blackwoods";
			Assert.IsTrue(outputArcadia_isCampNameCorrect1);

			bool outputArcadia_isCampNameCorrect2 = testNamesOfCampsAcadia[2].name == "Schoodic Woods";
			Assert.IsTrue(outputArcadia_isCampNameCorrect2);

			bool outputArches_isCampNameCorrect1 = testNamesOfCampsAcadia[0].name == "Devil's Garden";
			Assert.IsTrue(outputArches_isCampNameCorrect1);

			bool outputArches_isCampNameCorrect2 = testNamesOfCampsAcadia[2].name == "Juniper Group Site";
			Assert.IsTrue(outputArches_isCampNameCorrect2);

			bool outputCuyahogaValley_isCampNameCorrect1 = testNamesOfCampsAcadia[2].name == "The Unnamed Primitive Campsites";
			Assert.IsTrue(outputCuyahogaValley_isCampNameCorrect1);

			testNames.Dispose();
		}
	}
}
