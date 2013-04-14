using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_Presentation
{
    public class CategoriesPresenter
    {
        private readonly ICategories _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (this._view.serviceUser == null || !this._view.serviceUser.IsUserLoggedIn)
                this._navigation.NavigateToPage(EnumView.Home, null);
        }

        public CategoriesPresenter(ICategories view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion
    }
}