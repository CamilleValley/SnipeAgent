using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;

namespace UltimateSniper_DAL
{
    public abstract class BaseDataContext: IDataContext, IDisposable
    {
        protected abstract IDataMapper<T> GetDataMapper<T>();

        private bool _isInTransaction = false;
        protected bool _isDirty = false;

        public virtual bool IsDirty
        {
            get { return _isDirty; }
            private set { _isDirty = value; }
        }

        public bool IsInTransaction
        {
            get { return _isInTransaction; }
            private set { _isInTransaction = value; }
        }

        public object Add<T>(object item, T type)
        {
            Type t = item.GetType();
            return GetDataMapper<T>().Create(item, type);
        }

        public void Delete<T>(object item, T type)
        {
            Type t = item.GetType();
            GetDataMapper<T>().Delete(item, type);
        }

        public void Save<T>(object item, T type)
        {
            Type t = item.GetType();
            GetDataMapper<T>().Update(item, type);
        }

        public virtual void Dispose()
        {
            if (this.IsInTransaction) RollBack();
        }

        public virtual IList<T> GetAll<T>() where T : class, new()
        {
            return GetDataMapper<T>().GetAll();
        }

        public virtual IList<T> GetByCriteria<T>(Query query) where T : class, new()
        {
             return GetDataMapper<T>().GetByCriteria(query);
        }

        #region IDataContext Members

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void RollBack()
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
