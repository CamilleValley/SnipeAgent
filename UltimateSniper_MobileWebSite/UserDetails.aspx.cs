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
using UltimateSniper_WebSite;
using System.Globalization;

namespace UltimateSniper_MobileWebSite
{
    public partial class UserDetails : Page, IUserDetails
    {
        private UserDetailsPresenter presenter;
        private SL_User userService;

        #region Accessors

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            this.lblComments.Text += information + "<br/>";
        }

        public string UserIPAddress
        {
            get { return Request.UserHostAddress; }
        }
        
        public bool eBayUserIDSet
        {
            set { }
        }

        public CultureInfo CurrentCulture
        {
            get { return new CultureInfo("en-US"); }
        }

        public string UserID
        {
            set { this.lblUserID.Text = value; }
            get { return this.lblUserID.Text; }
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

        public string UserName
        {
            set { this.lblUserName.Text = value; }
            get { return this.lblUserName.Text; }
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
            set { this.lblSiteID.Text = value.ToString(); }
            get { return (EnumSites)Enum.Parse(typeof(EnumSites), this.lblSiteID.Text); }
        }

        public string UserDisactivationDate
        {
            set { }
        }

        public string EBayUserTokenExpirationDate
        {
            set { }
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

        public EnumCurrencyCode UserCurrency
        {
            set { this.lblCurrency.Text = value.ToString(); }
            get { return (EnumCurrencyCode)Enum.Parse(typeof(EnumCurrencyCode), this.lblCurrency.Text); }
        }

        public SL_User serviceUser
        {
            get { return userService; }
            set { userService = value; }
        }

        public bool ConfirmationRegistration
        {
            set { }
        }

        public bool GetToken
        {
            set { }
        }

        #endregion

        #region Functions

        protected void btn_Save_Click(object sender, EventArgs e)
        {
            this.presenter.SaveUser();
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            this.presenter.GetUserToken();
        }

        protected void lnkMenu_Click(object sender, EventArgs e)
        {
            this.presenter.Back();
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

            presenter.Initialize(IsPostBack);
        }

        #endregion

    }
}
