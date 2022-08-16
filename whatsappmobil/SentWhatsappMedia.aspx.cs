using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class SentWhatsappMedia : System.Web.UI.Page
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        Function fcn = new Function();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void gvSentWhatsappMedia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                string strKey = e.CommandArgument.ToString();
                BindDataDetail(strKey.Split('|')[0]);
                hiddenPlanId.Value = strKey.Split('|')[0];
                lblHeaderName.Text = strKey.Split('|')[1];
                imgWhatsappMediaView.ImageUrl = "Media/" + strKey.Split('|')[2];
            }
            if (e.CommandName == "SentAll")
            {
                string strKey = e.CommandArgument.ToString();
                SentAll(strKey);
            }
            if (e.CommandName == "Delete")
            {
                string strKey = e.CommandArgument.ToString();
                fcn.DeleteMessageHeader(strKey);
                fcn.DeleteMessageDetail(strKey);
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void gvSentWhatsappMedia_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSentWhatsappMedia.PageIndex = e.NewPageIndex;
            BindData();
        }

        protected void gvSentWhatsappMedia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvSentWhatsappMedia.Rows)
            {
                Label label = (Label)row.FindControl("lblstatus");
                if (label.Text == "waiting for schedule")
                {
                    label.CssClass = "gradient-custom-card-1";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 150;
                    label.ToolTip = "Menunggu Jadwal";

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
                    label.Width = 70;
                }
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalView').modal('show');", true);
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
                string Number = strKey.Split('|')[0];
                string Message = strKey.Split('|')[1];
                string SenderId = strKey.Split('|')[2];
                string TrxId = strKey.Split('|')[3];
                string Media = strKey.Split('|')[4];

                string strFilePath = Server.MapPath("Media/" + Media);
                SentRow(SenderId, Number, Message, TrxId, strFilePath);
            }
        }

        protected void gvView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvView.PageIndex = e.NewPageIndex;
            BindDataDetail(hiddenPlanId.Value);
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
                    cmd.CommandText = "select trxid, wa_header, wa_date, decode(status_session, '0', 'waiting for schedule', '1', 'Close') as status_session, sender_id, scheduled_date, scheduled_time, wa_media from trx_whatsapp_header where is_media = '1' and wa_header like :waheader order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("waheader", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvSentWhatsappMedia.DataSource = dt;
                        gvSentWhatsappMedia.DataBind();
                    }
                }
            }
        }

        private void BindDataDetail(string strTrxId)
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand("select trx_whatsapp_message.sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(trx_whatsapp_message.status_session, '0', 'Open', '1', 'Sent', '2', 'Failed') status_session, session_notic, trx_whatsapp_message.trxid, trx_whatsapp_header.wa_media from trx_whatsapp_message inner join trx_whatsapp_header on trx_whatsapp_message.trxid = trx_whatsapp_header.trxid where trx_whatsapp_message.trxid = '" + strTrxId + "'"))
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

        private void SentAll(string strTrxId)
        {
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringH2"];
            OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            oracleConnection.Open();

            List<string> SenderId = new List<string>();
            List<string> Number = new List<string>();
            List<string> Message = new List<string>();
            List<string> Media = new List<string>();
            List<string> PushName = new List<string>();

            string strGetData = "";
            OracleCommand oracleCommand = null;

            strGetData = "select trx_whatsapp_message.sender_id, wa_number, message_content, trx_whatsapp_header.wa_media, push_name from trx_whatsapp_message inner join trx_whatsapp_header on trx_whatsapp_message.trxid = trx_whatsapp_header.trxid where trx_whatsapp_message.trxid = '" + strTrxId + "' and trx_whatsapp_message.status_session = '0'";
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
                        Media.Add(dataReader.GetString(3));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(4))
                    {
                        PushName.Add(dataReader.GetString(4));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
                dataReader.Close();
                for (int i = 0; i < Number.Count; i++)
                {
                    string strFilePath = Server.MapPath("Media/" + Media[i]);

                    using (var client = new HttpClient())
                    {
                        Sender sen = new Sender { sender = SenderId[i], number = Number[i], caption = Message[i], file = strFilePath };
                        client.BaseAddress = new Uri("http://192.168.100.1:9001/");
                        var response = client.PostAsJsonAsync("send-media", sen).Result;
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
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Pesan sudah terkirim semua yaaa')", true);
            }
        }

        private void SentRow(string strSenderId, string strNumber, string strMessage, string strTrxId, string strFile)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    Sender sen = new Sender { sender = strSenderId, number = strNumber, caption = strMessage, file = strFile };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/");
                    var response = client.PostAsJsonAsync("send-media", sen).Result;
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
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex + "')", true);
            }
        }
    }
}