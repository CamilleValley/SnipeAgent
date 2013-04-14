using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace UltimateSniper_Services
{
    static public class ServiceParametersHelper
    {
        static bool ProductionVersion = GeneralSettings.Default.ProductionVersion;

        public static string PayPalGateway()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.PayPalGateway;
            else return ServicesSettingsSandbox.Default.PayPalGateway;
        }

        public static string shoppingServerUrl()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.ShoppingServerUrl;
            else return ServicesSettingsSandbox.Default.ShoppingServerUrl;
        }
        
        public static string eBayFindingUrl()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.eBayFindingUrl;
            else return ServicesSettingsSandbox.Default.eBayFindingUrl;
        }

        public static string eBayUrl()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.eBayUrl;
            else return ServicesSettingsSandbox.Default.eBayUrl;
        }

        public static string eBayVersion()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.eBayVersion;
            else return ServicesSettingsSandbox.Default.eBayVersion;
        }

        public static string EndUserIP()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.EndUserIP;
            else return ServicesSettingsSandbox.Default.EndUserIP;
        }

        public static int nbAPIRetry()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.nbAPIRetry;
            else return ServicesSettingsSandbox.Default.nbAPIRetry;
        }

        public static string SignInURL()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.SignInURL;
            else return ServicesSettingsSandbox.Default.SignInURL;
        }

        public static string SMTPAddress()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.SMTPAddress;
            else return ServicesSettingsSandbox.Default.SMTPAddress;
        }

        public static string RUName()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.RUName;
            else return ServicesSettingsSandbox.Default.RUName;
        }

        public static string DefaultGeteBayTimeToken()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.DefaultGeteBayTimeToken;
            else return ServicesSettingsSandbox.Default.DefaultGeteBayTimeToken;
        }

        public static DateTime DefaultGeteBayTimeTokenExpirationDate()
        {
            if (ProductionVersion) return ServicesSettingsProduction.Default.DefaultGeteBayTimeTokenExpirationDate;
            else return ServicesSettingsSandbox.Default.DefaultGeteBayTimeTokenExpirationDate;
        }
    }

    public class eBayKeyHandler
    {
        public int NbApiCalls = 0;
        private DateTime _lastKeyChange;
        private List<eBayKeySet> _ebayKeySetList = null;
        private int _ebayKeyIndice = 0;

        public eBayKeyHandler()
        {
            this.LoadKeys();
        }

        public eBayKeySet GetEBayKeySet()
        {
            TimeSpan span = DateTime.Now.Subtract(_lastKeyChange);
            double DiffInSec = span.TotalSeconds;

            if (NbApiCalls >= 5000 || DiffInSec > 3600)
                this.GetNextEBayKeySet();

            return _ebayKeySetList[_ebayKeyIndice];
        }

        public void LoadKeys()
        {
            bool ProductionVersion = GeneralSettings.Default.ProductionVersion;

            this._ebayKeySetList = new List<eBayKeySet>();

            if (!ProductionVersion)
            {
                eBayKeySet newKeySet = new eBayKeySet();

                newKeySet.CertID = ServicesSettingsSandbox.Default.eBayCertID;
                newKeySet.AppID = ServicesSettingsSandbox.Default.eBayAppID;
                newKeySet.DevID = ServicesSettingsSandbox.Default.eBayDevID;

                this._ebayKeySetList.Add(newKeySet);
            }
            else
            {
                XmlDocument xDoc = new XmlDocument();

                xDoc.Load("eBayKeys.xml");
                XmlNodeList keysets = xDoc.GetElementsByTagName("eBayKeySet");

                foreach (XmlNode node in keysets)
                {
                    eBayKeySet newKeySet = new eBayKeySet();

                    newKeySet.AppID = node["AppID"].InnerText;
                    newKeySet.CertID = node["CertID"].InnerText;
                    newKeySet.DevID = node["DevID"].InnerText;

                    this._ebayKeySetList.Add(newKeySet);
                }
            }
        }

        public void GetNextEBayKeySet()
        {
            this.LoadKeys();

            if (GeneralSettings.Default.HandleSeveralKeySet)
            {
                if (_ebayKeyIndice >= _ebayKeySetList.Count - 1)
                    _ebayKeyIndice = 0;
                else
                    _ebayKeyIndice += 1;

                this._lastKeyChange = DateTime.Now;
            }

            this.NbApiCalls = 0;
        }
    }

    public struct eBayKeySet
    {
        public string AppID;
        public string DevID;
        public string CertID;

        public eBayKeySet(string appID, string devID, string certID)
        {
            AppID = appID;
            DevID = devID;
            CertID = certID;
        }
    }
}
