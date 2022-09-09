using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using whatsappmobil.ssl;
using whatsappmobil.sender;
using System.Threading;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace whatsappmobil.scheduler
{
    public class ScheduledMessage : IJob
    {
        FunctionPlan plan = new FunctionPlan();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        public async Task Execute(IJobExecutionContext context)
        {
            ScheduledMessagePlan();
        }
        private int RandomTime()
        {
            Random rnd = new Random();
            int number = rnd.Next(5000, 20000);
            return number;
        }
        private void ScheduledMessagePlan()
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();
            try
            {
                List<string> id = new List<string>();
                List<string> title = new List<string>();
                List<string> status = new List<string>();
                List<string> media = new List<string>();
                List<string> ismedia = new List<string>();
                List<string> scheduledate = new List<string>();
                List<string> scheduletime = new List<string>();

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select trxid, wa_header, wa_date, decode(status_session, '0', 'Open', '1', 'Close') as status_session, sender_id, to_char(scheduled_date, 'DD/MM/YYYY') scheduled_date, scheduled_time, is_media, nvl(wa_media, ' ') wa_media from trx_whatsapp_header where status_session = '0' order by create_date desc";
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
                            title.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            status.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(7))
                        {
                            ismedia.Add(dataReader.GetString(7));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(8))
                        {
                            media.Add(dataReader.GetString(8));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        DateTime today = DateTime.UtcNow.Date;
                        string datetime = today.ToString("dd/MM/yyyy");
                        string times = DateTime.Now.ToString("HH:mm");
                        string resultdate = datetime + " " + times;

                        //string scheduled = scheduledate[i];
                        //string time = scheduletime[i].Replace(":", ".");
                        //string result = scheduled + " " + time;

                        if (resultdate != "")
                        {
                            FetchMessage(id[i], media[i]);
                            plan.UpdateSuccessHeader(id[i]);
                            //plan.ExecuteQuery(@"update trx_whatsapp_header set status_session = '1' where trxid = '" + id[i] + "'");
                        }
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
        private void FetchMessage(string strid, string strmedia)
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();
            try
            {
                List<string> id = new List<string>();
                List<string> senderid = new List<string>();
                List<string> type = new List<string>();
                List<string> messagetitle = new List<string>();
                List<string> number = new List<string>();
                List<string> message = new List<string>();
                List<string> status = new List<string>();

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select * from trx_whatsapp_message where trxid = '" + strid + "' and status_session = '0'";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(7))
                        {
                            id.Add(dataReader.GetString(7));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(0))
                        {
                            senderid.Add(dataReader.GetString(0));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(2))
                        {
                            type.Add(dataReader.GetString(2));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            messagetitle.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(4))
                        {
                            number.Add(dataReader.GetString(4));
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
                        if (!dataReader.IsDBNull(6))
                        {
                            status.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    for (int i = 0; i < id.Count; i++)
                    {
                        if (strmedia == " ")
                        {
                            //ScheduleSendChat(senderid[i], number[i], message[i], id[i]);

                            //Using Baileys Plugin
                            SendingTextBaileys(senderid[i], number[i], message[i], id[i]);
                        }
                        else
                        {
                            string filefile = Path.Combine(HttpRuntime.AppDomainAppPath, @"Media\" + strmedia);
                            //ScheduleSendMedia(senderid[i], number[i], message[i], filefile, id[i]);

                            //Using Baileys Plugin
                            SendingMediaBaileys(senderid[i], number[i], message[i], strmedia, id[i]);
                        }
                        int Random = RandomTime();
                        Thread.Sleep(Random);
                    }
                }
                dataReader.Close();
                //oracleConnection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
            
        }
        private void ScheduleSendChat(string senderid, string number, string message, string strid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderSingleChat SenderSingleChat = new SenderSingleChat { sender = senderid, number = number, message = message };
                    client.BaseAddress = new Uri("http://36.67.190.179:9001/send-message");
                    var response = client.PostAsJsonAsync("", SenderSingleChat).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        plan.UpdateSuccessMessage(senderid, number, strid);
                        //plan.getDataTable(@"update trx_whatsapp_message set status_session = '1' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            plan.UpdateFailedMessage(senderid, number, strid);
                            //plan.getDataTable(@"update trx_whatsapp_message set session_notic = 'number is not registered', status_session = '2' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                plan.getDataTable(@"update trx_whatsapp_message set session_notic = '" + ex + "' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
            }
        }
        private void ScheduleSendMedia(string senderid, string number, string caption, string file, string strid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderSingleChatMedia SenderSingleChatMedia = new SenderSingleChatMedia { sender = senderid, number = number, caption = caption, file = file };
                    client.BaseAddress = new Uri("http://36.67.190.179:9001/send-media");
                    var response = client.PostAsJsonAsync("", SenderSingleChatMedia).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        plan.UpdateSuccessMessage(senderid, number, strid);
                        //plan.getDataTable(@"update trx_whatsapp_message set status_session = '1' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            plan.UpdateFailedMessage(senderid, number, strid);
                            //plan.getDataTable(@"update trx_whatsapp_message set session_notic = 'number is not registered', status_session = '2' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                plan.getDataTable(@"update trx_whatsapp_message set session_notic = '" + ex + "' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
            }
        }

        private void SendingTextBaileys(string senderid, string number, string message, string strid)
        {
            try
            {
                //If Number Not start with 62
                if (number.StartsWith("0"))
                {
                    number = "62" + number.Substring(1);
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/send?id="+senderid+"");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                request.Content = new StringContent("{\"receiver\":\""+number+"\",\"message\":{\"text\": \""+message+"\"}}", Encoding.UTF8, "application/json");
                client.SendAsync(request).ContinueWith(responseTask =>
                {
                    if (responseTask.Result.IsSuccessStatusCode)
                    {
                        plan.UpdateSuccessMessage(senderid, number, strid);
                    }
                    else
                    {
                        string getresponse = responseTask.Result.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                        foreach(var dd in result)
                        {
                            string messageAPI = dd.message;
                            if (messageAPI == "The receiver number is not exists.")
                            {
                                plan.UpdateFailedMessage(senderid, number, strid);
                            }
                            else
                            {

                            }
                        }
                    }
                });
            }
            catch(Exception ex)
            {

            }
        }

        private void SendingMediaBaileys(string senderid, string number, string caption, string file, string strid)
        {
            try
            {
                //If Number Not start with 62
                if (number.StartsWith("0"))
                {
                    number = "62" + number.Substring(1);
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/send-media?id="+senderid+"");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                request.Content = new StringContent("{\"receiver\":\""+number+"\",\"message\":{\"image\":{\"url\": \"http://36.67.190.179:8001/Media/"+file+" \"}, \"caption\": \""+caption+"\"}}", Encoding.UTF8, "application/json");
                client.SendAsync(request).ContinueWith(responseTask =>
                {
                    if (responseTask.Result.IsSuccessStatusCode)
                    {
                        plan.UpdateSuccessMessage(senderid, number, strid);
                    }
                    else
                    {
                        string getresponse = responseTask.Result.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("["+getresponse+"]");
                        foreach (var dd in result)
                        {
                            string messageAPI = dd.message;
                            if (messageAPI == "The receiver number is not exists.")
                            {
                                plan.UpdateFailedMessage(senderid, number, strid);
                            }
                            else
                            {

                            }
                        }
                    }
                });
            }
            catch(Exception ex)
            {

            }
        }
    }
}