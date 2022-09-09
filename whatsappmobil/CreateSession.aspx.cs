using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class CreateSession : System.Web.UI.Page
    {
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        FunctionChat fc = new FunctionChat();
        Function fn = new Function();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                //TestSend();
                //TestMedia();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalAdd').modal('show');", true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(ddlSessionId.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Nama Perangkat Kosong', 'warning');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalAdd').modal('show');", true);
                    return;
                }
                if(ddlIsLegecy.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Tipe Klien Belum Dipilih', 'warning');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalAdd').modal('show');", true);
                    return;
                }
                
                string strUser = Convert.ToString(Session["UserID"]);
                string checkdata = fn.getDataTable("select * from trx_whatsapp_session where session_id = '"+ddlSessionId.SelectedValue+"'");
                if(checkdata == "")
                {
                    fc.InsertSession(ddlSessionId.SelectedValue, ddlIsLegecy.SelectedValue, "0", strUser, txtSessDesc.Text);
                    Response.Redirect(Request.RawUrl, true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Session Devices Sudah Ada', 'warning');", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalAdd').modal('show');", true);
                    return;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void BindData()
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand("select session_id, is_legecy, decode(session_status, '0', 'Not Connected', '1', 'Connected...') session_status, session_description from trx_whatsapp_session"))
                {
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvListDevices.DataSource = dt;
                            gvListDevices.DataBind();
                        }
                    }
                }
            }
        }

        protected void gvListDevices_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName.ToString() == "barcode")
            {
                string eval = e.CommandArgument.ToString();
                string sessionid = eval.Split('|')[0];
                string isLegecy = eval.Split('|')[1];
                hiddenSessionId.Value = sessionid;
                AddSessionAPIBaileys(sessionid, isLegecy);
            }
            if(e.CommandName.ToString() == "connected")
            {
                string eval = e.CommandArgument.ToString();
                string sessionid = eval.Split('|')[0];
                string isLegecy = eval.Split('|')[1];
                CheckStatusConnected(sessionid);
            }
            if(e.CommandName.ToString() == "deleted")
            {
                string eval = e.CommandArgument.ToString();
                string sessionid = eval.Split('|')[0];
                string isLegecy = eval.Split('|')[1];
                fc.DeleteSession(sessionid);
                DeleteSessionBaileys(sessionid, isLegecy);
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void gvListDevices_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvListDevices.Rows)
            {
                Label SessionStatus = (Label)row.FindControl("lblSessionStatus");
                Label IsLegecy = (Label)row.FindControl("lblIsLegecy");

                if(SessionStatus.Text == "Not Connected")
                {
                    SessionStatus.CssClass = "gradient-custom-button-2 text-white";
                    SessionStatus.Width = 150;
                    SessionStatus.Style.Add("border-radius", "20px");
                }
                if(SessionStatus.Text == "Connected...")
                {
                    SessionStatus.CssClass = "gradient-custom-button-1 text-white";
                    SessionStatus.Width = 150;
                    SessionStatus.Style.Add("border-radius", "20px");
                }

                if(IsLegecy.Text == "false")
                {
                    IsLegecy.Text = "Multi-Device";
                    IsLegecy.CssClass = "gradient-custom-card-1 text-white";
                    IsLegecy.Width = 100;
                    IsLegecy.Style.Add("border-radius", "20px");
                }
                if(IsLegecy.Text == "true")
                {
                    IsLegecy.Text = "Normal";
                    IsLegecy.CssClass = "gradient-custom-card-1 text-white";
                    IsLegecy.Width = 100;
                    IsLegecy.Style.Add("border-radius", "20px");
                }
            }
        }

        private void AddSessionAPIBaileys(string SessionId, string isLegecy)
        {
            using (var client = new HttpClient())
            {
                AddSessionBaileys sessionBaileys = new AddSessionBaileys { id = SessionId, isLegacy = isLegecy };
                client.BaseAddress = new Uri("http://localhost:8000/sessions/add");
                var response = client.PostAsJsonAsync("", sessionBaileys).Result;
                if (response.IsSuccessStatusCode)
                {
                    string getresponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                    foreach (var dd in result)
                    {
                        string qr = dd.data.qr.ToString();
                        imgBarcode.Src = qr;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBarcode').modal('show');", true);
                    }
                }
                else
                {
                    string getresponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                    foreach(var dd in result)
                    {
                        string message = dd.message.ToString();
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('"+message+"', 'success');", true);
                    }
                }
            }
        }
        private void CheckStatusConnected(string strSessionId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/sessions/status/?id="+strSessionId+"");
                var response = client.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    string getresponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                    foreach (var dd in result)
                    {
                        string status = dd.data.status.ToString();
                        string success = dd.success.ToString();
                        if(success == "True")
                        {
                            if (status != "connected")
                            {
                                fc.UpdateSession("1", strSessionId);
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Connected', 'info');", true);
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Tidak Berhasil Menghubungkan', 'warning');", true);
                }
            }
        }
        private void DeleteSessionBaileys(string SessionId, string isLegecy)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/sessions/delete/?id="+SessionId+"&isLegacy="+isLegecy+"");
                var response = client.DeleteAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    string getresponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                    foreach (var dd in result)
                    {
                        string success = dd.success.ToString();
                        string message = dd.message.ToString();
                        if (success == "True")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('"+message+"', 'info');", true);
                        }
                    }
                }
                else
                {
                    
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8000/sessions/status/?id="+hiddenSessionId.Value+"");
                var response = client.GetAsync("").Result;
                if (response.IsSuccessStatusCode)
                {
                    string getresponse = response.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("[" + getresponse + "]");
                    foreach (var dd in result)
                    {
                        string status = dd.data.status.ToString();
                        string success = dd.success.ToString();
                        if (success == "True")
                        {
                            if(status != "connected")
                            {
                                fc.UpdateSession("1", hiddenSessionId.Value);
                                Response.Redirect(Request.RawUrl, true);
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Tidak Berhasil Menghubungkan', 'warning');", true);
                }
            }
        }

        private void TestSend()
        {
            string number = "08569711100333";
            string message = "Someone Open Form Create Session ;)";
            if (number.StartsWith("0"))
            {
                number = "62" + number.Substring(1);
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/send?id=BNJ01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent("{\"receiver\":\""+number+"\",\"message\":{\"text\": \""+message+"\"}}", Encoding.UTF8,"application/json");
            //client.SendAsync(request).ContinueWith(responseTask =>{Console.WriteLine("Response: {0}", responseTask.Result);});
            client.SendAsync(request).ContinueWith(responseTask => 
            { 
                if (responseTask.Result.IsSuccessStatusCode)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Send Message Success', 'success');", true);
                }
                else
                {
                    string getresponse = responseTask.Result.Content.ReadAsStringAsync().Result;
                    var result = JsonConvert.DeserializeObject<IEnumerable<RootSessionBaileys>>("["+getresponse+"]");
                    foreach(var dd in result)
                    {
                        string messageAPI = dd.message;
                        if(messageAPI == "The receiver number is not exists.")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Number is not exists', 'success');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Failed to send the message.', 'success');", true);
                        }
                    }
                }
            });
            
        }
        private void TestMedia()
        {
            string number = "085697111003";
            if (number.StartsWith("0"))
            {
                number = "62" + number.Substring(1);
            }
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/send-media?id=dadang");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
            request.Content = new StringContent("{\"receiver\":\"" + number + "\",\"message\":{\"image\":{\"url\": \"http://36.67.190.179:8001/Media/0582a9b9-7d73-4d37-a140-f2ee9d7da5f4Test-drive-HRV.jpg \"}, \"caption\": \"Halo There\"}}", Encoding.UTF8, "application/json");
            client.SendAsync(request).ContinueWith(responseTask =>
            {
                if (responseTask.Result.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            });
        }
    }
}