using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using UltimateSniper_DAL;
using System.Threading;
using System.Text.RegularExpressions;
using UltimateSniper_Logger;
using UltimateSniper_Services;
using System.Globalization;
using System.Web;

namespace UltimateSniper_ServiceLayer
{
    public class SL_User
    {
        private User _userLoggedIn = null;
        private bool _isUserLoggedIn = false;
        private SqlDataContext _myDataConnection = new SqlDataContext();
        private UltimateSniper_Services.ServiceEmail _serviceEmail = new UltimateSniper_Services.ServiceEmail();
        private UltimateSniper_Services.ServiceEBay _serviceEBay = new UltimateSniper_Services.ServiceEBay(false);
        private UltimateSniper_Services.ServiceOthers _serviceOther = new UltimateSniper_Services.ServiceOthers();
        public bool isMobile;
        public string subscriptionRandomHex;

        #region Accessors

        public User UserLoggedIn
        {
            get { return _userLoggedIn; }
            set { _userLoggedIn = value; }
        }

        public bool IsUserLoggedIn
        {
            get { return _isUserLoggedIn; }
            set { _isUserLoggedIn = value; }
        }

        #endregion

        #region Other Functions

        /// <summary>
        /// Return official ebay time
        /// </summary>
        /// <returns></returns>
        public DateTime GeteBayOfficialTime()
        {
            if (_serviceEBay.User == null)
            {
                User user = new User();
                user.EBayUserToken = _serviceOther.DefaultToken();
                user.EBayUserTokenExpirationDate = _serviceOther.DefaultTokenExpirationDate();
                _serviceEBay.User = user;
            }

            return ServiceTimeZone.DateTimeToUniversal(this._serviceEBay.GeteBayOfficialTime());
        }

        public string DisplayDateTime(DateTime dateTime, CultureInfo culture)
        {
            return ServiceTimeZone.DisplayDateTime(dateTime, this._userLoggedIn.HoursDiffStdTime, culture);
        }

        /// <summary>
        /// Convert the bid value from an currency to another
        /// It uses a webservice call
        /// </summary>
        /// <param name="bid">price in the ori currency</param>
        /// <param name="ori">currency of the bid</param>
        /// <param name="des">destination currency</param>
        /// <returns></returns>
        public double ConvertBidCurrency(double bid, EnumCurrencyCode ori, EnumCurrencyCode des)
        {
            return this._serviceOther.ConvertValue(bid, ori.ToString(), des.ToString());
        }

        /// <summary>
        /// Returns the item id from the item url
        /// </summary>
        /// <param name="itemURL"></param>
        /// <returns></returns>
        public long? ParseItemFromURL(string itemURL)
        {
            Uri tempUri = new Uri(itemURL);
            string query = tempUri.Query;

            long? itemId = null;
            string strItemId;

            string hash = HttpUtility.ParseQueryString(query).Get("hash");
            if (hash.Length > 0)
            {
                strItemId = hash.Remove(0, 4); //remove the term "item"
                itemId = long.Parse(strItemId, System.Globalization.NumberStyles.HexNumber);
            }

            //long? itemId = null;
            //string strItemId;

            //if (itemURL.Contains("ViewItem"))
            //{
            //    Uri tempUri = new Uri(itemURL);
            //    string query = tempUri.Query;
            //    strItemId = HttpUtility.ParseQueryString(query).Get("item")
            //                ?? HttpUtility.ParseQueryString(query).Get("itemId");

            //    if (strItemId == null || strItemId.Length == 0)
            //    {
            //        string hash = HttpUtility.ParseQueryString(query).Get("hash");
            //        if (hash.Length > 0)
            //        {
            //            strItemId = hash.Remove(0, 4); //remove the term "item"
            //            itemId = long.Parse(strItemId, System.Globalization.NumberStyles.HexNumber);
            //        }
            //    }
            //    else
            //    {
            //        itemId = long.Parse(strItemId);
            //    }
            //}

            return itemId;
        }

        /// <summary>
        /// Returns the list of available currencies
        /// </summary>
        /// <returns></returns>
        public List<EnumCurrencyCode> GetCurrencyList()
        {
            return this._serviceOther.GetCurrencyList();
        }

