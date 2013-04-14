using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public interface ISnipeManagement
    {
        SL_User serviceUser { get; }

        List<Snipe> snipeList { set; }
        EnumSnipeListType snipeListType { get;  set; }
        bool showSnipeDetails { set; }
        Snipe snipeDetails { set; }

        void AddInformation(string information, bool isMessageCode, EnumSeverity severity);
    }
}
