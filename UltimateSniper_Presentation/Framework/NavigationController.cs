using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_Presentation
{
    public static class NavigationController
    {
        private static INavigationWorkflow instance;
        public static object context;
        public static EnumView currentView;

        public static void Register(INavigationWorkflow service)
        {
            if (service == null)
                throw new ArgumentException();
            instance = service;
        }

        public static EnumView GetNextView(EnumView view, object context)
        {
            if (instance == null)
                throw new InvalidOperationException();

            EnumView nextView = instance.GetNextView(view, context);
            NavigationController.context = context;

            return nextView;
        }
    }
}
