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
    public partial class ForgotPassword : PageBase, IForgotPassword
    {
        private ForgotPasswordPresenter presenter;

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

            presenter = new ForgotPasswordPresenter(this, navigation);
        }

        #endregion

        public string emailOrUserName
        {
            set { this.txtBoxEmailOrName.Text = value; }
            get { return this.txtBoxEmailOrName.Text; }
        }

        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            this.presenter.SendPassword();
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
    }
}
