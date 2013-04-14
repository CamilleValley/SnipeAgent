using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class MenuPresenter
    {
        private readonly IMenu _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (this._view.serviceUser != null && this._view.serviceUser.IsUserLoggedIn)
            {
                this._view.ShowLoggedInOptions = true;
                this._view.ConnectionInfo = this._view.serviceUser.UserLoggedIn.UserName;
            }
            else
                this._view.ShowLoggedInOptions = false;
        }

        public MenuPresenter(IMenu view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion

        #region Actions

        public void NavigateTo (EnumView view)
        {
            this._navigation.NavigateToPage(view, null);
        }

        public void LogIn(string userName, string userPassword)
        {
            LogIn(userName, userPassword, false, 0);
        }

        public void LogIn(string userName, string userPassword, bool mobile, double hoursDiffStdTime)
        {
            try
            {
                this._view.serviceUser.UserLogin(userName, userPassword, mobile, hoursDiffStdTime);
                this._navigation.ReloadPage();
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
        }

        public void LogOut()
        {
            this._view.serviceUser.UserLogOut();
            this._navigation.NavigateToPage(EnumView.Home, null);
        }

        #endregion
    }
}
