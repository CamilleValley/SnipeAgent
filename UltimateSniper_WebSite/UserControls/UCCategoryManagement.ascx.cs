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

namespace UltimateSniper_WebSite.UserControls
{
    public partial class UCCategoryManagement : System.Web.UI.UserControl, ICategoryManagement
    {
        private CategoryManagementPresenter presenter;

        #region Initialisation

        protected void Page_Load(object sender, EventArgs e)
        {
            presenter.Initialize(IsPostBack);

            if (!IsPostBack)
            {
                btnCancel.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCCategoryManagement_ConfirmCancel));
                btnDelete.Attributes.Add("onclick", string.Format("if(confirm('{0}')){{}}else{{return false}}", Resources.lang.UCCategoryManagement_ConfirmDelete));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this.Parent.Page;

            presenter = new CategoryManagementPresenter(this, navigation);
        }

        #endregion

        #region Accessors

        public SL_User serviceUser
        {
            get { return ((Site)this.Page.Master).userService; }
            set { ((Site)this.Page.Master).userService = value; }
        }

        public List<Category> Categories
        {
            get {
                List<Category> categories = new List<Category>();
                Category categorie;

                foreach (GridViewRow row in this.grdVwCategories.Rows)
                {
                    categorie = new Category();

                    if (string.IsNullOrEmpty(row.Cells[0].Text))
                        categorie.CategoryID = null;
                    else
                        categorie.CategoryID = int.Parse(row.Cells[0].Text);

                    categorie.CategoryName = row.Cells[1].Text;

                    categories.Add(categorie);
                }

                return categories;
            }
            set
            {
                this.grdVwCategories.DataSource = value;
                this.grdVwCategories.DataBind();
            }
        }

        #endregion

        #region Actions

        public void ClearView()
        {
            this.pnlEdit.Visible = false;
            this.lblCategoryID.Text = string.Empty;
            this.txtBxName.Text = string.Empty;
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        protected void grdVwCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlEdit.Visible = true;
            this.txtBxName.Text = grdVwCategories.SelectedRow.Cells[1].Text;
            this.lblCategoryID.Text = grdVwCategories.SelectedRow.Cells[0].Text;
            this.btnDelete.Visible = true;
            this.btnUpdate.Text = "Update";
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            Category category = new Category();

            category.IsCategoryActive = true;
            category.CategoryName = this.txtBxName.Text;
            if (string.IsNullOrEmpty(this.lblCategoryID.Text))
                category.CategoryID = null;
            else
                category.CategoryID = int.Parse(this.lblCategoryID.Text);

            this.presenter.Save(category);
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            this.pnlEdit.Visible = true;
            this.txtBxName.Text = "";
            this.lblCategoryID.Text = "";
            this.btnDelete.Visible = false;
            this.btnUpdate.Text = "Create";
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Category category = new Category();

            category.CategoryName = this.txtBxName.Text;
            category.CategoryID = int.Parse(this.lblCategoryID.Text);

            this.presenter.Delete(category);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.pnlEdit.Visible = false;
            this.AddInformation("ActionCancelled", true, EnumSeverity.Information);
        }

        #endregion

    }
}