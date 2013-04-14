using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public interface IAboutUs
    {
        SL_User serviceUser { get; }

        bool showUserEmailAddress { set; }
        string emailTitle { set; get; }
        string userEmailAddress { set; get; }
        string emailBody { set; get; }

        void AddInformation(string information, bool isMessageCode, EnumSeverity severity);
    }
}
