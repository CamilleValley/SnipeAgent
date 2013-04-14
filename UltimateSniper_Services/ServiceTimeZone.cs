using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Runtime.Remoting.Messaging;
using System.Xml.XPath;
using System.Xml;
using System.Globalization;

namespace UltimateSniper_Services
{
    /// <summary>
    /// Summary description for TimeZoneManager
    /// </summary>
    public class ServiceTimeZone
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
                string zonesDocPath = "TimeZones.xml";
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
                    return ex.Message;
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
        public static DateTime DateTimeToUniversal(DateTime dateTime)
        {
            //Move to universal time (GMT) with Zero offset 
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Displays the date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        public static DateTime eBayDateTimeToUniversal(DateTime dateTime)
        {
            // Already Universal time
            return dateTime;
        }

        /// <summary>
        /// Displays the date time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        public static string DisplayDateTime(DateTime dateTime, double hoursDiffStdTime, CultureInfo culture)
        {
            //Add client side offset
            DateTime resultDateTime = dateTime.AddHours(hoursDiffStdTime + 1);
            return resultDateTime.ToString(culture);
            //return string.Concat(resultDateTime.ToShortDateString(), ", ", resultDateTime.ToShortTimeString(), ", ", GetTimeZone(hoursDiffStdTime));
        }
    }
}