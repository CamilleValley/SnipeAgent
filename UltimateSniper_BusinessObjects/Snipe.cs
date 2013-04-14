using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_Logger;

namespace UltimateSniper_BusinessObjects
{
    public class Snipe : IBusinessObject
    {
        #region Attributes

        private int? _SnipeID;
        private string _SnipeName;
        private double? _SnipeBid;
        private double _SnipeBidInFinalCurrency;
        private EnumSnipeStatus _SnipeStatus;
        private string _SnipeDescription;
        private int _SnipeDelay;
        private DateTime? _SnipeExecutionDate;
        private DateTime _ItemEndDate;
        private string _SnipeErrorReason;
        private string _ItemTitle;
        private DateTime? _SnipeCancellationDate;
        private EnumSnipeType _SnipeType;
        private long _ItemID;
        private int _UserID;
        private DateTime? _ItemLastUdpate;
        private EnumCurrencyCode _SnipeBidCurrency;
        private EnumCurrencyCode _SnipeFinalCurrency;
        private List<Category> _SnipeCategories = new List<Category>();
        private string _ItemSellerID;
        private string _ItemURL;
        private string _ItemPictureURL;
        private double _ItemLastKnownPrice;
        private EnumSnipeStyle _SnipeStyle = EnumSnipeStyle.Manual;

        private bool _SnipeGenNextSnipe;
        private int? _SnipeGenOriginalID;
        private int? _SnipeGenRemainingNb;
        private int _SnipeGenIncreaseBid;

        private bool _SnipeInProgress = false;
        private bool _ResultInProgress = false;
        private bool _ValidityCheckInProgress = false;

        private bool _ForInsert = false;
        private bool _CancelSnipesInCaseOfSuccess = false;

        public bool CheckTimeActive = false;
        public bool ActiveSnipe = false;

        public delegate void CheckTimeEventHandler(object sender, CheckTimeEventArgs e);
        public event PlaceSnipeEventHandler PlaceSnipeEvent;
        public event CheckSnipeResultEventHandler CheckSnipeResultEvent;

        #endregion

        #region Accessors

        public EnumSnipeStyle SnipeStyle
        {
            get { return _SnipeStyle; }
            set { _SnipeStyle = value; }
        }

        public bool ValidityCheckInProgress
        {
            get { return _ValidityCheckInProgress; }
            set { _ValidityCheckInProgress = value; }
        }

        public double ItemLastKnownPrice
        {
            get { return _ItemLastKnownPrice; }
            set { _ItemLastKnownPrice = value; }
        }

        public bool ResultInProgress
        {
            get { return _ResultInProgress; }
            set { _ResultInProgress = value; }
        }

        public string ItemURL
        {
            get { return _ItemURL; }
            set { _ItemURL = value; }
        }

        public string ItemTitle
        {
            get { return _ItemTitle; }
            set { _ItemTitle = value; }
        }

        public string ItemPictureURL
        {
            get { return _ItemPictureURL; }
            set { _ItemPictureURL = value; }
        }

        public bool CancelSnipesInCaseOfSuccess
        {
            get { return _CancelSnipesInCaseOfSuccess; }
            set { _CancelSnipesInCaseOfSuccess = value; }
        }

        public bool ForInsert
        {
            get { return _ForInsert; }
            set { _ForInsert = value; }
        }

        public int SnipeGenIncreaseBid
        {
            get { return _SnipeGenIncreaseBid; }
            set { _SnipeGenIncreaseBid = value; }
        }

        public bool SnipeGenNextSnipe
        {
            get { return _SnipeGenNextSnipe; }
            set { _SnipeGenNextSnipe = value; }
        }

        public int? SnipeGenRemainingNb
        {
            get { return _SnipeGenRemainingNb; }
            set { _SnipeGenRemainingNb = value; }
        }

        public int? SnipeGenOriginalID
        {
            get { return _SnipeGenOriginalID; }
            set { _SnipeGenOriginalID = value; }
        }

        public EnumCurrencyCode SnipeFinalCurrency
        {
            get { return _SnipeFinalCurrency; }
            set { _SnipeFinalCurrency = value; }
        }

        public string ItemSellerID
        {
            get { return _ItemSellerID; }
            set { _ItemSellerID = value; }
        }

        public double SnipeBidInFinalCurrency
        {
            get { return _SnipeBidInFinalCurrency; }
            set { _SnipeBidInFinalCurrency = value; }
        }

        public List<Category> SnipeCategories
        {
            get { return _SnipeCategories; }
            set { _SnipeCategories = value; }
        }

