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
    public partial class UCSnipeIt : System.Web.UI.UserControl, ISnipeIt
    {
        private SnipeItPresenter presenter;

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

            presenter = new SnipeItPresenter(this, navigation);
        }

        #endregion

        #region Accessors

        public SL_User serviceUser
        {
            get { return ((Site)this.Page.Master).userService; }
            set { ((Site)this.Page.Master).userService = value; }
        }

        public string itemID
        {
            get { return this.txtBoxItemID.Text; }
            set { this.txtBoxItemID.Text = value; }
        }

        public bool showItemDetails
        {
            set 
            { 
                if (value)
                {
#warning dirty!
                    Snipe snipe = new Snipe();
                    snipe.ItemID = long.Parse(this.txtBoxItemID.Text);
                    snipe.ForInsert = true;
                    snipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
                    this.SnipeDetails.SetSnipe(snipe);
                }
                this.pnlSnipeDetails.Visible = value;
            }
        }

        #endregion

        #region Actions

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        protected void btnLoadItem_Click(object sender, EventArgs e)
        {
            this.presenter.LoadItemDetails();
        }

        #endregion

    }
}