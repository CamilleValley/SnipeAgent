using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using UltimateSniper_Presentation;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_WebSite.MasterPages
{
    public partial class Categories : System.Web.UI.Page, ICategories
    {
        private CategoriesPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            this.serviceUser = (SL_User)Session["userService"];

            presenter.Initialize(IsPostBack);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new CategoriesPresenter(this, navigation);
        }

        public SL_User serviceUser
        {
            get { return ((Site)this.Page.Master).userService; }
            set { ((Site)this.Page.Master).userService = value; }
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

    }
}