        /// <summary>
        /// Return the list of available ebay sides
        /// first string of the dic: site id
        /// second: name
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetSiteList()
        {
            Dictionary<string, string> siteList = new Dictionary<string, string>();

            Array enumValArray = Enum.GetValues(typeof(EnumSites));
            List<EnumSites> enumValList = new List<EnumSites>(enumValArray.Length);

            foreach (EnumSites val in enumValArray)
                siteList.Add(((int)val).ToString(), val.ToString().Replace('_', ' '));

            return siteList;
        }

        /// <summary>
        /// Check if the email passed as parameter is correct
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        public bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAddress">Destination email</param>
        /// <param name="emailTitle"></param>
        /// <param name="emailBody"></param>
        public void SendContactEmail(string emailAddress, string emailTitle, string emailBody)
        {
            ServiceEmail.SendContactEmail(emailAddress, emailTitle, emailBody);
        }

        /// <summary>
        /// Sends an email to the user currently logged int
        /// </summary>
        /// <param name="emailTitle"></param>
        /// <param name="emailBody"></param>
        public void SendContactEmail(string emailTitle, string emailBody)
        {
            ServiceEmail.SendContactEmail(this.UserLoggedIn, emailTitle, emailBody);
        }

        /// <summary>
        /// Returns true if the current user can use the category feature
        /// </summary>
        /// <returns></returns>
        public bool isCategoryActive()
        {
            return (this._isUserLoggedIn && (this.UserLoggedIn.HasOptions() || !UltimateSniper_ServiceLayer.UserSettings.Default.isCategoryInOptionPackage));
        }

        /// <summary>
        /// Returns true if the current user can use the auto retry feature
        /// </summary>
        /// <returns></returns>
        public bool isGenNextSnipeActive()
        {
            return (this._isUserLoggedIn && (this.UserLoggedIn.HasOptions() || !UltimateSniper_ServiceLayer.UserSettings.Default.isGenNextSnipeInOptionPackage));
        }

        #endregion

        #region Category Functions

        /// <summary>
        /// Returns the list of active categories for the current user
        /// </summary>
        /// <returns></returns>
        public List<Category> GetActiveCategoriesForUser()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            List<Category> categories = new List<Category>();

            foreach (Category category in this.UserLoggedIn.UserCategoryList)
            {
                if (category.CategoryID != null && category.IsCategoryActive)
                    categories.Add(category);
            }

            return categories;
        }

        /// <summary>
        /// Get the list of category passed as paramter
        /// </summary>
        /// <param name="categoryIDs">List of category IDs to be returned</param>
        /// <returns></returns>
        public List<Category> GetCategories(List<int> categoryIDs)
        {
            List<Category> ListCategories = new List<Category>();
            Query q;
            Criterion crit;

            foreach (int catID in categoryIDs)
            {
                q = new Query();

                crit = new Criterion();
                crit.Operator = CriteriaOperator.Equal;
                crit.PropertyName = "CategoryID";
                crit.Value = catID;

                q.Criteria.Add(crit);

                q.Members.Add("*");

                ListCategories.AddRange(this._myDataConnection.GetByCriteria<Category>(q).ToList());
            }

            return ListCategories;
        }

        /// <summary>
        /// Save the category set as parameter
        /// </summary>
        /// <param name="category"></param>
        public void CategorySave(Category category)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            int nbCat = 0;
            bool found = false;
            Category catToBeSaved = null;

            foreach (Category cat in this.UserLoggedIn.UserCategoryList)
            {
                if (cat.IsCategoryActive) nbCat++;

                if (category.CategoryID != null && cat.CategoryID == category.CategoryID)
                {
                    cat.CategoryName = category.CategoryName;
                    catToBeSaved = cat;
                    found = true;
                }
            }

