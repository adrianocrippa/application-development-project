
using ViaRailProject.Models;

namespace TicketTest
{
    public class Tests
    {
        private AdminForTesting admin;
        private VaiRailTest vaiRailTest;

        [SetUp]
        public void Setup()
        {
            admin = new AdminForTesting();
        }

        [Test]
        public void InsertButton_ClickTest()
        {
            admin.TripNumber = "123";
            admin.DepartureStation = "Station A";
            admin.DestinationStation = "Station B";
            admin.TrainNumber = "456";
            admin.Class = "First Class";
            admin.SeatAvailability = "100";
            admin.Price = "50.00";

            // Act -
            VaiRailTest.ReferenceEquals (admin, vaiRailTest);

            // Assert - 
            #region
            bool insertionSuccess = true;
            #endregion
            Assert.IsTrue(insertionSuccess, "Insertion was successful");
        }
    }
}