using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UltimateSniper_BusinessObjects;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace UltimateSniper_DAL
{
    public class TokenFetcherDataMapper : IDataMapper<TokenFetcher>
    {
        private static readonly string _cmdInsertTokenFetcher = "INSERT INTO TokenFetcher (SessionID) VALUES ('{0}');";
        private static readonly string _cmdDeleteTokenFetcher = "DELETE * FROM TokenFetcher WHERE SessionID = '{0}';";
        private static readonly string _cmdUpdateTokenFetcher = "";

        private static readonly string _cmdGetAllTokenFetcher = "SELECT * FROM TokenFetcher;";

        public virtual object Create<T>(object item, T type)
        {
            TokenFetcher fetcher = (TokenFetcher)item;

            #region Control Object

            if (fetcher.SessionID == null)
                throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.TokenFetcherSessionIDNull);

            // We control that the User Name doesnt already exist.
            SqlDataContext myDataConnection = new SqlDataContext();
            Query qr = new Query();

            Criterion critStatus = new Criterion();
            critStatus.Operator = CriteriaOperator.Equal;
            critStatus.PropertyName = "SessionID";
            critStatus.Value = "'" + fetcher.SessionID + "'";

            qr.Criteria.Add(critStatus);

            qr.Members.Add("*");

            if (myDataConnection.GetByCriteria<TokenFetcher>(qr).Count > 0)
                throw new ControlObjectException(EnumSeverity.Error, EnumMessageCode.SessionIDAlreadyExist);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            string q = string.Format(_cmdInsertTokenFetcher, fetcher.SessionID);

            dbAdap.ExecuteNonQuery(q);

            return fetcher;
        }

        public virtual void Update<T>(object item, T type)
        {
            throw new Exception("Not supported");
        }

        public virtual void Delete<T>(object item, T type)
        {
            TokenFetcher fetcher = (TokenFetcher) item;

            #region Control Object

            if (fetcher.SessionID == null) throw new ControlObjectException(EnumSeverity.Bug, EnumMessageCode.TokenFetcherSessionIDNull);

            #endregion

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();
            string q = string.Format(_cmdDeleteTokenFetcher, fetcher.SessionID);

            dbAdap.ExecuteNonQuery(q);
        }

        public virtual IList<TokenFetcher> GetByCriteria(Query q)
        {
            throw new Exception("Not supported");
        }

        public virtual IList<TokenFetcher> GetAll()
        {
            List<TokenFetcher> TokenFetchers = new List<TokenFetcher>();

            DataBaseAdaptor dbAdap = new DataBaseAdaptor();

            DataSet dataset = dbAdap.ExecuteReader(_cmdGetAllTokenFetcher);

            foreach (DataRow row in dataset.Tables[0].Rows)
            {
                TokenFetchers.Add(BuildEntityFromRawData(row));
            }

            return TokenFetchers;
        }

        public virtual TokenFetcher BuildEntityFromRawData(DataRow row)
        {
            TokenFetcher tokenFetcher = new TokenFetcher();

            tokenFetcher.SessionID = row["SessionID"].ToString();
            tokenFetcher.UserID = int.Parse(row["UserID"].ToString());

            return tokenFetcher;
        }
    }
}
