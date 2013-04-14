using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_ServiceLayer;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public interface ICategoryManagement
    {
        SL_User serviceUser { get; }

        void AddInformation(string information, bool isCode, EnumSeverity severity);

        List<Category> Categories { get; set; }
        void ClearView();
    }
}
