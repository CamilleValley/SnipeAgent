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
using UltimateSniper_WebSite;
using System.Collections.Generic;

namespace UltimateSniper_MobileWebSite
{
    public partial class CategoriesView : Page, ICategoryManagement
    {
        private CategoryManagementPresenter presenter;
        private SL_User userService;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            this.serviceUser = (SL_User)Session["userService"];

            presenter.Initialize(IsPostBack);

            this.lblComments.Text = string.Empty;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new CategoryManagementPresenter(this, navigation);
        }

        public SL_User serviceUser
        {
            get { return userService; }
            set { userService = value; }
        }

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            this.lblComments.Text += information + "<br/>";
        }

        public void ClearView()
        {
            this.pnlEdit.Visible = false;
            this.lblCategoryID.Text = string.Empty;
            this.txtBxName.Text = string.Empty;
        }

        public List<Category> Categories
        {
            get
            {
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

        protected void lnkMenu_Click(object sender, EventArgs e)
        {
            this.presenter.Back();
        }

    }
}
