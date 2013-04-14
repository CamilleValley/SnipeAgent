using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_BusinessObjects
{
    public enum EnumMessageCode
    { 
        CategoryIDNotNull,
        CategoryIDNull,
        CategoryNameEmpty,
        SnipeIDNotNull,
        SnipeIDNull,
        SnipeAlreadyExists,
        SnipeItemEndDateEmpty,
        SnipeBidInvalid,
        SnipeDelayInvalid,
        SnipeNameEmpty,
        UserEbayUserIDEmpty,
        UserEbayUserPwdEmpty,
        UserNameEmpty,
        UserPasswordEmpty,
        UserEmailEmpty,
        UserIDNull,
        UserIDNotNull,
        UserAlreadyExists,
        UserIDsMismatching,
        UserPasswordAndLoginNotMatching,
        UserNotLoggedIn,
        ItemEndedOrDelayToShort,
        SnipeBidUnderCurrentPrice,
        CategoryMaxNumberReached,
        CategoryIDNotFound,
        UserEmailWrongFormat,
        UserMobileAccessDenied,
        UserIPAddressWrongFormat,
        UserIPAddressEmpty,
        MaxSnipeAutoRetryReached,
        SnipeNbAutoRetryNotSpecified,
        SnipeAutoRetryNotAllowed,
        UserTokenAlreadyAssociated,
        UserCouldNotBeLoaded,
        UserEBayAccountNotAssociated,
        SnipeBidNotANumber,
        UserSubscriptionHexMismatching,
        UserEmailOrUserNameNotFound,
        UserPasswordTooSmall,
        UserNameTooSmall,
        SnipeWrongGenIncreaseBid,
        SnipeWrongGenRemainingNb,
        SessionIDAlreadyExist,
        TokenFetcherSessionIDNull,
        SnipeYoungerSnipeSameCategoryExists,
        SnipeBidOptimizerMaxBidReached
    }

    public struct UserMessage
    {
        public EnumSeverity Severity;
        public EnumMessageCode MessageCode;

        public UserMessage(EnumSeverity severity, EnumMessageCode messageCode)
        {
            Severity = severity;
            MessageCode = messageCode;
        }
    }

    public enum EnumSeverity
    {
        Information,
        Warning,
        Error,
        Bug
    }
}
