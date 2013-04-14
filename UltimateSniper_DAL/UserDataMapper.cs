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
    public class UserDataMapper : IDataMapper<User>
    {
        private static readonly string _cmdInsertUser = "INSERT INTO [SnipeUser] (userID, UserName, UserPassword, eBayUserToken, UserEmailAddress, UserRegistrationDate, UserCredit, UserActive, UserCurrency, eBayRegistrationSiteID, UserOptionsEndDate, UserIPAddress, ShowSnipeStyles) VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', 0, 1, {6}, {7}, '{8}', '{9}', 0);";
        private static readonly string _cmdDeleteUser = "UPDATE [SnipeUser] SET UserActive = 0, UserDisactivationDate = '{0}' WHERE UserID = {1};";
        private static readonly string _cmdUpdateUser = "UPDATE [SnipeUser] SET UserPassword = '{0}', eBayUserToken = '{1}', UserCredit = {2}, EBayUserTokenExpirationDate = '{3}', UserEmailAddress = '{4}', eBayUserID = '{5}', UserCurrency = {6}, eBayRegistrationSiteID = {7}, UserOptionsEndDate = '{8}', UserIPAddress = '{9}' WHERE UserID = {10};";

        private static readonly string _cmdGetAllUsers = "SELECT * FROM [SnipeUser];";

        public virtual object Create<T>(object item, T type)
        {
            User user = (User)item;

            user.ControlObject();

            #region Control Object

            if (user.UserID != null) 
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDNotNull);

            // We control that the User Name doesnt already exist.
            SqlDataContext myDataConnection = new SqlDataContext();
            Query qr = new Query();

            Criterion critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "UserName";
            critStatus.Value = "'" + user.UserName + "'";

            qr.Criteria.Add(critStatus);

            qr.Members.Add("*");

            if (myDataConnection.GetByCriteria<User>(qr).Count > 0)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserAlreadyExists);

            //this.CheckUniqueToken(user);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            user.UserID = dbAdap.GetID("[SnipeUser]", "UserID");
            string q = string.Format(_cmdInsertUser, user.UserID, dbAdap.SafeSqlLiteral(user.UserName, 2), dbAdap.SafeSqlLiteral(user.UserPassword, 2), user.EBayUserToken, dbAdap.SafeSqlLiteral(user.UserEmailAddress, 2), user.UserRegistrationDate, (int)user.UserCurrency, (int)user.EBayRegistrationSiteID, user.UserOptionsEndDate, user.UserIPAddress);

            dbAdap.ExecuteNonQuery(q);

            return user;
        }

        public virtual void Update<T>(object item, T type)
        {
            User user = (User)item;

            #region Control Object

            user.ControlObject();

            if (user.UserID == null)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserIDNull);

            this.CheckUniqueToken(user);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            string q = string.Format(_cmdUpdateUser, dbAdap.SafeSqlLiteral(user.UserPassword, 2), user.EBayUserToken, user.UserCredit, user.EBayUserTokenExpirationDate, dbAdap.SafeSqlLiteral(user.UserEmailAddress, 2), dbAdap.SafeSqlLiteral(user.EBayUserID, 1), (int)user.UserCurrency, (int)user.EBayRegistrationSiteID, user.UserOptionsEndDate, user.UserIPAddress, user.UserID);

            dbAdap.ExecuteNonQuery(q);
        }

        private void CheckUniqueToken(User user)
        {
            // We control that the User Name doesnt already exist.
            SqlDataContext myDataConnection = new SqlDataContext();
            Query qr = new Query();

            Criterion critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "eBayUserToken";
            critStatus.Value = "'" + user.EBayUserToken + "'";

            qr.Criteria.Add(critStatus);

            if (user.UserID != null)
            {
                critStatus = new Criterion();
                critStatus.Operator = CriteriaOperator.NotEqual;
                critStatus.PropertyName = "UserID";
                critStatus.Value = user.UserID;

                qr.Criteria.Add(critStatus);
            }

            qr.Members.Add("*");

            if (myDataConnection.GetByCriteria<User>(qr).Count > 0)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserTokenAlreadyAssociated);
        }

        public virtual void Delete<T>(object item, T type)
        {
            User user = (User) item;

            #region Control Object

            if (user.UserID == null)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserIDNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            string q = string.Format(_cmdDeleteUser, System.DateTime.Now.ToUniversalTime(), user.UserID);

            dbAdap.ExecuteNonQuery(q);
        }

        public virtual IList<User> GetByCriteria(Query q)
        {
            List<User> users = new List<User>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(dbAdap.TranslateQuery(q, "[SnipeUser]"));

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                User user = BuildEntityFromRawData(row);
                user.UserCategoryList = this.GetCategories(user);
                users.Add(user);
            }

            return users;
        }

        public virtual IList<User> GetAll()
        {
            List<User> users = new List<User>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(_cmdGetAllUsers);

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                User user = BuildEntityFromRawData(row);
                user.UserCategoryList = this.GetCategories(user);
                users.Add(user);
            }

            return users;
        }

        private List<Category> GetCategories(User user)
        {
            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            SqlDataContext myDataConnection = new SqlDataContext();

            IList<Category> ListCategories = new List<Category>();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = user.UserID;

            q.Criteria.Add(crit);

            q.Members.Add("*");

            ListCategories = myDataConnection.GetByCriteria<Category>(q);

            return ListCategories.ToList();
        }

        protected virtual User BuildEntityFromRawData(DataRow row)
        {
            User User = new User();

            User.UserID = int.Parse(row["UserID"].ToString());
            User.UserName = row["UserName"].ToString();
            User.UserPassword = row["UserPassword"].ToString();
            User.EBayUserToken = row["eBayUserToken"].ToString();
            //User.EBayUserID = row["eBayUserID"].ToString();
            //User.EBayUserPwd = row["eBayUserPwd"].ToString();

            try { User.UserCredit = int.Parse(row["UserCredit"].ToString()); }
            catch { User.UserCredit = 0; }

            try { User.ShowSnipeStyles = Convert.ToBoolean(row["ShowSnipeStyles"].ToString()); }
            catch { User.ShowSnipeStyles = Convert.ToBoolean(int.Parse(row["ShowSnipeStyles"].ToString())); }

            try { User.UserActive = Convert.ToBoolean(row["UserActive"].ToString()); }
            catch{ User.UserActive = Convert.ToBoolean(int.Parse(row["UserActive"].ToString())); }
            
            if (!string.IsNullOrEmpty(row["EBayUserTokenExpirationDate"].ToString())) User.EBayUserTokenExpirationDate = DateTime.Parse(row["EBayUserTokenExpirationDate"].ToString());
            if (!string.IsNullOrEmpty(row["UserDisactivationDate"].ToString())) User.UserDisactivationDate = DateTime.Parse(row["UserDisactivationDate"].ToString());
            User.UserEmailAddress = row["UserEmailAddress"].ToString();
            User.UserIPAddress = row["UserIPAddress"].ToString();
            if (!string.IsNullOrEmpty(row["eBayRegistrationSiteID"].ToString())) User.EBayRegistrationSiteID = (EnumSites)int.Parse(row["eBayRegistrationSiteID"].ToString());
            if (!string.IsNullOrEmpty(row["UserOptionsEndDate"].ToString())) User.UserOptionsEndDate = DateTime.Parse(row["UserOptionsEndDate"].ToString());
            if (!string.IsNullOrEmpty(row["UserRegistrationDate"].ToString())) User.UserRegistrationDate = DateTime.Parse(row["UserRegistrationDate"].ToString());
            User.UserCurrency = (EnumCurrencyCode)int.Parse(row["UserCurrency"].ToString());

            return User;
        }
    }
}
