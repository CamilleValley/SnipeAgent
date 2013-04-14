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
using UltimateSniper_BusinessObjects;
using UltimateSniper_ServiceLayer;

namespace UltimateSniper_WebSite.MasterPages
{
    public partial class AboutUs : PageBase, IAboutUs
    {
        private AboutUsPresenter presenter;

        #region Initialisation

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

            presenter = new AboutUsPresenter(this, navigation);
        }

        #endregion

        public bool showUserEmailAddress
        {
            set
            {
                this.lblUserEmailAddress.Visible = value;
                this.txtBoxUserEmailAddress.Visible = value;
            }
        }

        public string emailTitle
        {
            set { this.txtBoxEmailTitle.Text = value; }
            get { return this.txtBoxEmailTitle.Text; }
        }

        public string emailBody
        {
            set { this.txtBoxEmailBody.Text = value; }
            get { return this.txtBoxEmailBody.Text; }
        }

        public string userEmailAddress
        {
            set { this.txtBoxUserEmailAddress.Text = value; }
            get { return this.txtBoxUserEmailAddress.Text; }
        }

        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            this.presenter.SendEmail();
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        public SL_User serviceUser
        {
            get { return ((Site)this.Master).userService; }
            set { ((Site)this.Master).userService = value; }
        }

        protected void paypalDonate_Click(object sender, ImageClickEventArgs e)
        {
            PayPalRemotePost remote = new PayPalRemotePost(serviceUser.GetPaypalGateway());
            remote.InitialiseDonation(Request.Url.ToString());
            remote.Post();
        }
    }
}
