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
using System.Collections.Generic;
using UltimateSniper_WebSite.UserControls;
using UltimateSniper_Logger;

namespace UltimateSniper_WebSite
{
    public partial class SnipeManagement : PageBase, ISnipeManagement
    {
        private SnipeManagementPresenter presenter;
        private bool openSnipes;

        #region Initialisation

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userService"] == null)
                Session["userService"] = new SL_User();

            this.serviceUser = (SL_User)Session["userService"];

            presenter.Initialize(IsPostBack);
            //this.showSnipeDetails = false;

            //if (Session["SnipeFirstLaunch"] == null)
            //{
            //    presenter.RefreshList();
            //    Session["SnipeFirstLaunch"] = false;
            //}
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Navigation navigation = new Navigation();
            navigation.Page = this;

            presenter = new SnipeManagementPresenter(this, navigation);
        }

        #endregion

        #region Actions

        protected void drpDwnListType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.openSnipes = (this.snipeListType == EnumSnipeListType.Open);
            presenter.RefreshList();
        }

        #endregion

        #region Accessors

        public EnumSnipeListType snipeListType
        {
            get 
            {
                if (this.drpDwnListType.SelectedValue == "Open") return EnumSnipeListType.Open;
                if (this.drpDwnListType.SelectedValue == "Recent") return EnumSnipeListType.Recent;
                if (this.drpDwnListType.SelectedValue == "Won") return EnumSnipeListType.Won;
                if (this.drpDwnListType.SelectedValue == "All") return EnumSnipeListType.All;

                return EnumSnipeListType.Recent;
            }
            set 
            {
                if (value == EnumSnipeListType.Open) this.drpDwnListType.SelectedValue = "Open";
                if (value == EnumSnipeListType.Recent) this.drpDwnListType.SelectedValue = "Recent";
                if (value == EnumSnipeListType.Won) this.drpDwnListType.SelectedValue = "Won";
                if (value == EnumSnipeListType.All) this.drpDwnListType.SelectedValue = "All";
            }
        }

        public bool showSnipeDetails
        {
            set { this.SnipeDetails.Visible = value; }
        }

        public Snipe snipeDetails
        {
            set 
            {
                this.showSnipeDetails = true;
                this.SnipeDetails.SetSnipe(value);
            }
        }

        public SL_User serviceUser
        {
            get { return ((Site)this.Master).userService; }
            set { ((Site)this.Master).userService = value; }
        }

        public List<Snipe> snipeList
        {
            set
            {
                this.RepeaterSnipeList.DataSource = value;
                this.RepeaterSnipeList.DataBind();

                if (value == null || value.Count == 0)
                    PanelNoSnipe.Visible = true;
                else
                    PanelNoSnipe.Visible = false;

                this.showSnipeDetails = false;
            }
        }

        protected void DataBound(object sender, RepeaterItemEventArgs Arg)
        {
            UCSnipeDetails WebUserC = Arg.Item.FindControl("SnipeDetails") as UCSnipeDetails;

            if (Arg.Item != null && Arg.Item.DataItem != null)
            {
                Logger.CreateLog("BindSnipe",((Snipe)Arg.Item.DataItem).SnipeID.ToString(), null, EnumLogLevel.INFO);
                WebUserC.SetSnipe((Snipe)Arg.Item.DataItem);
                if (this.openSnipes) WebUserC.Reduce();
            }
        } 

        public void AddInformation(string information, bool isMessageCode, EnumSeverity severity)
        {
            ((Site)this.Page.Master).AddComment(information, isMessageCode, severity);
        }

        #endregion

    }
}
