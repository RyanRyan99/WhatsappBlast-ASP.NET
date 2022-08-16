using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class FunctionDashboard
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];

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
        public string GetWhatsappMediaCount(string CreateBy)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select count(trx_whatsapp_message.trxid) as trxid from trx_whatsapp_message inner join trx_whatsapp_header on trx_whatsapp_message.trxid = trx_whatsapp_header.trxid where is_media = '1' and trx_whatsapp_message.status_session = '1' and create_by = '"+CreateBy+"'";
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            if (myRead.Read())
            {
                for (int ii = 0; ii < myRead.FieldCount; ii++)
                {
                    strResult = strResult + Convert.ToString(myRead[myRead.GetName(ii)]) + "#";
                }
            }
            else
            {
                strResult = "";
            }
            DBConnection.Close();
            return strResult;
        }
        public string GetWhatsappMessageCount(string CreateBy)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select count(trx_whatsapp_message.trxid) as trxid from trx_whatsapp_message inner join trx_whatsapp_header on trx_whatsapp_message.trxid = trx_whatsapp_header.trxid where is_media = '0' and trx_whatsapp_message.status_session = '1' and create_by = '" + CreateBy + "'";
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            if (myRead.Read())
            {
                for (int ii = 0; ii < myRead.FieldCount; ii++)
                {
                    strResult = strResult + Convert.ToString(myRead[myRead.GetName(ii)]) + "#";
                }
            }
            else
            {
                strResult = "";
            }
            DBConnection.Close();
            return strResult;
        }
        public string GetTotalTargetCount(string CreateBy)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select count(wa_number) wa_number from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where create_by = '"+ CreateBy + "'";
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            if (myRead.Read())
            {
                for (int ii = 0; ii < myRead.FieldCount; ii++)
                {
                    strResult = strResult + Convert.ToString(myRead[myRead.GetName(ii)]) + "#";
                }
            }
            else
            {
                strResult = "";
            }
            DBConnection.Close();
            return strResult;
        }
        public string GetTotalTargetPlan(string CreateBy)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select count(id_plan) id_plan from trx_whatsapp_plan where create_by = '"+CreateBy+"'";
            OracleDataReader myRead = null;
            DBConnection.Open();
            DBCommand = new OracleCommand(StrQuery, DBConnection);
            myRead = DBCommand.ExecuteReader();
            if (myRead.Read())
            {
                for (int ii = 0; ii < myRead.FieldCount; ii++)
                {
                    strResult = strResult + Convert.ToString(myRead[myRead.GetName(ii)]) + "#";
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