using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public class CategoryManagementPresenter
    {
        private readonly ICategoryManagement _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (!this._view.serviceUser.IsUserLoggedIn)
                this._navigation.NavigateToPage(EnumView.Home, null);

            this.LoadCategories();
        }

        public CategoryManagementPresenter(ICategoryManagement view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion

        #region Action

        public void LoadCategories()
        {
            this._view.Categories = this._view.serviceUser.GetActiveCategoriesForUser();
        }

        public void Save(Category category)
        {
            try
            {
                bool newcat = category.CategoryID == null;

                category.UserID = this._view.serviceUser.UserLoggedIn.UserID;
                this._view.serviceUser.CategorySave(category);
                this.LoadCategories();
                this._view.ClearView();

                if (newcat)
                    this._view.AddInformation("CategoryCreated", true, EnumSeverity.Information);
                else
                    this._view.AddInformation("CategoryUpdated", true, EnumSeverity.Information);
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

        public void Delete(Category category)
        {
            try
            {
                this._view.serviceUser.DeleteCategory(category);
                this.LoadCategories();
                this._view.ClearView();
                this._view.AddInformation("CategoryDeleted", true, EnumSeverity.Information);
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

        #endregion
    }
}
