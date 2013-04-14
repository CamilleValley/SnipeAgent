using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using UltimateSniper_Logger;

namespace UltimateSniper_DAL
{
    public class DataBaseAdaptor
    {
        private DataSet ConvertDataReaderToDataSet(OleDbDataReader reader)
        {
            DataSet dataSet = new DataSet();
            DataTable schemaTable = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();

            for (int cntr = 0; cntr < schemaTable.Rows.Count; ++cntr)
            {
                DataRow dataRow = schemaTable.Rows[cntr];
                string columnName = dataRow["ColumnName"].ToString();
                DataColumn column = new DataColumn(columnName, dataRow["ColumnName"].GetType());
                dataTable.Columns.Add(column);
            }

            while (reader.Read())
            {
                DataRow dataRow = dataTable.NewRow();
                for (int cntr = 0; cntr < reader.FieldCount; ++cntr)
                {
                    dataRow[cntr] = reader.GetValue(cntr);
                }

                dataTable.Rows.Add(dataRow);
            }

            dataSet.Tables.Add(dataTable);

            return dataSet;
        }

        private string GetConnexionString()
        {
            if (UltimateSniper_DAL.DALSettings.Default.Production)
                return UltimateSniper_DAL.DALSettings.Default.DBConnexionStringProd;
            else
                return UltimateSniper_DAL.DALSettings.Default.DBConnexionStringDev;
        }

        public void ExecuteNonQuery(string q)
        {
            q = CleanQuery(q);

            Logger.CreateLog("ExecuteNonReaderQuery", q, null, EnumLogLevel.INFO);

            OleDbConnection oConn = new OleDbConnection(GetConnexionString());
            oConn.Open();

            OleDbCommand oCmd = new OleDbCommand(q, oConn);
            oCmd.ExecuteNonQuery();

            oConn.Close();
        }

        public DataSet ExecuteReader(string q)
        {
            q = CleanQuery(q);

            Logger.CreateLog("ExecuteReaderQuery", q, null, EnumLogLevel.INFO);

            DataSet dataset = null;
            OleDbConnection oConn = new OleDbConnection(GetConnexionString());
            oConn.Open();

            try
            {
                OleDbCommand oCmd = new OleDbCommand(q, oConn);
                OleDbDataReader reader = oCmd.ExecuteReader();
                dataset = this.ConvertDataReaderToDataSet(reader);
            }
            catch (Exception ex)
            {
                oConn.Close();
                ex.HelpLink = q;
                throw ex;
            }
            oConn.Close();

            return dataset;
        }

