using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using System.Data;

namespace UltimateSniper_DAL
{
    public class SnipeDataMapper : IDataMapper<Snipe>
    {
        private static readonly string _cmdInsertSnipe = "INSERT INTO Snipe (SnipeId, SnipeName,SnipeBid,SnipeStatusID,SnipeDescription,SnipeDelay,SnipeTypeID,ItemID,ItemEndDate,UserID,SnipeBidCurrency, SnipeBidInFinalCurrency, SnipeFinalCurrency, ItemSellerID, SnipeGenNextSnipe, SnipeGenOriginalID, SnipeGenRemainingNb, SnipeGenIncreaseBid, ItemURL, ItemTitle, ItemPictureURL, ItemLastKnownPrice, ItemLastUpdate, ResultInProgress, SnipeInProgress, ValidityCheckInProgress, SnipeStyleID) VALUES({0}, '{1}',{2},{3},'{4}',{5},{6},'{7}','{8}',{9},{10},{11},{12},'{13}', {14}, {15}, {16}, {17},'{18}','{19}','{20}', 0, '{21}', 0, 0, 0, {22});";
        private static readonly string _cmdDeleteSnipe = "UPDATE Snipe SET SnipeStatusID = {0}, SnipeCancellationDate = '{1}' WHERE SnipeID = {2};";
        private static readonly string _cmdUpdateSnipe = "UPDATE Snipe SET SnipeName = '{0}',SnipeBid = {1},SnipeStatusID = {2},SnipeDescription = '{3}',SnipeDelay = {4},SnipeTypeID = {5},ItemID = '{6}',ItemEndDate = '{7}',UserID = {8}, SnipeErrorReason = '{9}', ItemLastUpdate = {10}, SnipeExecutionDate = {11}, SnipeBidCurrency = {12}, SnipeBidInFinalCurrency = {13}, SnipeFinalCurrency = {14}, SnipeInProgress = {15}, ItemSellerID = '{16}', SnipeGenNextSnipe = {17}, SnipeGenOriginalID = {18}, SnipeGenRemainingNb = {19}, SnipeGenIncreaseBid = {20}, ItemURL = '{21}', ItemTitle = '{22}', ItemPictureURL = '{23}',  ItemLastKnownPrice = {24}, ResultInProgress = {25}, ValidityCheckInProgress = {26}, SnipeStyleID = {27} WHERE SnipeID = {28};";

        private static readonly string _cmdGetAllSnipes = "SELECT * FROM Snipe;";
        private static readonly string _cmdGetSnipeCategories = "SELECT c.CategoryID, c.CategoryName, c.CategoryActive, c.CategoryDisactivationDate, c.UserID FROM SnipeCategory sc, Category c WHERE sc.SnipeCategoryActive = 1 AND sc.CategoryID = c.CategoryID AND sc.SnipeID = {0};";
        private static readonly string _cmdCancelSnipesSameCategories = "UPDATE Snipe SET SnipeStatusID = {0}, SnipeCancellationDate = '{1}' WHERE SnipeStatusID = 1 AND SnipeID IN (SELECT SnipeID FROM SnipeCategory WHERE CategoryID IN (SELECT CategoryID FROM SnipeCategory WHERE SnipeID = {2}));";
        private static readonly string _cmdGetActiveSnipesForCategory = "SELECT * FROM Snipe WHERE SnipeUserID = {0} AND SnipeStatus = 1 AND SnipeID IN (SELECT SnipeID FROM SnipeCategory WHERE CategoryID = {1});";
        private static readonly string _cmdGetEndingSnipesForSubscribedUsers = "SELECT Snipe.* FROM Snipe INNER JOIN [SnipeUser] ON Snipe.UserID = SnipeUser.UserID WHERE (((SnipeUser.UserOptionsEndDate)> ##DateNow##) AND ((Snipe.[ItemEndDate])<{0}) AND ((Snipe.[SnipeStatusID])=1) AND Snipe.SnipeStyleID = 1 AND ((Snipe.[ValidityCheckInProgress])=0));";

        private static readonly string _CmdGetSnipesSameCategoryAndYounger =
            "SELECT * FROM Snipe WHERE SnipeID <> {0} AND SnipeStatusID = 1 AND SnipeID IN " +
            "(" +
            "SELECT SnipeID " +
            "FROM SnipeCategory  " +
            "WHERE " +
            " CategoryID IN (SELECT CategoryID FROM SnipeCategory WHERE SnipeID = {0}) " +
            ")" +
            "AND ItemEndDate < " +
            "( " +
            "SELECT ItemEndDate " +
            "FROM Snipe " +
            "WHERE SnipeID = {0} " +
            "); ";

