using UltimateSniper_Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;
using System.Text.RegularExpressions;

namespace uTesting_UltimateSniper_Services
{
    
    
    /// <summary>
    ///This is a test class for ServiceOthersTest and is intended
    ///to contain all ServiceOthersTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceOthersTest
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
        ///A test for GetUserForSnipe
        ///</summary>
        [TestMethod()]
        public void GetUserForSnipeTest()
        {
            ServiceOthers target = new ServiceOthers(); // TODO: Initialize to an appropriate value
            Snipe snipe = null; // TODO: Initialize to an appropriate value
            User expected = null; // TODO: Initialize to an appropriate value
            User actual;
            actual = target.GetUserForSnipe(snipe);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void TestItemIDRegularExpr()
        {
            string input = "http://cgi.sandbox.ebay.com/PDSanitySOAP-Title-test-/110044950173?cmd=ViewItem&pt=LH_DefaultDomain_0&hash=item199f30ae9d#ht_500wt_1182";

            Match m = Regex.Match(input, @"^[\+\-]?\d\{12\}$");
            //return (m.Value);
            //string strRegex = @"^[\+\-]?\d\{12\}$";
            //// First we see the input string.
            //string input = "http://cgi.sandbox.ebay.com/PDSanitySOAP-Title-test-/110044950173?cmd=ViewItem&pt=LH_DefaultDomain_0&hash=item199f30ae9d#ht_500wt_1182";

            //// Here we call Regex.Match.
            //Match match = Regex.Match(input, strRegex,
            //    RegexOptions.IgnoreCase);

            //// Here we check the Match instance.
            //if (match.Success)
            //{
            //    // Finally, we get the Group value and display it.
            //    string key = match.Groups[1].Value;
            //}
        }

        /// <summary>
        ///A test for ConvertValue
        ///</summary>
        [TestMethod()]
        public void ConvertValueTest()
        {
            ServiceOthers target = new ServiceOthers(); // TODO: Initialize to an appropriate value
            double value = 0F; // TODO: Initialize to an appropriate value
            string currencyOrigine = string.Empty; // TODO: Initialize to an appropriate value
            string currencyDestination = string.Empty; // TODO: Initialize to an appropriate value
            double expected = 0F; // TODO: Initialize to an appropriate value
            double actual;
            actual = target.ConvertValue(value, currencyOrigine, currencyDestination);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for CheckSnipeValidity
        ///</summary>
        [TestMethod()]
        public void CheckSnipeValidityTest()
        {
            ServiceOthers target = new ServiceOthers(); // TODO: Initialize to an appropriate value
            Snipe snipe = null; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            UltimateSniper_Services.ServiceEBay _serviceEBay = new UltimateSniper_Services.ServiceEBay(false);
            actual = target.CheckSnipeValidity(snipe, _serviceEBay);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
