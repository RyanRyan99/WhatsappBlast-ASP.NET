using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class Autoreply : System.Web.UI.Page
    {
        Function fcn = new Function();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReplyBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ReplyBind();
        }

        protected void gvReply_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "btndelete")
            {
                string strEval = e.CommandArgument.ToString();
                string strSender = strEval.Split('|')[0].Trim();
                string strReply = strEval.Split('|')[1].Trim();
                string strTemplate = strEval.Split('|')[2].Trim();
                DeleteArrayJsonFile(strSender, strReply, strTemplate);
            }
        }

        protected void gvReply_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReply.PageIndex = e.NewPageIndex;
            ReplyBind();
        }

        protected void ddlSessionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                #region Whatsapp-Web.js
                //using (var client = new HttpClient())
                //{
                //    SenderReply sen = new SenderReply { sender = ddlSessionId.SelectedValue };
                //    client.BaseAddress = new Uri("http://192.168.100.1:9001/checksession");
                //    var response = client.PostAsJsonAsync("", sen).Result;
                //    if (response.IsSuccessStatusCode)
                //    {

                //    }
                //    else
                //    {
                //        if (response.StatusCode.ToString() == "422")
                //        {
                //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Session Tidak Ditemukan', 'error');", true);
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('"+ex+ "', 'error');", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlSessionId.SelectedValue == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Pilih Devices', 'info');", true);
                }
                if (txtReplyContent.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Pesan Reply', 'info');", true);
                }
                if (txtReplyDesc.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Pesan Template', 'info');", true);
                }
                else
                {
                    #region Baileys
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/insertreply");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                    request.Content = new StringContent("{\"id\":\""+ddlSessionId.SelectedValue+"\",\"content\": \""+txtReplyContent.Text+"\",\"isreply\": \""+txtReplyDesc.Text+"\"}", Encoding.UTF8, "application/json");
                    client.SendAsync(request).ContinueWith(responseTask =>
                    {
                        if (responseTask.Result.IsSuccessStatusCode)
                        {
                            fcn.InsertWhatsappReply(ddlSessionId.SelectedValue, txtReplyContent.Text, txtReplyDesc.Text);
                            Thread.Sleep(500);
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Insert Sukses', 'success');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Gagal Insert', 'warning');", true);
                        }
                    });
                    #endregion
                    #region Whatsapp-web.js
                    //using (var client = new HttpClient())
                    //{
                    //    SenderInsertReply sen = new SenderInsertReply { sender = ddlSessionId.SelectedValue, content = txtReplyContent.Text, messagecontent = txtReplyDesc.Text };
                    //    client.BaseAddress = new Uri("http://192.168.100.1:9001/insertreply");
                    //    var response = client.PostAsJsonAsync("", sen).Result;
                    //    if (response.IsSuccessStatusCode)
                    //    {
                    //        fcn.InsertWhatsappReply(ddlSessionId.SelectedValue, txtReplyContent.Text, txtReplyDesc.Text);
                    //        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Sukses', 'success');", true);
                    //        Thread.Sleep(500);
                    //        Response.Redirect(Request.RawUrl, false);
                    //    }
                    //    else
                    //    {
                    //        if (response.StatusCode.ToString() == "422")
                    //        {
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Terjadi Kesalahan', 'error');", true);
                    //        }
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ReplyBind()
        {
            using (OracleConnection con = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]))
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
                    cmd.CommandText = "select * from trx_whatsapp_reply where message_reply like :messagereply order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("messagereply", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvReply.DataSource = dt;
                        gvReply.DataBind();
                    }
                }
            }
        }

        public void DeleteArrayJsonFile(string strSender, string strReply, string strTemplate)
        {
            try
            {
                #region Baileys
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://127.0.0.1:8000/chats/deletereply");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "");
                request.Content = new StringContent("{\"id\":\""+strSender+"\",\"content\": \""+strReply+"\"}", Encoding.UTF8, "application/json");
                client.SendAsync(request).ContinueWith(responseTask =>
                {
                    if (responseTask.Result.IsSuccessStatusCode)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Delete Sukses', 'success');", true);
                        fcn.DeleteWhatsappReply(strSender, strReply, strTemplate);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Gagal Delete', 'warning');", true);
                    }
                });
                #endregion
                #region Whatsapp-web.js
                //using (var client = new HttpClient())
                //{
                //    SenderInsertReply sen = new SenderInsertReply { sender = strSender, content = strReply, messagecontent = strTemplate };
                //    client.BaseAddress = new Uri("http://192.168.100.1:9001/deletereply");
                //    var response = client.PostAsJsonAsync("", sen).Result;
                //    if (response.IsSuccessStatusCode)
                //    {
                //        fcn.DeleteWhatsappReply(strSender, strReply, strTemplate);
                //    }
                //    else
                //    {
                //        if (response.StatusCode.ToString() == "422")
                //        {
                //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Terjadi Kesalahan', 'error');", true);
                //        }
                //    }
                //    Response.Redirect(Request.RawUrl, false);
                //}
                #endregion
            }
            catch (Exception ex)
            {

            }
        }
    }
}