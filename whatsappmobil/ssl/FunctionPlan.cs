using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace whatsappmobil.ssl
{
    public class FunctionPlan
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];

        public DataSet ExecuteQuery(string strQuery)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleDataAdapter DBAdapter;
            OracleCommand DBCommand;
            DataSet ResultDataSet = new DataSet();
            DBConnection.Open();

            DBCommand = new OracleCommand(strQuery, DBConnection);
            DBCommand.CommandType = CommandType.Text;
            DBAdapter = new OracleDataAdapter(DBCommand);
            DBAdapter.Fill(ResultDataSet);
            DBConnection.Close();

            return ResultDataSet;
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

        public void UpdateSuccessMessage(string senderid, string number, string strid)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set status_session = '1' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'";
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
        public void UpdateFailedMessage(string senderid, string number, string strid)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_message set session_notic = 'number is not registered', status_session = '2' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'";
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

        public void UpdateSuccessHeader(string trxid)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_header set status_session = '1' where trxid = '"+trxid+"'";
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

        public void InsertPlan(string strPlanId, string strPlanTitle, string strInterval, string strBranch, string strMessageContent, string strIsSurvey, string strBranchName, string strSentDate, string strBranchFrom, string strIseMedia, string strMediaName, string strScheduled, string strScheduledTime, string strScheduledMessage, string strScheduledMessageTime, string strSessionId, string strCreateBy)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_plan(id_plan, plan_title, interval, branch_id, create_date, message_content, is_survey, branch_name, session_devices, sent_date, plan_status, branch_from, is_media, media_name, schedule_date, schedule_time, scheduled_message, scheduled_message_time, create_by) values('" + strPlanId + "','" + strPlanTitle + "','" + strInterval + "','" + strBranch + "', sysdate, '" + strMessageContent + "', '" + strIsSurvey + "', '" + strBranchName + "', '"+strSessionId+"', to_date('" + strSentDate + "', 'DD/MM/YYYY'), 0, '" + strBranchFrom + "', '" + strIseMedia + "', '" + strMediaName + "', to_date('" + strScheduled + "', 'DD/MM/YYYY'), '" + strScheduledTime + "', to_date('" + strScheduledMessage + "'), '" + strScheduledMessageTime + "', '"+strCreateBy+"')";
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

        public void UpdatePlan(string strPlanId, string strPlanTitle, string strInterval, string strBranch, string strMessageContent, string strBranchName, string strIseMedia, string strMediaName, string strScheduledMessage, string strScheduledMessageTime, string strSessionId, string strModifiedBy)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "update trx_whatsapp_plan set plan_title = '"+strPlanTitle+"', interval = '"+strInterval+"', branch_id = '"+strBranch+"', message_content = '"+strMessageContent+"', branch_name = '"+strBranchName+"', session_devices = '"+strSessionId+"', is_media = '"+strIseMedia+"', media_name = '"+strMediaName+"', scheduled_message = '"+strScheduledMessage+"', scheduled_message_time = '"+strScheduledMessageTime+"', modified_date = sysdate, modified_by = '"+strModifiedBy+"' where id_plan = '"+strPlanId+"'";
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
        public void InsertPlanFilter(string strKey, string strOperator, string strValue, string strPlanId)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_plan_filter(kunci, operator, isi, id_plan, create_date) values('" + strKey + "','" + strOperator + "','" + strValue + "','" + strPlanId + "',sysdate)";
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
        public void DeletePlan(string strIdPlan)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "delete from trx_whatsapp_plan where id_plan = '" + strIdPlan + "'";
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
        public void DeletePlanOperator(string strIdPlan)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "delete from trx_whatsapp_plan_filter where id_plan = '" + strIdPlan + "'";
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

        public string GetPlanFilterSumberData(string strPlanId)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select * from trx_whatsapp_plan_filter where id_plan = '" + strPlanId + "' and kunci = 'SUMBER DATA'";
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

        public string GetPlanFilter(string strQuery)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = strQuery;
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

        public string GetPlanView(string strPlanId)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select id_plan, plan_title, decode(interval, '1', 'Daily', '2', 'Weekly', '3', 'Monthly', '4', 'Custom') interval, branch_id, create_date, message_content, branch_name, to_date(sent_date, 'DD/MM/YYYY') as sent_date, plan_status, to_date(schedule_date, 'DD/MM/YYYY') schedule_date, schedule_time, session_devices from trx_whatsapp_plan where id_plan = '" + strPlanId + "'";
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
        public string GetPlanEdit(string strPlanId)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = "select * from trx_whatsapp_plan where id_plan = '" + strPlanId + "'";
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
        public string DataCountRow(string strquery)
        {
            string strResult = "";
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            String StrQuery = strquery;
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

        public void InsertGeneratePlan(string strSenderId, string strSentDate, string strTypeMessage, string strMessageTitle, string strNumber, string strMessageContent, string strTrxId, string strDocRef)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_message(sender_id, sent_date, type_message, message_title, wa_number, message_content, status_session, trxid, doc_ref, create_date) " +
                "values ('" + strSenderId + "','" + strSentDate + "','" + strTypeMessage + "','" + strMessageTitle + "','" + strNumber + "','" + strMessageContent + "', '0', '" + strTrxId + "', '" + strDocRef + "', sysdate)";
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

        public void InsertTrxMessageHeader(string strTrxId, string strTitleContent, string strWaDate, string strSenderId, string strWaMedia, string strIsMedia, string strScheduledDate, string strScheduledTime, string strIdRef, string strCreateBy)
        {
            OracleConnection DBConnection = new OracleConnection(ConnectionStringH2);
            OracleCommand DBCommand;
            DBConnection.Open();

            var Query = "insert into trx_whatsapp_header(TRXID,WA_HEADER,WA_DATE,STATUS_SESSION,SENDER_ID,WA_MEDIA,IS_MEDIA,CREATE_DATE, SCHEDULED_DATE, SCHEDULED_TIME, ID_REF, CREATE_BY) values('" + strTrxId + "','" + strTitleContent + "',to_date('" + strWaDate + "','DD/MM/YYYY'),'0','" + strSenderId + "', '" + strWaMedia + "', '" + strIsMedia + "', sysdate, to_date('" + strScheduledDate + "','DD/MM/YYYY'), '" + strScheduledTime + "', '"+strIdRef+"', '"+strCreateBy+"')";
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