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
using UltimateSniper_Presentation;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_WebSite.UserControls
{
    public partial class UCMenu : System.Web.UI.UserControl, IMenu
    {
        private MenuPresenter presenter;

        #region Initialisation

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter.Initialize(IsPostBack);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this.Parent.Page;

            presenter = new MenuPresenter(this, navigation);
        }

        #endregion

        #region Accessors

        public SL_User serviceUser
        {
            get { return ((Site)this.Page.Master).userService; }
            set { ((Site)this.Page.Master).userService = value; }
        }

        public bool ShowLoggedInOptions
        {
            set
            {
                this.PanelLoginDetails.Visible = value;
                this.PanelLoginForm.Visible = !value;
                this.PanelSubMenu.Visible = value;
            }
        }

        public string ConnectionInfo
        {
            set
            {
                this.lblConnectionInfo.Text = value;
            }
        }

        #endregion

        #region Actions

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            this.presenter.LogIn(this.txtBxUserName.Text, this.txtBxUserPassword.Text, false, Double.Parse(Request.Cookies["hoursDiffStdTime"].Value));
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        protected void lnkHome_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Home);
        }

        protected void lnkAboutUs_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.AboutUs);
        }

        protected void lnkRegister_Click(object sender, EventArgs e)
        {
            this.serviceUser.UserLogOut();
            this.presenter.NavigateTo(EnumView.Registration);
        }

        protected void lnkInformation_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.UserDetails);
        }

        protected void lnkSnipes_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Snipes);
        }

        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            this.presenter.LogOut();
        }

        protected void lnkMobility_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Mobility);
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.ForgotPassword);
        }

        protected void lnkCategories_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Categories);
        }

        protected void lnkFeatures_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.Features);
        }

        protected void lnkWhatsSnipe_Click(object sender, EventArgs e)
        {
            this.presenter.NavigateTo(EnumView.WhatsSnipe);
        }

        protected void imgLogOut_Click(object sender, ImageClickEventArgs e)
        {
            this.presenter.LogOut();
        }

        #endregion

    }
}