using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class ForgotPasswordPresenter
    {
        private readonly IForgotPassword _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (!isPostBack)
                CleanView();
        }

        public ForgotPasswordPresenter(IForgotPassword view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion

        #region Actions


        public void CleanView()
        {
            this._view.emailOrUserName = string.Empty;
        }

        public void SendPassword()
        {
            try
            {
                this._view.serviceUser.UserSendPassword(this._view.emailOrUserName);
                this._view.AddInformation("ForgotPasswordEmailSent", true, EnumSeverity.Information);
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

        #endregion
    }
}
