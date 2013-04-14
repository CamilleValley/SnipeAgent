using UltimateSniper_SchedulerEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Timers;

namespace uTesting_Scheduler_Engine
{
    
    
    /// <summary>
    ///This is a test class for Scheduler_EngineTest and is intended
    ///to contain all Scheduler_EngineTest Unit Tests
    ///</summary>
    [TestClass()]
    public class Scheduler_EngineTest
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
        ///A test for Timer_BidOptimizer_RefreshSnipes
        ///</summary>
        [TestMethod()]
        [DeploymentItem("UltimateSniper_SchedulerEngine.dll")]
        public void Timer_BidOptimizer_RefreshSnipesTest()
        {
            Scheduler_Engine_Accessor target = new Scheduler_Engine_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            ElapsedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.Timer_BidOptimizer_RefreshSnipes(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
