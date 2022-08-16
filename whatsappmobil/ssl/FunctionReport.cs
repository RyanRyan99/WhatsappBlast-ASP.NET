using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class FunctionReport
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        public void UpdateStatusMessageReport(string strDate, string strNumber)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set status_session = '1' where sent_date = to_date('" + strDate + "', 'dd/mm/yyyy') and wa_number = '" + strNumber + "'";
            try
            {
                DBCommand = new OracleCommand(Query, DBConnection);
                DBCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DBConnection.Close();
            }
        }
        public void UpdateSessionNotic(string strTrxId, string strWaNumber, string strSessionNotic)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set session_notic = '" + strSessionNotic + "' where trxid = '" + strTrxId + "' and wa_number = '" + strWaNumber + "'";
            try
            {
                DBCommand = new OracleCommand(Query, DBConnection);
                DBCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DBConnection.Close();
            }
        }

        public void UpdateSessionNoticReport(string strDate, string strNumber, string strSessionNotic)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set session_notic = '" + strSessionNotic + "' where sent_date = to_date('" + strDate + "', 'dd/mm/yyyy') and wa_number = '" + strNumber + "'";
            try
            {
                DBCommand = new OracleCommand(Query, DBConnection);
                DBCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                DBConnection.Close();
            }
        }
    }
}