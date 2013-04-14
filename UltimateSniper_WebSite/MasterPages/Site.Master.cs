using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using UltimateSniper_ServiceLayer;
using UltimateSniper_Presentation;
using UltimateSniper_BusinessObjects;
using System.Globalization;
using System.Threading;

namespace UltimateSniper_WebSite
{
    public partial class Site : MasterPageBase
    {
        public SL_User userService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            userService = (SL_User)Session["userService"];

            if (!Page.IsPostBack)
            {
                foreach (CultureInfo culture in LanguageManager.AvailableCultures)
                {
                    DropDownListLanguages.Items.Add(new System.Web.UI.WebControls.ListItem(culture.NativeName, culture.Name));
                }

                if (DropDownListLanguages.Items.Count == 1)
                    DropDownListLanguages.Items.Add(new System.Web.UI.WebControls.ListItem("Coming soon...", "NA"));
               
                DropDownListLanguages.SelectedValue = LanguageManager.CurrentCulture.Name;
            }
        }

        public void AddComment(string comment, bool isMessageCode, EnumSeverity severity)
        {
            this.CommentsMain.AddComment(comment, isMessageCode, severity);
        }

        protected void DropDownListLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListLanguages.Items.Count > 0 && DropDownListLanguages.SelectedValue != "NA") //make sure there is a SelectedValue
            { 
                ApplyNewLanguageAndRefreshPage(new CultureInfo(DropDownListLanguages.SelectedValue));
            }
        }
    }
}
