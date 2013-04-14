using UltimateSniper_ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;
using System.Collections.Generic;

namespace uTesting_UltimateSniper_SL_User
{
    
    
    /// <summary>
    ///This is a test class for SL_UserTest and is intended
    ///to contain all SL_UserTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SL_UserTest
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
        ///A test for UserUpdate
        ///</summary>
        [TestMethod()]
        public void UserUpdateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            User user = null; // TODO: Initialize to an appropriate value
            target.UserSave(user);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UserLogin
        ///</summary>
        [TestMethod()]
        public void UserLoginTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            string userName = string.Empty; // TODO: Initialize to an appropriate value
            string userPassword = string.Empty; // TODO: Initialize to an appropriate value
            target.UserLogin(userName, userPassword, false, 0);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UserDesactivate
        ///</summary>
        [TestMethod()]
        public void UserDesactivateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            User user = null; // TODO: Initialize to an appropriate value
            target.UserDesactivate(user);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for UserCreate
        ///</summary>
        [TestMethod()]
        public void UserCreateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            User user = null; // TODO: Initialize to an appropriate value
            User expected = null; // TODO: Initialize to an appropriate value
            User actual;
            actual = target.UserCreate(user);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SnipeUpdate
        ///</summary>
        [TestMethod()]
        public void SnipeUpdateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            Snipe snipe = null; // TODO: Initialize to an appropriate value
            target.SnipeUpdate(snipe);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SnipeDesactivate
        ///</summary>
        [TestMethod()]
        public void SnipeDesactivateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            Snipe snipe = null; // TODO: Initialize to an appropriate value
            target.SnipeDesactivate(snipe);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for GetSnipesByCategory
        ///</summary>
        [TestMethod()]
        public void GetSnipesByCategoryTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            int categoryID = 0; // TODO: Initialize to an appropriate value
            List<Snipe> expected = null; // TODO: Initialize to an appropriate value
            List<Snipe> actual;
            actual = target.GetSnipesByCategory(categoryID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for GetAllSnipes
        ///</summary>
        [TestMethod()]
        public void GetAllSnipesTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            List<Snipe> expected = null; // TODO: Initialize to an appropriate value
            List<Snipe> actual;

            target.UserLogin("Test", "Test", false,0);
            try
            {
                actual = target.GetAllSnipes();
                Assert.AreEqual(true, true);
            }
            catch
            { Assert.AreEqual(false, true); 
            }

        }

        /// <summary>
        ///A test for GetActiveSnipes
        ///</summary>
        [TestMethod()]
        public void GetActiveSnipesTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            List<Snipe> expected = null; // TODO: Initialize to an appropriate value
            List<Snipe> actual;
            actual = target.GetActiveSnipes();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SnipeCreate
        ///</summary>
        [TestMethod()]
        public void SnipeCreateTest()
        {
            SL_User target = new SL_User(); // TODO: Initialize to an appropriate value
            Snipe _snipe = null; // TODO: Initialize to an appropriate value
            Snipe expected = null; // TODO: Initialize to an appropriate value
            Snipe actual;
            actual = target.SnipeCreate(_snipe);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
