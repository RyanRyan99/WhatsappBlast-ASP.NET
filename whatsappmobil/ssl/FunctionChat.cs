using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class FunctionChat
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        public void InsertTemplateMessage(string strTemplate)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_template(template_value, template_desc) values('" + strTemplate + "','" + strTemplate + "')";
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

        public void InsertPrivateContact(string strNumber, string strUser, string strisGroup, string strTimestamp, string strUnreadCount, string strSender)
        {
            try
            {
                OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
                OracleCommand DBCommand;
                DBConnection.Open();

                var Query = "insert into trx_whatsapp_p_contact(whatsapp_number, whatsapp_user, is_group, whatsapp_timestamp, unread_count, create_date, sender_id) values ('" + strNumber + "','" + strUser + "','" + strisGroup + "','" + strTimestamp + "','" + strUnreadCount + "',sysdate, '" + strSender + "')";
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
            catch (Exception ex)
            {

            }
        }
        public void UpdatePrivateContact(string strNumber, string strUser, string strisGroup, string strTimestamp, string strUnreadCount, string strSender)
        {
            try
            {
                OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
                OracleCommand DBCommand;
                DBConnection.Open();

                var Query = "update trx_whatsapp_p_contact set whatsapp_user = '" + strUser + "', whatsapp_timestamp = '" + strTimestamp + "', unread_count = '" + strUnreadCount + "', update_date = sysdate where whatsapp_number = '" + strNumber + "' and sender_id = '" + strSender + "'";
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
            catch (Exception ex)
            {

            }
        }
    }
}