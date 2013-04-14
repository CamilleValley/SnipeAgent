using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_DAL
{
    public class SqlDataContext: BaseDataContext
    {
        protected override IDataMapper<T> GetDataMapper<T>()
        {
            if (typeof(T) == typeof(Category)) return (IDataMapper<T>)new CategoryDataMapper();
            if (typeof(T) == typeof(Snipe)) return (IDataMapper<T>)new SnipeDataMapper();
            if (typeof(T) == typeof(User)) return (IDataMapper<T>)new UserDataMapper();
            if (typeof(T) == typeof(TokenFetcher)) return (IDataMapper<T>)new TokenFetcherDataMapper();

            throw new Exception("Unsupported type");
        }
    }
}
