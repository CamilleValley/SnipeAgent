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
using System.Collections.Generic;
using System.Globalization;

namespace UltimateSniper_WebSite.UserControls
{
    public partial class UCSnipeDetails : System.Web.UI.UserControl, ISnipeDetails
    {
        private SnipeDetailsPresenter presenter;

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter.Initialize(IsPostBack);

            btnCancel.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCSnipeDetails_ConfirmCancel));
            btnDelete.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCSnipeDetails_ConfirmDelete));

            imgHelp.Attributes.Add("title", Resources.lang.UCSnipeDetails_AutoRetryHelp);
            imgCurrency.Attributes.Add("title", Resources.lang.UCSnipeDetails_CurrencyHelp);
            imgDelayHelp.Attributes.Add("title", Resources.lang.UCSnipeDetails_DelayHelp);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this.Parent.Page;

            presenter = new SnipeDetailsPresenter(this, navigation);
        }

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

        public CultureInfo CurrentCulture 
        {
            get { return LanguageManager.CurrentCulture; }
        }

        public SL_User serviceUser
        {
            get { return ((Site)this.Page.Master).userService; }
            set { ((Site)this.Page.Master).userService = value; }
        }

        public string snipeName
        {
            get { return this.txtBxSnipeName.Text; }
            set 
            { 
                this.txtBxSnipeName.Text = value;
                this.lblSnipeName.Text = value;
            }
        }

        public string imageURL
        {
            set
            {
                string url = "";

                if (string.IsNullOrEmpty(value))
                    url = "../Medias/Images/imgNoImg.gif";
                else
                    url = value;

                this.img_Main.ImageUrl = url;

                if (string.IsNullOrEmpty(value))
                    url = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "Medias\\Images\\imgNoImg.gif";
                    //url = "D:\\UltimateSniper\\UltimateSniper_WebSite\\Medias\\Images\\imgNoImg.gif";

                System.Net.WebClient c = new System.Net.WebClient();
                System.IO.Stream s = c.OpenRead(url);
      
                System.Drawing.Image image = System.Drawing.Image.FromStream(s);
                if (image.PhysicalDimension.Width > 220)
                    this.img_Main.Width = new Unit(220, UnitType.Pixel);

            }
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
            }
        }

        public bool snipeGenNextSnipe
        {
            get { return this.ckbx_AutoSnipe.Checked; }
            set { this.ckbx_AutoSnipe.Checked = value; }
        }

        public string SnipeGenIncreaseBid
        {
            get { return this.txtBx_Increase.Text; }
            set 
            { 
                this.txtBx_Increase.Text = value;

                if (string.IsNullOrEmpty(value))
                    value = Resources.lang.UCSnipeDetails_DefaultText;

                this.lbl_Increase.Text = value;
            }
        }

        public string SnipeGenRemainingNb
        {
            get { return this.txtBx_NbRetry.Text; }
            set 
            { 
                this.txtBx_NbRetry.Text = value;

                if (string.IsNullOrEmpty(value))
                    value = Resources.lang.UCSnipeDetails_DefaultText;

                this.lbl_NbRetry.Text = value;
            }
        }

        public string snipeDescription
        {
            get { return this.txtBxSnipeDescription.Text; }
            set 
            { 
                this.txtBxSnipeDescription.Text = value;

                if (string.IsNullOrEmpty(value))
                    value = Resources.lang.UCSnipeDetails_DefaultText;

                this.lblSnipeDescription.Text = value;
            }
        }

        public string snipeDelay
        {
            get { return this.drpDwnDelay.SelectedValue; }
            set { this.drpDwnDelay.SelectedValue = value; }
        }

        public string snipeBid
        {
            get { return this.txtbxSnipeBid.Text; }
            set 
            {
                this.txtbxSnipeBid.Text = value;
                this.lblSnipeBid.Text = value;
            }
        }

        public string itemEndDate
        {
            set { this.lblItemEndDate.Text = value; }
        }

        public string sellerID
        {
            set { this.lblSellerID.Text = value; }
        }

        public string currentPrice
        {
            set { this.lblCurrentPrice.Text = value; }
        }

        public string itemTitle
        {
            set { this.lblItemTitle.Text = value; }
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

                for (int counter = 0; counter < this.ckdLstBxCategories.Items.Count; counter++)
                {
                    if (this.ckdLstBxCategories.Items[counter].Selected)
                        categoryList.Add(int.Parse(this.ckdLstBxCategories.Items[counter].Value));
                }
                return (categoryList);
            }
            set
            {
                foreach (int id in value)
                    this.ckdLstBxCategories.Items.FindByValue(id.ToString()).Selected = true;
            }
        }

        public List<Category> categories
        {
            set 
            {
                this.ckdLstBxCategories.DataTextField = "CategoryName";
                this.ckdLstBxCategories.DataValueField = "CategoryID";
                this.ckdLstBxCategories.DataSource = value;
                this.ckdLstBxCategories.DataBind();
            }
        }

        public bool isEditEnable
        {
            set 
            {
                this.txtBx_Increase.Visible = value;
                this.txtBx_NbRetry.Visible = value;
                this.txtbxSnipeBid.Visible = value;
                this.txtBxSnipeDescription.Visible = value;
                this.txtBxSnipeName.Visible = value;

                this.btnCancel.Visible = value;
                this.btnDelete.Visible = value;
                this.btnCancel.Visible = value;
                this.btnEdit.Visible = !value;
                this.btnSave.Visible = value;

                this.btnBid.Visible = !value;
                this.lblClickNBid.Visible = !value;

                this.RadioButtonBidOptimizer.Enabled = value;
                this.RadioButtonStandard.Enabled = value;
                this.RadioButtonManual.Enabled = value;

                if (!value)
                {
                    if (this.pnlFurtherDetails.Visible)
                    {
                        this.btnExpand.Visible = false;
                        this.btnNarrow.Visible = true;
                        this.btnReduce.Visible = true;
                    }
                    else
                    {
                        this.btnExpand.Visible = true;
                        this.btnNarrow.Visible = false;
                        this.btnReduce.Visible = true;
                    }
                }
                else
                {
                    this.btnExpand.Visible = false;
                    this.btnNarrow.Visible = false;
                    this.btnReduce.Visible = false;
                }

                this.ckdLstBxCategories.Enabled = value;
                this.ckbx_AutoSnipe.Enabled = value;
                this.drpDwnDelay.Enabled = value;

                this.lbl_Increase.Visible = !value;
                this.lbl_NbRetry.Visible = !value;
                this.lblDelay.Visible = !value;
                this.lblSnipeBid.Visible = !value;
                this.lblSnipeDescription.Visible = !value;
                this.lblSnipeName.Visible = !value;

                this.pnlFurtherDetails.Visible = value;
            }
        }

        public bool isAutoGenSnipeEnabled
        {
            set
            {
                this.txtBx_Increase.Enabled = value;
                this.txtBx_NbRetry.Enabled = value;
                this.ckbx_AutoSnipe.Enabled = value;
            }
        }

        public bool isCategoryActive
        {
            set
            {
                this.ckdLstBxCategories.Enabled = value;
            }
        }

        public string currency
        {
            set
            {
                this.lblCurrencyCurrentPrice.Text = value;
                this.lblCurrencySnipeBid.Text = value;
                this.lblCurrencyIncrease.Text = value;
            }
        }

        public string snipeStatus
        {
            set
            {
                this.lblSnipeStatus.Text = value;
            }
        }

        public bool isEditAvailable
        {
            set
            {
                if (!value)
                {
                    this.isEditEnable = false;
                    this.btnEdit.Visible = false;
                    this.btnBid.Visible = false;
                    this.lblClickNBid.Visible = false;
                }
            }
        }

        public void SetSnipe(Snipe snipe)
        {
            this.ckbBoxInsertion.Checked = snipe.ForInsert;
            presenter.LoadSniper(snipe);
            this.pnlAll.Visible = true;
            this.snipeID = snipeID.ToString();

            if (this.ckbBoxInsertion.Checked)
                this.btnDelete.Visible = false;

            if (!snipe.ForInsert)
                btnSave.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCSnipeDetails_ConfirmSave));
            else
                btnSave.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCSnipeDetails_ConfirmCreation));
        }

        public void Expand()
        {
            presenter.ReloadCategories();
            this.pnlFurtherDetails.Visible = true;
            this.pnlNarrowDetails.Visible = true;
            this.btnExpand.Visible = false;
            this.btnNarrow.Visible = true;
            this.btnReduce.Visible = true;
            this.img_Main.Visible = true;
        }

        public void Narrow()
        {
            this.pnlFurtherDetails.Visible = false;
            this.pnlNarrowDetails.Visible = true;
            this.btnExpand.Visible = true;
            this.btnNarrow.Visible = false;
            this.btnReduce.Visible = true;
            this.img_Main.Visible = true;
        }

        public void Reduce()
        {
            this.pnlFurtherDetails.Visible = false;
            this.pnlNarrowDetails.Visible = false;
            this.btnExpand.Visible = true;
            this.btnNarrow.Visible = true;
            this.btnReduce.Visible = false;
            this.img_Main.Visible = false;
        }

        #endregion

        #region Actions

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            this.presenter.SaveSniper();
        }

        protected void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            this.Expand();
            this.presenter.EditSniper(true);
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            if (this.ckbBoxInsertion.Checked)
                this.presenter.CancelChanges(true);
            else
            {
                this.Narrow();
                this.presenter.CancelChanges(false);
            }
        }

        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            this.presenter.DisableSniper();
            this.Narrow();
        }

        protected void btnExpand_Click(object sender, ImageClickEventArgs e)
        {
            this.Expand();
        }

        protected void btnNarrow_Click(object sender, ImageClickEventArgs e)
        {
            this.Narrow();
        }

        protected void btnReduce_Click(object sender, ImageClickEventArgs e)
        {
            this.Reduce();
        }

        #endregion

        protected void btnBid_Click(object sender, ImageClickEventArgs e)
        {
            this.presenter.Bid();
        }

    }
}