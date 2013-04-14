using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using UltimateSniper_Services.eBayPublic;
using UltimateSniper_Services.eBayFindingService;
using System.Net;
using UltimateSniper_Logger;
using System.Xml;

namespace UltimateSniper_Services
{

    public class ServiceEBay
    {
        private string _serverUrl;
        private string _shoppingServerUrl;
        private string _version;
        private User _user;
        private string _endUserIP;
        private string _signInURL;
        private string _RUName;
        private eBayAPIInterfaceService service = new eBayAPIInterfaceService();
        private eBayKeyHandler _eBayKeyHandler;
        private bool _backEndObject;

        private string _currentSessionID = string.Empty;

        public ServiceEBay(User user, bool backEndObject)
        {
            InitVariables();

            _user = user;
            _backEndObject = backEndObject;
        }

        public ServiceEBay(bool backEndObject)
        {
            InitVariables();

            _backEndObject = backEndObject;
        }

        private void InitVariables()
        {
            _serverUrl = ServiceParametersHelper.eBayUrl();
            _version = ServiceParametersHelper.eBayVersion();
            _endUserIP = ServiceParametersHelper.EndUserIP();
            _signInURL = ServiceParametersHelper.SignInURL();
            _RUName = ServiceParametersHelper.RUName();
            _shoppingServerUrl = ServiceParametersHelper.shoppingServerUrl();

            _eBayKeyHandler = new eBayKeyHandler();
        }

        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        private void GetSessionID()
        {
            string callname = "GetSessionID";

            this.BuildService(callname);

            GetSessionIDRequestType request = new GetSessionIDRequestType();

            request.Version = _version;

            request.RuName = this._RUName;

            GetSessionIDResponseType response = this.service.GetSessionID(request);

            if (response.Ack != AckCodeType.Success)
            {
                string error = "";

                foreach (ErrorType err in response.Errors) error += err.LongMessage;

                throw new Exception(error);
            }

            this._currentSessionID = response.SessionID;
        }

        public string BuildTokenURL()
        {
            this.GetSessionID();
            return (string.Format(this._signInURL, this._RUName, this._currentSessionID));
        }

        public TokenData FetchToken(TokenFetcher fetcher)
        {
            this._currentSessionID = fetcher.SessionID;
            return this.FetchToken();
        }

        public TokenData FetchToken()
        {
            string callname = "FetchToken";

            this.BuildService(callname);

            FetchTokenRequestType request = new FetchTokenRequestType();
            request.Version = _version;

            request.SessionID = this._currentSessionID;

            FetchTokenResponseType response = this.service.FetchToken(request);

            if (response.Ack != AckCodeType.Success)
            { 
                string error = "";

                foreach (ErrorType err in response.Errors) error+= err.LongMessage;

                throw new Exception(error);
            }

            TokenData token = new TokenData();

            token.ExpirationDate = ServiceTimeZone.eBayDateTimeToUniversal(response.HardExpirationTime);
            token.Token = response.eBayAuthToken;

            return (token);
        }

        public DateTime GeteBayOfficialTime()
        {
            return this.GeteBayOfficialTime(0);
        }

