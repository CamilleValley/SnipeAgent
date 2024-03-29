﻿using System;
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
using UltimateSniper_Presentation;

namespace UltimateSniper_WebSite
{
    public class Navigation : BaseNavigationHandler
    {
        private System.Web.UI.Page _page;

        public System.Web.UI.Page Page
        {
            get { return _page; }
            set { _page = value; }
        }

        public override void NavigateToPage(EnumView view, object newContext)
        {
            switch (view)
            {
                case EnumView.Home:
                    _page.Response.Redirect("Home.aspx", true);
                    break;
                case EnumView.UserDetails:
                    _page.Response.Redirect("UserDetails.aspx", true);
                    break;
                case EnumView.Registration:
                    _page.Response.Redirect("UserDetails.aspx", true);
                    break;
                case EnumView.Snipes:
                    _page.Response.Redirect("SnipeManagement.aspx", true);
                    break;
                case EnumView.Error:
                    _page.Response.Redirect("Error.aspx", true);
                    break;
                case EnumView.eBayRegistration:
                    _page.Response.Redirect(newContext.ToString(), true);
                    break;
                case EnumView.AboutUs:
                    _page.Response.Redirect("AboutUs.aspx", true);
                    break;
                case EnumView.ForgotPassword:
                    _page.Response.Redirect("ForgotPassword.aspx", true);
                    break;
                case EnumView.Mobility:
                    _page.Response.Redirect("Mobility.aspx", true);
                    break;
                case EnumView.WhatsSnipe:
                    _page.Response.Redirect("WhatsSnipe.aspx", true);
                    break;
                case EnumView.Categories:
                    _page.Response.Redirect("Categories.aspx", true);
                    break;
                case EnumView.Features:
                    _page.Response.Redirect("Features.aspx", true);
                    break;
                default:
                    _page.Response.Redirect("Error.aspx", true);
                    break;
            }
        }

        public override void NavigateToNextPage(EnumView view, object newContext)
        {
            EnumView newView = this.GetNextView(view, newContext);

            this.NavigateToPage(newView, newContext);
        }

        public override void ReloadPage()
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            if (url.Contains('?'))
                url = url.Substring(0, url.IndexOf("?"));

            Random randNum = new Random();

            url += "?" + randNum.Next().ToString();

            _page.Response.Redirect(url);
        }

        public override void ReloadPage(string param)
        {
            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            if (url.Contains('?'))
                url = url.Substring(0, url.IndexOf("?"));

            url += "?" + param;

            _page.Response.Redirect(url);
        }
    }
}
