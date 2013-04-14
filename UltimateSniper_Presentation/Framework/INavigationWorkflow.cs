using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_Presentation
{
    public interface INavigationWorkflow
    {
        EnumView GetNextView(EnumView view, object newContext);
        void NavigateToPage(EnumView view, object newContext);
        void NavigateToNextPage(EnumView view, object newContext);
        void ReloadPage();
        void ReloadPage(string param);
    }
}
