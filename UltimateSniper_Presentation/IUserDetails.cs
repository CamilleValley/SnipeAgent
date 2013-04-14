using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;
using System.Globalization;

namespace UltimateSniper_Presentation
{
    public interface IUserDetails
    {
        void AddInformation(string information, bool isMessageCode, EnumSeverity severity);

        string UserID { get; set; }
        string UserName { get; set; }
        string UserPassword { get; set; }
        string UserEmailAddress { get; set; }
        EnumSites eBayRegistrationSiteID { get; set; }
        string UserDisactivationDate { set; }
        string EBayUserTokenExpirationDate { set; }
        string UserRegistrationDate { set; }
        string UserOptionPackEndDate { set; }
        string UserEbayUserID { get; set; }
        string UserEbayUserPwd { get; set; }
        EnumCurrencyCode UserCurrency { get; set; }
        bool eBayUserIDSet { set; }

        SL_User serviceUser { get; }
        string UserIPAddress { get; }

        bool ConfirmationRegistration { set; }
        bool GetToken { set; }

        CultureInfo CurrentCulture { get; }
    }
}
