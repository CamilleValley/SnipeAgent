using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UltimateSniper_BusinessObjects
{
    public class User : IBusinessObject
    {

        #region Attributes

        private int? _UserID;
        private string _UserName;
        private string _UserPassword;
        private string _eBayUserToken;
        private float _UserCredit;
        private bool _UserActive;
        private List<Category> _UserCategoryList = new List<Category>();
        private DateTime? _UserDisactivationDate;
        private string _UserEmailAddress;
        private EnumSites _eBayRegistrationSiteID;
        private DateTime _eBayUserTokenExpirationDate;
        private string _eBayUserID;
        private string _eBayUserPwd;
        private DateTime _UserRegistrationDate;
        private EnumCurrencyCode _UserCurrency;
        private DateTime _UserOptionsEndDate;
        private string _UserIPAddress;
        private double _HoursDiffStdTime;
        private bool _ShowSnipeStyles;

        #endregion

        #region Accessors

        public bool ShowSnipeStyles
        {
            get { return _ShowSnipeStyles; }
            set { _ShowSnipeStyles = value; }
        }

        public double HoursDiffStdTime
        {
            get { return _HoursDiffStdTime; }
            set { _HoursDiffStdTime = value; }
        }

        public DateTime UserOptionsEndDate
        {
            get { return _UserOptionsEndDate; }
            set { _UserOptionsEndDate = value; }
        }

        public string UserIPAddress
        {
            get { return _UserIPAddress; }
            set { _UserIPAddress = value; }
        }

        public string EBayUserID
        {
            get { return _eBayUserID; }
            set { _eBayUserID = value; }
        }

        public string EBayUserPwd
        {
            get { return _eBayUserPwd; }
            set { _eBayUserPwd = value; }
        }

        public EnumCurrencyCode UserCurrency
        {
            get { return _UserCurrency; }
            set { _UserCurrency = value; }
        }

        public DateTime UserRegistrationDate
        {
            get { return _UserRegistrationDate; }
            set { _UserRegistrationDate = value; }
        }

        public DateTime EBayUserTokenExpirationDate
        {
            get { return _eBayUserTokenExpirationDate; }
            set { _eBayUserTokenExpirationDate = value; }
        }

        public EnumSites EBayRegistrationSiteID
        {
            get { return _eBayRegistrationSiteID; }
            set { _eBayRegistrationSiteID = value; }
        }

        public string UserEmailAddress
        {
            get { return _UserEmailAddress; }
            set { _UserEmailAddress = value; }
        }

        public DateTime? UserDisactivationDate
        {
            get { return _UserDisactivationDate; }
            set { _UserDisactivationDate = value; }
        }

        public List<Category> UserCategoryList
        {
            get { return _UserCategoryList; }
            set { _UserCategoryList = value; }
        }

        public string EBayUserToken
        {
            get { return _eBayUserToken; }
            set { _eBayUserToken = value; }
        }

        public bool UserActive
        {
            get { return _UserActive; }
            set { _UserActive = value; }
        }

        public float UserCredit
        {
            get { return _UserCredit; }
            set { _UserCredit = value; }
        }

        public string UserPassword
        {
            get { return _UserPassword; }
            set { _UserPassword = value; }
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }

        public int? UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        #endregion

        #region Functions

        public void ControlObject()
        {
            List<UserMessage> errorList = new List<UserMessage>();

            //if (string.IsNullOrEmpty(this._eBayUserID)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserEbayUserIDEmpty));
            //if (string.IsNullOrEmpty(this._eBayUserPwd)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserEbayUserPwdEmpty));

            if (string.IsNullOrEmpty(this._UserName)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserNameEmpty));
            else if (this._UserName.Length < 5) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserNameTooSmall));
            if (string.IsNullOrEmpty(this._UserPassword)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserPasswordEmpty));
            else if (this._UserPassword.Length < 5) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserPasswordTooSmall));
            if (string.IsNullOrEmpty(this._UserEmailAddress)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserEmailEmpty));
            else
                if (!isValidEmail()) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserEmailWrongFormat));
            if (string.IsNullOrEmpty(this._UserIPAddress)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserIPAddressEmpty));
            else
                if (!IsValidIP()) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.UserIPAddressWrongFormat));

            if (errorList.Count != 0)
            {
                ControlObjectException ex = new ControlObjectException(errorList);
                throw ex;
            }
        }
        
        private bool isValidEmail()
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);

            if (re.IsMatch(this.UserEmailAddress))
                return (true);
            else
                return (false);
        }

        /// <summary>
        /// method to validate an IP address
        /// using regular expressions. The pattern
        /// being used will validate an ip address
        /// with the range of 1.0.0.0 to 255.255.255.255
        /// </summary>
        /// <param name="addr">Address to validate</param>
        /// <returns></returns>
        public bool IsValidIP()
        {
            //create our match pattern
            string pattern = @"\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b";
            //create our Regular Expression object
            Regex check = new Regex(pattern);
            //boolean variable to hold the status
            bool valid = false;
            //check to make sure an ip address was provided
            if (this.UserIPAddress == "")
            {
                //no address provided so return false
                valid = false;
            }
            else
            {
                //address provided so use the IsMatch Method
                //of the Regular Expression object
                valid = check.IsMatch(UserIPAddress, 0);
            }

            if (this.UserIPAddress == "::1") valid = true;

            //return the results
            return valid;
        }

        public bool HasOptions()
        {
            return (this.UserOptionsEndDate > DateTime.Now.ToUniversalTime()) ;
        }

        public bool HasValidAssignedEBayAccount()
        {
            return (!string.IsNullOrEmpty(this.EBayUserToken) && this.EBayUserTokenExpirationDate > DateTime.Now.ToUniversalTime());
        }

        #endregion
    }

    public struct TokenData
    {
        public string Token;
        public DateTime ExpirationDate;
        public string UserID;
    }
}
