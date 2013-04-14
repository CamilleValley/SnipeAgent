using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_BusinessObjects
{
    public delegate void PlaceSnipeEventHandler(Snipe e);
    public delegate void CheckSnipeResultEventHandler(Snipe e);
    
    // Class that contains the data for
    // the checktime event. Derives from System.EventArgs.
    //
    public class CheckTimeEventArgs : EventArgs
    {
        private DateTime _eBayTime;
        private int _snipeExecutionDelay;

        //Constructor.
        //
        public CheckTimeEventArgs(DateTime _eBayTime, int _snipeExecutionDelay)
        {
            this._eBayTime = _eBayTime;
            this._snipeExecutionDelay = _snipeExecutionDelay;
        }

        public DateTime EBayTime
        {
            get { return _eBayTime; }
        }

        public int SnipeExecutionDelay
        {
            get { return _snipeExecutionDelay; }
        }
    }
}
