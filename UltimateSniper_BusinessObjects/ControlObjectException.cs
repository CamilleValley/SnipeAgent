using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_BusinessObjects
{
    public class ControlObjectException : Exception
    {
        private List<UserMessage> _errorList = new List<UserMessage>();

        public List<UserMessage> ErrorList
        {
          get { return _errorList; }
        }

        public ControlObjectException(List<UserMessage> errorList)
        {
            _errorList = errorList;
        }

        public ControlObjectException(EnumSeverity severity, EnumMessageCode messageCode)
        {
            UserMessage message = new UserMessage();

            message.MessageCode = messageCode;
            message.Severity = severity;

            this._errorList.Add(message);
        }
    }
}
