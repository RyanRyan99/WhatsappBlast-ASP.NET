using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using whatsappmobil.ssl;

namespace whatsappmobil.scheduler
{
    public class ScheduledPlanAfter : IJob
    {
        FunctionPlan plan = new FunctionPlan();
        public async Task Execute(IJobExecutionContext context)
        {
            LoopStatus();
        }
        private void LoopStatus()
        {
            OracleConnection oracleConnection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]);
            oracleConnection.Open();
            try
            {
                #region List
                List<string> id = new List<string>();
                List<string> plantitle = new List<string>();
                List<string> branch = new List<string>();
                List<string> message = new List<string>();
                List<string> planstatus = new List<string>();
                List<string> branchform = new List<string>();
                List<string> ismedia = new List<string>();
                List<string> medianame = new List<string>();
                List<string> scheduledate = new List<string>();
                List<string> scheduletime = new List<string>();
                List<string> schedulemessage = new List<string>();
                List<string> schedulemessagetime = new List<string>();
                List<string> sessiondevices = new List<string>();
                #endregion

                OracleCommand oracleCommand = null;
                string strGetData = @"select id_plan, plan_title, interval, branch_id, create_date, message_content, is_survey, branch_name, session_devices, sent_date, plan_status, branch_from, is_media, nvl(media_name, ' ') media_name, to_char(schedule_date, 'DD/MM/YYYY') schedule_date, schedule_time, to_char(scheduled_message, 'DD/MM/YYYY') scheduled_message, scheduled_message_time from trx_whatsapp_plan where plan_status = '1' order by create_date desc";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                        {
                            id.Add(dataReader.GetString(0));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(1))
                        {
                            plantitle.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(5))
                        {
                            message.Add(dataReader.GetString(5));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            branch.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(12))
                        {
                            ismedia.Add(dataReader.GetString(12));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(13))
                        {
                            medianame.Add(dataReader.GetString(13));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(14))
                        {
                            scheduledate.Add(dataReader.GetString(14));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(15))
                        {
                            scheduletime.Add(dataReader.GetString(15));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(16))
                        {
                            schedulemessage.Add(dataReader.GetString(16));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(17))
                        {
                            schedulemessagetime.Add(dataReader.GetString(17));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(8))
                        {
                            sessiondevices.Add(dataReader.GetString(8));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '0' where id_plan = '" + id[i] + "'");
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
    }
}