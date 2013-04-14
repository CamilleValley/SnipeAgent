using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using UltimateSniper_Logger;

namespace UltimateSniper_Presentation
{
    public class SnipeManagementPresenter
    {
        private readonly ISnipeManagement _view;
        private INavigationWorkflow _navigation;

        private EnumSnipeListType _snipeListType = EnumSnipeListType.Recent;

        public SnipeManagementPresenter(ISnipeManagement view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        public void Initialize(bool isPostBack)
        {
            if (this._view.serviceUser == null || !this._view.serviceUser.IsUserLoggedIn)
                this._navigation.NavigateToPage(EnumView.Home, null);

            if (!isPostBack)
                this.RefreshList();
        }

        public void LoadSnipeDetails(int snipeID)
        {
            Snipe snipe = this._view.serviceUser.GetSnipe(snipeID);
            this._view.snipeDetails = snipe;
        }

        public void RefreshList()
        {
            Logger.CreateLog("RefreshSnipeList_Beginning", EnumLogLevel.INFO);

            this._snipeListType = this._view.snipeListType;

            try
            {
                switch (this._snipeListType)
                {
                    case EnumSnipeListType.All:
                        this._view.snipeList = this._view.serviceUser.GetAllSnipes();
                        break;
                    case EnumSnipeListType.Open:
                        this._view.snipeList = this._view.serviceUser.GetActiveSnipes();
                        break;
                    case EnumSnipeListType.Recent:
                        this._view.snipeList = this._view.serviceUser.GetRecentSnipes();
                        break;
                    case EnumSnipeListType.Won:
                        this._view.snipeList = this._view.serviceUser.GetWonSnipes();
                        break;
                    default:
                        this._view.snipeList = this._view.serviceUser.GetAllSnipes();
                        break;
                }
            }
            catch (ControlObjectException ex)
            {
                this._view.snipeList = null;

                foreach (UserMessage error in ex.ErrorList)
                    this._view.AddInformation(error.MessageCode.ToString(), true, error.Severity);
            }
            catch (Exception ex)
            {
                this._view.snipeList = null;

                this._view.AddInformation(ex.Message + ex.StackTrace, false, EnumSeverity.Bug);
            }

            Logger.CreateLog("RefreshSnipeList_End", EnumLogLevel.INFO);
        }

    }
}
