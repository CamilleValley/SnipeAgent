using UltimateSniper_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;
using System.Collections.Generic;

namespace uTesting_UltimateSniper_DAL
{
    
    
    /// <summary>
    ///This is a test class for SnipeDataMapperTest and is intended
    ///to contain all SnipeDataMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SnipeDataMapperTest
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
        ///A test for Update
        ///</summary>
        public void UpdateTestHelper<T>()
        {
            // Should work
            // All attributes are set

            Snipe snipe = new Snipe();

            snipe.SnipeID = 1;
            snipe.ItemEndDate = System.DateTime.Now;
            snipe.SnipeBid = 10;
            snipe.SnipeBidCurrency = EnumCurrencyCode.AMD;
            snipe.SnipeDelay = 7;
            snipe.SnipeDescription = "Test";
            snipe.SnipeName = "Test";
            snipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
            snipe.SnipeType = EnumSnipeType.MOBILE;
            snipe.UserID = 1;

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<Snipe>((object)snipe, snipe);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }

            // Shouldn't work
            // Category name wasn't provided

            snipe = new Snipe();

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<Snipe>((object)snipe, snipe);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(true, true);
            }
        }

        [TestMethod()]
        public void UpdateTest()
        {
            UpdateTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetByCriteria
        ///</summary>
        [TestMethod()]
        public void GetByCriteriaTest()
        {
            IList<Snipe> snipes = new List<Snipe>();

            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "SnipeID";
            crit.Value = "1";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            snipes = myDataConnection.GetByCriteria<Snipe>(q);

            Assert.AreEqual(true, true);
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        public void DeleteTestHelper<T>()
        {
            // Should NOT work
            // ID is missing

            Snipe snipe = new Snipe();

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<Snipe>((object)snipe, snipe);
                Assert.AreEqual(true, false);
            }
            catch { }

            // Should work

            snipe = new Snipe();

            snipe.SnipeID = 1;

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<Snipe>((object)snipe, snipe);
                Assert.AreEqual(true, true);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }
        }

        [TestMethod()]
        public void DeleteTest()
        {
            DeleteTestHelper<GenericParameterHelper>();
        }

        /// <summary>
        ///A test for Create
        ///</summary>
        public void CreateTestHelper<T>()
        {
            // Should work
            // All attributes are set

            Snipe snipe = new Snipe();

            snipe.ItemEndDate = System.DateTime.Now;
            snipe.SnipeBid = 10;
            snipe.SnipeBidCurrency = EnumCurrencyCode.AMD;
            snipe.SnipeDelay = 7;
            snipe.SnipeDescription = "Test";
            snipe.SnipeName = "Test";
            snipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
            snipe.SnipeType = EnumSnipeType.MOBILE;
            snipe.UserID = 1;

            Category cat = new Category();
            cat.CategoryID = 2;
            cat.CategoryName = "fdsfd";
            cat.IsCategoryActive = true;
            cat.UserID = 1;

            snipe.SnipeCategories.Add(cat);

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                snipe = (Snipe)myDataConnection.Add<Snipe>((object)snipe, snipe);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }

            // Shouldn't work
            // Category name wasn't provided

            snipe = new Snipe();

            myDataConnection = new SqlDataContext();

            try
            {
                snipe = (Snipe)myDataConnection.Add<Snipe>((object)snipe, snipe);
                Assert.AreEqual(true, false);
            }
            catch
            {
                Assert.AreEqual(true, true);
            }
        }

        [TestMethod()]
        public void CreateTest()
        {
            CreateTestHelper<GenericParameterHelper>();
        }
    }
}