        private static readonly string _CmdGetSnipesSameCategoryAndYoungerForSaving =
        "SELECT * FROM Snipe WHERE SnipeID <> {0} AND SnipeStatusID = 1 AND SnipeID IN " +
        "(" +
        "SELECT SnipeID " +
        "FROM SnipeCategory  " +
        "WHERE " +
        " CategoryID IN ({1}) " +
        ")" +
        "AND ItemEndDate < {2}" +
        ";";

        public virtual object Create<T>(object item, T type)
        {
            Snipe snipe = (Snipe)item;

            #region Control Object

            snipe.ControlObject();

            if (snipe.SnipeID != null) 
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.CategoryIDNotNull);

            // We control that the User doesnt have already an active Snipe on this item.
            SqlDataContext myDataConnection = new SqlDataContext();
            Query qr = new Query();

            Criterion critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "UserID";
            critStatus.Value = snipe.UserID;

            qr.Criteria.Add(critStatus);

            critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "SnipeStatusID";
            critStatus.Value = "1";

            qr.Criteria.Add(critStatus);

            critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "ItemID";
            critStatus.Value = "'" + snipe.ItemID + "'";

            qr.Criteria.Add(critStatus);

            qr.Members.Add("*");

            if (myDataConnection.GetByCriteria<Snipe>(qr).Count > 0)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.SnipeAlreadyExists);

