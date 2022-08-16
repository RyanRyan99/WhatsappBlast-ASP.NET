using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;

namespace whatsappmobil
{
    public partial class Singlechat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlSessionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderReply sen = new SenderReply { sender = ddlSessionId.SelectedValue };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/checksession");
                    var response = client.PostAsJsonAsync("", sen).Result;
                    if (response.IsSuccessStatusCode)
                    {

                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Device Tidak Ditemukan', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('ERROR', 'error');", true);
            }
        }

        protected void btnSendText_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderSingleChat SenderSingleChat = new SenderSingleChat { sender = ddlSessionId.SelectedValue, number = txtTO.Text, message = txtMESSAGE.Text };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-message");
                    var response = client.PostAsJsonAsync("", SenderSingleChat).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Sukses', 'success');", true);
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Error', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('"+ex+"', 'error');", true);
            }
        }

        protected void ddlSessionIdMedia_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderReply sen = new SenderReply { sender = ddlSessionIdMedia.SelectedValue };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/checksession");
                    var response = client.PostAsJsonAsync("", sen).Result;
                    if (response.IsSuccessStatusCode)
                    {

                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Device Tidak Ditemukan', 'error');", true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('ERROR', 'error');", true);
            }
        }

        protected void btnSendMedia_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileUplad.HasFile)
                {
                    string filename = Path.GetFileName(fileUplad.PostedFile.FileName);
                    string changeName = Guid.NewGuid() + filename;
                    fileUplad.PostedFile.SaveAs(Server.MapPath("~/media/" + changeName));

                    string strFileLocation = Server.MapPath("media/" + changeName);
                    using (var client = new HttpClient())
                    {
                        SenderSingleChatMedia SenderSingleChatMedia = new SenderSingleChatMedia { sender = ddlSessionIdMedia.SelectedValue, number = txtTOMedia.Text, caption = txtMESSAGEMEDIA.Text, file = strFileLocation };
                        client.BaseAddress = new Uri("http://192.168.100.1:9001/send-media");
                        var response = client.PostAsJsonAsync("", SenderSingleChatMedia).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Sukses', 'success');", true);
                            File.Delete(strFileLocation); //Delete File
                        }
                        else
                        {
                            if (response.StatusCode.ToString() == "422")
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Error', 'error');", true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('ERROR', 'error');", true);
            }
        }
    }
}