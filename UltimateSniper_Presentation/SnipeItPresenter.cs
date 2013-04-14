using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using System.Web;

namespace UltimateSniper_Presentation
{
    public class SnipeItPresenter
    {
        private readonly ISnipeIt _view;
        private INavigationWorkflow _navigation;

        #region Initialisation

        public void Initialize(bool isPostBack)
        {
            if (!isPostBack)
                this._view.showItemDetails = false;
        }

        public SnipeItPresenter(ISnipeIt view, INavigationWorkflow navigation)
        {
            this._view = view;
            this._navigation = navigation;
        }

        #endregion

        #region Actions

        public void LoadItemDetails()
        {
            try
            {
                long? itemID = null;

                try 
                { 
                    itemID = long.Parse(this._view.itemID); 
                }
                catch
                {
                    itemID = this._view.serviceUser.ParseItemFromURL(this._view.itemID);

                    if (itemID == null)
                        this._view.AddInformation("ItemIDNotANumber", true, EnumSeverity.Error);
                    else
                        this._view.itemID = itemID.ToString();
                }

                if (itemID != null)
                {
                    this._view.showItemDetails = true;
                }
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
