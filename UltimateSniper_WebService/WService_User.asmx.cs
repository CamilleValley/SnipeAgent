using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using UltimateSniper_BusinessObjects;
using System.Globalization;

namespace UltimateSniper_WebService
{
    /// <summary>
    /// Summary description for WService_User
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WService_User : System.Web.Services.WebService
    {
        private bool userVerified = true;

        private UltimateSniper_ServiceLayer.SL_User userHandler;

        [WebMethod(EnableSession=true)]
        public bool SetCertificate(string certificate)
        {
            if (certificate == SecurityResource.UserCertificate || certificate == SecurityResource.AdminCertificate)
            {
                this.userVerified = true;
                Session["UserVerified"] = true;
                return true;
            }

            return false;
        }

        private void Initialisation()
        {
            if (Session["userHandler"] == null)
                Session["userHandler"] = new UltimateSniper_ServiceLayer.SL_User();

            userHandler = (UltimateSniper_ServiceLayer.SL_User)Session["userHandler"];

            if (Session["UserVerified"] == null)
                Session["UserVerified"] = false;

            userVerified = (bool)Session["UserVerified"];

            if (!this.userVerified)
                throw new Exception("You need to invoke SetCertificate first.");
        }

        #region Other Functions

        /// <summary>
        /// Return official ebay time
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public DateTime GeteBayOfficialTime()
        {
            Initialisation();

            return userHandler.GeteBayOfficialTime();
        }

        /// <summary>
        /// Convert the bid value from an currency to another
        /// It uses a webservice call
        /// </summary>
        /// <param name="bid">price in the ori currency</param>
        /// <param name="ori">currency of the bid</param>
        /// <param name="des">destination currency</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public double ConvertBidCurrency(double bid, EnumCurrencyCode ori, EnumCurrencyCode des)
        {
            Initialisation();

            return userHandler.ConvertBidCurrency(bid, ori, des);
        }

        /// <summary>
        /// Returns the list of available currencies
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<EnumCurrencyCode> GetCurrencyList()
        {
            Initialisation();

            return userHandler.GetCurrencyList();
        }

        /// <summary>
        /// Return the list of available ebay sides
        /// first string of the dic: site id
        /// second: name
        /// Comment: was disable cause webservices dont handle dictionnaries
        /// </summary>
        /// <returns></returns>
        //[WebMethod(EnableSession = true)]
        //public Dictionary<string, string> GetSiteList()
        //{
        //    Initialisation();

        //    return userHandler.GetSiteList();
        //}

        /// <summary>
        /// Check if the email passed as parameter is correct
        /// </summary>
        /// <param name="inputEmail"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool isValidEmail(string inputEmail)
        {
            Initialisation();

            return userHandler.isValidEmail(inputEmail);
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="emailAddress">Destination email</param>
        /// <param name="emailTitle"></param>
        /// <param name="emailBody"></param>
        [WebMethod(EnableSession = true)]
        public void SendContactEmail(string emailAddress, string emailTitle, string emailBody)
        {
            Initialisation();

            userHandler.SendContactEmail(emailAddress, emailTitle, emailBody);
        }

        /// <summary>
        /// Sends an email to the user currently logged int
        /// </summary>
        /// <param name="emailTitle"></param>
        /// <param name="emailBody"></param>
        [WebMethod(EnableSession = true)]
        public void SendContactEmailShort(string emailTitle, string emailBody)
        {
            Initialisation();

            userHandler.SendContactEmail(emailTitle, emailBody);
        }

        /// <summary>
        /// Returns true if the current user can use the category feature
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool isCategoryActive()
        {
            Initialisation();

            return userHandler.isCategoryActive();
        }

        /// <summary>
        /// Returns true if the current user can use the auto retry feature
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public bool isGenNextSnipeActive()
        {
            Initialisation();

            return userHandler.isGenNextSnipeActive();
        }

        #endregion

        #region Category Functions

        /// <summary>
        /// Returns the list of active categories for the current user
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Category> GetActiveCategoriesForUser()
        {
            Initialisation();

            return userHandler.GetActiveCategoriesForUser();
        }

        /// <summary>
        /// Get the list of category passed as paramter
        /// </summary>
        /// <param name="categoryIDs">List of category IDs to be returned</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Category> GetCategories(List<int> categoryIDs)
        {
            Initialisation();

            return userHandler.GetCategories(categoryIDs);
        }

        /// <summary>
        /// Save the category set as parameter
        /// </summary>
        /// <param name="category"></param>
        [WebMethod(EnableSession = true)]
        public void CategorySave(Category category)
        {
            Initialisation();

            userHandler.CategorySave(category);
        }

        /// <summary>
        /// Disable the category set as parameter
        /// </summary>
        /// <param name="category"></param>
        [WebMethod(EnableSession = true)]
        public void DeleteCategory(Category category)
        {
            Initialisation();

            userHandler.DeleteCategory(category);
        }

        #endregion

        #region Snipe Functions

        /// <summary>
        /// Use this function to create a new Snipe
        /// You need to be logged in
        /// </summary>
        /// <param name="_snipe">Snipe to be created</param>
        /// <returns>Snipe with the ID</returns>
        [WebMethod(EnableSession = true)]
        public Snipe SnipeCreate(Snipe snipe)
        {
            Initialisation();

            return userHandler.SnipeCreate(snipe);
        }

        /// <summary>
        /// Use this function to update a Snipe
        /// You need to be logged in - it must be your Snipe
        /// </summary>
        /// <param name="snipe">Snipe to be updated</param>
        [WebMethod(EnableSession = true)]
        public void SnipeUpdate(Snipe snipe)
        {
            Initialisation();

            userHandler.SnipeUpdate(snipe);
        }

        /// <summary>
        /// Use this function to desactivate a snipe
        /// You need to be logged in - it must be your Snipe
        /// </summary>
        /// <param name="snipe">Snipe to be desactivated</param>
        [WebMethod(EnableSession = true)]
        public void SnipeDesactivate(Snipe snipe)
        {
            Initialisation();

            userHandler.SnipeDesactivate(snipe);
        }

        /// <summary>
        /// Set the bid for the snipe passed as parameter
        /// </summary>
        /// <param name="snipe">Snipe to be executed</param>
        [WebMethod(EnableSession = true)]
        public void SnipeBid(Snipe snipe)
        {
            Initialisation();

            userHandler.SnipeBid(snipe);
        }

        /// <summary>
        /// Return the snipes placed under a specific category
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Snipe> GetSnipesByCategory(int categoryID)
        {
            Initialisation();

            return userHandler.GetSnipesByCategory(categoryID);
        }

        /// <summary>
        /// Returns the list of snipes placed by the user over the last month
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Snipe> GetRecentSnipes()
        {
            Initialisation();

            return userHandler.GetRecentSnipes();
        }

        /// <summary>
        /// Returns all the snipes for the user currently logged in
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Snipe> GetAllSnipes()
        {
            Initialisation();

            return userHandler.GetAllSnipes();
        }

        /// <summary>
        /// Return the details of a specific snipe
        /// </summary>
        /// <param name="snipeID">Snipe ID of the snipe to be returned</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public Snipe GetSnipe(int snipeID)
        {
            Initialisation();

            return userHandler.GetSnipe(snipeID);
        }

        /// <summary>
        /// Return the list of snipes that the user placed and are still pending
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Snipe> GetActiveSnipes()
        {
            Initialisation();

            return userHandler.GetActiveSnipes();
        }

        /// <summary>
        /// Return the list of snipes that the user placed and won
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public List<Snipe> GetWonSnipes()
        {
            Initialisation();

            return userHandler.GetWonSnipes();
        }

        /// <summary>
        /// Return the details about the item ID sent as parameter
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public eBayItemData GetItemDetails(long itemID)
        {
            Initialisation();

            return userHandler.GetItemDetails(itemID);
        }

        /// <summary>
        /// Return the ItemCurrentHighestBidUserCurrency
        /// </summary>
        /// <param name="itemID">The item ID</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public double GetItemCurrentPrice(long itemID)
        {
            Initialisation();

            return userHandler.GetItemCurrentPrice(itemID);
        }

        #endregion

        #region User Functions

        /// <summary>
        /// Get the eBay URL from where the user need to sign in
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string GetSignInURL()
        {
            Initialisation();

            return userHandler.GetSignInURL();
        }

        /// <summary>
        /// Fetch the token from eBay and assign it to the user currently logged in
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void SetUserTokens()
        {
            Initialisation();

            userHandler.SetUserTokens();
        }

        /// <summary>
        /// Reload user data from the DB
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void UserRefreshInfo()
        {
            Initialisation();

            userHandler.UserRefreshInfo();
        }

        /// <summary>
        /// Send the password to the user
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void UserSendPassword(string emailOrUserName)
        {
            Initialisation();

            userHandler.UserSendPassword(emailOrUserName);
        }

        /// <summary>
        /// Use this function to log in
        /// </summary>
        /// <param name="userName">User name</param>
        /// <param name="userPassword">Password</param>
        [WebMethod(EnableSession = true)]
        public void UserLogin(string userName, string userPassword, bool mobile, double hoursDiffStdTime)
        {
            Initialisation();

            userHandler.UserLogin(userName, userPassword, mobile, hoursDiffStdTime);
        }

        /// <summary>
        /// Log the current user out
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void UserLogOut()
        {
            Initialisation();

            userHandler.UserLogOut();
        }

        /// <summary>
        /// Use this function to update a user.
        /// You need to have signed in first - with the same user
        /// </summary>
        /// <param name="user">User with data to be updated</param>
        [WebMethod(EnableSession = true)]
        public void UserSave(User user)
        {
            Initialisation();

            userHandler.UserSave(user);
        }

        /// <summary>
        /// Saves the user currently logged in.
        /// </summary>
        [WebMethod(EnableSession = true)]
        public void CurrentUserSave()
        {
            Initialisation();

            userHandler.UserSave();
        }

        /// <summary>
        /// Function to add X months to the subscription of the user
        /// </summary>
        /// <param name="subscriptionHex">Session key</param>
        [WebMethod(EnableSession = true)]
        public void UserAddSubscription(string subscriptionHex)
        {
            Initialisation();

            userHandler.UserAddSubscription(subscriptionHex);
        }

        /// <summary>
        /// Use this function to desactivate a user
        /// You need to be signed in - with the same user
        /// </summary>
        /// <param name="user"></param>
        [WebMethod(EnableSession = true)]
        public void UserDesactivate(User user)
        {
            Initialisation();

            userHandler.UserDesactivate(user);
        }

        /// <summary>
        /// Returns the item id from the item url
        /// </summary>
        /// <param name="user"></param>
        [WebMethod(EnableSession = true)]
        public long? ParseItemFromURL(string itemURL)
        {
            Initialisation();

            return userHandler.ParseItemFromURL(itemURL);
        }

        /// <summary>
        /// Create the user passed as parameter in the DB
        /// </summary>
        /// <param name="user">User to be created</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public User UserCreate(User user)
        {
            Initialisation();

            return userHandler.UserCreate(user);
        }

        #endregion

    }
}
