using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_DAL
{
    public sealed class Query
    {
        public string QueryName { get; set; }
        private List<Criterion> _criteria = new List<Criterion>();
        private List<string> _members = new List<string>();
        private IList<string> _orderClauses = new List<string>();

        /// <summary>
        /// List of criteria to be included to the QUERY
        /// </summary>
        public List<Criterion> Criteria
        {
            get { return _criteria; }
            set { _criteria = value; }
        }

        /// <summary>
        /// Orderby clause
        /// </summary>
        public IList<string> OrderClauses
        {
            get { return _orderClauses; }
            set { _orderClauses = value; }
        }

        /// <summary>
        /// List of members to be included in the SELECT
        /// </summary>
        public List<string> Members
        {
            get { return _members; }
            set { _members = value; }
        }
    }
}
