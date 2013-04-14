using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_BusinessObjects
{
    public class Category : IBusinessObject
    {
        #region Attributes

        private int? _CategoryID;
        private string _CategoryName;
        private int? _UserID;
        private bool _isCategoryActive;
        private DateTime? _CategoryDisactivationDate;

        private bool _LoadedThroughSnipe = false;

        #endregion

        #region Accessors

        public bool LoadedThroughSnipe
        {
            get { return _LoadedThroughSnipe; }
            set { _LoadedThroughSnipe = value; }
        }

        public DateTime? CategoryDisactivationDate
        {
            get { return _CategoryDisactivationDate; }
            set { _CategoryDisactivationDate = value; }
        }

        public bool IsCategoryActive
        {
            get { return _isCategoryActive; }
            set { _isCategoryActive = value; }
        }

        public int? UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string CategoryName
        {
            get { return _CategoryName; }
            set { _CategoryName = value; }
        }

        public int? CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        #endregion

        #region Functions

        public void ControlObject()
        {
            List<UserMessage> errorList = new List<UserMessage>();

            if (string.IsNullOrEmpty(this._CategoryName)) errorList.Add(new UserMessage(EnumSeverity.Error, EnumMessageCode.CategoryNameEmpty));

            if (errorList.Count != 0)
            {
                ControlObjectException ex = new ControlObjectException(errorList);
                throw ex;
            }
        }

        #endregion
    }
}
