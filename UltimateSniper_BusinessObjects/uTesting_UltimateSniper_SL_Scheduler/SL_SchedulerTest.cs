using UltimateSniper_ServiceLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;

namespace uTesting_UltimateSniper_SL_Scheduler
{
    
    
    /// <summary>
    ///This is a test class for SL_SchedulerTest and is intended
    ///to contain all SL_SchedulerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SL_SchedulerTest
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
        ///A test for CheckSnipeResult
        ///</summary>
        [TestMethod()]
        [DeploymentItem("UltimateSniper_ServiceLayer.dll")]
        public void CheckSnipeResultTest()
        {
            SL_Scheduler_Accessor target = new SL_Scheduler_Accessor(); // TODO: Initialize to an appropriate value
            Snipe snipe = new Snipe(); // TODO: Initialize to an appropriate value
            snipe.SnipeID = 52;
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            target.CheckSnipeResult(snipe);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CreateFollowingSnipe
        ///</summary>
        [TestMethod()]
        [DeploymentItem("UltimateSniper_ServiceLayer.dll")]
        public void CreateFollowingSnipeTest()
        {
            SL_Scheduler_Accessor target = new SL_Scheduler_Accessor(); // TODO: Initialize to an appropriate value
            UltimateSniper_Services.ServiceOthers otherService = new UltimateSniper_Services.ServiceOthers();
            Snipe snipe = otherService.GetSnipe(1);
            User user = otherService.GetUserForSnipe(snipe);
            SL_Scheduler_Accessor.eBayService.User = user;
            target.CreateFollowingSnipe(snipe);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
