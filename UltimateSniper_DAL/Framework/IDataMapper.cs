using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_DAL
{
    public interface IDataMapper<T>
    {
        object Create<T>(object item, T type);
        void Update<T>(object item, T type);
        void Delete<T>(object item, T type);
        IList<T> GetAll();
        IList<T> GetByCriteria(Query query);
    }
}
