using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using UltimateSniper_Logger;

namespace UltimateSniper_WebService
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://ultimatesniper.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WService_Scheduler : System.Web.Services.WebService
    {
        public UltimateSniper_ServiceLayer.SL_Scheduler scheduler;

        private void Initialisation()
        {
            if (Session["schedulerHandler"] == null)
            {
                Session.Timeout = 40;
                Session["schedulerHandler"] = new UltimateSniper_ServiceLayer.SL_Scheduler();
            }

            Logger.CreateLog("WebService_Initialisation_SessionID_" + Session.SessionID, EnumLogLevel.INFO);

            scheduler = (UltimateSniper_ServiceLayer.SL_Scheduler)Session["schedulerHandler"];
        }

        [WebMethod(EnableSession = true)]
        public string Timer_CheckTime()
        {
            Initialisation();

            this.scheduler.Timer_CheckTime();

            return Session.SessionID;
        }

        [WebMethod(EnableSession = true)]
        public string Timer_UpdateEndingSnipes(bool ActiveSnipe)
        {
            Initialisation();

            this.scheduler.Timer_UpdateEndingSnipes(ActiveSnipe);

            return Session.SessionID;
        }

        [WebMethod(EnableSession = true)]
        public string Timer_BidOptimizer_RefreshSnipes()
        {
            Initialisation();

            this.scheduler.Timer_BidOptimizer_RefreshSnipes();

            return Session.SessionID;
        }

        [WebMethod(EnableSession = true)]
        public string Timer_CheckSnipeValidity()
        {
            Initialisation();

            this.scheduler.Timer_Snipe_CheckSnipeValidity();

            return Session.SessionID;
        }

    }
}
