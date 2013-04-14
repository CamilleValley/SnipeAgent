using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace UltimateSniper_WebSite
{
    public class PayPalRemotePost
    {
        public System.Collections.Specialized.NameValueCollection Inputs = new System.Collections.Specialized.NameValueCollection();
        public string Url;
        public string Method = "post";
        public string FormName = "form1";

        public PayPalRemotePost(string url)
        {
            this.Url = url;
        }

        public void Add(string name, string value)
        {
            Inputs.Add(name, value);
        }

        public void Post()
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.Write("<html><head>");
            System.Web.HttpContext.Current.Response.Write(String.Format("</head><body onload=document.{0}.submit()>", FormName));
            System.Web.HttpContext.Current.Response.Write(String.Format("<form name='{0}' method='{1}' action='{2}' >", FormName, Method, Url));

            for (int i = 0; i < Inputs.Keys.Count - 1; i++)
                System.Web.HttpContext.Current.Response.Write(String.Format("<input name='{0}' type='hidden' value='{1}'>", Inputs.Keys[i], Inputs[Inputs.Keys[i]]));

            System.Web.HttpContext.Current.Response.Write("</form>");
            System.Web.HttpContext.Current.Response.Write("</body></html>");
            System.Web.HttpContext.Current.Response.End();
        }

        public void InitialiseDonation(string requestUrl)
        {
            this.Add("cmd", "_s-xclick");
            this.Add("encrypted", "-----BEGIN PKCS7-----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-----END PKCS7-----");
            this.Add("return", requestUrl);
        }

        public void InitialisePaiment(string requestUrl)
        {
            this.Add("cmd", "_s-xclick");

            if (this.Url == "https://www.paypal.com/cgi-bin/webscr")
                this.Add("encrypted", "-----BEGIN PKCS7-----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-----END PKCS7-----");
            else
                this.Add("hosted_button_id", "KQ6HC4DJB79A2");
            
            this.Add("return", requestUrl);
            this.Add("rm", "2");
        }
    }
}