using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_Presentation
{
    public abstract class BaseNavigationHandler : INavigationWorkflow
    {
        public abstract void NavigateToNextPage(EnumView view, object newContext);
        public abstract void NavigateToPage(EnumView view, object newContext);
        public abstract void ReloadPage();
        public abstract void ReloadPage(string param);

        public static object context;
        public static EnumView currentView;

        /// <summary>
        /// Old context available by:
        /// NavigationController.context
        /// </summary>
        /// <param name="view">Current view</param>
        /// <param name="newContext">Context to be used to define the next view</param>
        /// <returns></returns>
        public EnumView GetNextView(EnumView view, object newContext)
        {
#warning TBD
            EnumView newView = EnumView.Error;

            if (view == EnumView.UserDetails)
                newView = EnumView.Home;

            if (view == EnumView.Registration)
                newView = EnumView.eBayRegistration;

            if (view == EnumView.Login)
                newView = (EnumView)newContext;

            NavigationController.context = newContext;

            return newView;
        }
    }

    /// <summary>
    /// List of available views
    /// </summary>
    public enum EnumView
    { 
        Home,
        UserDetails,
        Registration,
        Error,
        eBayRegistration,
        Login,
        AboutUs,
        Snipes,
        Mobility,
        ForgotPassword,
        Categories,
        Features,
        WhatsSnipe
    }
}
