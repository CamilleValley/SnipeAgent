using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_Presentation
{
    public class HomePresenter
    {
        private readonly IHome _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            this._view.showSnipeIt = (this._view.serviceUser != null && this._view.serviceUser.IsUserLoggedIn);
        }

        public HomePresenter(IHome view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion
    }
}
