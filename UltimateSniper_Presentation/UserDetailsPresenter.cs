using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class UserDetailsPresenter
    {
        private readonly IUserDetails _view;
        private INavigationWorkflow _navigation;

        public UserDetailsPresenter(IUserDetails view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        public void Initialize(bool isPostBack)
        {
            if (!this._view.serviceUser.IsUserLoggedIn && this._view.serviceUser.isMobile)
                this._navigation.NavigateToPage(EnumView.Home, null);

            if (!isPostBack)
            {
                ClearView();

                if (this._view.serviceUser != null && this._view.serviceUser.IsUserLoggedIn)
                    UpdateView(this._view.serviceUser.UserLoggedIn);
            }
            else
            {
                // We load the user only if he just signed in.
                if (this._view.serviceUser != null && 
                    this._view.serviceUser.IsUserLoggedIn && 
                    string.IsNullOrEmpty(this._view.UserID))
                    UpdateView(this._view.serviceUser.UserLoggedIn);
            }
        }

        private void ClearView()
        {
            _view.UserID = String.Empty;
            _view.UserDisactivationDate = String.Empty;
            _view.UserEmailAddress = String.Empty;
            _view.UserName = String.Empty;
            _view.UserPassword = String.Empty;
            _view.UserRegistrationDate = String.Empty;
            _view.eBayUserIDSet = false;
            _view.UserEbayUserID = String.Empty;
            _view.UserEbayUserPwd = String.Empty;

            _view.ConfirmationRegistration = false;
            _view.GetToken = false;
        }

        private void UpdateView(User user)
        {
            _view.UserID = user.UserID.ToString();
            _view.eBayRegistrationSiteID = user.EBayRegistrationSiteID;
            _view.UserDisactivationDate = user.UserDisactivationDate.ToString();
            _view.UserEmailAddress = user.UserEmailAddress;
            _view.UserName = user.UserName;
            _view.UserPassword = user.UserPassword;
            _view.UserEbayUserID = user.EBayUserID;
            _view.UserEbayUserPwd = user.EBayUserPwd;
            _view.UserRegistrationDate = this._view.serviceUser.DisplayDateTime(user.UserRegistrationDate, this._view.CurrentCulture);
            _view.UserCurrency = user.UserCurrency;
            _view.UserOptionPackEndDate = this._view.serviceUser.DisplayDateTime(user.UserOptionsEndDate, this._view.CurrentCulture);

            if (user.HasValidAssignedEBayAccount())
            {
                _view.eBayUserIDSet = true;
                _view.EBayUserTokenExpirationDate = this._view.serviceUser.DisplayDateTime(user.EBayUserTokenExpirationDate, this._view.CurrentCulture);
            }
            else
            {
                _view.eBayUserIDSet = false;
                _view.EBayUserTokenExpirationDate = String.Empty;
            }

            _view.GetToken = true;
        }

        public void GetUserToken()
        {
            this._navigation.NavigateToNextPage(EnumView.Registration, this._view.serviceUser.GetSignInURL());
        }

        public string GetTokenUrl()
        {
            return this._view.serviceUser.GetSignInURL();
        }

        public string GetSubscriptionHex()
        {
            return this._view.serviceUser.subscriptionRandomHex;
        }

        public void SetUserToken(string auth)
        {
            bool success = bool.Parse(auth);

            try
            {
                if (success)
                    this._view.serviceUser.SetUserToken();
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.AddInformation(ex.Message, false, EnumSeverity.Bug);
            }
        }

        public void AddUserSubscription(string randSubHex)
        {
            try
            {
                this._view.serviceUser.UserAddSubscription(randSubHex);
                this._view.AddInformation("SubscriptionAdded", true, EnumSeverity.Information); 
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.AddInformation(ex.Message, false, EnumSeverity.Bug);
            }
        }

        public void SaveUser()
        {
            User user;

            try
            {
                if (this._view.serviceUser.IsUserLoggedIn)
                    user = this._view.serviceUser.UserLoggedIn;
                else
                    user = new User();

                user.EBayRegistrationSiteID = this._view.eBayRegistrationSiteID;

                user.UserCredit = 0;

                user.UserEmailAddress = this._view.UserEmailAddress;
                user.UserName = this._view.UserName;
                user.UserPassword = this._view.UserPassword;
                user.UserCurrency = this._view.UserCurrency;
                user.UserIPAddress = this._view.UserIPAddress;
                //user.EBayUserPwd = this._view.UserEbayUserPwd;
                //user.EBayUserID = this._view.UserEbayUserID;

                if (this._view.serviceUser.IsUserLoggedIn)
                {
                    this._view.serviceUser.UserSave(user);
                    this._view.AddInformation("UserUpdated", true, EnumSeverity.Information); 
                }
                else
                {
                    this._view.serviceUser.UserCreate(user);
                    this._navigation.ReloadPage("reg=ok");
                }
            }
            catch (ControlObjectException ex)
            {
                if (this._view.serviceUser.IsUserLoggedIn)
                    this._view.serviceUser.UserRefreshInfo();

                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                if (this._view.serviceUser.IsUserLoggedIn)
                    this._view.serviceUser.UserRefreshInfo();

                this._view.AddInformation(ex.Message, false, EnumSeverity.Bug);
            }
        }

        public void DisableUser()
        {
            User user;

            try
            {
                user = this._view.serviceUser.UserLoggedIn;
                this._view.serviceUser.UserSave(user);
                this._view.serviceUser.UserLogOut();

            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.AddInformation(ex.Message, false, EnumSeverity.Bug);
            }
        }

        /// <summary>
        /// Only used for the mobile version
        /// Sends the user back to the main menu
        /// </summary>
        public void Back()
        {
            this._navigation.NavigateToPage(EnumView.Home, null);
        }
    }
}
