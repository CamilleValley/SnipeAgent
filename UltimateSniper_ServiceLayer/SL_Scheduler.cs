using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using UltimateSniper_DAL;
using System.Timers;
using System.Threading;
using UltimateSniper_Services;
using UltimateSniper_Logger;

namespace UltimateSniper_ServiceLayer
{
    public class SL_Scheduler
    {

        #region Attributes

        private IList<Snipe> ListSnipesToSurvey = new List<Snipe>();
        public static UltimateSniper_Services.ServiceEBay eBayService = new UltimateSniper_Services.ServiceEBay(true);
        private static UltimateSniper_Services.ServiceOthers otherService = new UltimateSniper_Services.ServiceOthers();
        private static SqlDataContext myDataConnection = new SqlDataContext();

        public event UltimateSniper_BusinessObjects.Snipe.CheckTimeEventHandler CheckTimeEvent;

        public static bool CurrentEBAyTimeSet = false;
        public static double DiffLocalEbayTime;
        public static DateTime CurrentEBAyTimeLastUpdate;

        #endregion

        #region Event Functions

        /// <summary>
        /// This code should be executed just before the item ends
        /// </summary>
        /// <param name="snipe">Snipe to be executed</param>
        /// <returns></returns>
        public void PlaceSnipe(Snipe snipe)
        {
            if (snipe.SnipeInProgress)
                return;

            Logger.CreateLog("Beginning__PlaceSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            try
            {
                // We reload the snipe from the database... Maybe something changed!
                Snipe currsnipe = SL_Scheduler.otherService.GetSnipe((int)snipe.SnipeID);

                snipe.SnipeStatus = currsnipe.SnipeStatus;
                snipe.SnipeInProgress = currsnipe.SnipeInProgress;

                if (snipe.SnipeStatus != EnumSnipeStatus.ACTIVE || snipe.SnipeInProgress)
                {
                    Logger.CreateLog("ExitForConcurency__PlaceSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);
                    return;
                }

                snipe.SnipeInProgress = true;
                SL_Scheduler.myDataConnection.Save<Snipe>((object)snipe, snipe);

                User user = SL_Scheduler.otherService.GetUserForSnipe(snipe);

                if (user == null)
                {
                    snipe.SnipeStatus = EnumSnipeStatus.ERROR;
                    snipe.SnipeErrorReason = "The user assocated to the snipe couldn't be loaded properly.";
                    Logger.CreateLog("Error__PlaceSnipe", snipe.SnipeID.ToString(), new Exception("The user assocated to the snipe couldn't be loaded properly."), EnumLogLevel.WARN);
                }
                else
                {
                    SL_Scheduler.eBayService.User = user;

                    try
                    {
                        SL_Scheduler.eBayService.SetSnipe(snipe);

                        snipe.SnipeStatus = EnumSnipeStatus.EXECUTED;
                    }
                    catch (Exception ex)
                    {
                        snipe.SnipeStatus = EnumSnipeStatus.ERROR;
                        snipe.SnipeErrorReason = ex.Message;
                        Logger.CreateLog("Error__PlaceSnipe", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
                    }
                }

                snipe.SnipeExecutionDate = ServiceTimeZone.DateTimeToUniversal(DateTime.Now);
                snipe.SnipeInProgress = false;

                SL_Scheduler.myDataConnection.Save<Snipe>((object)snipe, snipe);
                Logger.CreateLog("SnipeSave__PlaceSnipe", snipe.SnipeID.ToString() + "_" + snipe.SnipeStatus.ToString(), null, EnumLogLevel.INFO);
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    Logger.CreateLog("Error__PlaceSnipe_" + error.MessageCode.ToString() + error.Severity.ToString(), snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__PlaceSnipe", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }

            snipe.SnipeInProgress = false;

            Logger.CreateLog("Ending__PlaceSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);
        }

        /// <summary>
        /// This code should be executed when the item ENDED.
        /// </summary>
        /// <param name="snipe">Snipe associated to the ENDED item</param>
        /// <returns></returns>
        private void CheckSnipeResult(Snipe snipe)
        {
            // We reload the snipe from the database... Maybe something changed!
            snipe = SL_Scheduler.otherService.GetSnipe((int)snipe.SnipeID);

            if (snipe.ResultInProgress)
                return;

            Logger.CreateLog("Beginning__CheckingSnipeResult", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            try
            {
                snipe.ResultInProgress = true;

                User user = SL_Scheduler.otherService.GetUserForSnipe(snipe);

                if (user == null)
                {
                    snipe.SnipeStatus = EnumSnipeStatus.ERROR;
                    snipe.SnipeErrorReason = "The user assocated to the snipe couldn't be loaded properly.";
                    Logger.CreateLog("Error__CheckingSnipeResult", snipe.SnipeID.ToString(), new Exception("The user assocated to the snipe couldn't be loaded properly."), EnumLogLevel.WARN);
                }
                else
                {
                    if (snipe.SnipeStatus == EnumSnipeStatus.EXECUTED || (snipe.SnipeStatus == EnumSnipeStatus.ACTIVE && snipe.SnipeStyle == EnumSnipeStyle.BidOptimizer))
                    {
                        SL_Scheduler.eBayService.User = user;

                        eBayItemData item = SL_Scheduler.eBayService.GetItemDetails(snipe.ItemID, true);
                        item.ItemCurrentHighestBidUserCurrency = this.ConvertBidCurrency(item.ItemCurrentHighestBid, item.ItemCurrencyCode, user.UserCurrency);
                        snipe.ItemLastKnownPrice = item.ItemCurrentHighestBidUserCurrency;

                        if (SL_Scheduler.eBayService.UserWonTheItem(snipe.ItemID))
                            snipe.SnipeStatus = EnumSnipeStatus.SUCCEED;
                        else
                            snipe.SnipeStatus = EnumSnipeStatus.FAILED;
                    }

                    if (snipe.SnipeStatus == EnumSnipeStatus.FAILED || snipe.SnipeStatus == EnumSnipeStatus.OVERBID)
                        this.CreateFollowingSnipe(snipe);

                    if (snipe.SnipeStatus == EnumSnipeStatus.ACTIVE)
                        snipe.SnipeStatus = EnumSnipeStatus.UNKNOW;

                    snipe.CancelSnipesInCaseOfSuccess = true;
                }
                SL_Scheduler.myDataConnection.Save<Snipe>((object)snipe, snipe);
                Logger.CreateLog("SnipeSave__PlaceSnipe", snipe.SnipeID.ToString() + "_" + snipe.SnipeStatus.ToString(), null, EnumLogLevel.INFO);

            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    Logger.CreateLog("Error__CheckingSnipeResult_" + error.MessageCode.ToString() + error.Severity.ToString(), snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__CheckingSnipeResult", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }

            Logger.CreateLog("Ending__CheckingSnipeResult", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            snipe.ResultInProgress = false;

            // Delete object
            ListSnipesToSurvey.Remove(snipe);

            foreach (Snipe currSnipe in ListSnipesToSurvey)
            {
                if (currSnipe.SnipeID == snipe.SnipeID)
                {
                    ListSnipesToSurvey.Remove(currSnipe);
                    break;
                }
            }

            // Cleaning
            UltimateSniper_BusinessObjects.Snipe.CheckTimeEventHandler ctHandler = new Snipe.CheckTimeEventHandler(snipe.CheckTime);
            this.CheckTimeEvent -= ctHandler;
            snipe = null;
            //System.GC.Collect();
        }

        private double ConvertBidCurrency(double bid, EnumCurrencyCode ori, EnumCurrencyCode des)
        {
            return SL_Scheduler.otherService.ConvertValue(bid, ori.ToString(), des.ToString());
        }

        private void CreateFollowingSnipe(Snipe snipe)
        {
            Logger.CreateLog("Beginning__CreateFollowingSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            // We create a now following snipe if required
            if (snipe.SnipeGenNextSnipe && snipe.SnipeGenRemainingNb > 0)
            {
                try
                {

                    List<eBayItemData> items = SL_Scheduler.eBayService.GetSimilarItems(snipe);

                    if (items != null && items.Count > 0 && items[0].ItemID != null)
                    {
                        eBayItemData item = items[0];

                        Snipe newSnipe = snipe;

                        newSnipe.ItemEndDate = item.ItemEndDate;
                        newSnipe.ItemID = (long)item.ItemID;
                        newSnipe.ItemSellerID = item.ItemSellerID;
                        newSnipe.SnipeBid += snipe.SnipeGenIncreaseBid;
                        snipe.SnipeBidInFinalCurrency = this.ConvertBidCurrency((double)snipe.SnipeBid, snipe.SnipeBidCurrency, snipe.SnipeFinalCurrency);
                        newSnipe.SnipeErrorReason = String.Empty;
                        newSnipe.SnipeExecutionDate = null;
                        newSnipe.SnipeGenRemainingNb = newSnipe.SnipeGenRemainingNb - 1;
                        newSnipe.SnipeID = null;
                        newSnipe.SnipeName += " auto";
                        newSnipe.SnipeStatus = EnumSnipeStatus.ACTIVE;
                        if (newSnipe.SnipeGenOriginalID == null) newSnipe.SnipeGenOriginalID = snipe.SnipeID;

                        SL_Scheduler.myDataConnection.Add<Snipe>((object)newSnipe, newSnipe);
                    }

                }
                catch (ControlObjectException ex)
                {
                    foreach (UserMessage error in ex.ErrorList)
                        Logger.CreateLog("Error__CreateFollowingSnipe_" + error.MessageCode.ToString() + error.Severity.ToString(), snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
                }
                catch (Exception ex)
                {
                    Logger.CreateLog("Error__CreateFollowingSnipe", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
                }
            }

            Logger.CreateLog("Ending__CreateFollowingSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);
        }

        /// <summary>
        /// This code should be executed regularely to make sure the SNIPE is still valid.
        /// </summary>
        /// <param name="snipe">Snipe to be checked </param>
        /// <returns></returns>
        private bool CheckSnipeValidity(Snipe snipe)
        {
            Logger.CreateLog("Beginning__CheckSnipeValidity", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            bool res = false;

            try
            {
                res = SL_Scheduler.otherService.CheckSnipeValidity(snipe, SL_Scheduler.eBayService);
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    Logger.CreateLog("Error__CheckSnipeValidity_" + error.MessageCode.ToString() + error.Severity.ToString(), snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__CheckSnipeValidity", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
            }

            Logger.CreateLog("Ending__CheckSnipeValidity", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            return res;
        }

        #endregion

        public string Timer_CheckTime()
        {
            try
            {
                UltimateSniper_BusinessObjects.Snipe.CheckTimeEventHandler handler = CheckTimeEvent;

                if (handler != null)
                {
                    Logger.CreateLog("Beginning__Timer_Snipe_CheckTime", handler.GetHashCode().ToString(), null, EnumLogLevel.INFO);

                    CheckTimeEventArgs e = new CheckTimeEventArgs(SL_Scheduler.GeteBayOfficialTime(), UserSettings.Default.SnipeExecutionDelay);
                    handler(this, e);
                }

                Logger.CreateLog("Ending__Timer_Snipe_CheckTime", EnumLogLevel.INFO);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__Timer_Snipe_CheckTime", string.Empty, ex, EnumLogLevel.INFO);
            }

            return DateTime.Now.ToString();
        }

        public string Timer_UpdateEndingSnipes(bool ActiveSnipe)
        {
            try
            {
                Logger.CreateLog("Beginning__UpdateEndingSnipes", EnumLogLevel.INFO);

                IList<Snipe> ListNewSnipes = new List<Snipe>();

                Query q = new Query();

                Criterion critDateSup = new Criterion();
                critDateSup.Operator = CriteriaOperator.LessThan;
                critDateSup.PropertyName = "ItemEndDate";
                DateTime nowPlusOne = SL_Scheduler.GeteBayOfficialTime().AddHours(1);
                critDateSup.Value = "#DateParse#" + nowPlusOne.ToString() + "#EndDateParse#";

                q.Criteria.Add(critDateSup);

                Criterion critStatus = new Criterion();
                critStatus.Operator = CriteriaOperator.Equal;
                critStatus.PropertyName = "SnipeStatusID";
                critStatus.Value = 1;

                q.Criteria.Add(critStatus);

                q.Members.Add("*");

                try
                {
                    Logger.CreateLog("Snipe_UpdateEndingSnipes", "Number in memory: " + this.ListSnipesToSurvey.Count.ToString(), null, EnumLogLevel.INFO);

                    ListNewSnipes = SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(q);

                    Logger.CreateLog("Snipe_UpdateEndingSnipes", "Number found: " + ListNewSnipes.Count.ToString(), null, EnumLogLevel.INFO);
                }
                catch (ControlObjectException ex)
                {
                    foreach (UserMessage error in ex.ErrorList)
                        Logger.CreateLog("Error__UpdateEndingSnipes_" + error.MessageCode.ToString() + error.Severity.ToString(), null, ex, EnumLogLevel.ERROR);
                }
                catch (Exception ex)
                {
                    Logger.CreateLog("Error__UpdateEndingSnipes", null, ex, EnumLogLevel.ERROR);
                }

                foreach (Snipe snipe in ListNewSnipes)
                {
                    bool found = false;
                    foreach (Snipe currSnipe in ListSnipesToSurvey)
                    {
                        if (currSnipe.SnipeID == snipe.SnipeID)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        snipe.CheckTimeActive = true;
                        snipe.ActiveSnipe = ActiveSnipe;

                        this.ListSnipesToSurvey.Add(snipe);
                        UltimateSniper_BusinessObjects.Snipe.CheckTimeEventHandler ctHandler = new Snipe.CheckTimeEventHandler(snipe.CheckTime);
                        this.CheckTimeEvent += ctHandler;

                        UltimateSniper_BusinessObjects.PlaceSnipeEventHandler pseHandler = new PlaceSnipeEventHandler(this.PlaceSnipe);
                        snipe.PlaceSnipeEvent += pseHandler;

                        UltimateSniper_BusinessObjects.CheckSnipeResultEventHandler csrHandler = new CheckSnipeResultEventHandler(this.CheckSnipeResult);
                        snipe.CheckSnipeResultEvent += csrHandler;
                    }
                }

                Logger.CreateLog("Ending__UpdateEndingSnipes", EnumLogLevel.INFO);

                // Cleaning
                ListNewSnipes.Clear();
                //System.GC.Collect();
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__UpdateEndingSnipes", string.Empty, ex, EnumLogLevel.INFO);
            }

            return DateTime.Now.ToString();
        }

        #region Schedule Functions For Bid Optimizer

        public string Timer_BidOptimizer_RefreshSnipes()
        {
            try
            {
                Logger.CreateLog("Beginning__Timer_BidOptimizer_RefreshSnipes", EnumLogLevel.INFO);

                IList<Snipe> endingSnipeList = new List<Snipe>();

                DateTime nowMin1 = SL_Scheduler.GeteBayOfficialTime().AddMinutes(1);

                #region Load snipes ending later than a minute

                // Load snipes ending later than a minute
                Query qAll = new Query();

                Criterion cAll = new Criterion();
                cAll.Operator = CriteriaOperator.GreaterThan;
                cAll.PropertyName = "ItemEndDate";
                cAll.Value = "#DateParse#" + nowMin1.ToString() + "#EndDateParse#";

                qAll.Criteria.Add(cAll);

                Criterion cAllNotEnded = new Criterion();
                cAllNotEnded.Operator = CriteriaOperator.GreaterThan;
                cAllNotEnded.PropertyName = "ItemEndDate";
                cAllNotEnded.Value = "#DateParse#" + SL_Scheduler.GeteBayOfficialTime().ToString() + "#EndDateParse#";

                qAll.Criteria.Add(cAllNotEnded);

                Criterion cAllNotPending = new Criterion();
                cAllNotPending.Operator = CriteriaOperator.Equal;
                cAllNotPending.PropertyName = "SnipeInProgress";
                cAllNotPending.Value = 0;

                qAll.Criteria.Add(cAllNotPending);

                Criterion cLastUp = new Criterion();
                cLastUp.Operator = CriteriaOperator.LessThan;
                cLastUp.PropertyName = "ItemLastUpdate";
                DateTime nowMin2H = SL_Scheduler.GeteBayOfficialTime().AddHours(-2);
                cLastUp.Value = "#DateParse#" + nowMin2H.ToString() + "#EndDateParse#";

                qAll.Criteria.Add(cLastUp);

                Criterion critStatusAll = new Criterion();
                critStatusAll.Operator = CriteriaOperator.Equal;
                critStatusAll.PropertyName = "SnipeStatusID";
                critStatusAll.Value = 1;

                qAll.Criteria.Add(critStatusAll);

                Criterion critStyleAll = new Criterion();
                critStyleAll.Operator = CriteriaOperator.Equal;
                critStyleAll.PropertyName = "SnipeStyleID";
                critStyleAll.Value = "2";

                qAll.Criteria.Add(critStyleAll);

                qAll.Members.Add("*");

                #endregion

                #region Load & process first the snipes ending in the next minute

                // Load & process first the snipes ending in the next minute
                Query q = new Query();

                Criterion c = new Criterion();
                c.Operator = CriteriaOperator.LessThan;
                c.PropertyName = "ItemEndDate";
                c.Value = "#DateParse#" + nowMin1.ToString() + "#EndDateParse#";

                q.Criteria.Add(c);

                Criterion critStatus = new Criterion();
                critStatus.Operator = CriteriaOperator.Equal;
                critStatus.PropertyName = "SnipeStatusID";
                critStatus.Value = 1;

                q.Criteria.Add(critStatus);

                Criterion cNotPending = new Criterion();
                cNotPending.Operator = CriteriaOperator.Equal;
                cNotPending.PropertyName = "SnipeInProgress";
                cNotPending.Value = 0;

                q.Criteria.Add(cNotPending);

                Criterion critStyle = new Criterion();
                critStyle.Operator = CriteriaOperator.Equal;
                critStyle.PropertyName = "SnipeStyleID";
                critStyle.Value = "2";

                q.Criteria.Add(critStyle);

                Criterion cNotEnded = new Criterion();
                cNotEnded.Operator = CriteriaOperator.GreaterThan;
                cNotEnded.PropertyName = "ItemEndDate";
                cNotEnded.Value = "#DateParse#" + SL_Scheduler.GeteBayOfficialTime().ToString() + "#EndDateParse#";

                q.Criteria.Add(cNotEnded);

                q.Members.Add("*");

                endingSnipeList = SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(q);

                foreach (Snipe snipe in endingSnipeList)
                {
                    Query qBuf = new Query();

                    qBuf.QueryName = "CheckCategoryYounger";

                    Criterion critere = new Criterion();
                    critere.Operator = CriteriaOperator.Equal;
                    critere.PropertyName = "SnipeID";
                    critere.Value = snipe.SnipeID;

                    qBuf.Criteria.Add(critere);

                    qBuf.Members.Add("*");

                    if (SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(qBuf).Count == 0)
                    {
                        MyThreadHandle threadHandle = new MyThreadHandle(snipe);
                        Thread myThread = new Thread(new ThreadStart(threadHandle.PlaceBids));

                        myThread.Start();
                    }
                }

                #endregion

                IList<Snipe> AllSnipeList = new List<Snipe>();

                AllSnipeList = SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(qAll);

                foreach (Snipe snipe in AllSnipeList)
                {
                    snipe.SnipeInProgress = true;
                    SL_Scheduler.myDataConnection.Save<Snipe>((object)snipe, snipe);
                }

                foreach (Snipe snipe in AllSnipeList)
                {
                    Query qBuf = new Query();

                    qBuf.QueryName = "CheckCategoryYounger";

                    Criterion critere = new Criterion();
                    critere.Operator = CriteriaOperator.Equal;
                    critere.PropertyName = "SnipeID";
                    critere.Value = snipe.SnipeID;

                    qBuf.Criteria.Add(critere);

                    qBuf.Members.Add("*");

                    if (SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(qBuf).Count == 0)
                    {
                        MyThreadHandle threadHandle = new MyThreadHandle(snipe);
                        threadHandle.PlaceBids();
                    }
                }
            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    Logger.CreateLog("Error__Timer_BidOptimizer_RefreshSnipes_" + error.MessageCode.ToString() + error.Severity.ToString(), null, ex, EnumLogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__Timer_BidOptimizer_RefreshSnipes", null, ex, EnumLogLevel.ERROR);
            }

            return DateTime.Now.ToString();
        }

        #endregion

        #region Schedule Functions For Snipe

        public string Timer_Snipe_CheckSnipeValidity()
        {
            try
            {
                Logger.CreateLog("Beginning__Snipe_CheckSnipeValidity", EnumLogLevel.INFO);

                IList<Snipe> snipeList = new List<Snipe>();
                IList<Snipe> tempList = new List<Snipe>();

                // Loading snipes not updated since one day:
                Query q = new Query();

                Criterion c = new Criterion();
                c.Operator = CriteriaOperator.LessThan;
                c.PropertyName = "ItemLastUpdate";
                DateTime nowMin24H = SL_Scheduler.GeteBayOfficialTime().AddHours(-24);
                c.Value = "#DateParse#" + nowMin24H.ToString() + "#EndDateParse#";

                q.Criteria.Add(c);

                Criterion critStatus = new Criterion();
                critStatus.Operator = CriteriaOperator.Equal;
                critStatus.PropertyName = "SnipeStatusID";
                critStatus.Value = 1;

                q.Criteria.Add(critStatus);

                Criterion critInProgres = new Criterion();
                critInProgres.Operator = CriteriaOperator.Equal;
                critInProgres.PropertyName = "ValidityCheckInProgress";
                critInProgres.Value = "0";

                q.Criteria.Add(critInProgres);

                Criterion critStyle = new Criterion();
                critStyle.Operator = CriteriaOperator.NotEqual;
                critStyle.PropertyName = "SnipeStyleID";
                critStyle.Value = "2";

                q.Criteria.Add(critStyle);

                q.Members.Add("*");

                try
                {
                    tempList = SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(q);
                }
                catch (ControlObjectException ex)
                {
                    foreach (UserMessage error in ex.ErrorList)
                        Logger.CreateLog("Error__Snipe_CheckSnipeValidity_" + error.MessageCode.ToString() + error.Severity.ToString(), null, ex, EnumLogLevel.ERROR);
                }
                catch (Exception ex)
                {
                    Logger.CreateLog("Error__Snipe_CheckSnipeValidity", null, ex, EnumLogLevel.ERROR);
                }

                foreach (Snipe snipe in tempList)
                    snipeList.Add(snipe);

                // Loading snipes ending in the next day - IF THE USER HAS THE OPTION
#warning Check that the query is working!
                q = new Query();

                c = new Criterion();
                c.PropertyName = "ItemEndDate";
                DateTime nowPlus24H = SL_Scheduler.GeteBayOfficialTime().AddHours(-24);
                c.Value = "#DateParse#" + nowPlus24H.ToString() + "#EndDateParse#";

                q.Criteria.Add(c);

                q.Members.Add("*");
                q.QueryName = "ForSuscribedUser";

                try
                {
                    tempList = new List<Snipe>();
                    tempList = SL_Scheduler.myDataConnection.GetByCriteria<Snipe>(q);
                }
                catch (ControlObjectException ex)
                {
                    foreach (UserMessage error in ex.ErrorList)
                        Logger.CreateLog("Error__Snipe_CheckSnipeValidity_" + error.MessageCode.ToString() + error.Severity.ToString(), null, ex, EnumLogLevel.ERROR);
                }
                catch (Exception ex)
                {
                    Logger.CreateLog("Error__Snipe_CheckSnipeValidity", null, ex, EnumLogLevel.ERROR);
                }

                foreach (Snipe snipe in tempList)
                    snipeList.Add(snipe);

#warning the locking of the snipes for validity check isnt very clean

                tempList = new List<Snipe>();

                Snipe tempS;

                // Set ValidityCheckInProgress at True
                foreach (Snipe snipe in snipeList)
                {
                    // We reload the snipe from the database... Maybe something changed!
                    tempS = SL_Scheduler.otherService.GetSnipe((int)snipe.SnipeID);

                    if (!tempS.SnipeInProgress && !tempS.ResultInProgress && !tempS.ValidityCheckInProgress)
                    {
                        tempS.ValidityCheckInProgress = true;
                        SL_Scheduler.myDataConnection.Save<Snipe>((object)tempS, tempS);

                        tempList.Add(tempS);
                    }
                }

                tempS = null;

                // Updating snipes
                foreach (Snipe snipe in tempList)
                    this.CheckSnipeValidity(snipe);

                // Set ValidityCheckInProgress at False
                foreach (Snipe snipe in tempList)
                {
                    snipe.ValidityCheckInProgress = false;
                    SL_Scheduler.myDataConnection.Save<Snipe>((object)snipe, snipe);
                }

                Logger.CreateLog("Ending__Snipe_CheckSnipeValidity", EnumLogLevel.INFO);

                // Cleaning
                tempList.Clear();
                snipeList.Clear();
                //System.GC.Collect();
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__Snipe_CheckSnipeValidity", string.Empty, ex, EnumLogLevel.INFO);
            }

            return DateTime.Now.ToString();
        }

        #endregion

        #region Execution Functions

        private static DateTime GeteBayOfficialTime()
        {
            if (SL_Scheduler.CurrentEBAyTimeSet && DateTime.Now < SL_Scheduler.CurrentEBAyTimeLastUpdate.AddMinutes(2))
                return DateTime.Now.AddSeconds(SL_Scheduler.DiffLocalEbayTime * -1);
            else
            {
                if (SL_Scheduler.eBayService.User == null)
                {
                    User user = new User();
                    user.EBayUserToken = otherService.DefaultToken();
                    user.EBayUserTokenExpirationDate = otherService.DefaultTokenExpirationDate();
                    SL_Scheduler.eBayService.User = user;
                }

                DateTime CurrentEBAyTime = ServiceTimeZone.DateTimeToUniversal(SL_Scheduler.eBayService.GeteBayOfficialTime());
                SL_Scheduler.CurrentEBAyTimeLastUpdate = DateTime.Now;
                SL_Scheduler.CurrentEBAyTimeSet = true;
                TimeSpan span = SL_Scheduler.CurrentEBAyTimeLastUpdate.Subtract(CurrentEBAyTime);
                SL_Scheduler.DiffLocalEbayTime = Math.Round(span.TotalSeconds,0);

                return CurrentEBAyTime;
            }
        }

        #endregion
    }

    public class MyThreadHandle
    {
        Snipe _Snipe;

        public MyThreadHandle(Snipe snipe)
        {
            this._Snipe = snipe;
        }

        public void SetParam(Snipe snipe)
        {
            this._Snipe = snipe;
        }

        /// <summary>
        /// This code should be executed just before the item ends
        /// </summary>
        /// <param name="snipe">Snipe to be executed</param>
        /// <returns></returns>
        public void PlaceBids()
        {
            try
            {

                SqlDataContext myDataConnection = new SqlDataContext();

                UltimateSniper_Services.ServiceOthers otherServ = new UltimateSniper_Services.ServiceOthers();

                User user = otherServ.GetUserForSnipe(_Snipe);

                if (user == null)
                {
                    _Snipe.SnipeErrorReason = "The user assocated to the snipe couldn't be loaded properly.";
                }
                else
                {
                    if (!_Snipe.SnipeInProgress)
                    {
                        _Snipe.SnipeInProgress = true;

                        myDataConnection.Save<Snipe>((object)_Snipe, _Snipe);
                    }

                    try
                    {
                        SL_Scheduler.eBayService.User = user;

                        while (true)
                        {
                            if (SL_Scheduler.eBayService.IsUserWinning(this._Snipe)) break;
                            SL_Scheduler.eBayService.SetSnipe(this._Snipe);
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    catch (ControlObjectException ex)
                    {
                        foreach (UserMessage message in ex.ErrorList)
                        {
                            if (message.MessageCode == EnumMessageCode.SnipeBidOptimizerMaxBidReached)
                            {
                                UltimateSniper_Services.ServiceEmail emailService = new UltimateSniper_Services.ServiceEmail();

                                _Snipe.SnipeStatus = EnumSnipeStatus.OVERBID;
                                _Snipe.SnipeErrorReason = "The snipe bid is lower the current price.";

                                ServiceEmail.SendEmail(user, "[SnipeAgent] Maximum bid reached for: " + _Snipe.SnipeName, "Hello, the maximum bid for your item has been reached. You can still modify it and win the item. To do so, go on www.snipeagent.com and into the section 'My Snipes'. Kind regards, Snipe Agent.");
                            }
                        }
                        _Snipe.SnipeErrorReason = ex.Message;
                    }
                    catch (Exception ex)
                    {
                        _Snipe.SnipeErrorReason = ex.Message;
                    }
                }

                _Snipe.ItemLastUdpate = ServiceTimeZone.DateTimeToUniversal(DateTime.Now);
                _Snipe.SnipeInProgress = false;

                myDataConnection.Save<Snipe>((object)_Snipe, _Snipe);

            }
            catch (ControlObjectException ex)
            {
                foreach (UserMessage error in ex.ErrorList)
                    Logger.CreateLog("Error__Timer_BidOptimizer_RefreshSnipes_" + error.MessageCode.ToString() + error.Severity.ToString(), null, ex, EnumLogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.CreateLog("Error__Timer_BidOptimizer_RefreshSnipes", null, ex, EnumLogLevel.ERROR);
            }
        }
    }
}
