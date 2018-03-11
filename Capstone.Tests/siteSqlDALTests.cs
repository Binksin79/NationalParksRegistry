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
    public class siteSqlDALTests
    {
		private string connectionString = @"Data Source=.\SQLEXPRESS;Database=campground-tiny;Trusted_Connection=True;";
		TransactionScope testScope;
		
		[TestMethod]
        public void TestListSiteDAL()
        {
			testScope = new TransactionScope();
			DateTime startTime = new DateTime(2016, 08, 01);
			DateTime endTime = new DateTime(2016, 08, 03);

			ParkSqlDAL parkDAL = new ParkSqlDAL(connectionString);
			List<Park> park = parkDAL.GetParks();

			CampgroundSqlDAL campgroundDAL = new CampgroundSqlDAL(connectionString);
			List<Campground> testNumberOfCampsAcadia = campgroundDAL.GetAllCampgroundFromPark(park[0]);

			SiteSqlDAL siteDal = new SiteSqlDAL(connectionString);
			
			List<Site> availableSites = siteDal.GetAvailableSites(testNumberOfCampsAcadia[0], startTime, endTime);

			Assert.IsNotNull(availableSites);
			Assert.IsTrue(availableSites.Count > 0);

			testScope.Dispose();
		}
    }
}
