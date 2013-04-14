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
using UltimateSniper_ServiceLayer;

namespace UltimateSniper_SSLPages
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SL_User serviceUser = new SL_User();

            serviceUser.SetUserTokens();

            Response.Redirect("http://www.snipeagent.com/UserDetails.aspx?authRes=true");
        }
    }
}
