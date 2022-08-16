using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class SentWhatsappGroup : System.Web.UI.Page
    {
        Function fcn = new Function();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            txtBlastGroupHeader.Text = "";
            txtNamaGroup.Text = "";
            ddlSessionId.SelectedValue = "";
            txtPesanGroup.Text = "";
            txtBlastDate.Text = "";
            hiddenTrxId.Value = "";
            hiddenBlastStatus.Value = "";
            lblAlert.Visible = false;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
        }

        protected void gvBlastGroup_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditData")
            {
                string strEval = e.CommandArgument.ToString();
                Session["IsEdit"] = strEval;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
                ShowDataEdit(strEval);
            }
            if (e.CommandName == "Sent")
            {
                string strEval = e.CommandArgument.ToString();
                string SenderId = strEval.Split('|')[3];
                string Message = strEval.Split('|')[2];
                string strGroupName = strEval.Split('|')[1];
                string TrxId = strEval.Split('|')[0];
                SentBlastGroup(SenderId, strGroupName, Message, TrxId);
            }
            if (e.CommandName == "Delete")
            {
                string strEval = e.CommandArgument.ToString();
                fcn.getDataTable(@"delete from trx_whatsapp_message_group where trxid = '" + strEval + "'");
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void gvBlastGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBlastGroup.PageIndex = e.NewPageIndex;
            DataBind();
        }

        protected void gvBlastGroup_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvBlastGroup.Rows)
            {
                Label label = (Label)row.FindControl("lblStatus");
                LinkButton btnSent = (LinkButton)row.FindControl("btnSent");
                LinkButton btnDelete = (LinkButton)row.FindControl("btnDelete");
                if (label.Text == "Invalid")
                {
                    label.CssClass = "gradient-custom-button-2";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.ToolTip = "Group Tidak Ditemukan";
                    btnSent.Enabled = false;
                    btnSent.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                    btnDelete.Enabled = false;
                    btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
                if (label.Text == "Open")
                {
                    label.CssClass = "gradient-custom-card-1";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 150;
                    label.Text = "waiting for schedule";

                    btnSent.Enabled = true;
                    btnSent.CssClass = "btn btn-sm gradient-custom-button-1 text-white";
                    btnDelete.Enabled = true;
                    btnDelete.CssClass = "btn btn-sm gradient-custom-button-2 text-white";
                }
                if (label.Text == "Sent")
                {
                    label.CssClass = "gradient-custom-button-1 text-white";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    btnSent.Enabled = false;
                    btnSent.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                    btnDelete.Enabled = false;
                    btnDelete.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
            }
        }

        protected void ChkisMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkisMedia.Checked)
            {
                ajaxfileupload.Visible = true;
            }
            else
            {
                ajaxfileupload.Visible = false;
            }
        }

        protected void btnAddBlastGroup_Click(object sender, EventArgs e)
        {
            if (txtBlastGroupHeader.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Judul Kosong', 'info');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
            }
            if (txtNamaGroup.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Nama Group', 'info');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
            }
            if (ddlSessionId.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Pilih Devices', 'info');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
            }
            if (txtBlastDate.Text == "" && txtScheduledtime.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Tanggal / Waktu Scheduled Kosong', 'info');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
            }
            if (txtPesanGroup.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Pesan', 'info');", true);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
            }
            else
            {
                string TrxId = "Group." + Guid.NewGuid().ToString("N");
                string strIsMedia = ChkisMedia.Checked ? "1" : "0";
                string strBranchId = Convert.ToString(Session["UserBranch"]);
                string strCreateBy = Convert.ToString(Session["UserName"]);
                string strBlastDate = txtBlastDate.Text.Split(' ')[0];
                string strMediaName = "";
                if (hiddenTrxId.Value == "")
                {
                    fcn.InsertWhatsappBlastGroup(TrxId, txtBlastGroupHeader.Text, txtNamaGroup.Text, ddlSessionId.SelectedValue, txtPesanGroup.Text, strBranchId, strIsMedia, strCreateBy, strBlastDate, strMediaName, strBlastDate, txtScheduledtime.Text);

                }
                else
                {
                    if (hiddenBlastStatus.Value != "2" && hiddenBlastStatus.Value != "1")
                    {
                        fcn.UpdateWhatsappBlastGroup(hiddenTrxId.Value, txtBlastGroupHeader.Text, txtNamaGroup.Text, ddlSessionId.SelectedValue, txtPesanGroup.Text, strIsMedia, strBlastDate, strMediaName, strBlastDate, txtScheduledtime.Text);

                    }
                    else
                    {
                        lblAlert.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalBlastGroupAdd').modal('show');", true);
                        updModalView.Update();
                    }
                }
            }
        }

        protected void ajaxfileupload_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {

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
                    cmd.CommandText = "select trxid, title_blast, group_name, sender_id, message_content, branch_id, is_media, blast_date, decode(blast_status, '0','Open', '1', 'Sent', '2', 'Invalid') blast_status, scheduled_date, scheduled_time from trx_whatsapp_message_group where create_by = '"+Convert.ToString(Session["UserID"])+"' and group_name like :groupname order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("groupname", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvBlastGroup.DataSource = dt;
                        gvBlastGroup.DataBind();
                    }
                }
            }
        }
        private void ShowDataEdit(string strTrxId)
        {
            string strTrxGroup;
            string[] strGetData = new string[40];
            strTrxGroup = fcn.ShowDataWhatsappGroup(strTrxId);
            if (strTrxGroup != null || strTrxGroup != "")
            {
                strGetData = strTrxGroup.Split('#');
                hiddenTrxId.Value = strGetData[0];
                txtBlastGroupHeader.Text = strGetData[1];
                txtNamaGroup.Text = strGetData[2];
                ddlSessionId.SelectedValue = strGetData[3];
                txtPesanGroup.Text = strGetData[4];
                txtBlastDate.Text = strGetData[10];
                hiddenBlastStatus.Value = strGetData[11];
                txtScheduledtime.Text = strGetData[14];
            }
        }
        private void SentBlastGroup(string strSenderId, string strGroupName, string strMessage, string strTrxId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderGroup sen = new SenderGroup { sender = strSenderId, name = strGroupName, message = strMessage };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-group-message");
                    var response = client.PostAsJsonAsync("", sen).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        fcn.getDataTable(@"UPDATE TRX_WHATSAPP_MESSAGE_GROUP SET BLAST_STATUS = '1' WHERE TRXID = '" + strTrxId + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            fcn.getDataTable(@"UPDATE TRX_WHATSAPP_MESSAGE_GROUP SET BLAST_STATUS = '2', NOTIFICATION = 'GRUP TIDAK DITEMUKAN' WHERE TRXID = '" + strTrxId + "'");
                        }
                    }
                    Response.Redirect(Request.RawUrl, false);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}