            if (snipe.SnipeStyle == EnumSnipeStyle.BidOptimizer && snipe.SnipeCategories != null && snipe.SnipeCategories.Count > 0)
            {
                Query qBuf = new Query();

                qBuf.QueryName = "CheckCategoryYoungerForSaving";

                Criterion critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "SnipeID";
                critere.Value = snipe.SnipeID;

                qBuf.Criteria.Add(critere);

                critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "Categories";

                string cat = "";

                foreach (Category cattp in snipe.SnipeCategories)
                    cat += cattp.CategoryID.ToString() + ",";

                if (cat.Length != 0)
                    cat = cat.Substring(0, cat.Length - 1);

                critere.Value = cat;

                qBuf.Criteria.Add(critere);

                critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "EndDate";
                critere.Value = "#DateParse#" + snipe.ItemEndDate.ToString() + "#EndDateParse#";

                qBuf.Criteria.Add(critere);

                qBuf.Members.Add("*");

                if (myDataConnection.GetByCriteria<Snipe>(qBuf).Count > 0)
                    throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.SnipeYoungerSnipeSameCategoryExists);
            }

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            string SnipeGenOriginalID;
            if (snipe.SnipeGenOriginalID != null) SnipeGenOriginalID = snipe.SnipeGenOriginalID.ToString();
            else SnipeGenOriginalID = "null";

            string SnipeGenRemainingNb;
            if (snipe.SnipeGenRemainingNb != null) SnipeGenRemainingNb = snipe.SnipeGenRemainingNb.ToString();
            else SnipeGenRemainingNb = "null";

            snipe.SnipeID = dbAdap.GetID("Snipe", "SnipeID");
            string q = string.Format(_cmdInsertSnipe, snipe.SnipeID, dbAdap.SafeSqlLiteral(snipe.SnipeName, 2), snipe.SnipeBid.ToString().Replace(',', '.'), (int)snipe.SnipeStatus, dbAdap.SafeSqlLiteral(snipe.SnipeDescription, 2), snipe.SnipeDelay, (int)snipe.SnipeType, snipe.ItemID, snipe.ItemEndDate, snipe.UserID, (int)snipe.SnipeBidCurrency, snipe.SnipeBidInFinalCurrency.ToString().Replace(',', '.'), (int)snipe.SnipeFinalCurrency, snipe.ItemSellerID, Convert.ToInt32(snipe.SnipeGenNextSnipe), SnipeGenOriginalID, SnipeGenRemainingNb, snipe.SnipeGenIncreaseBid, snipe.ItemURL, snipe.ItemTitle, snipe.ItemPictureURL, DateTime.Now, (int)snipe.SnipeStyle);
            
            dbAdap.ExecuteNonQuery(q);

            myDataConnection = new SqlDataContext();

            foreach (Category category in snipe.SnipeCategories)
            {
                Category cat = category;

                if (category.CategoryID == null) cat = (Category)myDataConnection.Add<Category>((object)category, category);
                if (!category.IsCategoryActive) myDataConnection.Delete<Category>((object)category, category);
                if (category.CategoryID != null && category.IsCategoryActive) myDataConnection.Save<Category>((object)category, category);

                if (cat.IsCategoryActive)
                {
                    q = "INSERT INTO SnipeCategory (SnipeID, CategoryID, SnipeCategoryActive) VALUES (" + snipe.SnipeID + ", " + cat.CategoryID + ", 1);";
                    dbAdap.ExecuteNonQuery(q);
                }
            }

            return snipe;
        }

        public virtual void Update<T>(object item, T type)
        {
            Snipe snipe = (Snipe)item;

            #region Control Object

            snipe.ControlObject();

            if (snipe.SnipeID == null)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.SnipeIDNull);

            SqlDataContext myDataConnection = new SqlDataContext();

            if (snipe.SnipeStyle == EnumSnipeStyle.BidOptimizer && snipe.SnipeCategories != null && snipe.SnipeCategories.Count > 0)
            {
                Query qBuf = new Query();

                qBuf.QueryName = "CheckCategoryYoungerForSaving";

                Criterion critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "SnipeID";
                critere.Value = snipe.SnipeID;

                qBuf.Criteria.Add(critere);

                critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "Categories";

                string cat = "";

                foreach (Category cattp in snipe.SnipeCategories)
                    cat += cattp.CategoryID.ToString() + ",";

                if (cat.Length != 0)
                    cat = cat.Substring(0, cat.Length - 1);

                critere.Value = cat;

                qBuf.Criteria.Add(critere);

                critere = new Criterion();
                critere.Operator = CriteriaOperator.Equal;
                critere.PropertyName = "EndDate";
                critere.Value = "#DateParse#" + snipe.ItemEndDate.ToString() + "#EndDateParse#";

                qBuf.Criteria.Add(critere);

                qBuf.Members.Add("*");

                if (myDataConnection.GetByCriteria<Snipe>(qBuf).Count > 0)
                    throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.SnipeYoungerSnipeSameCategoryExists);
            }

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            string ItemLU;
            if (snipe.ItemLastUdpate != null) ItemLU = "'" + snipe.ItemLastUdpate.ToString() + "'";
            else ItemLU = "null";

            string SED;
            if (snipe.SnipeExecutionDate != null) SED = "'" + snipe.SnipeExecutionDate.ToString() + "'";
            else SED = "null";

            string SnipeGenOriginalID;
            if (snipe.SnipeGenOriginalID != null) SnipeGenOriginalID = snipe.SnipeGenOriginalID.ToString();
            else SnipeGenOriginalID = "null";

            string SnipeGenRemainingNb;
            if (snipe.SnipeGenRemainingNb != null) SnipeGenRemainingNb = snipe.SnipeGenRemainingNb.ToString();
            else SnipeGenRemainingNb = "null";

            string q = string.Format(_cmdUpdateSnipe, dbAdap.SafeSqlLiteral(snipe.SnipeName, 2), snipe.SnipeBid.ToString().Replace(',', '.'), (int)snipe.SnipeStatus, dbAdap.SafeSqlLiteral(snipe.SnipeDescription, 2), snipe.SnipeDelay, (int)snipe.SnipeType, snipe.ItemID, snipe.ItemEndDate, snipe.UserID, dbAdap.SafeSqlLiteral(snipe.SnipeErrorReason, 1), ItemLU, SED, (int)snipe.SnipeBidCurrency, snipe.SnipeBidInFinalCurrency.ToString().Replace(',', '.'), (int)snipe.SnipeFinalCurrency, Convert.ToInt32(snipe.SnipeInProgress), snipe.ItemSellerID, Convert.ToInt32(snipe.SnipeGenNextSnipe), SnipeGenOriginalID, SnipeGenRemainingNb, snipe.SnipeGenIncreaseBid, snipe.ItemURL, snipe.ItemTitle, snipe.ItemPictureURL, snipe.ItemLastKnownPrice.ToString().Replace(',', '.'), Convert.ToInt32(snipe.ResultInProgress), Convert.ToInt32(snipe.ValidityCheckInProgress), (int)snipe.SnipeStyle, snipe.SnipeID);

            dbAdap.ExecuteNonQuery(q);

            myDataConnection = new SqlDataContext();

            // We delete the snipe category. they will be added again afterwards
            q = "DELETE * FROM SnipeCategory WHERE SnipeID = " + snipe.SnipeID.ToString();
            dbAdap.ExecuteNonQuery(q);

            foreach (Category category in snipe.SnipeCategories)
            {
                Category cat = category;

                // UPDATE OF THE ASSOCIATED CATEGORIES
                if (category.CategoryID == null) cat = (Category)myDataConnection.Add<Category>((object)category, category);
                if (!category.IsCategoryActive) myDataConnection.Delete<Category>((object)category, category);
                if (category.CategoryID != null && category.IsCategoryActive) myDataConnection.Save<Category>((object)category, category);

                if (cat.IsCategoryActive)
                {
                    q = "INSERT INTO SnipeCategory (SnipeID, CategoryID, SnipeCategoryActive) VALUES (" + snipe.SnipeID + ", " + cat.CategoryID + ", 1);";
                    dbAdap.ExecuteNonQuery(q);
                }
            }

            // CANCEL THE OTHER SNIPES, IN CASE OF SUCCESS
            if (snipe.CancelSnipesInCaseOfSuccess && snipe.SnipeStatus == EnumSnipeStatus.SUCCEED)
            {
                q = string.Format(_cmdCancelSnipesSameCategories, (int)EnumSnipeStatus.CANCELLED, System.DateTime.Now.ToUniversalTime(), snipe.SnipeID);

                dbAdap.ExecuteNonQuery(q);
            }
        }

        public virtual void Delete<T>(object item, T type)
        {
            Snipe snipe = (Snipe)item;

            #region Control Object

            if (snipe.SnipeID == null)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.SnipeIDNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            string q = string.Format(_cmdDeleteSnipe, (int)EnumSnipeStatus.CANCELLED,  System.DateTime.Now.ToUniversalTime(), snipe.SnipeID);

            dbAdap.ExecuteNonQuery(q);
        }

        private List<Category> GetCategories(Snipe snipe)
        {
            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            CategoryDataMapper CatDM = new CategoryDataMapper();

            List<Category> catList = new List<Category>();

            DataSet datasetCat = dbAdap.ExecuteReader(string.Format(_cmdGetSnipeCategories, snipe.SnipeID));

            foreach (DataRow rowCat in datasetCat.Tables[0].Rows)
            {
                Category newC = CatDM.BuildEntityFromRawData(rowCat);
                newC.LoadedThroughSnipe = true;

                catList.Add(newC);
            }

            return catList;
        }

        public virtual IList<Snipe> GetByCriteria(Query q)
        {
            List<Snipe> snipes = new List<Snipe>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            string query = "";

            // HACK FOR GETBY CATEGORIES
            if (q.QueryName == "ForCategory")
            {
                string userID;
                string categoryID;
                
                if (q.Criteria[0].PropertyName.ToLower() == "userid")
                {
                    userID = q.Criteria[0].Value.ToString();
                    categoryID = q.Criteria[1].Value.ToString();
                }
                else
                {
                    userID = q.Criteria[1].Value.ToString();
                    categoryID = q.Criteria[0].Value.ToString();
                }

                query = string.Format(_cmdGetActiveSnipesForCategory, userID, categoryID);
            }
            else if (q.QueryName == "ForSuscribedUser")
            {                
                string itemEndDate;

                itemEndDate = q.Criteria[0].Value.ToString();

                query = string.Format(_cmdGetEndingSnipesForSubscribedUsers, itemEndDate);
            }
            else if (q.QueryName == "CheckCategoryYounger")
            {
                if (q.Criteria[0].Value != null)
                {
                    string itemID;

                    itemID = q.Criteria[0].Value.ToString();

                    query = string.Format(_CmdGetSnipesSameCategoryAndYounger, itemID);
                }
                else
                    query = string.Format(_CmdGetSnipesSameCategoryAndYounger, "null");
            }
            else if (q.QueryName == "CheckCategoryYoungerForSaving")
            {

                    string itemID;

                    if (q.Criteria[0].Value != null)
                        itemID = q.Criteria[0].Value.ToString();
                    else
                        itemID = "null";

                    string categoryList;

                    categoryList = q.Criteria[1].Value.ToString();

                    string enddate;

                    enddate = q.Criteria[2].Value.ToString();

                    query = string.Format(_CmdGetSnipesSameCategoryAndYoungerForSaving, itemID, categoryList, enddate);
            }
            else
                query = dbAdap.TranslateQuery(q, "Snipe");

            DataSet dataset = dbAdap.ExecuteReader(query);

            SqlDataContext myDataConnection = new SqlDataContext();

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Snipe newS = BuildEntityFromRawData(row);

                newS.SnipeCategories = this.GetCategories(newS);

                snipes.Add(newS);
            }

            return snipes;
        }

        public virtual IList<Snipe> GetAll()
        {
            List<Snipe> Snipes = new List<Snipe>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(_cmdGetAllSnipes);

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                Snipe newS = BuildEntityFromRawData(row);

                newS.SnipeCategories = this.GetCategories(newS);

                Snipes.Add(newS);
            }

            return Snipes;
        }

        protected virtual Snipe BuildEntityFromRawData(DataRow row)
        {
            Snipe snipe = new Snipe();

            snipe.SnipeID = int.Parse(row["SnipeID"].ToString());
            snipe.SnipeName = row["SnipeName"].ToString();
            snipe.SnipeBid = Math.Round(double.Parse(row["SnipeBid"].ToString()), 2, MidpointRounding.AwayFromZero);
            snipe.SnipeBidInFinalCurrency = double.Parse(row["SnipeBidInFinalCurrency"].ToString());
            snipe.SnipeStatus = (EnumSnipeStatus)int.Parse(row["SnipeStatusID"].ToString());
            if (!string.IsNullOrEmpty(row["ItemLastUpdate"].ToString())) snipe.ItemLastUdpate = DateTime.Parse(row["ItemLastUpdate"].ToString());
            if (!string.IsNullOrEmpty(row["SnipeExecutionDate"].ToString())) snipe.SnipeExecutionDate = DateTime.Parse(row["SnipeExecutionDate"].ToString());
            if (!string.IsNullOrEmpty(row["SnipeCancellationDate"].ToString())) snipe.SnipeCancellationDate = DateTime.Parse(row["SnipeCancellationDate"].ToString());
            snipe.SnipeDelay = int.Parse(row["SnipeDelay"].ToString());
            snipe.SnipeDescription = row["SnipeDescription"].ToString();
            snipe.ItemEndDate = DateTime.Parse(row["ItemEndDate"].ToString());
            snipe.SnipeErrorReason = row["SnipeErrorReason"].ToString();
            snipe.SnipeType = (EnumSnipeType)int.Parse(row["SnipeTypeID"].ToString());
            snipe.SnipeStyle = (EnumSnipeStyle)int.Parse(row["SnipeStyleID"].ToString());
            snipe.ItemID = long.Parse(row["ItemID"].ToString());

            try { snipe.UserID = int.Parse(row["UserID"].ToString()); }
            catch { snipe.UserID = int.Parse(row["user.UserID"].ToString()); }

            snipe.ItemSellerID = row["ItemSellerID"].ToString();
            snipe.ItemLastKnownPrice = Math.Round(double.Parse(row["ItemLastKnownPrice"].ToString()), 2, MidpointRounding.AwayFromZero);

            snipe.ItemURL = row["ItemURL"].ToString();
            snipe.ItemTitle = row["ItemTitle"].ToString();
            snipe.ItemPictureURL = row["ItemPictureURL"].ToString();

            try { snipe.SnipeInProgress = Convert.ToBoolean(row["SnipeInProgress"].ToString()); }
            catch { snipe.SnipeInProgress = Convert.ToBoolean(int.Parse(row["SnipeInProgress"].ToString())); }
            try { snipe.ResultInProgress = Convert.ToBoolean(row["ResultInProgress"].ToString()); }
            catch { snipe.ResultInProgress = Convert.ToBoolean(int.Parse(row["ResultInProgress"].ToString())); }
            try { snipe.ValidityCheckInProgress = Convert.ToBoolean(row["ValidityCheckInProgress"].ToString()); }
            catch { snipe.ValidityCheckInProgress = Convert.ToBoolean(int.Parse(row["ValidityCheckInProgress"].ToString())); }

#warning snipe.SnipeGenIncreaseBid = 0;
            try { snipe.SnipeGenIncreaseBid = int.Parse(row["SnipeGenIncreaseBid"].ToString()); }
            catch { snipe.SnipeGenIncreaseBid = 0; }

            try { snipe.SnipeGenNextSnipe = Convert.ToBoolean(row["SnipeGenNextSnipe"].ToString()); }
            catch { snipe.SnipeGenNextSnipe = Convert.ToBoolean(int.Parse(row["SnipeGenNextSnipe"].ToString())); }
            if (!string.IsNullOrEmpty(row["SnipeGenOriginalID"].ToString())) snipe.SnipeGenOriginalID = int.Parse(row["SnipeGenOriginalID"].ToString());
            if (!string.IsNullOrEmpty(row["SnipeGenRemainingNb"].ToString())) snipe.SnipeGenRemainingNb = int.Parse(row["SnipeGenRemainingNb"].ToString());

            snipe.SnipeBidCurrency = (EnumCurrencyCode)int.Parse(row["SnipeBidCurrency"].ToString());
            snipe.SnipeFinalCurrency = (EnumCurrencyCode)int.Parse(row["SnipeFinalCurrency"].ToString());

            return snipe;
        }
    }
}
