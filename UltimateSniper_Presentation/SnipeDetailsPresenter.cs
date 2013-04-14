using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class SnipeDetailsPresenter
    {
        private readonly ISnipeDetails _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
        }

        public SnipeDetailsPresenter(ISnipeDetails view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        public void EditSniper(bool edit)
        {
            this._view.isEditEnable = edit;

            if (edit)
            {
                // The user has access to the categories only if he has the options
                this._view.isCategoryActive = this._view.serviceUser.isCategoryActive();
            }
        }

        public void ClearView()
        {
            this.EditSniper(false);

            this._view.snipeName = string.Empty;
            this._view.snipeBid = string.Empty;
            this._view.snipeID = string.Empty;
            this._view.itemEndDate = string.Empty;
            this._view.snipeDescription = string.Empty;
            this._view.itemID = string.Empty;
            this._view.SnipeGenIncreaseBid = string.Empty;
            this._view.SnipeGenRemainingNb = string.Empty;
            this._view.snipeGenNextSnipe = false;

            if (this._view.serviceUser.IsUserLoggedIn)
            {
                this._view.categories = this._view.serviceUser.GetActiveCategoriesForUser();
            }
            else
                this._view.isCategoryActive = false;

            this._view.isAutoGenSnipeEnabled = this._view.serviceUser.isGenNextSnipeActive();
        }

        #endregion

        #region Actions

        public void CancelChanges(bool insert)
        {
            if (!insert)
                this.EditSniper(false);
            this._view.AddInformation("ChangesCancelled", true, EnumSeverity.Information);
            this._navigation.ReloadPage();
        }

        public void LoadSniper(int snipeID)
        {
            LoadSniper(this._view.serviceUser.GetSnipe(snipeID));
        }

        public void LoadSniper (Snipe snipe)
        {
            double itemPrice;
            eBayItemData item;

            if (snipe.SnipeStatus == EnumSnipeStatus.ACTIVE)
            {
                if (snipe.ForInsert)
                {
                    item = this._view.serviceUser.GetItemDetails(snipe.ItemID);
                    itemPrice = item.ItemCurrentHighestBidUserCurrency;
                    snipe.ItemSellerID = item.ItemSellerID;
                    snipe.ItemEndDate = item.ItemEndDate;
                    snipe.ItemPictureURL = item.ItemPictureURL;
                    snipe.ItemTitle = item.ItemTitle;
                    snipe.ItemURL = item.ItemURL;
                }
                else
                {
#warning we never refresh the item price from the website
                    itemPrice = snipe.ItemLastKnownPrice;
                    //itemPrice = this._view.serviceUser.GetItemCurrentPrice(snipe.ItemID);
                }
            }
            else
                itemPrice = snipe.ItemLastKnownPrice;

            this._view.showSnipeStyles = this._view.serviceUser.UserLoggedIn.ShowSnipeStyles;
            this._view.SnipeStyle = snipe.SnipeStyle;

            this.ClearView();

            this._view.snipeID = snipe.SnipeID.ToString();
            this._view.itemEndDate = this._view.serviceUser.DisplayDateTime(snipe.ItemEndDate, this._view.CurrentCulture);
            this._view.itemID = snipe.ItemID.ToString();
            this._view.itemURL = snipe.ItemURL;
            this._view.imageURL = snipe.ItemPictureURL;
            this._view.sellerID = snipe.ItemSellerID;
            this._view.currentPrice = itemPrice.ToString();
            this._view.itemTitle = snipe.ItemTitle;
            this._view.currency = this._view.serviceUser.UserLoggedIn.UserCurrency.ToString();
            this._view.snipeStatus = snipe.SnipeStatus.ToString();

            if (snipe.ForInsert)
                this._view.isEditEnable = true;

            if (snipe.SnipeID != null)
            {
                this._view.snipeName = snipe.SnipeName;
                this._view.snipeBid = snipe.SnipeBid.ToString();
                this._view.snipeID = snipe.SnipeID.ToString();
                //this._view.snipeDelay = snipe.SnipeDelay.ToString();
                this._view.snipeDescription = snipe.SnipeDescription;
                this._view.snipeGenNextSnipe = snipe.SnipeGenNextSnipe;
                this._view.SnipeGenIncreaseBid = snipe.SnipeGenIncreaseBid.ToString();
                
                if (snipe.SnipeGenRemainingNb != null) this._view.SnipeGenRemainingNb = snipe.SnipeGenRemainingNb.ToString();
                else this._view.SnipeGenRemainingNb = string.Empty;

                if (snipe.SnipeStatus != EnumSnipeStatus.ACTIVE || snipe.SnipeExecutionDate != null || snipe.ItemEndDate < this._view.serviceUser.GeteBayOfficialTime())
                    this._view.isEditAvailable = false;
                else
                    this._view.isEditAvailable = true;

                List<int> categoryIDs = new List<int>();

                foreach (Category category in snipe.SnipeCategories)
                    categoryIDs.Add((int)category.CategoryID);

                this._view.categoriesSelectedID = categoryIDs;
            }
        }

        public void DisableSniper()
        {
            Snipe snipe = this._view.serviceUser.GetSnipe(int.Parse(this._view.snipeID));

            this._view.serviceUser.SnipeDesactivate(snipe);

            this._view.AddInformation("SnipeDeleted", true, EnumSeverity.Information);

            this.EditSniper(false);
        }

        public void Bid()
        {
            try
            {
                Snipe snipe = this._view.serviceUser.GetSnipe(int.Parse(this._view.snipeID));

                this._view.serviceUser.SnipeBid(snipe);

                this._view.AddInformation("SnipeBidPlaced", true, EnumSeverity.Information);
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.AddInformation(ex.Message, false, EnumSeverity.Warning);
            }
        }

        public bool SaveSniper()
        {
            try
            {
                Snipe snipe = null;

                if (string.IsNullOrEmpty(this._view.snipeID))
                {
                    snipe = new Snipe();

                    snipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
                    snipe.SnipeType = EnumSnipeType.ONLINE;
                }
                else
                    snipe = this._view.serviceUser.GetSnipe(int.Parse(this._view.snipeID));

                try 
                { 
#warning We consider that the separator is ,
                    snipe.SnipeBid = double.Parse(this._view.snipeBid.Replace('.',',')); 
                }
                catch { 
                    // Will be handled in the SL_User function 
                }

                snipe.ItemID = long.Parse(this._view.itemID);

                if (this._view.serviceUser.UserLoggedIn.ShowSnipeStyles)
                    snipe.SnipeStyle = this._view.SnipeStyle;
                else
                    snipe.SnipeStyle = EnumSnipeStyle.Manual;

                snipe.SnipeCategories = this._view.serviceUser.GetCategories(this._view.categoriesSelectedID);
                snipe.SnipeBidCurrency = this._view.serviceUser.UserLoggedIn.UserCurrency;
                //snipe.SnipeDelay = int.Parse(this._view.snipeDelay);
                snipe.SnipeDescription = this._view.snipeDescription;
                snipe.SnipeName = this._view.snipeName;

                if (this._view.snipeGenNextSnipe && this._view.serviceUser.isGenNextSnipeActive())
                {
                    snipe.SnipeGenNextSnipe = true;
                    try { snipe.SnipeGenIncreaseBid = int.Parse(this._view.SnipeGenIncreaseBid); }
                    catch
                    {
                        
                        UserMessage message = new UserMessage();
                        message.MessageCode = EnumMessageCode.SnipeWrongGenIncreaseBid;
                        List<UserMessage> errorList = new List<UserMessage>();
                        errorList.Add(message);

                        ControlObjectException ex = new ControlObjectException(errorList);

                        throw ex;
                    }

                    try { snipe.SnipeGenRemainingNb = int.Parse(this._view.SnipeGenRemainingNb); }
                    catch
                    {
                        UserMessage message = new UserMessage();
                        message.MessageCode = EnumMessageCode.SnipeWrongGenRemainingNb;
                        List<UserMessage> errorList = new List<UserMessage>();
                        errorList.Add(message);

                        ControlObjectException ex = new ControlObjectException(errorList);

                        throw ex;
                    }
                }
                else
                {
                    snipe.SnipeGenNextSnipe = false;
                    snipe.SnipeGenIncreaseBid = 0;
                    snipe.SnipeGenRemainingNb = null;
                }

                snipe.UserID = (int)this._view.serviceUser.UserLoggedIn.UserID;

                if (string.IsNullOrEmpty(this._view.snipeID))
                {
                    this._view.serviceUser.SnipeCreate(snipe);
                    this._view.AddInformation("SnipeCreated", true, EnumSeverity.Information);
                    this._navigation.ReloadPage("created=1");
                }
                else
                {
                    this._view.serviceUser.SnipeUpdate(snipe);
                    this._view.AddInformation("SnipeUpdated", true, EnumSeverity.Information);
                    this.EditSniper(false);
                    this.LoadSniper(snipe);
                }

                return true;
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.AddInformation(ex.Message + ex.StackTrace, false, EnumSeverity.Bug);
            }

            return false;
        }

        public void ReloadCategories()
        {
            Snipe snipe = this._view.serviceUser.GetSnipe(int.Parse(this._view.snipeID));

            List<int> categoryIDs = new List<int>();

            foreach (Category category in snipe.SnipeCategories)
                categoryIDs.Add((int)category.CategoryID);

            this._view.categoriesSelectedID = categoryIDs;
        }

        /// <summary>
        /// Only used for the mobile version
        /// Sends the user back to the main menu
        /// </summary>
        public void Back()
        {
            this._navigation.NavigateToPage(EnumView.Home, null);
        }

        #endregion

    }
}