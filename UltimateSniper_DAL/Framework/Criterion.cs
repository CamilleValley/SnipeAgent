using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_DAL
{
    public class Criterion
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public CriteriaOperator Operator { get; set;}
    }

    public enum CriteriaOperator
    {
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        Like
    }
}
