using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UltimateSniper_DAL
{
    public interface IDataContext : IDisposable
    {
        #region Query services

        IList<T> GetAll<T>() where T : class, new();
        IList<T> GetByCriteria<T>(Query query) where T : class, new();

        #endregion

        #region CRUD

        object Add<T>(object item, T type);
        void Delete<T>(object item, T type);
        void Save<T>(object item, T type);

        #endregion

        #region Transaction Management

        bool IsInTransaction { get; }
        bool IsDirty { get; }
        void BeginTransaction();
        void RollBack();
        void Commit();

        #endregion

    }
}
