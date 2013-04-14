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
using System.Threading;
using System.Globalization;
using UltimateSniper_WebSite;

namespace UltimateSniper_MobileWebSite
{
    public partial class Menu : Page, IMenu
    {
        private MenuPresenter presenter;
        private SL_User userService;

        #region Initialisation

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            userService = (SL_User)Session["userService"];

            presenter.Initialize(IsPostBack);

            this.lblComments.Text = string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new MenuPresenter(this, navigation);
        }

        #endregion

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            this.lblComments.Text += information + "<br/>";
        }

        public SL_User serviceUser
        {
            get { return userService; }
            set { userService = value; }
        }

        public bool ShowLoggedInOptions
        {
            set
            {
                this.divLog.Visible = !value;
                this.divMenu.Visible = value;
            }
        }

        public string ConnectionInfo
        {
            set
            {
                this.lblUserInfo.Text = value;
            }
        }

        protected void lnkInfo_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.UserDetails);
        }

        protected void lnkCategory_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Categories);
        }

        protected void lnkSnipes_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Snipes);
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            this.presenter.LogOut();
        }

        protected void btnLog_Click(object sender, EventArgs e)
        {
            this.presenter.LogIn(this.txtBxLogin.Text, this.txtBxPassword.Text, true, 0);
        }

    }
}
