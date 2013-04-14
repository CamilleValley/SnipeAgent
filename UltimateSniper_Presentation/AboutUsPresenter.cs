using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class AboutUsPresenter
    {
        private readonly IAboutUs _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (!isPostBack)
                CleanView();
        }

        public AboutUsPresenter(IAboutUs view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion

        #region Actions

        public void CleanView()
        {
            this._view.emailBody = string.Empty;
            this._view.userEmailAddress = string.Empty;
            this._view.emailTitle = string.Empty;

            this._view.showUserEmailAddress = !this._view.serviceUser.IsUserLoggedIn;
        }

        public void SendEmail()
        {
            bool send = true;
            string emailAddress = string.Empty;
            string emailTitle = string.Empty;
            string emailBody = string.Empty;

            if (this._view.serviceUser.IsUserLoggedIn)
                emailAddress = this._view.serviceUser.UserLoggedIn.UserEmailAddress;
            else
            {
                if (this._view.serviceUser.isValidEmail(this._view.userEmailAddress))
                    emailAddress = this._view.userEmailAddress;
                else
                {
                    send = false;
                    this._view.AddInformation("AboutUsEmailWrongFormat", true, EnumSeverity.Warning);
                }
            }

            if (string.IsNullOrEmpty(this._view.emailTitle))
            {
                send = false;
                this._view.AddInformation("AboutUsEmailTitleEmpty", true, EnumSeverity.Warning);
            }
            else
                emailTitle = this._view.emailTitle;

            if (string.IsNullOrEmpty(this._view.emailBody))
            {
                send = false;
                this._view.AddInformation("AboutUsEmailBodyEmpty", true, EnumSeverity.Warning);
            }
            else
                emailBody = this._view.emailBody;

            if (send)
            {
                this._view.serviceUser.SendContactEmail(emailAddress, emailTitle, emailBody);
                this._view.AddInformation("AboutUsEmailSent", true, EnumSeverity.Information);
            }
        }

        #endregion
    }
}
