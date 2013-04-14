using UltimateSniper_Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using UltimateSniper_BusinessObjects;
using System.Collections.Generic;
using UltimateSniper_DAL;

namespace uTesting_UltimateSniper_Services
{
    
    
    /// <summary>
    ///This is a test class for ServiceEBayTest and is intended
    ///to contain all ServiceEBayTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceEBayTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for GetSessionID
        ///</summary>
        [TestMethod()]
        [DeploymentItem("UltimateSniper_Services.dll")]
        public void GetSessionIDTest()
        {
            ServiceEBay_Accessor target = new ServiceEBay_Accessor(true); // TODO: Initialize to an appropriate value
            target.GetSessionID();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GeteBayOfficialTime
        ///</summary>
        [TestMethod()]
        public void GeteBayOfficialTimeTest()
        {
            ServiceEBay target = new ServiceEBay(true); // TODO: Initialize to an appropriate value
            DateTime expected = new DateTime(); // TODO: Initialize to an appropriate value
            DateTime actual;
            try
            {
                actual = target.GeteBayOfficialTime();
                Assert.AreEqual(true, true);
            }
            catch
            {
                Assert.AreEqual(false, true);
            }
        }

        /// <summary>
        ///A test for UserWonTheItem
        ///</summary>
        [TestMethod()]
        public void UserWonTheItemTest()
        {
            User user = new User();
            user.EBayUserTokenExpirationDate = DateTime.Now.AddMonths(3);
            user.EBayUserToken = "AgAAAA**AQAAAA**aAAAAA**N5ewSw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAJCLowidj6x9nY+seQ**UUsBAA**AAMAAA**f/Kor0HEN77MiUcmv086pizupwLl//2qkvRIrryhDXZtT62f7GO0mSLp9ufBbXEAKkaqUERJqeJqE+dmn16sWrpWr4BqLNOwlWIXsm75nAsEhtD36H9vb04cubUqvXVMHrsXnLAMIdAit2LHgrxYCZKo/BsvrbIVrn/1llC7l3/TiBHUskO51/+sEd3LSjqHj5Bzn6SLTLRvDetRPA0K2KfjhvYFh3wI0tsKnsdczx72+MzHfAHoD7snNtffmHSkiG5HuVXgSGogiT1S38ICcSevuQ89FXXypS5QaAz2a6M+xnTdNFMoayXhvNT1d/Mhh425FExSvzntyLO0Ct6j04Ct0ibzMhJwTScHW9cOyXx8mRrsnNQKjxjlL3Y0dvllqbAwdqai+M8v1A7z1zBD4jdWkDVrOH3U4jpbfwZefBhVQZRvkQ97qRdqGFX+Kz5smmnBJSnQYsIcEaIe4lSkWq+ESiU+V14q0MkwnJBY9FBuHSwio1YF9DafnnJc0j9Me5MqicYb+aO8LO/mt76DOLQfJwQCu9KOPsM/nGvaW1IKxSqQa1uMr/2zDs7iTsolLasjmlu3pWjpeKow9mnZ3P9S47UgSh2LkzV70oUCEX7uelFdeuBzJgNNX+clI7y24jUAznQ+BcVi3U67+UVPt0/BTi6ZIVRCbvI7WyLVniVa3eCSXZtsLW85HjsjsgX2SAItywulWncA9znH6OqbzMAjQMWWSBginJhIjadfmstxYGvcOPLMKTq/cMhPeAGb";
            user.UserIPAddress = "82.101.25.10";

            ServiceEBay target = new ServiceEBay(user, true); // TODO: Initialize to an appropriate value
            long itemID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.UserWonTheItem(110048891589);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PlaceBid
        ///</summary>
        [TestMethod()]
        public void SetSnipeTest()
        {
            User user = new User();
            user.EBayUserTokenExpirationDate = DateTime.Now.AddMonths(3);
            user.EBayUserToken = "AgAAAA**AQAAAA**aAAAAA**N5ewSw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAJCLowidj6x9nY+seQ**UUsBAA**AAMAAA**f/Kor0HEN77MiUcmv086pizupwLl//2qkvRIrryhDXZtT62f7GO0mSLp9ufBbXEAKkaqUERJqeJqE+dmn16sWrpWr4BqLNOwlWIXsm75nAsEhtD36H9vb04cubUqvXVMHrsXnLAMIdAit2LHgrxYCZKo/BsvrbIVrn/1llC7l3/TiBHUskO51/+sEd3LSjqHj5Bzn6SLTLRvDetRPA0K2KfjhvYFh3wI0tsKnsdczx72+MzHfAHoD7snNtffmHSkiG5HuVXgSGogiT1S38ICcSevuQ89FXXypS5QaAz2a6M+xnTdNFMoayXhvNT1d/Mhh425FExSvzntyLO0Ct6j04Ct0ibzMhJwTScHW9cOyXx8mRrsnNQKjxjlL3Y0dvllqbAwdqai+M8v1A7z1zBD4jdWkDVrOH3U4jpbfwZefBhVQZRvkQ97qRdqGFX+Kz5smmnBJSnQYsIcEaIe4lSkWq+ESiU+V14q0MkwnJBY9FBuHSwio1YF9DafnnJc0j9Me5MqicYb+aO8LO/mt76DOLQfJwQCu9KOPsM/nGvaW1IKxSqQa1uMr/2zDs7iTsolLasjmlu3pWjpeKow9mnZ3P9S47UgSh2LkzV70oUCEX7uelFdeuBzJgNNX+clI7y24jUAznQ+BcVi3U67+UVPt0/BTi6ZIVRCbvI7WyLVniVa3eCSXZtsLW85HjsjsgX2SAItywulWncA9znH6OqbzMAjQMWWSBginJhIjadfmstxYGvcOPLMKTq/cMhPeAGb";
            user.UserIPAddress = "82.101.25.10";

            ServiceEBay target = new ServiceEBay(user, true); // TODO: Initialize to an appropriate value
            long itemID = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;

            Snipe snipe = new Snipe();
            snipe.ItemID = 110048667899;
            snipe.SnipeStyle = EnumSnipeStyle.Snipe;
            snipe.SnipeBid = 170;

            target.SetSnipe(snipe);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetSimilarItems
        ///</summary>
        [TestMethod()]
        public void GetSimilarItemsTest()
        {
            ServiceEBay target = new ServiceEBay(true); // TODO: Initialize to an appropriate value
            Snipe snipe = new Snipe();
            List<eBayItemData> expected = null; // TODO: Initialize to an appropriate value
            List<eBayItemData> actual;

            IList<User> users = new List<User>();

            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = "2";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            users = myDataConnection.GetByCriteria<User>(q);

            target.User = users[0];

            IList<Snipe> snipes = new List<Snipe>();

            myDataConnection = new SqlDataContext();

            q = new Query();

            crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "SnipeID";
            crit.Value = "1";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            snipes = myDataConnection.GetByCriteria<Snipe>(q);

            snipe = snipes[0];

            actual = target.GetSimilarItems(snipe);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetItemDetails
        ///</summary>
        [TestMethod()]
        public void GetItemDetailsTest()
        {
            User user = new User();
            user.EBayUserTokenExpirationDate = DateTime.Now.AddMonths(3);
            user.EBayUserToken = "AgAAAA**AQAAAA**aAAAAA**N5ewSw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAJCLowidj6x9nY+seQ**UUsBAA**AAMAAA**f/Kor0HEN77MiUcmv086pizupwLl//2qkvRIrryhDXZtT62f7GO0mSLp9ufBbXEAKkaqUERJqeJqE+dmn16sWrpWr4BqLNOwlWIXsm75nAsEhtD36H9vb04cubUqvXVMHrsXnLAMIdAit2LHgrxYCZKo/BsvrbIVrn/1llC7l3/TiBHUskO51/+sEd3LSjqHj5Bzn6SLTLRvDetRPA0K2KfjhvYFh3wI0tsKnsdczx72+MzHfAHoD7snNtffmHSkiG5HuVXgSGogiT1S38ICcSevuQ89FXXypS5QaAz2a6M+xnTdNFMoayXhvNT1d/Mhh425FExSvzntyLO0Ct6j04Ct0ibzMhJwTScHW9cOyXx8mRrsnNQKjxjlL3Y0dvllqbAwdqai+M8v1A7z1zBD4jdWkDVrOH3U4jpbfwZefBhVQZRvkQ97qRdqGFX+Kz5smmnBJSnQYsIcEaIe4lSkWq+ESiU+V14q0MkwnJBY9FBuHSwio1YF9DafnnJc0j9Me5MqicYb+aO8LO/mt76DOLQfJwQCu9KOPsM/nGvaW1IKxSqQa1uMr/2zDs7iTsolLasjmlu3pWjpeKow9mnZ3P9S47UgSh2LkzV70oUCEX7uelFdeuBzJgNNX+clI7y24jUAznQ+BcVi3U67+UVPt0/BTi6ZIVRCbvI7WyLVniVa3eCSXZtsLW85HjsjsgX2SAItywulWncA9znH6OqbzMAjQMWWSBginJhIjadfmstxYGvcOPLMKTq/cMhPeAGb";
            user.UserIPAddress = "82.101.25.10";

            ServiceEBay target = new ServiceEBay(user, true); // TODO: Initialize to an appropriate value
 
            bool backEndObject = false; // TODO: Initialize to an appropriate value
            long itemID = 110048891589; // TODO: Initialize to an appropriate value
            eBayItemData expected = new eBayItemData(); // TODO: Initialize to an appropriate value
            eBayItemData actual;
            actual = target.GetItemDetails(itemID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for IsUserWinning
        ///</summary>
        [TestMethod()]
        [DeploymentItem("UltimateSniper_Services.dll")]
        public void IsUserWinningTest()
        {
            User user = new User();
            user.EBayUserTokenExpirationDate = DateTime.Now.AddMonths(3);
            user.EBayUserToken = "AgAAAA**AQAAAA**aAAAAA**N5ewSw**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAJCLowidj6x9nY+seQ**UUsBAA**AAMAAA**f/Kor0HEN77MiUcmv086pizupwLl//2qkvRIrryhDXZtT62f7GO0mSLp9ufBbXEAKkaqUERJqeJqE+dmn16sWrpWr4BqLNOwlWIXsm75nAsEhtD36H9vb04cubUqvXVMHrsXnLAMIdAit2LHgrxYCZKo/BsvrbIVrn/1llC7l3/TiBHUskO51/+sEd3LSjqHj5Bzn6SLTLRvDetRPA0K2KfjhvYFh3wI0tsKnsdczx72+MzHfAHoD7snNtffmHSkiG5HuVXgSGogiT1S38ICcSevuQ89FXXypS5QaAz2a6M+xnTdNFMoayXhvNT1d/Mhh425FExSvzntyLO0Ct6j04Ct0ibzMhJwTScHW9cOyXx8mRrsnNQKjxjlL3Y0dvllqbAwdqai+M8v1A7z1zBD4jdWkDVrOH3U4jpbfwZefBhVQZRvkQ97qRdqGFX+Kz5smmnBJSnQYsIcEaIe4lSkWq+ESiU+V14q0MkwnJBY9FBuHSwio1YF9DafnnJc0j9Me5MqicYb+aO8LO/mt76DOLQfJwQCu9KOPsM/nGvaW1IKxSqQa1uMr/2zDs7iTsolLasjmlu3pWjpeKow9mnZ3P9S47UgSh2LkzV70oUCEX7uelFdeuBzJgNNX+clI7y24jUAznQ+BcVi3U67+UVPt0/BTi6ZIVRCbvI7WyLVniVa3eCSXZtsLW85HjsjsgX2SAItywulWncA9znH6OqbzMAjQMWWSBginJhIjadfmstxYGvcOPLMKTq/cMhPeAGb";
            user.UserIPAddress = "82.101.25.10";

            ServiceEBay target = new ServiceEBay(user, true); // TODO: Initialize to an appropriate value
 
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Snipe snipe = new Snipe(); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            snipe.ItemID = 110048895593;
            actual = target.IsUserWinning(snipe);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
