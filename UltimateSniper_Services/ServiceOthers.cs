using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using Zayko.Finance;
using UltimateSniper_DAL;

namespace UltimateSniper_Services
{
    public class ServiceOthers
    {
        public string DefaultToken()
        {
            return ServiceParametersHelper.DefaultGeteBayTimeToken();
        }

        public DateTime DefaultTokenExpirationDate()
        {
            return ServiceParametersHelper.DefaultGeteBayTimeTokenExpirationDate();
        }

        public List<EnumCurrencyCode> GetCurrencyList()
        {
            List<EnumCurrencyCode> currencies = new List<EnumCurrencyCode>();

            currencies.Add(EnumCurrencyCode.USD);
            currencies.Add(EnumCurrencyCode.EUR);
            currencies.Add(EnumCurrencyCode.GBP);
            currencies.Add(EnumCurrencyCode.SEK);
            currencies.Add(EnumCurrencyCode.DKK);
            currencies.Add(EnumCurrencyCode.CHF);

            Array enumValArray = Enum.GetValues(typeof(EnumCurrencyCode));
            List<EnumCurrencyCode> enumValList = new List<EnumCurrencyCode>(enumValArray.Length);

            foreach (int val in enumValArray)
            {
                if (!currencies.Contains((EnumCurrencyCode)val))
                    currencies.Add((EnumCurrencyCode)Enum.Parse(typeof(EnumCurrencyCode), val.ToString()));
            }

            return currencies;
        }

        public double ConvertValue(double value, string currencyOrigine, string currencyDestination)
        {
            return this.ConvertValue(value, currencyOrigine, currencyDestination, 0);
        }

        private double ConvertValue(double value, string currencyOrigine, string currencyDestination, int nbRetry)
        {
            if (currencyDestination == currencyOrigine)
                return value;

            try
            {

                CurrencyConverter myCurrencyConverter = new CurrencyConverter();
                CurrencyData cd = new CurrencyData(currencyOrigine, currencyDestination);
                // Creates new structure and set Base currency
                // to Euro and Target to Russian Ruble

                myCurrencyConverter.GetCurrencyData(ref cd);

                double newValue = value * cd.Rate;

                newValue = Math.Round(newValue, 2, MidpointRounding.AwayFromZero);

                return newValue;

            }
            catch (Exception ex)
            {
                if (nbRetry < ServiceParametersHelper.nbAPIRetry())
                    this.ConvertValue(value,currencyOrigine,currencyDestination, nbRetry+1);
                else
                    throw ex;
            }

            return value;
        }

        public User GetUserForSnipe(Snipe snipe)
        {
            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion c = new Criterion();
            c.Operator = CriteriaOperator.Equal;
            c.PropertyName = "UserID";
            c.Value = snipe.UserID;

            q.Criteria.Add(c);

            q.Members.Add("*");

            IList<User> userList = myDataConnection.GetByCriteria<User>(q);

            if (userList.Count != 1)
                return null;
            else
                return userList[0];
        }


        public Snipe GetSnipe(int snipeID)
        {
            SqlDataContext myDataConnection = new SqlDataContext();

            Query q = new Query();

            Criterion crit = new Criterion();
            crit.Operator = CriteriaOperator.Equal;
            crit.PropertyName = "SnipeID";
            crit.Value = snipeID;

            q.Criteria.Add(crit);

            q.Members.Add("*");

            List<Snipe> snipes = myDataConnection.GetByCriteria<Snipe>(q).ToList();

            if (snipes.Count != 1)
                throw new Exception("Error when loading the snipe");
            else
                return snipes[0];
        }

        public bool CheckSnipeValidity(Snipe snipe, UltimateSniper_Services.ServiceEBay eBayService)
        {
            bool res = false;
            SqlDataContext myDataConnection = new SqlDataContext();

            User user = this.GetUserForSnipe(snipe);

            if (user == null)
            {
                snipe.SnipeStatus = EnumSnipeStatus.ERROR;
                snipe.SnipeErrorReason = "The user assocated to the snipe couldn't be loaded properly.";
            }
            else
            {
                eBayItemData item = eBayService.GetItemDetails(snipe.ItemID); 

                if (item.ItemCurrentHighestBid > snipe.SnipeBidInFinalCurrency)
                {
                    UltimateSniper_Services.ServiceEmail emailService = new UltimateSniper_Services.ServiceEmail();

                    snipe.SnipeStatus = EnumSnipeStatus.OVERBID;
                    snipe.SnipeErrorReason = "The snipe bid is lower the current price.";

                    ServiceEmail.SendEmail(user, "[SnipeAgent] Maximum bid reached for: " + snipe.SnipeName, "Hello, you snipe bid is lower the current price. You can still modify it by going on www.snipeagent.com and into the section 'My Snipes'. Kind regards, Snipe Agent.");
                }

                res = true;

                item.ItemCurrentHighestBidUserCurrency = this.ConvertValue(item.ItemCurrentHighestBid, item.ItemCurrencyCode.ToString(), user.UserCurrency.ToString());
                snipe.ItemLastKnownPrice = item.ItemCurrentHighestBidUserCurrency;

                snipe.ItemEndDate = item.ItemEndDate;
                snipe.ItemTitle = item.ItemTitle;
                snipe.ItemURL = item.ItemURL;
                snipe.ItemSellerID = item.ItemSellerID;
                snipe.ItemPictureURL = item.ItemPictureURL;

                snipe.ItemLastUdpate = ServiceTimeZone.DateTimeToUniversal(DateTime.Now);
            }

            snipe.ValidityCheckInProgress = false;

            myDataConnection.Save<Snipe>((object)snipe, snipe);

            return res;
        }
    }
}
