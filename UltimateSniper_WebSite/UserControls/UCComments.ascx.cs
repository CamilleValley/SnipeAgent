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
using System.Collections.Generic;
using UltimateSniper_Presentation;
using UltimateSniper_BusinessObjects;
using System.Resources;
using System.Threading;
using System.Globalization;

namespace UltimateSniper_WebSite
{
    public partial class Comments : System.Web.UI.UserControl
    {
        private List<string> comments = new List<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.CleanComments();

#warning extremely: the comment is added for the creation of a snipe
            string created = Request.Params["created"];

            if (!string.IsNullOrEmpty(created))
                this.AddComment("SnipeCreated", true, EnumSeverity.Information);
        }

        public void AddComment(string comment, bool isMessageCode, EnumSeverity severity)
        {
            ListItem item = new ListItem();

            if (!isMessageCode) item.Text = comment;
            else
            {
#warning >> The messages are extracted from the RES at the root of the project. This should be changed.
                try
                {
                    ResourceManager LocRM = new ResourceManager("UltimateSniper_WebSite.mess", typeof(Comments).Assembly);
                    item.Text = LocRM.GetString(comment);
                }
                catch
                {
                    item.Text = comment;
                }
            }

            switch (severity)
            {
                case EnumSeverity.Information:
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "black");
                    break;
                case EnumSeverity.Bug:
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "brown");
                    break;
                case EnumSeverity.Error:
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "red");
                    break;
                case EnumSeverity.Warning:
                    item.Attributes.CssStyle.Add(HtmlTextWriterStyle.Color, "orange");
                    break;
            }

            this.BulletListComments.Items.Add(item);

            if (this.BulletListComments.Items.Count > 0) this.PanelComments.Visible = true;
            else this.PanelComments.Visible = false;
        }

        public void CleanComments()
        {
            comments = new List<string>();
            this.BulletListComments.DataSource = this.comments;
            this.BulletListComments.DataBind();
            this.PanelComments.Visible = false;
        }
    }
}