        public DateTime GeteBayOfficialTime(int nbRetry)
        { 
            string callname = "GeteBayOfficialTime";

            try
            {

                this.BuildService(callname);

                GeteBayOfficialTimeRequestType request = new GeteBayOfficialTimeRequestType();
                request.Version = _version;

                GeteBayOfficialTimeResponseType response = this.service.GeteBayOfficialTime(request);

                if (response.Ack == AckCodeType.Success)
                    return ServiceTimeZone.eBayDateTimeToUniversal(response.Timestamp);
                else
                {
                    string error = "";

                    foreach (ErrorType err in response.Errors) error += err.LongMessage;

                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.GeteBayOfficialTime(nbRetry + 1);
                else
                {
                    Logger.CreateLog("Error__GeteBayOfficialTime", string.Empty, ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            throw new Exception("Unknow error when getting the eBay time.");
        }

        public bool IsUserWinning(Snipe snipe)
        {
            return this.IsUserWinning(snipe, 0);
        }

        private bool IsUserWinning(Snipe snipe, int nbRetry)
        {
            Logger.CreateLog("Beginning__IsUserWinning", snipe.ItemID.ToString(), null, EnumLogLevel.INFO);

            string callname = "GetMyeBayBuying";
            int nbPage = 0;

            try
            {
                this.BuildService(callname);

                GetMyeBayBuyingRequestType request = new GetMyeBayBuyingRequestType();
                request.Version = _version;
                request.BidList = new ItemListCustomizationType();
                request.BidList.Include = true;
                request.BidList.IncludeSpecified = true;
                request.BidList.ListingType = ListingTypeCodeType.Chinese;
                request.BidList.ListingTypeSpecified = true;

                request.BidList.Pagination = new PaginationType();
                request.BidList.Pagination.EntriesPerPage = 25;
                request.BidList.Pagination.EntriesPerPageSpecified = true;
                request.BidList.Pagination.PageNumber = 1;
                request.BidList.Pagination.PageNumberSpecified = true;

                DetailLevelCodeType level = DetailLevelCodeType.ReturnAll;
                List<DetailLevelCodeType> l = new List<DetailLevelCodeType>();
                l.Add(level);

                request.DetailLevel = l.ToArray();

                List<string> outputs = new List<string>();
                outputs.Add("BidList.ItemArray");
                outputs.Add("BidList.PaginationResult");

                request.OutputSelector = outputs.ToArray();

                GetMyeBayBuyingResponseType response = this.service.GetMyeBayBuying(request);

                if (response.Ack == AckCodeType.Success)
                {
                    if (response.BidList != null)
                        nbPage = response.BidList.PaginationResult.TotalNumberOfPages;
                    // There must be at least 1 page
                    if (nbPage > 0)
                    {
                        // We loop throw the pages
                        for (int i = 1; i <= response.BidList.PaginationResult.TotalNumberOfPages; i++)
                        {
                            // Dont reload if we are on the first page
                            if (i != 1)
                            {
                                request.BidList.Pagination = new PaginationType();
                                request.BidList.Pagination.EntriesPerPage = 25;
                                request.BidList.Pagination.EntriesPerPageSpecified = true;
                                request.BidList.Pagination.PageNumber = i;
                                request.WonList.Pagination.PageNumberSpecified = true;

                                response = service.GetMyeBayBuying(request);
                            }

                            foreach (ItemType items in response.BidList.ItemArray)
                            {
                                if (items.BiddingDetails != null && items.ItemID == snipe.ItemID.ToString())
                                {
                                    UltimateSniper_Services.ServiceOthers otherService = new UltimateSniper_Services.ServiceOthers();
                                    snipe.ItemLastKnownPrice = otherService.ConvertValue(items.BiddingDetails.MaxBid.Value, items.BiddingDetails.MaxBid.currencyID.ToString(), User.UserCurrency.ToString());

                                    if (items.BiddingDetails.QuantityWon == 1)
                                        return true;
                                    else
                                        return false;
                                }
                            }
                        }
                    }
                }
                else
                {
                    string error = "";

                    foreach (ErrorType err in response.Errors) error += err.LongMessage + Environment.NewLine;

                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.IsUserWinning(snipe, nbRetry + 1);
                else
                {
                    Logger.CreateLog("Error__IsUserWinning", snipe.ItemID.ToString(), ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            Logger.CreateLog("Ending__IsUserWinning", snipe.ItemID.ToString(), null, EnumLogLevel.INFO);

            return false;
        }

        public void SetSnipe(Snipe snipe)
        {
            this.SetSnipe(snipe, 0);
        }

        private void SetSnipe(Snipe snipe, int nbRetry)
        {
            Logger.CreateLog("Beginning__SetSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            string callname = "PlaceOffer";

            try
            {

                this.BuildService(callname);

                PlaceOfferRequestType request = new PlaceOfferRequestType();
                request.Version = _version;

                request.ItemID = snipe.ItemID.ToString();

                request.Offer = new OfferType();
                request.Offer.Action = BidActionCodeType.Bid;
                request.Offer.ActionSpecified = true;

                request.Offer.MaxBid = new AmountType();

                if (snipe.SnipeStyle == EnumSnipeStyle.Snipe || snipe.SnipeStyle == EnumSnipeStyle.Manual)
                {
                    request.Offer.MaxBid.currencyID = (UltimateSniper_Services.eBayPublic.CurrencyCodeType)snipe.SnipeBidCurrency;
                    request.Offer.MaxBid.Value = snipe.SnipeBidInFinalCurrency;
                }
                if (snipe.SnipeStyle == EnumSnipeStyle.BidOptimizer)
                {
                    // Creating an object to the BestMatchService class
                    UltimateSniper_Services.ebayShopping.Shopping shoppingService = new UltimateSniper_Services.ebayShopping.Shopping();

                    int siteID = 1;
                    if (this._user != null) siteID = (int)this._user.EBayRegistrationSiteID;

                    string requestURL = _shoppingServerUrl + "?callname=GetSingleItem&siteid=" + siteID.ToString()
                    + "&appid=" + _eBayKeyHandler.GetEBayKeySet().AppID + "&version=" + _version + "&routing=default";
                    shoppingService.Url = requestURL;

                    UltimateSniper_Services.ebayShopping.GetSingleItemRequestType itemRequest = new UltimateSniper_Services.ebayShopping.GetSingleItemRequestType();

                    itemRequest.ItemID = snipe.ItemID.ToString();
                    itemRequest.IncludeSelector = "Details";

                    UltimateSniper_Services.ebayShopping.GetSingleItemResponseType shoppingResponse = shoppingService.GetSingleItem(itemRequest);
   
                    if (shoppingResponse.Ack == UltimateSniper_Services.ebayShopping.AckCodeType.Success)
                    {
                        if (shoppingResponse.Item.MinimumToBid != null && shoppingResponse.Item.ListingStatus == UltimateSniper_Services.ebayShopping.ListingStatusCodeType.Active)
                        {
                            if (shoppingResponse.Item.MinimumToBid.Value > snipe.SnipeBidInFinalCurrency)
                            {
                                List<UserMessage> errorList = new List<UserMessage>();

                                errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.SnipeBidOptimizerMaxBidReached));

                                ControlObjectException ex = new ControlObjectException(errorList);
                                throw ex;
                            }

                            request.Offer.MaxBid.currencyID = (UltimateSniper_Services.eBayPublic.CurrencyCodeType)shoppingResponse.Item.MinimumToBid.currencyID;
                            request.Offer.MaxBid.Value = shoppingResponse.Item.MinimumToBid.Value;
                        }
                        else
                            throw new Exception("This item isn't valid anymore.");
                    }
                    else
                    {
                        string error = "";

                        foreach (UltimateSniper_Services.ebayShopping.ErrorType err in shoppingResponse.Errors) error += err.LongMessage + Environment.NewLine;

                        throw new Exception(error);
                    }
                }

                request.Offer.Quantity = 1;
                request.Offer.QuantitySpecified = true;
                request.EndUserIP = User.UserIPAddress;

                PlaceOfferResponseType response = this.service.PlaceOffer(request);

                if (response.BotBlock != null)
                {
                    this._eBayKeyHandler.GetNextEBayKeySet();

                    throw new Exception("BotBlock detected.");
                }

                if (response.Ack != AckCodeType.Success)
                {
#warning implement retry if type = bidoptimizer

                    string error = "";

                    foreach (ErrorType err in response.Errors) error += err.LongMessage + Environment.NewLine;

                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {   
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.SetSnipe(snipe, nbRetry + 1);
                else
                {
                    Logger.CreateLog("Error__SetSnipe", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            Logger.CreateLog("Ending__SetSnipe", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);
        }

        public List<eBayItemData> GetSimilarItems(Snipe snipe)
        {
            return this.GetSimilarItems(snipe, 0);
        }

        private List<eBayItemData> GetSimilarItems(Snipe snipe, int nbRetry)
        {
            if (this._backEndObject) this._eBayKeyHandler.NbApiCalls = this._eBayKeyHandler.NbApiCalls + 1;

            Logger.CreateLog("Beginning__GetSimilarItems", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

            string callname = "findItemsByKeywords";

            try
            {
                // Creating an object to the BestMatchService class
                CustomFindingService service = new CustomFindingService(callname, this.User.EBayRegistrationSiteID, this._eBayKeyHandler.GetEBayKeySet().AppID);
                service.Url = ServiceParametersHelper.eBayFindingUrl();
                
                FindItemsByKeywordsRequest request = new FindItemsByKeywordsRequest();
                request.keywords = snipe.ItemTitle;
                request.sortOrder = SortOrderType.EndTimeSoonest;
                request.sortOrderSpecified = true;

                List<ItemFilter> itemF = new List<ItemFilter>();

                ItemFilter itemType = new ItemFilter();
                itemType.name = ItemFilterType.ListingType;
                List<string> itemT = new List<string>();
                itemT.Add("Auction");
                itemType.value = itemT.ToArray();               
                itemF.Add(itemType);

                itemType = new ItemFilter();
                itemType.name = ItemFilterType.Seller;
                itemT = new List<string>();
                itemT.Add(snipe.ItemSellerID);
                itemType.value = itemT.ToArray();
                itemF.Add(itemType);

                request.itemFilter = itemF.ToArray();

                List<eBayItemData> items = new List<eBayItemData>();

                FindItemsByKeywordsResponse response = service.findItemsByKeywords(request);

                if (response.ack == AckValue.Success)
                {
                    SearchResult result = response.searchResult;

                    if (response.searchResult.item != null)
                    {
                        foreach (SearchItem itemC in response.searchResult.item)
                        {
                            if (itemC.title == snipe.ItemTitle)
                                items.Add(this.GetItemDetails(long.Parse(itemC.itemId)));
                        }
                    }
                }
                else
                {
                    string error = "";

                    foreach (ErrorData err in response.errorMessage) error += err.message + Environment.NewLine;

                    throw new Exception(error);
                }

                Logger.CreateLog("Ending__GetSimilarItems", snipe.SnipeID.ToString(), null, EnumLogLevel.INFO);

                return items;

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.GetSimilarItems(snipe, nbRetry + 1);
                else
                {
                    Logger.CreateLog("Error__GetSimilarItems", snipe.SnipeID.ToString(), ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            throw new Exception("Unknow error when getting similar items.");
        }

        public bool UserWonTheItem(long itemID)
        {
            return this.UserWonTheItem(itemID, 0);
        }

        private bool UserWonTheItem(long itemID, int nbRetry)
        {
            Logger.CreateLog("Beginning__UserWonTheItem", itemID.ToString(), null, EnumLogLevel.INFO);

            string callname = "GetMyeBayBuying";
            int nbPage = 0;

            try
            {

                this.BuildService(callname);

                GetMyeBayBuyingRequestType request = new GetMyeBayBuyingRequestType();
                request.Version = _version;
                request.WonList = new ItemListCustomizationType();
                request.WonList.Include = true;
                request.WonList.IncludeSpecified = true;
                request.WonList.ListingType = ListingTypeCodeType.Chinese;
                request.WonList.ListingTypeSpecified = true;
                request.WonList.DurationInDays = 1;
                request.WonList.DurationInDaysSpecified = true;

                request.WonList.Pagination = new PaginationType();
                request.WonList.Pagination.EntriesPerPage = 25;
                request.WonList.Pagination.EntriesPerPageSpecified = true;
                request.WonList.Pagination.PageNumber = 1;
                request.WonList.Pagination.PageNumberSpecified = true;

                DetailLevelCodeType level = DetailLevelCodeType.ReturnAll;
                List<DetailLevelCodeType> l = new List<DetailLevelCodeType>();
                l.Add(level);

                request.DetailLevel = l.ToArray();

                List<string> outputs = new List<string>();
                outputs.Add("WonList.OrderTransactionArray");
                outputs.Add("WonList.PaginationResult");

                request.OutputSelector = outputs.ToArray();

                GetMyeBayBuyingResponseType response = this.service.GetMyeBayBuying(request);

                if (response.Ack == AckCodeType.Success)
                {
                    if (response.WonList != null)
                        nbPage = response.WonList.PaginationResult.TotalNumberOfPages;
                    // There must be at least 1 page
                    if (nbPage > 0)
                    {
                        // We loop throw the pages
                        for (int i = 1; i <= response.WonList.PaginationResult.TotalNumberOfPages; i++)
                        {
                            // Dont reload if we are on the first page
                            if (i != 1)
                            {
                                request.WonList.Pagination = new PaginationType();
                                request.WonList.Pagination.EntriesPerPage = 25;
                                request.WonList.Pagination.EntriesPerPageSpecified = true;
                                request.WonList.Pagination.PageNumber = i;
                                request.WonList.Pagination.PageNumberSpecified = true;

                                response = service.GetMyeBayBuying(request);
                            }

                            foreach (OrderTransactionType orderTransaction in response.WonList.OrderTransactionArray)
                            {
                                if (orderTransaction.Transaction != null && orderTransaction.Transaction.Item != null && orderTransaction.Transaction.Item.ItemID == itemID.ToString()) return true;

                                if (orderTransaction.Order != null && orderTransaction.Order.TransactionArray != null)
                                {
                                    foreach (TransactionType transaction in orderTransaction.Order.TransactionArray)
                                    {
                                        if (transaction.Item != null && transaction.Item.ItemID == itemID.ToString()) return true;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string error = "";

                    foreach (ErrorType err in response.Errors) error += err.LongMessage + Environment.NewLine;

                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.UserWonTheItem(itemID, nbRetry + 1);
                else
                {
                    Logger.CreateLog("Error__UserWonTheItem", itemID.ToString(), ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            Logger.CreateLog("Ending__UserWonTheItem", itemID.ToString(), null, EnumLogLevel.INFO);

            return false;
        }

        public eBayItemData GetItemDetails(long itemID, bool returnJustPrice)
        {
            return GetItemDetails(itemID, 0, true);
        }

        public eBayItemData GetItemDetails(long itemID)
        {
            return GetItemDetails(itemID, 0, false);
        }

        /// <summary>
        /// Should be also using GetItemStatus
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="nbRetry"></param>
        /// <param name="returnJustPrice"></param>
        /// <returns></returns>
        private eBayItemData GetItemDetails(long itemID, int nbRetry, bool returnJustPrice)
        {
            Logger.CreateLog("Beginning__GetItemDetails", itemID.ToString(), null, EnumLogLevel.INFO);

            string callname = "GetItem";

            try
            {

                this.BuildService(callname);

                GetItemRequestType request = new GetItemRequestType();
                request.Version = _version;
                request.ItemID = itemID.ToString();

                List<string> outputs = new List<string>();
                outputs.Add("Item.SellingStatus.CurrentPrice");
                outputs.Add("Item.ListingType");

                if (!returnJustPrice)
                {
                    outputs.Add("Item.ListingDetails.EndTime");
                    outputs.Add("Item.ListingDetails.ViewItemURL");
                    outputs.Add("Item.Title");
                    outputs.Add("Item.PictureDetails");
                    outputs.Add("Item.Description");
                    outputs.Add("Item.Seller.UserID");
                    outputs.Add("Item.ShippingDetails");
                    outputs.Add("Item.PaymentMethods");
                }

                request.OutputSelector = outputs.ToArray();

                GetItemResponseType response = this.service.GetItem(request);

                if (response.Ack == AckCodeType.Success)
                {
                    if (response.Item.ListingType != ListingTypeCodeType.Chinese)
                        throw new Exception("This listing type can't be sniped.");
                    // http://developer.ebay.com/devzone/xml/docs/Reference/eBay/types/ListingTypeCodeType.html

                    eBayItemData itemData = new eBayItemData();

                    if (response.Item.SellingStatus != null) itemData.ItemCurrentHighestBid = response.Item.SellingStatus.CurrentPrice.Value;
                    if (response.Item.SellingStatus != null) itemData.ItemCurrencyCode = (EnumCurrencyCode)response.Item.SellingStatus.CurrentPrice.currencyID;
                    itemData.ItemID = itemID;

                    if (!returnJustPrice)
                    {
                        itemData.ItemEndDate = ServiceTimeZone.eBayDateTimeToUniversal(response.Item.ListingDetails.EndTime);
                        itemData.ItemURL = response.Item.ListingDetails.ViewItemURL;
                        itemData.ItemTitle = response.Item.Title;

                        if (response.Item.PictureDetails != null && response.Item.PictureDetails.PictureURL != null && response.Item.PictureDetails.PictureURL.ToList().Count() > 0)
                            itemData.ItemPictureURL = response.Item.PictureDetails.PictureURL[0];

                        if (string.IsNullOrEmpty(itemData.ItemPictureURL) && !string.IsNullOrEmpty(response.Item.PictureDetails.GalleryURL))
                            itemData.ItemPictureURL = response.Item.PictureDetails.GalleryURL;
   
                        itemData.ItemDescription = response.Item.Description;
                        itemData.ItemSellerID = response.Item.Seller.UserID;

#warning to be verified
                        itemData.ItemShippingInfo = response.Item.ShippingDetails.ToString();

                        itemData.ItemPaymentOptions = new List<string>();
                        foreach (BuyerPaymentMethodCodeType paymentMethod in response.Item.PaymentMethods.ToArray())
                            itemData.ItemPaymentOptions.Add(paymentMethod.ToString());
                    }

                    Logger.CreateLog("Ending__GetItemDetails", itemID.ToString(), null, EnumLogLevel.INFO);

                    return itemData;
                }
                else
                {
                    string error = "";

                    foreach (ErrorType err in response.Errors)
                        error += err.LongMessage;

                    throw new Exception(error);
                }

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.GetItemDetails(itemID, nbRetry + 1, returnJustPrice);
                else
                {
                    Logger.CreateLog("Error__GetItemDetails", itemID.ToString(), ex, EnumLogLevel.ERROR);
                    throw ex;
                }
            }

            throw new Exception("Unknow error when loading the item.");
        }

        private void BuildService (string callname)
        {
            eBayKeySet keys = this._eBayKeyHandler.GetEBayKeySet();

            int siteID = 1;
            if (this._user != null) siteID = (int)this._user.EBayRegistrationSiteID;

            string requestURL = _serverUrl + "?callname=" + callname + "&siteid=" + siteID.ToString()
                                + "&appid=" + keys.AppID + "&version=" + _version + "&routing=default";
            this.service.Url = requestURL;
            this.service.RequesterCredentials = new CustomSecurityHeaderType();
            this.service.RequesterCredentials.Credentials = new UserIdPasswordType();

            this.service.RequesterCredentials.Credentials.AppId = keys.AppID;
            this.service.RequesterCredentials.Credentials.DevId = keys.DevID;
            this.service.RequesterCredentials.Credentials.AuthCert = keys.CertID;

            if (this._backEndObject) this._eBayKeyHandler.NbApiCalls = this._eBayKeyHandler.NbApiCalls + 1;

            this.service.RequesterCredentials.eBayAuthToken = _user.EBayUserToken;

            if (!_user.HasValidAssignedEBayAccount())
            {
                List<UserMessage> errorList = new List<UserMessage>();

                errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserEBayAccountNotAssociated));

                ControlObjectException ex = new ControlObjectException(errorList);
                throw ex;
            }
        }
    }

    class CustomFindingService : FindingService
    {
        private string _callName;
        private string _site;
        private string _eBayAppID;

        public CustomFindingService(string callName, EnumSites site, string eBayAppID)
        {
            _callName = callName;
            _eBayAppID = eBayAppID;
            _site = ConvertSite(site);
        }

        protected override System.Net.WebRequest GetWebRequest(Uri url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)base.GetWebRequest(url);

                request.Headers.Add("X-EBAY-SOA-SECURITY-APPNAME", _eBayAppID);
                request.Headers.Add("X-EBAY-SOA-OPERATION-NAME", _callName);
                request.Headers.Add("X-EBAY-SOA-SERVICE-NAME", "FindingService");
                request.Headers.Add("X-EBAY-SOA-MESSAGE-PROTOCOL", "SOAP11");
                request.Headers.Add("X-EBAY-SOA-SERVICE-VERSION", "1.0.0");
                request.Headers.Add("X-EBAY-SOA-GLOBAL-ID", _site);

                return request;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ConvertSite(EnumSites site)
        {
            string siteName = string.Empty;

            switch (site)
            {
                case EnumSites.eBay_Australia:
                    siteName = "EBAY-AU";
                    break;
                case EnumSites.eBay_Austria:
                    siteName = "EBAY-AT";
                    break;
                case EnumSites.eBay_Belgium_Dutch:
                    siteName = "EBAY-NLBE";
                    break;
                case EnumSites.eBay_Belgium_French:
                    siteName = "EBAY-FRBE";
                    break;
                case EnumSites.eBay_Canada_English:
                    siteName = "EBAY-ENCA";
                    break;
                case EnumSites.eBay_Canada_French:
                    siteName = "EBAY-FRBE";
                    break;
                case EnumSites.eBay_France:
                    siteName = "EBAY-FR";
                    break;
                case EnumSites.eBay_Germany:
                    siteName = "EBAY-DE";
                    break;
                case EnumSites.eBay_Hong_Kong:
                    siteName = "EBAY-HK";
                    break;
                case EnumSites.eBay_India:
                    siteName = "EBAY-IN";
                    break;
                case EnumSites.eBay_Ireland:
                    siteName = "EBAY-IE";
                    break;
                case EnumSites.eBay_Italy:
                    siteName = "EBAY-IT";
                    break;
                case EnumSites.eBay_Malaysia:
                    siteName = "EBAY-MY";
                    break;
                case EnumSites.eBay_Netherlands:
                    siteName = "EBAY-NL";
                    break;
                case EnumSites.eBay_Philippines:
                    siteName = "EBAY-PH";
                    break;
                case EnumSites.eBay_Poland:
                    siteName = "EBAY-PL";
                    break;
                case EnumSites.eBay_Singapore:
                    siteName = "EBAY-SG";
                    break;
                case EnumSites.eBay_Spain:
                    siteName = "EBAY-ES";
                    break;
                case EnumSites.eBay_Switzerland:
                    siteName = "EBAY-CH";
                    break;
                case EnumSites.eBay_UK:
                    siteName = "EBAY-GB";
                    break;
                case EnumSites.eBay_United_States:
                    siteName = "EBAY-US";
                    break;
                default:
                    siteName = "EBAY-US";
                    break;
            }

            return siteName;
        }
    }

}
