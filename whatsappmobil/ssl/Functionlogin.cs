using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class Functionlogin
    {
        string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        public int CekData(string strUser_ID, string strUser_Password)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "SELECT * FROM Mst_User where User_ID='" + strUser_ID + "' AND User_Password = '" + strUser_Password + "'";
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            int intChk = 0;
            if (myRead.Read())
            {
                intChk = 1;
            }
            else
            {
                intChk = 0;
            }
            DBConnection.Close();
            return intChk;
        }

        public String getDataTable(string strpQuery)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = strpQuery;
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            string strResult = "";
            if (myRead.Read())
            {
                for (int ii = 0; ii < myRead.FieldCount; ii++)
                {
                    if (myRead[myRead.GetName(ii)] != DBNull.Value)
                        strResult = strResult + Convert.ToString(myRead[myRead.GetName(ii)]) + "#";
                    else
                        strResult = strResult + " #";
                }
            }
            else
            {
                strResult = "";
            }
            DBConnection.Close();
            return strResult;
        }
    }
}