using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_Logger;
using System.Timers;

namespace UltimateSniper_SchedulerEngine
{
    public class Scheduler_Engine
    {
        #region Attributes

        //private UltimateSniper_SchedulerEngine.SnipeAgentScheduler.WService_Scheduler scheduler;
        private UltimateSniper_ServiceLayer.SL_Scheduler scheduler;

        public bool SnipeEnabled = true;

        private System.Timers.Timer T_CheckSnipeValidity = new System.Timers.Timer();
        private System.Timers.Timer T_Snipe_UpdateEndingSnipes = new System.Timers.Timer();
        private System.Timers.Timer T_Snipe_CheckTime = new System.Timers.Timer();

        public bool BidOptimizerEnabled = true;
        private System.Timers.Timer T_BidOptimizer_RefreshSnipes = new System.Timers.Timer();

        #endregion

        #region Schduler functions

        public Scheduler_Engine()
        {
            Logger.CreateLog("Beginning__SLScheduler", EnumLogLevel.INFO);

            T_CheckSnipeValidity.Interval = SchedulerSettings.Default.Frequency_Snipe_CheckSnipeValidity * 1000;
            T_CheckSnipeValidity.Elapsed += new ElapsedEventHandler(Timer_CheckSnipeValidity);

            T_Snipe_UpdateEndingSnipes.Interval = SchedulerSettings.Default.Frequency_Snipe_LoadEndingSnipes * 1000;
            T_Snipe_UpdateEndingSnipes.Elapsed += new ElapsedEventHandler(Timer_Snipe_UpdateEndingSnipes);

            T_Snipe_CheckTime.Interval = SchedulerSettings.Default.Frequency_Snipe_CheckTime * 1000;
            T_Snipe_CheckTime.Elapsed += new ElapsedEventHandler(Timer_Snipe_CheckTime);

            T_BidOptimizer_RefreshSnipes.Interval = SchedulerSettings.Default.Frequency_BidOptimizer_RefreshSnipes * 1000;
            T_BidOptimizer_RefreshSnipes.Elapsed += new ElapsedEventHandler(Timer_BidOptimizer_RefreshSnipes);

            scheduler = new UltimateSniper_ServiceLayer.SL_Scheduler();
            //scheduler = new UltimateSniper_SchedulerEngine.SnipeAgentScheduler.WService_Scheduler();
            //scheduler.CookieContainer = new System.Net.CookieContainer(); 
        }

        public void StopScheduler()
        {
            T_CheckSnipeValidity.Stop();
            T_Snipe_UpdateEndingSnipes.Stop();
            T_Snipe_CheckTime.Stop();
        }

        /// <summary>
        /// Run this function to initialise the schedulers
        /// </summary>
        public void StartScheduler()
        {
            if (this.BidOptimizerEnabled)
                T_BidOptimizer_RefreshSnipes.Start();

            T_Snipe_UpdateEndingSnipes.Start();
            T_Snipe_CheckTime.Start();
            T_CheckSnipeValidity.Start();
        }

        #endregion

        #region functions

        private void Timer_BidOptimizer_RefreshSnipes (object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.scheduler.Timer_BidOptimizer_RefreshSnipes();
            }
            catch (Exception ex)
            {
                Logger.CreateLog(ex.Message, EnumLogLevel.FATAL);
            }
        }

        private void Timer_CheckSnipeValidity(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.scheduler.Timer_Snipe_CheckSnipeValidity();
            }
            catch (Exception ex)
            {
                Logger.CreateLog(ex.Message, EnumLogLevel.FATAL);
            }
        }

        private void Timer_Snipe_UpdateEndingSnipes(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string session = this.scheduler.Timer_UpdateEndingSnipes(SnipeEnabled);
            }
            catch (Exception ex)
            {
                Logger.CreateLog(ex.Message, EnumLogLevel.FATAL);
            }
        }

        private void Timer_Snipe_CheckTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.scheduler.Timer_CheckTime();
            }
            catch (Exception ex)
            {
                Logger.CreateLog(ex.Message, EnumLogLevel.FATAL);
            }
        }

        #endregion
    }
}