        public bool SnipeInProgress
        {
            get { return _SnipeInProgress; }
            set { _SnipeInProgress = value; }
        }

        public EnumCurrencyCode SnipeBidCurrency
        {
            get { return _SnipeBidCurrency; }
            set { _SnipeBidCurrency = value; }
        }

        public DateTime? ItemLastUdpate
        {
            get { return _ItemLastUdpate; }
            set { _ItemLastUdpate = value; }
        }

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public long ItemID
        {
            get { return _ItemID; }
            set { _ItemID = value; }
        }

        public EnumSnipeType SnipeType
        {
            get { return _SnipeType; }
            set { _SnipeType = value; }
        }

        public DateTime? SnipeCancellationDate
        {
            get { return _SnipeCancellationDate; }
            set { _SnipeCancellationDate = value; }
        }

        public string SnipeErrorReason
        {
            get { return _SnipeErrorReason; }
            set { _SnipeErrorReason = value; }
        }

        public int? SnipeID
        {
            get { return _SnipeID; }
            set { _SnipeID = value; }
        }

        public string SnipeName
        {
            get { return _SnipeName; }
            set { _SnipeName = value; }
        }

        public double? SnipeBid
        {
            get { return _SnipeBid; }
            set { _SnipeBid = value; }
        }

        public EnumSnipeStatus SnipeStatus
        {
            get { return _SnipeStatus; }
            set { _SnipeStatus = value; }
        }

        public string SnipeDescription
        {
            get { return _SnipeDescription; }
            set { _SnipeDescription = value; }
        }

        public int SnipeDelay
        {
            get { return _SnipeDelay; }
            set { _SnipeDelay = value; }
        }

        public DateTime? SnipeExecutionDate
        {
            get { return _SnipeExecutionDate; }
            set { _SnipeExecutionDate = value; }
        }

        public DateTime ItemEndDate
        {
            get { return _ItemEndDate; }
            set { _ItemEndDate = value; }
        }

        #endregion

        #region Functions

        public void CheckTime(object sender, CheckTimeEventArgs e)
        {
            Logger.CreateLog("CheckTime_Snipe" + this._SnipeID.ToString(), "Item end: " + this.ItemEndDate.ToString() + " eBay Time: " + e.EBayTime.ToString(), null, EnumLogLevel.INFO);
            
            if (this.CheckTimeActive)
            {

                if (this.SnipeStyle == EnumSnipeStyle.Snipe && ActiveSnipe)
                {
                    if (this.ItemEndDate.AddSeconds((this.SnipeDelay + e.SnipeExecutionDelay) * -1) <= e.EBayTime && this.SnipeStatus == EnumSnipeStatus.ACTIVE && !this.SnipeInProgress)
                    {
                        PlaceSnipeEventHandler handler = PlaceSnipeEvent;

                        if (handler != null) handler(this);
                    }
                }

#warning the delay between the end of the auction is currently hard coded to 5 seconds.
                if (this.ItemEndDate.AddSeconds(5) < e.EBayTime && (this.SnipeStatus == EnumSnipeStatus.EXECUTED || this.SnipeStatus == EnumSnipeStatus.ACTIVE))
                {
                    CheckSnipeResultEventHandler handler = CheckSnipeResultEvent;

                    if (handler != null) handler(this);
                }

                //System.GC.Collect();
            }
        }

        public void ControlObject()
        {
            List<UserMessage> errorList = new List<UserMessage>();

            if (this._ItemEndDate == null) errorList.Add(new UserMessage(EnumSeverity.Bug, EnumMessageCode.SnipeItemEndDateEmpty));
            if (this._SnipeBid == null) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeBidNotANumber));
            if (this._SnipeBid <= 0) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeBidInvalid));
            if (this._SnipeDelay <= 0) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeDelayInvalid));
            if (string.IsNullOrEmpty(this._SnipeName)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeNameEmpty));
            if (this._SnipeGenNextSnipe && this._SnipeGenRemainingNb == null)
                errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeNbAutoRetryNotSpecified));

            if (errorList.Count != 0)
            {
                ControlObjectException ex = new ControlObjectException(errorList);
                throw ex;
            }
        }

        #endregion
    }

    public struct eBayItemData
    {
        public long? ItemID;
        public DateTime ItemEndDate;
        public double ItemCurrentHighestBid;
        public double ItemCurrentHighestBidUserCurrency;
        public EnumCurrencyCode ItemCurrencyCode;
        public string ItemURL;
        public string ItemTitle;
        public string ItemDescription;
        public string ItemSellerID;
        public string ItemShippingInfo;
        public List<string> ItemPaymentOptions;
        public string ItemPictureURL;
    }
}
