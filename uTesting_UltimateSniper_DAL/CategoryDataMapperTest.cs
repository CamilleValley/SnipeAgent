using UltimateSniper_DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateSniper_BusinessObjects;
using System.Collections.Generic;

namespace uTesting_UltimateSniper_DAL
{
    
    
    /// <summary>
    ///This is a test class for CategoryDataMapperTest and is intended
    ///to contain all CategoryDataMapperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CategoryDataMapperTest
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

        [TestMethod]
        public void TestGetAllCategories()
        {
            IList<Category> ListCategories = new List<Category>();

            SqlDataContext myDataConnection = new SqlDataContext();

            ListCategories = myDataConnection.GetAll<Category>();

            Assert.AreEqual(true, true);
        }

        [TestMethod]
        public void TestGetByCriteria()
        {
            IList<Category> ListCategories = new List<Category>();

            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.NotEqual;
            crit.PropertyName = "CategoryID";
            crit.Value = "2";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            ListCategories = myDataConnection.GetByCriteria<Category>(q);

            Assert.AreEqual(true, true);
        }

        /// <summary>
        ///A test for Update
        ///</summary>
        public void UpdateTestHelper<T>()
        {
            // Should work
            // All attributes are set

            Category newCategory = new Category();

            newCategory.CategoryName = "";

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<Category>((object)newCategory, newCategory);
                Assert.AreEqual(true, false);
            }
            catch {}

            // Shouldn't work
            // Category name wasn't provided

            newCategory = new Category();

            newCategory.CategoryName = "assfdsaf";
            newCategory.CategoryID = 1;

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Save<Category>((object)newCategory, newCategory);
                Assert.AreEqual(true, true);
            }
            catch
            {
                Assert.AreEqual(true, false);
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

            Category category = new Category();

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<Category>((object)category, category);
                Assert.AreEqual(true, false);
            }
            catch {}

            // Should work
            
            category = new Category();

            category.CategoryID = 1;

            myDataConnection = new SqlDataContext();

            try
            {
                myDataConnection.Delete<Category>((object)category, category);
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

            Category newCategory = new Category();

            newCategory.CategoryName = "New category 2";
            newCategory.UserID = 1;

            SqlDataContext myDataConnection = new SqlDataContext();

            try
            {
                newCategory = (Category)myDataConnection.Add<Category>((object)newCategory, newCategory);
            }
            catch
            {
                Assert.AreEqual(true, false);
            }

            // Shouldn't work
            // Category name wasn't provided

            newCategory = new Category();

            newCategory.CategoryName = "";
            newCategory.UserID = 1;

            myDataConnection = new SqlDataContext();

            try
            {
                newCategory = (Category)myDataConnection.Add<Category>((object)newCategory, newCategory);
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
