using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class Function
    {
        //string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
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

        #region ImportMessage

        public void InsertTrxMessageHeader(string strTrxId, string strTitleContent, string strWaDate, string strSenderId, string strWaMedia, string strIsMedia, string strScheduledDate, string strScheduledTime, string strCreateBy)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_header(TRXID,WA_HEADER,WA_DATE,STATUS_SESSION,SENDER_ID,WA_MEDIA,IS_MEDIA,CREATE_DATE, SCHEDULED_DATE, SCHEDULED_TIME, CREATE_BY) values('" + strTrxId + "','" + strTitleContent + "',to_date('" + strWaDate + "','DD/MM/YYYY'),'0','" + strSenderId + "', '" + strWaMedia + "', '" + strIsMedia + "', sysdate, to_date('" + strScheduledDate + "','DD/MM/YYYY'), '" + strScheduledTime + "', '"+strCreateBy+"')";
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
        public void InsertWhatsappImport(string strSender, string strSentData, string strTypeMessage, string strMessageTitle, string strNumber, string strMessage, string strStatusSession, string strTrxId)
        {
            try
            {
                OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
                OracleCommand DBCommand;
                DBConnection.Open();

                var Query = "insert into trx_whatsapp_message(sender_id, sent_date, type_message, message_title, wa_number, message_content, status_session, trxid) values('" + strSender + "',to_date('" + strSentData + "', 'DD/MM/YYYY'),'" + strTypeMessage + "','" + strMessageTitle + "','" + strNumber + "','" + strMessage + "','" + strStatusSession + "','" + strTrxId + "')";
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
        #endregion

        #region WhatsappMessageText

        public void UpdateStatusHeader(string strTrxId)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_header set status_session = '1' where trxid = '" + strTrxId + "' ";
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

        public void UpdateStatusMessage(string strTrxId, string strWaNumber, string strStatus)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set status_session = '" + strStatus + "' where trxid = '" + strTrxId + "' and wa_number = '" + strWaNumber + "'";
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

            var Query = "update trx_whatsapp_message set session_notic = '" + strSessionNotic + "', status_session = '2' where trxid = '" + strTrxId + "' and wa_number = '" + strWaNumber + "'";
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

        public void DeleteMessageHeader(string strTrxId)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "delete from trx_whatsapp_header where trxid = '" + strTrxId + "'";
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
        public void DeleteMessageDetail(string strTrxId)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "delete from trx_whatsapp_message where trxid = '" + strTrxId + "'";
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

        #endregion

        #region BlastGroup

        public void InsertWhatsappBlastGroup(string strTrxId, string strTitleBlast, string strGroupName, string strSenderId, string strMessageContent, string strBranchId, string strIsMedia, string strCreateBy, string strBlastDate, string strMediaName, string strScheduled, string strScheduledTime)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_message_group(trxid, title_blast, group_name, sender_id, message_content, branch_id, is_media, create_date, create_by, blast_date, blast_status, media_name, scheduled_date, scheduled_time) values('" + strTrxId + "','" + strTitleBlast + "','" + strGroupName + "','" + strSenderId + "','" + strMessageContent + "','" + strBranchId + "','" + strIsMedia + "',sysdate,'" + strCreateBy + "',to_date('" + strBlastDate + "', 'dd/mm/yyyy'), '0', '" + strMediaName + "', to_date('" + strScheduled + "', 'DD/MM/YYYY'), '" + strScheduledTime + "')";
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

        public void UpdateWhatsappBlastGroup(string strTrxId, string strTitleBlast, string strGroupName, string strSenderId, string strMessageContent, string strIsMedia, string strBlastDate, string strMediaName, string strScheduled, string strScheduledTime)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message_group set title_blast = '" + strTitleBlast + "', group_name = '" + strGroupName + "', sender_id = '" + strSenderId + "', message_content = '" + strMessageContent + "', is_media = '" + strIsMedia + "', blast_date = '" + strBlastDate + "', media_name = '" + strMediaName + "', scheduled_date = to_date('" + strScheduled + "', 'DD/MM/YYYY'), scheduled_time = '" + strScheduledTime + "' where trxid = '" + strTrxId + "'";
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

        public string ShowDataWhatsappGroup(string strTrxId)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select * from trx_whatsapp_message_group where trxid = '" + strTrxId + "'";
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

        #endregion

        #region Reply

        public void InsertWhatsappReply(string strSender, string strReply, string strTemplateReply)
        {
            try
            {
                OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
                OracleCommand DBCommand;
                DBConnection.Open();

                var Query = "insert into trx_whatsapp_reply(sender_id, message_reply, template_reply, create_date) values('" + strSender + "','" + strReply + "','" + strTemplateReply + "', sysdate)";
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
        public void DeleteWhatsappReply(string strSender, string strReply, string strTemplateReply)
        {
            try
            {
                OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
                OracleCommand DBCommand;
                DBConnection.Open();

                var Query = "delete from trx_whatsapp_reply where sender_id = '" + strSender + "' and message_reply = '" + strReply + "' and template_reply = '" + strTemplateReply + "'";
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

        #endregion
    }
}