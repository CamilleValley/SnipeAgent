using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_BusinessObjects
{
    public class TokenFetcher : IBusinessObject
    {
        #region Attributes

        private string _SessionID;
        private int _UserID;

        #endregion

        #region Accessors

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string SessionID
        {
            get { return _SessionID; }
            set { _SessionID = value; }
        }

        #endregion

        #region Functions

        public void ControlObject()
        {
        }

        #endregion
    }
}