            if (category.CategoryID != null)
            {
                if (found)
                    this._myDataConnection.Save<Category>((object)catToBeSaved, catToBeSaved);
                else
                    throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.CategoryIDNotFound);
            }
            else
            {

                if (nbCat >= 10)
                    throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.CategoryMaxNumberReached);

                Category newCat = (Category)this._myDataConnection.Add<Category>((object)category, category);
                this.UserLoggedIn.UserCategoryList.Add(newCat);
            }
        }

        /// <summary>
        /// Disable the category set as parameter
        /// </summary>
        /// <param name="category"></param>
        public void DeleteCategory(Category category)
        {
            Category cat = null;

            foreach (Category currentCat in this.UserLoggedIn.UserCategoryList)
            {
                if (currentCat.CategoryID == category.CategoryID)
                {
                    currentCat.IsCategoryActive = false;
                    currentCat.CategoryDisactivationDate = ServiceTimeZone.DateTimeToUniversal(DateTime.Now);
                    cat = currentCat;
                    break;
                }
            }

            this._myDataConnection.Delete<Category>((object)cat, cat);
        }

        #endregion

        #region Snipe Functions

        /// <summary>
        /// Use this function to create a new Snipe
        /// You need to be logged in
        /// </summary>
        /// <param name="_snipe">Snipe to be created</param>
        /// <returns>Snipe with the ID</returns>
        public Snipe SnipeCreate(Snipe snipe)
        {
            if (!this._isUserLoggedIn || snipe.UserID != this._userLoggedIn.UserID)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDsMismatching);

            this.CheckSnipeItemData(snipe);

            System.Random RandNum = new System.Random();
            snipe.SnipeDelay = RandNum.Next(5, 15);

            snipe.SnipeBidInFinalCurrency = this.ConvertBidCurrency((double)snipe.SnipeBid, snipe.SnipeBidCurrency, snipe.SnipeFinalCurrency);

            snipe = (Snipe)this._myDataConnection.Add<Snipe>((object)snipe, snipe);

            return snipe;
        }

        private void CheckSnipeItemData(Snipe snipe)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserNotLoggedIn);

            eBayItemData item = this.GetItemDetails(snipe.ItemID);

            snipe.ItemEndDate = item.ItemEndDate;
            snipe.SnipeFinalCurrency = item.ItemCurrencyCode;
            snipe.ItemSellerID = item.ItemSellerID;
            snipe.ItemURL = item.ItemURL;
            snipe.ItemTitle = item.ItemTitle;
            snipe.ItemPictureURL = item.ItemPictureURL;

            if (snipe.SnipeID == null && !this.isGenNextSnipeActive() && snipe.SnipeGenNextSnipe)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.SnipeAutoRetryNotAllowed);

            if (snipe.SnipeGenNextSnipe && snipe.SnipeGenRemainingNb > UserSettings.Default.MaxSnipeAutoRetry)
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.MaxSnipeAutoRetryReached);

            if (item.ItemEndDate < this.GeteBayOfficialTime().AddSeconds(snipe.SnipeDelay + UserSettings.Default.SnipeExecutionDelay))
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.ItemEndedOrDelayToShort);

            if (snipe.SnipeBid == null)
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.SnipeBidNotANumber);

            if (item.ItemCurrentHighestBidUserCurrency > snipe.SnipeBid)
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.SnipeBidUnderCurrentPrice);
            else
                if (snipe.SnipeStatus == EnumSnipeStatus.OVERBID) snipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
        }

        /// <summary>
        /// Use this function to update a Snipe
        /// You need to be logged in - it must be your Snipe
        /// </summary>
        /// <param name="snipe">Snipe to be updated</param>
        public void SnipeUpdate(Snipe snipe)
        {
            if (!this._isUserLoggedIn || snipe.UserID != this._userLoggedIn.UserID)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDsMismatching);

            this.CheckSnipeItemData(snipe);

            snipe.SnipeBidInFinalCurrency = this.ConvertBidCurrency((double)snipe.SnipeBid, snipe.SnipeBidCurrency, snipe.SnipeBidCurrency);

            this._myDataConnection.Save <Snipe>((object)snipe, snipe);
        }

        /// <summary>
        /// Return the paypal gateway (prod or sandbox)
        /// </summary>
        /// <returns></returns>
        public string GetPaypalGateway()
        {
            return UltimateSniper_Services.ServiceParametersHelper.PayPalGateway();
        }

        /// <summary>
        /// Use this function to desactivate a snipe
        /// You need to be logged in - it must be your Snipe
        /// </summary>
        /// <param name="snipe">Snipe to be desactivated</param>
        public void SnipeDesactivate(Snipe snipe)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserNotLoggedIn);

            if (snipe.UserID != this._userLoggedIn.UserID)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDsMismatching);

            this._myDataConnection.Delete<Snipe>((object)snipe, snipe);
        }

        /// <summary>
        /// Place the bid for the snipe passed as parameter
        /// </summary>
        /// <param name="snipe"></param>
        public void SnipeBid(Snipe snipe)
        {
            this._serviceOther.CheckSnipeValidity(snipe, this._serviceEBay);
            this._serviceEBay.SetSnipe(snipe);
        }

        /// <summary>
        /// Return the snipes placed under a specific category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public List<Snipe> GetSnipesByCategory(int categoryID)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(crit);

            crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "CategoryID";
            crit.Value = categoryID;

            q.Criteria.Add(crit);

            q.QueryName = "ForCategory";

            q.Members.Add("*");

            return this._myDataConnection.GetByCriteria<Snipe>(q).ToList();
        }

        /// <summary>
        /// Returns the list of snipes placed by the user over the last month
        /// </summary>
        /// <returns></returns>
        public List<Snipe> GetRecentSnipes()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(crit);

            crit = new Criterion();
            crit.Operator = CriteriaOperator.GreaterThan;
            crit.PropertyName = "ItemEndDate";
            DateTime nowMin1Month = this.GeteBayOfficialTime().AddMonths(-1);
            crit.Value = "#DateParse#" + nowMin1Month.ToString() + "#EndDateParse#";

            q.Criteria.Add(crit);

            q.Members.Add("*");

            return this._myDataConnection.GetByCriteria<Snipe>(q).ToList();
        }

        /// <summary>
        /// Returns all the snipes for the user currently logged in
        /// </summary>
        /// <returns></returns>
        public List<Snipe> GetAllSnipes()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(crit);

            q.Members.Add("*");

            return this._myDataConnection.GetByCriteria<Snipe>(q).ToList();
        }

        /// <summary>
        /// Return the details of a specific snipe
        /// </summary>
        /// <param name="snipeID">Snipe ID of the snipe to be returned</param>
        /// <returns></returns>
        public Snipe GetSnipe(int snipeID)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            return this._serviceOther.GetSnipe(snipeID);
        }

        /// <summary>
        /// Return the list of snipes that the user placed and are still pending
        /// </summary>
        /// <returns></returns>
        public List<Snipe> GetActiveSnipes()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(crit);

            crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "SnipeStatusID";
            crit.Value = 1;

            q.Criteria.Add(crit);

            q.Members.Add("*");

            return this._myDataConnection.GetByCriteria<Snipe>(q).ToList();
        }

        /// <summary>
        /// Return the list of snipes that the user placed and won
        /// </summary>
        /// <returns></returns>
        public List<Snipe> GetWonSnipes()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "UserID";
            crit.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(crit);

            crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "SnipeStatusID";
            crit.Value = 4;

            q.Criteria.Add(crit);

            q.Members.Add("*");

            return this._myDataConnection.GetByCriteria<Snipe>(q).ToList();
        }

        /// <summary>
        /// Return the details about the item ID sent as parameter
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <returns></returns>
        public eBayItemData GetItemDetails(long itemID)
        {
            if (this._isUserLoggedIn)
            {
                eBayItemData item = _serviceEBay.GetItemDetails(itemID);

                if (this.IsUserLoggedIn)
                    item.ItemCurrentHighestBidUserCurrency = this.ConvertBidCurrency(item.ItemCurrentHighestBid, item.ItemCurrencyCode, this._userLoggedIn.UserCurrency);
               
                return item;
            }
            else
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);
        }

        /// <summary>
        /// Return the ItemCurrentHighestBidUserCurrency
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <returns></returns>
        public double GetItemCurrentPrice(long itemID)
        {
            if (this._isUserLoggedIn)
            {
                eBayItemData item = _serviceEBay.GetItemDetails(itemID, true);

                if (this.IsUserLoggedIn)
                    item.ItemCurrentHighestBidUserCurrency = this.ConvertBidCurrency(item.ItemCurrentHighestBid, item.ItemCurrencyCode, this._userLoggedIn.UserCurrency);

                return item.ItemCurrentHighestBidUserCurrency;
            }
            else
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);
        }

        #endregion

        #region User Functions

        /// <summary>
        /// Get the eBay URL from where the user need to sign in
        /// </summary>
        /// <returns></returns>
        public string GetSignInURL()
        {
            if (this._isUserLoggedIn)
            {
                if (!this.UserLoggedIn.HasValidAssignedEBayAccount())
                {
                    User user = new User();
                    user.EBayUserToken = _serviceOther.DefaultToken();
                    user.EBayUserTokenExpirationDate = _serviceOther.DefaultTokenExpirationDate();
                    _serviceEBay.User = user;
                }

                return _serviceEBay.BuildTokenURL();
            }
            else
                throw new ControlObjectException(EnumSeverity.Information, EnumMessageCode.UserNotLoggedIn);
        }

        public void SetUserTokens()
        {
            IList<TokenFetcher> fetchers = this._myDataConnection.GetAll<TokenFetcher>();

            foreach (TokenFetcher fetch in fetchers)
            {
                this.SetUserToken(fetch);
            }
        }

        public void SetUserToken()
        {
            TokenData token = _serviceEBay.FetchToken();

            this._userLoggedIn.EBayUserToken = token.Token;
            this._userLoggedIn.EBayUserTokenExpirationDate = token.ExpirationDate;

            this.UserSave();
        }

        /// <summary>
        /// Fetch the token from eBay and assign it to the user currently logged in
        /// </summary>
        public bool SetUserToken(TokenFetcher fetcher)
        {
            if (_serviceEBay.User == null)
            {
                User user = new User();
                user.EBayUserToken = _serviceOther.DefaultToken();
                user.EBayUserTokenExpirationDate = _serviceOther.DefaultTokenExpirationDate();
                _serviceEBay.User = user;
            }

            try
            {
                TokenData token = _serviceEBay.FetchToken(fetcher);

                Query q = new Query();

                Criterion c = new Criterion();
                c.Operator = CriteriaOperator.Equal;
                c.PropertyName = "UserID";
                c.Value = fetcher.UserID;

                q.Criteria.Add(c);

                q.Members.Add("*");

                IList<User> users = this._myDataConnection.GetByCriteria<User>(q);

                if (users.Count == 1)
                {
                    User user = users[0];

                    this._userLoggedIn.EBayUserToken = token.Token;
                    this._userLoggedIn.EBayUserTokenExpirationDate = token.ExpirationDate;
                    this.UserSave(user);
                }
                else
                    throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserCouldNotBeLoaded);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reload user data from the DB
        /// </summary>
        public void UserRefreshInfo()
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            Query q = new Query();

            Criterion c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserID";
            c.Value = this._userLoggedIn.UserID;

            q.Criteria.Add(c);

            q.Members.Add("*");

            IList<User> users = this._myDataConnection.GetByCriteria<User>(q);

            if (users.Count == 1)
            {
                double hoursDiffStdTime = this._userLoggedIn.HoursDiffStdTime;

                this._userLoggedIn = users[0];
                this._userLoggedIn.HoursDiffStdTime = hoursDiffStdTime;
            }
            else
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.UserCouldNotBeLoaded);
        }

        /// <summary>
        /// Send the password to the user
        /// </summary>
        public void UserSendPassword(string emailOrUserName)
        {
            Query q = new Query();

            Criterion c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserName";
            c.Value = "'" + emailOrUserName + "'";

            q.Criteria.Add(c);

            c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserActive";
            c.Value = "1";

            q.Criteria.Add(c);

            q.Members.Add("*");

            IList<User> users = this._myDataConnection.GetByCriteria<User>(q);

            if (users.Count == 1)
            {
                ServiceEmail.SendEmail(users[0], "[SnipeAgent] Your password", "Here is your password: " + users[0].UserPassword);
                return;
            }

            q = new Query();

            c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserEmailAddress";
            c.Value = "'" + emailOrUserName + "'";

            q.Criteria.Add(c);

            c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserActive";
            c.Value = "1";

            q.Criteria.Add(c);

            q.Members.Add("*");

            users = this._myDataConnection.GetByCriteria<User>(q);

            if (users.Count == 1)
            {
                ServiceEmail.SendEmail(users[0], "[SnipeAgent] Your password", "Here is your password: " + users[0].UserPassword);
                return;
            }
            else
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.UserEmailOrUserNameNotFound);
        }   

        /// <summary>
        /// Use this function to log in
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">Password</param>
        public void UserLogin(string userName, string userPassword, bool mobile, double hoursDiffStdTime)
        {
            Query q = new Query();

            Criterion c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserName";
            c.Value = "'" + userName + "'";

            q.Criteria.Add(c);

            c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserPassword";
            c.Value = "'" + userPassword + "'";

            q.Criteria.Add(c);

            c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserActive";
            c.Value = "1";

            q.Criteria.Add(c);

            q.Members.Add("*");

            IList<User> users = this._myDataConnection.GetByCriteria<User>(q);

            if (users.Count == 1)
            {
                if (mobile && !users[0].HasOptions() && UltimateSniper_ServiceLayer.UserSettings.Default.isMobileInOptionPackage)
                    throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.UserMobileAccessDenied);

                this._isUserLoggedIn = true;
                this._userLoggedIn = users[0];
                this._userLoggedIn.HoursDiffStdTime = hoursDiffStdTime;

                Random random = new Random();
                int num = random.Next();
                this.subscriptionRandomHex = num.ToString("X");

                this.isMobile = mobile;

                // We assign the user for the token!!
                this._serviceEBay.User = this._userLoggedIn;

                //Thread myThread = new Thread(new ThreadStart(UpdateSnipeValidity));

                //myThread.Start();
            }
            else
                throw new ControlObjectException(EnumSeverity.Warning, EnumMessageCode.UserPasswordAndLoginNotMatching);
        }

        private void UpdateSnipeValidity()
        {
            Logger.CreateLog("Beginning__UpdateSnipeValidity", EnumLogLevel.INFO);

            try
            {
                foreach (Snipe snipe in this.GetActiveSnipes())
                {
                    try { this._serviceOther.CheckSnipeValidity(snipe, this._serviceEBay); }
                    catch (Exception ex)
                    {
                        Logger.CreateLog("Error__UpdateSnipeValidity", snipe.SnipeID.ToString(), ex, EnumLogLevel.FATAL);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__UpdateSnipeValidity", "", ex, EnumLogLevel.FATAL);
            }

            Logger.CreateLog("Ending__UpdateSnipeValidity", EnumLogLevel.INFO);
        }

        /// <summary>
        /// Log the current user out
        /// </summary>
        public void UserLogOut()
        {
            this._isUserLoggedIn = false;
            this._userLoggedIn = null;
        }

        /// <summary>
        /// Use this function to update a user.
        /// You need to have signed in first - with the same user
        /// </summary>
        /// <param name="user">User with data to be updated</param>
        public void UserSave(User user)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.UserNotLoggedIn);

            if (this._userLoggedIn.UserID != user.UserID)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDsMismatching);

            this._myDataConnection.Save<User>((object)user, user);
        }

        /// <summary>
        /// Saves the user currently logged in.
        /// </summary>
        public void UserSave()
        {
            this.UserSave(this._userLoggedIn);
        }

        /// <summary>
        /// Function to add X months to the subscription of the user
        /// </summary>
        /// <param name="subscriptionHex">Session key</param>
        public void UserAddSubscription(string subscriptionHex)
        { 
            if (subscriptionHex != this.subscriptionRandomHex)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserSubscriptionHexMismatching);

            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            this._userLoggedIn.UserOptionsEndDate = this._userLoggedIn.UserOptionsEndDate.AddMonths(UserSettings.Default.UserOptionsSubscriptionNbMonths);

            // We change the random hex so that the user cant refresh the page
            Random random = new Random();
            int num = random.Next();
            this.subscriptionRandomHex = num.ToString("X");

            this.UserSave();
        }

        /// <summary>
        /// Use this function to desactivate a user
        /// You need to be signed in - with the same user
        /// </summary>
        /// <param name="user"></param>
        public void UserDesactivate(User user)
        {
            if (!this._isUserLoggedIn)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserNotLoggedIn);

            if (this._userLoggedIn.UserID != user.UserID)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.UserIDsMismatching);

            this._myDataConnection.Delete<User>((object)user, user);
        }

        /// <summary>
        /// Create the user passed as parameter in the DB
        /// </summary>
        /// <param name="user">User to be created</param>
        /// <returns></returns>
        public User UserCreate(User user)
        {
            user.UserRegistrationDate = ServiceTimeZone.DateTimeToUniversal(DateTime.Now);
            user.UserOptionsEndDate = user.UserRegistrationDate.AddMonths(UserSettings.Default.UserOptionsFreeNbMonths);
            this._serviceEBay.User = user;

            user = (User)this._myDataConnection.Add<User>((object)user, user);

            Category category = new Category();

            category.CategoryName = "Cat Num1";
            category.IsCategoryActive = true;
            category.UserID = user.UserID;

            this._myDataConnection.Add<Category>((object)category, category);

            category = new Category();

            category.CategoryName = "Cat Num2";
            category.IsCategoryActive = true;
            category.UserID = user.UserID;

            this._myDataConnection.Add<Category>((object)category, category);

            category = new Category();

            category.CategoryName = "Cat Num3";
            category.IsCategoryActive = true;
            category.UserID = user.UserID;

            this._myDataConnection.Add<Category>((object)category, category);

            ServiceEmail.SendEmail(user, "[SnipeAgent] Welcome", "Hello, thanks for registering on www.snipeagent.com.");

            this._userLoggedIn = user;
            this._isUserLoggedIn = true;

            return user;
        }

        #endregion
    }
}
