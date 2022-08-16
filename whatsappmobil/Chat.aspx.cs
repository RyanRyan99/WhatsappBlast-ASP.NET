using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
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
    public partial class Chat : System.Web.UI.Page
    {
        Function fcn = new Function();
        FunctionChat fchat = new FunctionChat();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindTemplateMessage();
            }
        }
        protected void gvContact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvContact, "Select$" + e.Row.RowIndex);
                e.Row.Attributes["style"] = "cursor:pointer";
            }
        }
        protected void gvContact_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = gvContact.SelectedRow.RowIndex;
            string number = gvContact.SelectedRow.Cells[0].Text;
            string name = gvContact.SelectedRow.Cells[1].Text;
            hiddenNumberContact.Value = number;
            Session["number"] = number;
            ltlCc.Text = "";
            lblHeaderChat.Text = name;
            ShowChat(number);
        }
        public void ShowChat(string strNumber)
        {
            try
            {
                SenderGetChat senderId = new SenderGetChat { sender = ddlDevices.SelectedValue, number = strNumber };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/getchatbyid");
                    var response = client.PostAsJsonAsync("", senderId).Result;
                    Thread.Sleep(500);
                    if (response.IsSuccessStatusCode)
                    {
                        var getresponse = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<IEnumerable<Root>>("[" + getresponse + "]");
                        foreach (var dd in result)
                        {
                            foreach (var ii in dd.response)
                            {
                                string message = "";
                                string fromme = ii.fromMe.ToString();
                                string type = ii.type;
                                string hasQuotedMsg = ii.hasQuotedMsg.ToString(); //Untuk Status Reply
                                string styles = "";
                                string strbadges = "";
                                string Pmessage = "";

                                string CSSSTYLE = "";
                                string MESSAGE = "";
                                string ISQUOTES = "";
                                if (fromme == "False")
                                {
                                    CSSSTYLE = @"from-them";
                                }
                                else
                                {
                                    CSSSTYLE = @"from-me";
                                }
                                if (type == "image")
                                {
                                    if (fromme == "False")
                                    {
                                        MESSAGE = @"<div class='fromreplythem'><div class='row'><div class='col-md-12'><img src='data:image/png;base64," + ii._data.body + "' width='150' height='150' /></div><div class='col-md-12'><span>" + ii.body + "</span></div></div></div>";
                                    }
                                    else
                                    {
                                        MESSAGE = @"<div class='fromreply'><div class='row'><div class='col-md-12'><img src='data:image/png;base64," + ii._data.body + "' width='150' height='150' /></div><div class='col-md-12'><span>" + ii.body + "</span></div></div></div>";
                                    }
                                }
                                if (type == "chat")
                                {
                                    MESSAGE = @"<p class='" + CSSSTYLE + "'>" + ii.body + "</p>";
                                }
                                if (type == "revoked")
                                {
                                    MESSAGE = "<p class='" + CSSSTYLE + "'>Pesan Telah Dihapus</p>";
                                }
                                if (hasQuotedMsg == "True")
                                {
                                    string FromReplay = ii._data.quotedMsg.body;
                                    if (fromme == "False")
                                    {
                                        ISQUOTES = @"<div class='fromreplythem'><div class='row'><div class='col-md-12'><span>" + FromReplay + "</span></div><div class='col-md-12'><hr/><span>" + ii.body + "</span></div></div></div>";
                                    }
                                    else
                                    {
                                        ISQUOTES = @"<div class='fromreply'><div class='row'><div class='col-md-12'><span>" + FromReplay + "</span></div><div class='col-md-12'><hr/><span>" + ii.body + "</span></div></div></div>";
                                    }
                                }
                                else
                                {
                                    if(MESSAGE.Length > 800)
                                    {
                                        ISQUOTES = "error : PESAN TIDAK DAPAT DI TAMPILKAN";
                                    }
                                    else
                                    {
                                        ISQUOTES = MESSAGE;
                                    }
                                }
                                ltlCc.Text += ISQUOTES;
                            }
                        }
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert();", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex + "')", true);
            }
        }
        protected void btnSent_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadmedia.HasFile)
                {
                    string filename = Path.GetFileName(uploadmedia.PostedFile.FileName);
                    string changename = Guid.NewGuid() + filename;
                    uploadmedia.PostedFile.SaveAs(Server.MapPath("~/Media/" + changename));
                    string filelocation = Server.MapPath("Media/" + changename);
                    using (var client = new HttpClient())
                    {
                        SenderChatPersonalMedia senderchatpersonalmedia = new SenderChatPersonalMedia { sender = ddlDevices.SelectedValue, number = hiddenNumberContact.Value, caption = txtSent.Text, file = filelocation };
                        client.BaseAddress = new Uri("http://192.168.100.1:9001/send-media");
                        var response = client.PostAsJsonAsync("", senderchatpersonalmedia).Result;
                        Thread.Sleep(500);
                        if (response.IsSuccessStatusCode)
                        {
                            File.Delete(filelocation); //Delete File
                        }
                        else
                        {
                            if (response.StatusCode.ToString() == "422")
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error')", true);
                            }
                        }
                    }
                }
                else
                {
                    if (txtSent.Text != "")
                    {
                        using (var client = new HttpClient())
                        {
                            SenderChatPersonal SenderSingleChat = new SenderChatPersonal { sender = ddlDevices.SelectedValue, number = hiddenNumberContact.Value, message = txtSent.Text };
                            client.BaseAddress = new Uri("http://192.168.100.1:9001/send-message");
                            var response = client.PostAsJsonAsync("", SenderSingleChat).Result;
                            Thread.Sleep(500);
                            if (response.IsSuccessStatusCode)
                            {
                                txtSent.Text = "";
                                ltlCc.Text = "";
                                Thread.Sleep(400);
                                ShowChat(hiddenNumberContact.Value);
                            }
                            else
                            {
                                if (response.StatusCode.ToString() == "422")
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Error')", true);
                                }
                            }
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Isi Pesan')", true);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnSearchContact_Click(object sender, EventArgs e)
        {
            PrivateChatBind(ddlDevices.SelectedValue);
        }
        protected void ddlTemplateMessage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTemplateMessage.SelectedValue != "" && ddlTemplateMessage.SelectedValue != "Add")
            {
                txtSent.Text += ddlTemplateMessage.SelectedValue;
            }
            if (ddlTemplateMessage.SelectedValue == "Add")
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModaladdTemplate').modal('show');", true);
                ddlTemplateMessage.ForeColor = Color.White;
                ddlTemplateMessage.BackColor = Color.ForestGreen;
            }
            else
            {
                ddlTemplateMessage.ForeColor = Color.Black;
                ddlTemplateMessage.BackColor = Color.White;
            }
        }
        protected void btnaddtemplate_Click(object sender, EventArgs e)
        {
            if (txtTemplate.Text != "")
            {
                string strCheckData = fcn.getDataTable(@"select * from trx_whatsapp_template where template_value = '" + txtTemplate.Text + "'");
                if (strCheckData == "")
                {
                    fchat.InsertTemplateMessage(txtTemplate.Text);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Template sudah ada')", true);
                }
            }
            else
            {

            }
        }
        private void BindTemplateMessage()
        {
            OracleConnection connection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]);
            string q = @"select * from trx_whatsapp_template";
            OracleDataAdapter adapter = new OracleDataAdapter(q, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            ddlTemplateMessage.DataSource = dt;
            ddlTemplateMessage.DataTextField = "template_desc";
            ddlTemplateMessage.DataValueField = "template_value";
            ddlTemplateMessage.DataBind();
            ddlTemplateMessage.Items.Insert(0, new ListItem("Pilih", ""));
            ddlTemplateMessage.Items.Insert(1, new ListItem("Tambah Template", "Add"));
        }
        protected void ddlDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrivateChatBind(ddlDevices.SelectedValue);
            PrivateChat();
        }
        private void PrivateChat()
        {
            try
            {
                SenderPrivateChat senderId = new SenderPrivateChat { sender = ddlDevices.SelectedValue };
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/getchat");
                    var response = client.PostAsJsonAsync("", senderId).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var getresponse = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<IEnumerable<RootPrivateChat>>("[" + getresponse + "]");
                        foreach (var dd in result)
                        {
                            foreach (var ii in dd.response)
                            {
                                string isgroup = ii.isGroup.ToString();
                                if (isgroup == "False")
                                {
                                    string checkdata = fcn.getDataTable(@"select * from trx_whatsapp_p_contact where whatsapp_number = '" + ii.id.user + "'");
                                    if (checkdata == "")
                                    {
                                        fchat.InsertPrivateContact(ii.id.user.ToString(), ii.name, ii.isGroup.ToString(), ii.timestamp.ToString(), ii.unreadCount.ToString(), ddlDevices.SelectedValue);
                                    }
                                    else
                                    {
                                        fchat.UpdatePrivateContact(ii.id.user.ToString(), ii.name, ii.isGroup.ToString(), ii.timestamp.ToString(), ii.unreadCount.ToString(), ddlDevices.SelectedValue);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "warningsalert();", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void PrivateChatBind(string strsenderid)
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    if (txtSearchContact.Text.Trim() == "")
                    {
                        strSearch = "%%";
                    }
                    else
                    {
                        strSearch = "%" + txtSearchContact.Text.Trim() + "%";
                    }
                    cmd.CommandText = "select whatsapp_number, whatsapp_user, whatsapp_timestamp, unread_count from trx_whatsapp_p_contact where sender_id = '" + strsenderid + "' and whatsapp_number like :whatsapp_number order by whatsapp_timestamp desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("whatsapp_number", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvContact.DataSource = dt;
                        gvContact.DataBind();
                    }
                }
            }
        }
    }
}