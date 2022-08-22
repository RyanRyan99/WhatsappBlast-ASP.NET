using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.ssl;
using whatsappmobil.sender;
using System.Drawing;
using Quartz;
using Quartz.Impl;
using whatsappmobil.scheduler;
using System.Threading.Tasks;
using System.IO;

namespace whatsappmobil
{
    public partial class SentWhatsapp : System.Web.UI.Page
    {
        Function fcn = new Function();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ddlViewAllBranch.SelectedValue == "false")
                {
                    BindData();
                }
                else
                {
                    BindDataViewAllBranch();
                }
            }
        }
       
        private void BindData()
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    if (txtSearch.Text.Trim() == "")
                    {
                        strSearch = "%%";
                    }
                    else
                    {
                        strSearch = "%" + txtSearch.Text.Trim() + "%";
                    }
                    cmd.CommandText = "select trxid, wa_header, wa_date, decode(status_session, '0', 'waiting for action', '1', 'Close') as status_session, sender_id, scheduled_date, scheduled_time, is_media, decode(is_media, '0', 'Text', '1', 'Media') type_message, wa_media from trx_whatsapp_header where create_by = '"+Convert.ToString(Session["UserID"])+"' and wa_header like :waheader order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("waheader", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvSentWhatsapp.DataSource = dt;
                        gvSentWhatsapp.DataBind();
                    }
                }
            }
        }
        private void BindDataViewAllBranch()
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    if (txtSearch.Text.Trim() == "")
                    {
                        strSearch = "%%";
                    }
                    else
                    {
                        strSearch = "%" + txtSearch.Text.Trim() + "%";
                    }
                    cmd.CommandText = "select trxid, wa_header, wa_date, decode(status_session, '0', 'waiting for action', '1', 'Close') as status_session, sender_id, scheduled_date, scheduled_time, is_media, decode(is_media, '0', 'Text', '1', 'Media') type_message, wa_media from trx_whatsapp_header where wa_header like :waheader order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("waheader", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvSentWhatsapp.DataSource = dt;
                        gvSentWhatsapp.DataBind();
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataViewAllBranch();
            }
        }

        protected void gvSentWhatsapp_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                string getKey = e.CommandArgument.ToString();
                BindDataDetail(getKey.Split('|')[0]);
                hiddenPlanId.Value = getKey.Split('|')[0];
                lblTitleContentView.Text = getKey.Split('|')[1];
                string ismedia = getKey.Split('|')[2];
                if(ismedia == "0")
                {
                    colsrc.Visible = false;
                }
                else
                {
                    colsrc.Visible = true;
                }
                imgWhatsappMediaView.ImageUrl = "Media/" + getKey.Split('|')[3];
                HiddenMediaName.Value = getKey.Split('|')[3];
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalView').modal('show');", true);
            }
            if (e.CommandName == "SentAll")
            {
                string getKeySent = e.CommandArgument.ToString();
                string trxid = getKeySent.Split('|')[0];
                string media = getKeySent.Split('|')[1];
                SentAll(trxid, media);
                //SentScheduled();
                Response.Redirect(Request.RawUrl, true);
            }
            if (e.CommandName == "Delete")
            {
                string getKeySent = e.CommandArgument.ToString();
                fcn.DeleteMessageHeader(getKeySent);
                fcn.DeleteMessageDetail(getKeySent);
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void gvSentWhatsapp_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvSentWhatsapp.Rows)
            {
                Label label = (Label)row.FindControl("lblStatus");
                Label type = (Label)row.FindControl("lblTypeMessage");
                if(type.Text == "Text")
                {
                    type.CssClass = "gradient-custom-card-1";
                    type.ForeColor = Color.FromKnownColor(KnownColor.White);
                    type.Width = 50;
                }
                if(type.Text == "Media")
                {
                    type.CssClass = "gradient-custom-card-2";
                    type.ForeColor = Color.FromKnownColor(KnownColor.White);
                    type.Width = 50;
                }
                if (label.Text == "waiting for action")
                {
                    label.CssClass = "gradient-custom-card-1";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 150;

                    LinkButton button = (LinkButton)row.FindControl("btnSentAll");
                    button.Enabled = true;
                    button.CssClass = "btn btn-sm gradient-custom-button-1 text-white";

                    LinkButton button2 = (LinkButton)row.FindControl("btnDelete");
                    button2.Enabled = true;
                    button2.CssClass = "btn btn-sm gradient-custom-button-2 text-white";
                }
                if (label.Text == "Close")
                {
                    LinkButton button = (LinkButton)row.FindControl("btnSentAll");
                    button.Enabled = false;
                    button.BackColor = Color.FromKnownColor(KnownColor.DarkGray);

                    LinkButton button2 = (LinkButton)row.FindControl("btnDelete");
                    button2.Enabled = false;
                    button2.BackColor = Color.FromKnownColor(KnownColor.DarkGray);

                    label.CssClass = "gradient-custom-button-2";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 50;
                }
            }
        }

        protected void gvSentWhatsapp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSentWhatsapp.PageIndex = e.NewPageIndex;
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataViewAllBranch();
            }
        }

        //View
        protected void btnView_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalView').modal('show');", true);
        }
        protected void ChkShowImages_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkShowImages.Checked)
            {
                imgWhatsappMediaView.Visible = true;
            }
            else
            {
                imgWhatsappMediaView.Visible = false;
            }
        }

        protected void gvView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvView.Rows)
            {
                string Status = row.Cells[6].Text;
                LinkButton button = (LinkButton)row.FindControl("btnSentRow");
                if (Status == "Sent")
                {
                    button.Enabled = false;
                    button.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
                else
                {
                    button.Enabled = true;
                    button.CssClass = "btn btn-sm gradient-custom-button-1 text-white";
                }
            }
        }

        protected void gvView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SentRow")
            {
                string strKey = e.CommandArgument.ToString();
                string number = strKey.Split('|')[0];
                string message = strKey.Split('|')[1];
                string senderId = strKey.Split('|')[2];
                string trxid = strKey.Split('|')[3];
                string pushname = strKey.Split('|')[4];
                SentRow(senderId, number, message, trxid, pushname, HiddenMediaName.Value);
            }
        }

        protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvView.PageIndex = e.NewPageIndex;
            BindDataDetail(hiddenPlanId.Value);
        }

        private void BindDataDetail(string strTrxId)
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand("select sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(status_session, '0', 'Open', '1', 'Sent', '2', 'Failed') status_session, session_notic, trxid, push_name from trx_whatsapp_message where trxid = '" + strTrxId + "'"))
                {
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvView.DataSource = dt;
                            gvView.DataBind();
                        }
                    }
                }
            }
        }

        private int RandomTime()
        {
            Random rnd = new Random();
            int number = rnd.Next(1000, 30000);
            return number;
        }

        private void SentScheduled()
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]))
                {
                    OracleCommand command = new OracleCommand("select * from trx_whatsapp_header where status_session = '0'", connection);
                    OracleDataAdapter da = new OracleDataAdapter(command);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0) //Edited
                    {
                        IScheduler schedulerMessage = StdSchedulerFactory.GetDefaultScheduler().Result;
                        schedulerMessage.Start();
                        IJobDetail jobMessage = JobBuilder.Create<ScheduledMessage>().WithIdentity("job1", "group1").Build();
                        ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("trigger5", "group1")
                        .StartAt(DateBuilder.FutureDate(3, IntervalUnit.Minute)) // use DateBuilder to create a date in the future
                        .ForJob(jobMessage) // identify job with its JobKey
                        .Build();
                        schedulerMessage.ScheduleJob(jobMessage, trigger);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Data OPEN tidak ditemukan', 'warning');", true);
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void SentAll(string strTrxId, string media)
        {
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringH2"];
            OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            oracleConnection.Open();
            try
            {
                List<string> SenderId = new List<string>();
                List<string> Number = new List<string>();
                List<string> Message = new List<string>();
                List<string> PushName = new List<string>();

                string strGetData = "";
                OracleCommand oracleCommand = null;

                strGetData = "select sender_id, wa_number, message_content, push_name from trx_whatsapp_message where trxid = '" + strTrxId + "' and status_session = '0'";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                        {
                            SenderId.Add(dataReader.GetString(0));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(1))
                        {
                            Number.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(2))
                        {
                            Message.Add(dataReader.GetString(2));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            PushName.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < Number.Count; i++)
                    {
                        try
                        {
                            if (media == "")
                            {
                                using (var client = new HttpClient())
                                {
                                    Sender sen = new Sender { sender = SenderId[i], number = Number[i], message = Message[i] };
                                    client.BaseAddress = new Uri("http://192.168.100.1:9001/");
                                    var response = client.PostAsJsonAsync("send-message", sen).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        fcn.UpdateStatusHeader(strTrxId);
                                        fcn.UpdateStatusMessage(strTrxId, Number[i], "1");
                                    }
                                    else
                                    {
                                        if (response.StatusCode.ToString() == "422")
                                        {
                                            fcn.UpdateSessionNotic(strTrxId, Number[i], "The number is not registered");
                                        }
                                    }
                                    int Random = RandomTime();
                                    Thread.Sleep(Random);
                                }
                            }
                            else
                            {
                                using (var client = new HttpClient())
                                {
                                    string filefile = Path.Combine(HttpRuntime.AppDomainAppPath, @"Media\" + media);
                                    SenderSingleChatMedia SenderSingleChatMedia = new SenderSingleChatMedia { sender = SenderId[i], number = Number[i], caption = Message[i], file = filefile };
                                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-media");
                                    var response = client.PostAsJsonAsync("", SenderSingleChatMedia).Result;
                                    if (response.IsSuccessStatusCode)
                                    {
                                        fcn.UpdateStatusHeader(strTrxId);
                                        fcn.UpdateStatusMessage(strTrxId, Number[i], "1");
                                    }
                                    else
                                    {
                                        if (response.StatusCode.ToString() == "422")
                                        {
                                            fcn.UpdateSessionNotic(strTrxId, Number[i], "The number is not registered");
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Terjadi kesalahan pada session devices')", true);
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pesan sudah terkirim semua yaaa')", true);
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
        private void SentRow(string strSenderId, string strNumber, string strMessage, string strTrxId, string strpushname, string strMedia)
        {
            if (strMedia == "")
            {
                using (var client = new HttpClient())
                {
                    Sender sen = new Sender { sender = strSenderId, number = strNumber, message = strMessage };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-message");
                    var response = client.PostAsJsonAsync("", sen).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        fcn.UpdateStatusHeader(strTrxId);
                        fcn.UpdateStatusMessage(strTrxId, strNumber, "1");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            fcn.UpdateSessionNotic(strTrxId, strNumber, "The number is not registered");
                        }
                    }
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    //string filefile = Path.Combine(HttpRuntime.AppDomainAppPath, @"Media\" + strMedia);
                    string filefile = Server.MapPath("Media/" + strMedia);
                    SenderSingleChatMedia SenderSingleChatMedia = new SenderSingleChatMedia { sender = strSenderId, number = strNumber, caption = strMessage, file = filefile };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-media");
                    var response = client.PostAsJsonAsync("", SenderSingleChatMedia).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        fcn.UpdateStatusHeader(strTrxId);
                        fcn.UpdateStatusMessage(strTrxId, strNumber, "1");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            fcn.UpdateSessionNotic(strTrxId, strNumber, "The number is not registered");
                        }
                    }
                }
            }
        }

        protected void btnSentAllScheduled_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnSentAllSecound_Click(object sender, EventArgs e)
        {
            SentScheduled();
        }

        protected void ddlViewAllBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataViewAllBranch();
            }
        }
    }
}