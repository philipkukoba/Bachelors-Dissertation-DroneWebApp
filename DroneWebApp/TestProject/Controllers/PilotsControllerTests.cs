using Microsoft.VisualStudio.TestTools.UnitTesting;
using DroneWebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneWebApp.Models;
using Moq;
using System.Data.Entity;
using System.Web.Mvc;

namespace DroneWebApp.Controllers.Tests
{
    [TestClass()]
    public class PilotsControllerTests
    {
        [TestMethod()]
        public void Index_ActionExecutes_ReturnsViewForIndex()
        {
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create mock context
            //Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            var mockSet = new Mock<DbSet<Pilot>>();
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var queryable = GetPilots().AsQueryable();
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod()]
        public void IndexTest_ViewData()
        {
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create mock context
            //Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            var mockSet = new Mock<DbSet<Pilot>>();
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var queryable = GetPilots().AsQueryable();
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Index() as ViewResult;
            var resultModel = result.ViewData.Model;
            
            Assert.AreEqual(GetPilots().GetType(), resultModel.GetType());
            Assert.AreEqual(GetPilots().ToString(), resultModel.ToString());
        }

        [TestMethod()]
        public void DetailsTest_ActionExecutes_ReturnsViewForDetails()
        {
            Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();
            PilotsController controller = new PilotsController(mockContext.Object);

            // Create mock context
            //Mock<DroneDBEntities> mockContext = new Mock<DroneDBEntities>();

            // Create a mock DbSet
            var mockSet = new Mock<DbSet<Pilot>>();
            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var queryable = GetPilots().AsQueryable();
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<Pilot>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            // Set up the Pilots property so it returns the mocked DbSet
            mockContext.Setup(o => o.Pilots).Returns(() => mockSet.Object);

            var result = controller.Details(3) as ViewResult;
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod()]
        public void CreateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void CreateTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EditTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteConfirmedTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DroneFlightsTest()
        {
            Assert.Fail();
        }

        private List<Pilot> GetPilots()
        {
            var pilots = new List<Pilot>();
            for (int i=0; i<10; i++)
            {
                Pilot pilot = new Pilot
                {
                    PilotId = i,
                    PilotName = "Pilot" + i,
                    Country = "België",
                    City = "Gent",
                    Street = "Straat",
                    ZIP = 9000,
                    Phone = "0474000000",
                    LicenseNo = i,
                    Email = "pilot@jdn.com"
                };

                pilots.Add(pilot);
            }
            return pilots;
        }
    }
}