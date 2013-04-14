using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;
using System.Globalization;

namespace UltimateSniper_Presentation
{
    public interface ISnipeDetails
    {
        SL_User serviceUser { get; }

        string snipeName { get; set; }
        string snipeID { get; set; }
        string snipeDescription { get; set; }
        string snipeDelay { get; set; }
        string itemEndDate { set; }
        string itemID { get; set; }
        string snipeBid { get; set; }
        string itemURL { set; }
        string imageURL { set; }
        bool isEditEnable { set; }
        bool isCategoryActive { set; }
        List<Category> categories { set; }
        List<int> categoriesSelectedID { get; set; }
        bool snipeGenNextSnipe { get; set; }
        bool isAutoGenSnipeEnabled { set; }
        bool isEditAvailable { set; }
        string SnipeGenIncreaseBid { get; set; }
        string SnipeGenRemainingNb { get; set; }
        string sellerID { set; }
        string currentPrice { set; }
        string itemTitle { set; }
        CultureInfo CurrentCulture { get; }
        string currency { set; }
        string snipeStatus { set; }
        bool showSnipeStyles { set; }
        EnumSnipeStyle SnipeStyle { get; set; }

        void AddInformation(string information, bool isMessageCode, EnumSeverity severity);
    }
}
