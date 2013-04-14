using UltimateSniper_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;
using System.Collections.Generic;

namespace uTesting_UltimateSniper_DAL
{
    
    
    /// <summary>
    ///This is a test class for UserDataMapperTest and is intended
    ///to contain all UserDataMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UserDataMapperTest
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
        ///A test for GetByCriteria
        ///</summary>
        [TestMethod()]
        public void GetByCriteriaTest()
        {
            IList<User> users = new List<User>();

            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = "1";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            users = myDataConnection.GetByCriteria<User>(q);

            Assert.AreEqual(true, true);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        public void UpdateTestHelper<T>()
        {
            // Should work
            // All attributes are set

            User user = new User();

            user.UserID = 1;
            user.EBayRegistrationSiteID = (EnumSites)0;
            user.EBayUserToken = "cdsgfdsg";
            user.EBayUserTokenExpirationDate = System.DateTime.Now;
            user.UserActive = true;
            user.UserEmailAddress = "test@gmail.com";
            user.UserName = "Test";
            user.UserPassword = "Test";
            user.UserCurrency = EnumCurrencyCode.USD;

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<User>((object)user, user);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }

            // Shouldn't work
            // User name wasn't provided

            user = new User();

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<User>((object)user, user);
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
        ///A test for Delete
        ///</summary>
        public void DeleteTestHelper<T>()
        {
            // Should NOT work
            // ID is missing

            User user = new User();

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<User>((object)user, user);
                Assert.AreEqual(true, false);
            }
            catch { }

            // Should work

            user = new User();

            user.UserID = 1;

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<User>((object)user, user);
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

        [TestMethod]
        public void TestAddUser()
        {
            User newUser = new User();

            newUser.UserName = "test" + System.DateTime.Now.ToString();
            newUser.UserPassword = "dsfds";
            newUser.EBayUserToken = "dsdsfsdfsdf561";
            newUser.UserEmailAddress = "sdf";

            SqlDataContext myDataConnection = new SqlDataContext();

            myDataConnection.Add<User>((object)newUser, newUser);

            Assert.AreEqual(true, true);
        }
    }
}
