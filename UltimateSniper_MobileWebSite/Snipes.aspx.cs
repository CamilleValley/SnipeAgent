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
    public partial class Snipes : Page, ISnipeDetails
    {
        private SnipeDetailsPresenter presenter;
        private SL_User userService;

        #region Accessors

        public bool showSnipeStyles
        {
            set
            {
                this.pnlStyleChoice.Visible = value;
            }
        }

        public EnumSnipeStyle SnipeStyle
        {
            get
            {
                if (this.RadioButtonStandard.Checked) return EnumSnipeStyle.Snipe;
                if (this.RadioButtonManual.Checked) return EnumSnipeStyle.Manual;
                if (this.RadioButtonBidOptimizer.Checked) return EnumSnipeStyle.BidOptimizer;

                return EnumSnipeStyle.Manual;
            }
            set
            {
                if (value == EnumSnipeStyle.BidOptimizer) this.RadioButtonBidOptimizer.Checked = true;
                if (value == EnumSnipeStyle.Manual) this.RadioButtonManual.Checked = true;
                if (value == EnumSnipeStyle.Snipe) this.RadioButtonStandard.Checked = true;
            }
        }

        public SL_User serviceUser
        {
            get { return userService; }
            set { userService = value; }
        }

        public CultureInfo CurrentCulture
        {
            get { return new CultureInfo("en-US"); }
        }

        public bool snipeGenNextSnipe
        {
            get { return this.ckbx_AutoSnipe.Checked; }
            set { this.ckbx_AutoSnipe.Checked = value; }
        }

        public bool isEditAvailable
        {
            set
            {
              // TBD
            }
        }

        public string snipeStatus
        {
            set
            {
            }
        }

        public string currency
        {
            set
            {
                this.lblCurrency.Text = value;
            }
        }

        public string itemTitle
        {
            set
            {
                this.lblItemTitle.Text = value;
            }
        }

        public string SnipeGenIncreaseBid
        {
            get { return this.txtBx_Increase.Text; }
            set { this.txtBx_Increase.Text = value; }
        }

        public string SnipeGenRemainingNb
        {
            get { return this.txtBx_NbRetry.Text; }
            set { this.txtBx_NbRetry.Text = value; }
        }

        public bool isAutoGenSnipeEnabled
        {
            set
            {
                this.txtBx_Increase.Enabled = value;
                this.txtBx_NbRetry.Enabled = value;
                this.txtbxSnipeBid.Enabled = value;
            }
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            this.lblComments.Text += information + "<br/>";
        }

        public string itemURL
        {
            set
            {
                this.lnkItem.Target = "_blank";
                this.lnkItem.NavigateUrl = value;
            }
        }

        public string snipeID
        {
            get { return this.lblSnipeID.Text; }
            set
            {
                this.lblSnipeID.Text = value;
                this.btnDisable.Visible = !string.IsNullOrEmpty(this.lblSnipeID.Text);
            }
        }

        public string snipeDescription
        {
            get { return this.txtbxSnipeDescription.Text; }
            set { this.txtbxSnipeDescription.Text = value; }
        }

        public string snipeDelay
        {
            get { return this.drpDwnDelay.SelectedValue; }
            set { this.drpDwnDelay.SelectedValue = value; }
        }

        public string snipeBid
        {
            get { return this.txtbxSnipeBid.Text; }
            set { this.txtbxSnipeBid.Text = value; }
        }

        public string itemEndDate
        {
            set { this.lblItemEndDate.Text = value; }
        }

        public string itemID
        {
            get { return this.lnkItem.Text; }
            set { this.lnkItem.Text = value; }
        }

        public List<int> categoriesSelectedID
        {
            get
            {
                List<int> categoryList = new List<int>();

                for (int counter = 0; counter < this.ckbxListCategories.Items.Count; counter++)
                {
                    if (this.ckbxListCategories.Items[counter].Selected)
                        categoryList.Add(int.Parse(this.ckbxListCategories.Items[counter].Value));
                }
                return (categoryList);
            }
            set
            {
                foreach (int id in value)
                    this.ckbxListCategories.Items.FindByValue(id.ToString()).Selected = true;
            }
        }

        public List<Category> categories
        {
            set
            {
                this.ckbxListCategories.DataTextField = "CategoryName";
                this.ckbxListCategories.DataValueField = "CategoryID";
                this.ckbxListCategories.DataSource = value;
                this.ckbxListCategories.DataBind();
            }
        }

        public string snipeName
        {
            get { return this.txtbxSnipeName.Text; }
            set { this.txtbxSnipeName.Text = value; }
        }

        public bool isEditEnable
        {
            set
            {
                this.pnlAll.Enabled = value;
                this.btnCancel.Enabled = value;
                this.btnDisable.Enabled = value;
                this.btnSave.Enabled = value;
            }
        }

        public string imageURL
        {
            set { }
        }

        public string sellerID
        {
            set
            {
                this.lblsellerID.Text = value;
            }
        }

        public string currentPrice
        {
            set 
            {
                this.lblLastKnownPrice.Text = value;
            }
        }

        public bool showItemInfo
        {
            set
            {
                this.pnlItemInfo.Visible = value;
            }
        }

        public bool isCategoryActive
        {
            set
            {
                this.ckbxListCategories.Enabled = value;
            }
        }

        public void SetSnipe(Snipe snipe)
        {
            presenter.LoadSniper(snipe);
            this.snipeID = snipeID.ToString();
        }

        #endregion

        #region Functions

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.presenter.SaveSniper())
            {
                this.pnlAll.Visible = false;
                this.pnlSnipeList.Visible = true;
            }
        }

        protected void lnkMenu_Click(object sender, EventArgs e)
        {
            this.presenter.Back();
        }

        protected void GrdVwSnipeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.presenter.LoadSniper(int.Parse(GrdVwSnipeList.SelectedRow.Cells[0].Text));
            this.pnlAll.Visible = true;
            this.pnlItemInfo.Visible = true;
            this.pnlSnipeList.Visible = false;
            this.isEditEnable = true;
        }

        protected void btnLoadDetails_Click(object sender, EventArgs e)
        {
            try
            {
                Snipe snipe = new Snipe();
                snipe.ItemID = long.Parse(this.txtBoxItemID.Text);
                snipe.ForInsert = true;
                this.presenter.LoadSniper(snipe);
                this.pnlAll.Visible = true;
                this.pnlSnipeList.Visible = false;
                this.pnlItemInfo.Visible = false;
            }
            catch
            {
                this.AddInformation("WrongItemID", false, EnumSeverity.Warning);
            }
        }

        protected void btnDisable_Click(object sender, EventArgs e)
        {
            this.presenter.DisableSniper();
            this.pnlAll.Visible = false;
            this.pnlSnipeList.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.presenter.CancelChanges(false);
            this.pnlAll.Visible = false;
            this.pnlSnipeList.Visible = true;
        }

        #endregion

        #region Loading

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new SnipeDetailsPresenter(this, navigation);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            this.serviceUser = (SL_User)Session["userService"];

            presenter.Initialize(IsPostBack);

            this.lblComments.Text = String.Empty;

            // Not clean!
            try
            {
                this.GrdVwSnipeList.DataSource = this.userService.GetActiveSnipes();
                this.GrdVwSnipeList.DataBind();

                if (this.GrdVwSnipeList.Rows.Count == 0)
                    this.lblComments.Text = "No snipe to be displayed.<br/>";
            }
            catch
            {
                Navigation navigation = new Navigation();
                navigation.Page = this;
                navigation.NavigateToPage(EnumView.Error, null);
            }
        }

        #endregion

    }
}
