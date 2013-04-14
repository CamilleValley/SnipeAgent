using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace UltimateSniper_DAL
{
    public class CategoryDataMapper : IDataMapper<Category>
    {
        private static readonly string _cmdInsertCategory = "INSERT INTO Category (CategoryID, CategoryName, UserID, CategoryActive, CategoryDisactivationDate) VALUES ({0}, '{1}', '{2}', 1, null);";
        private static readonly string _cmdDeleteCategory = "UPDATE Category SET CategoryActive = 0, CategoryDisactivationDate = '{0}' WHERE CategoryID = {1};";
        private static readonly string _cmdUpdateCategory = "UPDATE Category SET CategoryName = '{0}', CategoryActive = {1}, UserID = {2} WHERE CategoryID = {3};";

        private static readonly string _cmdGetAllCategories = "SELECT * FROM Category;";

        public virtual object Create<T>(object item, T type)
        {
            Category cat = (Category) item;

            #region Control Object

            cat.ControlObject();

            if (cat.CategoryID != null)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.CategoryIDNotNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            cat.CategoryID = dbAdap.GetID("Category", "CategoryID");
            string q = string.Format(_cmdInsertCategory, cat.CategoryID, dbAdap.SafeSqlLiteral(cat.CategoryName, 2), cat.UserID);

            dbAdap.ExecuteNonQuery(q);

            return cat;
        }

        public virtual void Update<T>(object item, T type)
        {
            Category cat = (Category)item;

            #region Control Object

            cat.ControlObject();

            if (cat.CategoryID == null) 
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.CategoryIDNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            string uID;
            if (cat.UserID != null) uID = cat.UserID.ToString();
            else uID = "null";

            string q = string.Format(_cmdUpdateCategory, dbAdap.SafeSqlLiteral(cat.CategoryName, 2), Convert.ToInt32(cat.IsCategoryActive), uID, cat.CategoryID);

            dbAdap.ExecuteNonQuery(q);
        }

        public virtual void Delete<T>(object item, T type)
        {
            Category cat = (Category) item;

            #region Control Object

            if (cat.CategoryID == null) throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.CategoryIDNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            string q = string.Format(_cmdDeleteCategory, System.DateTime.Now.ToUniversalTime(), cat.CategoryID);

            dbAdap.ExecuteNonQuery(q);
        }

        public virtual IList<Category> GetByCriteria(Query q)
        {
            List<Category> Categories = new List<Category>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(dbAdap.TranslateQuery(q, "Category"));

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Categories.Add(BuildEntityFromRawData(row));
            }

            return Categories;
        }

        public virtual IList<Category> GetAll()
        {
            List<Category> Categories = new List<Category>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(_cmdGetAllCategories);

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Categories.Add(BuildEntityFromRawData(row));
            }

            return Categories;
        }

        public virtual Category BuildEntityFromRawData(DataRow row)
        {
            Category category = new Category();

            category.CategoryID = int.Parse(row["CategoryID"].ToString());
            category.CategoryName = row["CategoryName"].ToString();

            try { category.IsCategoryActive = Convert.ToBoolean(row["CategoryActive"].ToString()); }
            catch { category.IsCategoryActive = Convert.ToBoolean(int.Parse(row["CategoryActive"].ToString())); }

            if (!string.IsNullOrEmpty(row["CategoryDisactivationDate"].ToString())) category.CategoryDisactivationDate = DateTime.Parse(row["CategoryDisactivationDate"].ToString());
            if (!string.IsNullOrEmpty(row["UserID"].ToString())) category.UserID = int.Parse(row["UserID"].ToString());

            return category;
        }
    }
}