        public string SafeSqlLiteral(System.Object theValue, System.Object theLevel)
        {

            // Written by user CWA, CoolWebAwards.com Forums. 2 February 2010
            // http://forum.coolwebawards.com/threads/12-Preventing-SQL-injection-attacks-using-C-NET

            // intLevel represent how thorough the value will be checked for dangerous code
            // intLevel (1) - Do just the basic. This level will already counter most of the SQL injection attacks
            // intLevel (2) -   (non breaking space) will be added to most words used in SQL queries to prevent unauthorized access to the database. Safe to be printed back into HTML code. Don't use for usernames or passwords

            if (theValue != null && !string.IsNullOrEmpty(theValue.ToString()))
            {

                string strValue = (string)theValue.ToString();
                int intLevel = (int)theLevel;

                bool addFL = false;

                if (strValue.Substring(0, 1) == "'" && strValue.Substring(strValue.Length - 1, 1) == "'")
                {
                    addFL = true;

                    strValue = strValue.Substring(0, strValue.Length - 1);
                    strValue = strValue.Substring(1, strValue.Length - 1);
                }

                if (strValue != null)
                {
                    if (intLevel > 0)
                    {
                        strValue = strValue.Replace("'", "''"); // Most important one! This line alone can prevent most injection attacks
                        strValue = strValue.Replace("--", "");
                        strValue = strValue.Replace("[", "[[]");
                        strValue = strValue.Replace("%", "[%]");
                    }
                    if (intLevel > 1)
                    {
                        string[] myArray = new string[] { "xp_ ", "update ", "insert ", "select ", "drop ", "alter ", "create ", "rename ", "delete ", "replace " };
                        int i = 0;
                        int i2 = 0;
                        int intLenghtLeft = 0;
                        for (i = 0; i < myArray.Length; i++)
                        {
                            string strWord = myArray[i];
                            Regex rx = new Regex(strWord, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            MatchCollection matches = rx.Matches(strValue);
                            i2 = 0;
                            foreach (Match match in matches)
                            {
                                GroupCollection groups = match.Groups;
                                intLenghtLeft = groups[0].Index + myArray[i].Length + i2;
                                strValue = strValue.Substring(0, intLenghtLeft - 1) + "&nbsp;" + strValue.Substring(strValue.Length - (strValue.Length - intLenghtLeft), strValue.Length - intLenghtLeft);
                                i2 += 5;
                            }
                        }
                    }
                }

                if (addFL)
                    strValue = "'" + strValue + "'";

                return strValue;
            }

            return "";
        }

        public int GetID(string tableName, string tableID)
        {
            string q = "SELECT MAX(" + tableID + ") + 1 as NewID FROM " + tableName;

            DataSet ds = this.ExecuteReader(q);

            return int.Parse(ds.Tables[0].Rows[0]["NewID"].ToString());
        }

        public string TranslateQuery(Query query, string from)
        {
            string q = "SELECT ";

            for (int i = 0; i < query.Members.Count; i++)
            {
                q += query.Members[i];

                if (i < query.Members.Count -1) q += ",";
            }

            q += " FROM " + from + " WHERE 1=1 ";

            foreach (Criterion criter in query.Criteria)
            {
                q += " AND " + criter.PropertyName;

                switch (criter.Operator)
                {
                    case CriteriaOperator.Equal:
                        q += " = ";
                        break;

                    case CriteriaOperator.GreaterThan:
                        q += " > ";
                        break;

                    case CriteriaOperator.LessThan:
                        q += " < ";
                        break;

                    case CriteriaOperator.Like:
                        q += " LIKE ";
                        break;

                    case CriteriaOperator.NotEqual:
                        q += " <> ";
                        break;
                }

                q += SafeSqlLiteral(criter.Value, 2);
            }

            return q;
        }

        public string CleanQuery(string query)
        {
            while (query.Contains("#DateParse#"))
            {
                int debindex = query.IndexOf("#DateParse#") + 11;
                int endindex = query.IndexOf("#EndDateParse#");
                string datestg = query.Substring(debindex, endindex - debindex);
                DateTime date = DateTime.Parse(datestg);

                string formatedDate = string.Empty;

                if (UltimateSniper_DAL.DALSettings.Default.Production)
                    formatedDate = "'" + date.ToString() + "'";
                else
                    formatedDate = "#" + date.Month.ToString() + "/" + date.Day.ToString() + "/" + date.Year.ToString() + " " + date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString() + "#";

                query = query.Replace("#DateParse#" + datestg + "#EndDateParse#", formatedDate);
            }

            if (UltimateSniper_DAL.DALSettings.Default.Production)
                query = query.Replace("DELETE * FROM", "DELETE");

            DateTime dateNow = DateTime.Now.ToUniversalTime();

            if (UltimateSniper_DAL.DALSettings.Default.Production)
                query = query.Replace("##DateNow##", "'" + dateNow.ToString() + "'");
            else
                query = query.Replace("##DateNow##", "#" + dateNow.Month.ToString() + "/" + dateNow.Day.ToString() + "/" + dateNow.Year.ToString() + " " + dateNow.Hour.ToString() + ":" + dateNow.Minute.ToString() + ":" + dateNow.Second.ToString() + "#");

            return query;
        }
    }
}
