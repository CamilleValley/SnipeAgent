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
using System.Collections.Generic;
using UltimateSniper_BusinessObjects;
using System.Globalization;

namespace UltimateSniper_WebSite
{
    public partial class UserDetails : PageBase, IUserDetails
    {
        private UserDetailsPresenter presenter;

        #region Accessors

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        public bool eBayUserIDSet
        {
            set 
            {
                this.lblEBayUserTokenExpirationDateExplaination.Visible = value;
                this.lblEBayUserTokenExpirationDate.Visible = value;

                if (value)
                    this.lblEbayAccount.Text = Resources.lang.UserDetails_AccountAssociated;
                else
                    this.lblEbayAccount.Text = Resources.lang.UserDetails_AccountNotAssociated;
            }
        }

        public CultureInfo CurrentCulture
        {
            get { return LanguageManager.CurrentCulture; }
        }

        public string UserID
        {
            set { this.lblUserID.Text = value; }
            get { return this.lblUserID.Text; }
        }

        public string UserName
        {
            set { this.txtBoxUserName.Text = value; }
            get { return this.txtBoxUserName.Text; }
        }

        public string UserPassword
        {
            set { this.txtBoxUserPassword.Text = value; }
            get { return this.txtBoxUserPassword.Text; }
        }

        public string UserEmailAddress
        {
            set { this.txtBoxUserEmailAddress.Text = value; }
            get { return this.txtBoxUserEmailAddress.Text; }
        }

        public EnumSites eBayRegistrationSiteID
        {
            set { this.DrpDwnSiteID.SelectedValue = ((int)value).ToString(); }
            get { return (EnumSites)Enum.Parse(typeof(EnumSites), this.DrpDwnSiteID.SelectedValue); }
        }

        public string UserDisactivationDate
        {
            set { this.lblActivityDate.Text = value; }
        }

        public string EBayUserTokenExpirationDate
        {
            set { this.lblEBayUserTokenExpirationDate.Text = value; }
        }

        public string UserOptionPackEndDate
        {
            set { this.lblUserOptionPackEndDate.Text = value; }
        }

        public string UserRegistrationDate
        {
            set { this.lblActivityDate.Text = value; }
            get { return this.lblActivityDate.Text; }
        }

        public string UserEbayUserID
        {
            set { this.txtBoxUserEbayUserID.Text = value; }
            get { return this.txtBoxUserEbayUserID.Text; }
        }

        public string UserEbayUserPwd
        {
            set { this.txtBoxUserEbayUserPwd.Text = value; }
            get { return this.txtBoxUserEbayUserPwd.Text; }
        }

        public EnumCurrencyCode UserCurrency
        {
            set { this.drpDwnCurrency.SelectedValue = value.ToString(); }
            get { return (EnumCurrencyCode) Enum.Parse(typeof(EnumCurrencyCode), this.drpDwnCurrency.SelectedValue); }
        }

        public SL_User serviceUser
        {
            get { return ((Site)this.Master).userService; }
            set { ((Site)this.Master).userService = value; }
        }

        public string UserIPAddress
        {
            get { return Request.UserHostAddress; }
        }
        
        public bool ConfirmationRegistration
        {
            set { this.PanelConfirmationRegistration.Visible = value; }
        }

        public bool GetToken
        {
            set { this.PanelGetToken.Visible = value; }
        }

        #endregion

        #region Functions

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            this.presenter.SaveUser();
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
        }

        protected void btnUpdateToken_Click(object sender, EventArgs e)
        {
            this.presenter.SetUserToken("true");
        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            this.presenter.DisableUser();
        }

        #endregion

        #region Loading

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new UserDetailsPresenter(this, navigation);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            this.serviceUser = (SL_User)Session["userService"];

            if (this.serviceUser.IsUserLoggedIn && !Page.IsPostBack)
                btnRedirect.Attributes.Add("onclick", "window.open('" + this.presenter.GetTokenUrl() + "');");

            this.txtBoxUserName.Enabled = !this.serviceUser.IsUserLoggedIn;

            string auth = Request.Params["authRes"];

            if (!string.IsNullOrEmpty(auth))
                presenter.SetUserToken(auth);

            string randSubHex = Request.Params["randSubHex"];

            if (!string.IsNullOrEmpty(randSubHex))
                presenter.AddUserSubscription(randSubHex);

            this.btnDisable.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblEBayUserTokenExpirationDate.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblActivityDate.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblEBayUserTokenExpirationDate.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblActivityDateExplaination.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblOptionsExplained.Visible = !this.serviceUser.IsUserLoggedIn;
            this.pnlExtendOptions.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblUserOptionPackEndDate.Visible = this.serviceUser.IsUserLoggedIn;
            this.lblUserOptionPackEndDateExplaination.Visible = this.serviceUser.IsUserLoggedIn;

            if (!IsPostBack)
            {
                btnDisable.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UserDetails_ConfirmDisable));

                this.drpDwnCurrency.DataSource = this.serviceUser.GetCurrencyList();
                this.drpDwnCurrency.DataBind();

                this.DrpDwnSiteID.Items.Clear();

                this.DrpDwnSiteID.DataSource = this.serviceUser.GetSiteList();
                this.DrpDwnSiteID.DataTextField = "Value";
                this.DrpDwnSiteID.DataValueField = "Key";
                this.DrpDwnSiteID.DataBind();
            }

            presenter.Initialize(IsPostBack);

            string reg = Request.Params["reg"];

            if (!string.IsNullOrEmpty(reg))
            {
                this.ConfirmationRegistration = true;
                //this.GetToken = true;
            }

            imgSiteIDHelp.Attributes.Add("title", Resources.lang.UserDetails_SiteIDHelp);
            imgRedirectHelp.Attributes.Add("title", Resources.lang.UserDetails_RedirectHelpHelp);
            imgUpdateHelp.Attributes.Add("title", Resources.lang.UserDetails_UpdateHelpHelp);
        }

        protected void paypalPay_Click(object sender, ImageClickEventArgs e)
        {
            PayPalRemotePost remote = new PayPalRemotePost(serviceUser.GetPaypalGateway());
            remote.InitialisePaiment(Request.Url.ToString() + "?randSubHex=" + presenter.GetSubscriptionHex());
            remote.Post();
        }

        protected void paypalDonate_Click(object sender, ImageClickEventArgs e)
        {
            PayPalRemotePost remote = new PayPalRemotePost(serviceUser.GetPaypalGateway());
            remote.InitialiseDonation(Request.Url.ToString());
            remote.Post();
        }        

        #endregion

    }
}
