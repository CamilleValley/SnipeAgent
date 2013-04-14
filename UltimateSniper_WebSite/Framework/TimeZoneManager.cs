using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.Remoting.Messaging;
using System.Xml.XPath;
using System.Xml;

/// <summary>
/// Summary description for TimeZoneManager
/// </summary>
public class TimeZoneManager
{
    /// <summary>
    /// Gets the time zone.
    /// </summary>
    /// <param name="offset">The offset.</param>
    /// <returns></returns>
    private static string GetTimeZone(double offset)
    {
        //Adjust result based on the server time zone
        string timeZoneKey = "timeOffset" + offset.ToString("F");
        string result = (CallContext.GetData(timeZoneKey) ?? "").ToString();
        if (result.Length == 0)
        {
            string zonesDocPath = HttpRuntime.AppDomainAppPath +"app_data/TimeZones.xml";
            XPathDocument timeZonesDoc = new XPathDocument(zonesDocPath);
            try
            {
                string xPath = "/root/option[@value='" + offset.ToString("F") + "']";
                XPathNavigator searchNavigator = timeZonesDoc.CreateNavigator();
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(searchNavigator.NameTable);
                XPathNodeIterator iterator = searchNavigator.Select(xPath, nsmgr);
                if (iterator.MoveNext())
                {
                    result = iterator.Current.Value;
                    if (!string.IsNullOrEmpty(result))
                    {
                        CallContext.SetData(timeZoneKey, result);
                    }
                }

            }
            catch (Exception ex)
            {
                return  ex.Message;
            }
        }

        return result;

    }

    /// <summary>
    /// Displays the date time.
    /// </summary>
    /// <param name="dateTime">The date time.</param>
    /// <param name="profile">The profile.</param>
    /// <returns></returns>
    public static string DisplayDateTime(DateTime dateTime, double hoursDiffStdTime)
    {
        //Move to universal time (GMT) with Zero offset 
        DateTime utcDateTime = dateTime.ToUniversalTime();
        //Add client side offset
        DateTime resultDateTime = utcDateTime.AddHours(hoursDiffStdTime + 1);
        return string.Concat(resultDateTime.ToShortDateString(), ", ", resultDateTime.ToShortTimeString(), ", ", GetTimeZone(hoursDiffStdTime));
    }
}
