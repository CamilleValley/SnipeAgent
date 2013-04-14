using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public interface ISnipeIt
    {
        SL_User serviceUser { get; }
        string itemID { get; set; }
        bool showItemDetails { set; }

        void AddInformation(string information, bool isMessageCode, EnumSeverity severity);
    }
}
