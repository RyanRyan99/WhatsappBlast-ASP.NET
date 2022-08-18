using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class SentPlan : System.Web.UI.Page
    {
        FunctionPlan plan = new FunctionPlan();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateBranch();
                txtPlanTitleView.Attributes.Add("readonly","readonly");
                lblSentDate.Attributes.Add("readonly", "readonly");
                txtDevicesView.Attributes.Add("readonly", "readonly");
                txtScheduledTimeView.Attributes.Add("readonly", "readonly");
                lblMessageContentView.Attributes.Add("readonly", "readonly");
                if (ddlViewAllBranch.SelectedValue == "false")
                {
                    BindData();
                }
                else
                {
                    BindDataAllBranch();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if(ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataAllBranch();
            }
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataAllBranch();
            }
        }

        #region Badge
        protected void btnTemplateName_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{name}";
        }

        protected void btnTemplateFullName_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{full_name}";
        }
        protected void btnTemplateTglBeli_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{tgl_beli}";
        }

        protected void btnTemplateTypeKendaraan_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{type_kendaraan}";
        }

        protected void btnTemplateTglSTNK_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{tgl_stnk}";
        }

        protected void btnTemplatePlatNo_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{plat_no}";
        }

        protected void btnTemplateLastService_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{last_service}";
        }

        protected void btnTemplateBirthDate_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{birth_date}";
        }

        protected void btnTemplateBranchName_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{branch_name}";
        }

        protected void btnTemplateBranchAddress_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{branch_address}";
        }

        protected void btnTemplateBranchPhone_Click(object sender, EventArgs e)
        {
            txtMessageContent.Text = txtMessageContent.Text + "{branch_phone}";
        }
        #endregion

        protected void chkIsMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsMedia.Checked)
            {
                ajaxfileupload.Visible = true;
                lblalertnote.Visible = true;
            }
            else
            {
                ajaxfileupload.Visible = false;
                lblalertnote.Visible = false;
            }
        }

        protected void ajaxfileupload_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string ChangeName = "";
            string FileName = e.FileName;
            ChangeName = Guid.NewGuid() + FileName;
            string fileNameWithPath = Server.MapPath("~/Media/") + ChangeName;
            ajaxfileupload.SaveAs(fileNameWithPath);
            Session["IsFileName"] = ChangeName;
        }

        protected void ddlSumberData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSumberData.SelectedValue == "H1")
            {
                checkboxServiceTerakhir.Enabled = false;
                ddlServiceTerakhir.Enabled = false;
                txtServieTerakhir.Enabled = false;

                checkboxJumlahService.Enabled = false;
                ddlJumlahService.Enabled = false;
                txtJumlahService.Enabled = false;
            }
            else
            {
                checkboxServiceTerakhir.Enabled = true;
                ddlServiceTerakhir.Enabled = true;
                txtServieTerakhir.Enabled = true;

                checkboxJumlahService.Enabled = true;
                ddlJumlahService.Enabled = true;
                txtJumlahService.Enabled = true;
            }

            if (ddlSumberData.SelectedValue == "H2")
            {
                checkboxTanggalSTNK.Enabled = false;
                ddlTanggalSTNK.Enabled = false;
                txtTanggalSTNK.Enabled = false;

                checkboxTanggalPembelian.Enabled = false;
                ddlTanggalPembelian.Enabled = false;
                txtTanggalPembelian.Enabled = false;
            }
            else
            {
                checkboxTanggalSTNK.Enabled = true;
                ddlTanggalSTNK.Enabled = true;
                txtTanggalSTNK.Enabled = true;

                checkboxTanggalPembelian.Enabled = true;
                ddlTanggalPembelian.Enabled = true;
                txtTanggalPembelian.Enabled = true;
            }
        }

        protected void btnSavePlan_Click(object sender, EventArgs e)
        {
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            if (txtPlanTitle.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Nama Plan Kosong', 'info');", true);
                return;
            }
            if (txtSentDate.Text == "" && txtScheduledTime.Text == "")
            {
                //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Tanggal/Waktu Scheduled Kosong', 'info');", true);
                //return;
            }
            if (txtMessageContent.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Pesan Kosong', 'info');", true);
                return;
            }
            if (checkBoxInterval.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Interval Belum Dipilih', 'info');", true);
                return;
            }
            if (checkboxBranch.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Branch Belum Dipilih', 'info');", true);
                return;
            }
            if(ddlSessionId.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Devices Belum Dipilih', 'info');", true);
                return;
            }
            if(ddlSumberData.SelectedValue == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Pilih Sumber Data', 'info');", true);
                return;
            }
            if(ddlSumberData.SelectedValue == "H1")
            {
                //if(checkboxTanggalSTNK.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Check Tanggal STNK', 'info');", true);
                //    return;
                //}
                //if(checkboxTanggalPembelian.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Check Tanggal Pembelian', 'info');", true);
                //    return;
                //}
                //if(txtTanggalSTNK.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Tanggal STNK', 'info');", true);
                //    return;
                //}
                //if (txtTanggalPembelian.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Tanggal Pembelian', 'info');", true);
                //    return;
                //}
            }
            if(ddlSumberData.SelectedValue == "H2")
            {
                //if (checkboxServiceTerakhir.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Check Service Terakhir', 'info');", true);
                //    return;
                //}
                //if (checkboxJumlahService.Checked == false)
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Check Jumlah Service', 'info');", true);
                //    return;
                //}
                //if(txtServieTerakhir.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Service Terakhir', 'info');", true);
                //    return;
                //}
                //if (txtJumlahService.Text == "")
                //{
                //    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Isi Jumlah Service', 'info');", true);
                //    return;
                //}
            }

            string selectedItems = String.Join(",", checkboxBranch.Items.OfType<ListItem>().Where(r => r.Selected).Select(r => r.Value));
            string selectedItemsName = String.Join(",", checkboxBranch.Items.OfType<ListItem>().Where(r => r.Selected).Select(r => r.Text));
            string PlanId = "Plan." + DateTime.Now.ToString("ddMMyyyyhhmmss");
            //string strIsSurvey = checkboxSurvey.Checked ? "1" : "0";
            string strIsSurvey = "0";
            string strBranchFrom = Convert.ToString(Session["UserBranch"]);
            string strUser = Convert.ToString(Session["UserID"]);
            string strIsMedia = chkIsMedia.Checked ? "1" : "0";
            string UploadFileName = Convert.ToString(Session["IsFileName"]);
            string strSendDateConvert = txtSentDate.Text.Split(' ')[0];

            //Insert Plan
            plan.InsertPlan(PlanId, txtPlanTitle.Text, checkBoxInterval.SelectedValue, selectedItems, txtMessageContent.Text, strIsSurvey, selectedItemsName, strSendDateConvert, strBranchFrom, strIsMedia, UploadFileName, txtSentDate.Text, txtScheduledTime.Text, txtScheduledMessage.Text, txtScheduledTimePesan.Text, ddlSessionId.SelectedValue, strUser);
            if (checkboxSumberData.Checked)
            {
                plan.InsertPlanFilter("SUMBER DATA", ddlSumberDataOperator.SelectedValue, ddlSumberData.SelectedValue, PlanId);
            }
            if (checkboxTanggalSTNK.Checked)
            {
                plan.InsertPlanFilter("TANGGAL STNK", ddlTanggalSTNK.SelectedValue, txtTanggalSTNK.Text, PlanId);
                if (checkboxTanggalPembelian.Checked == false)
                {
                    plan.InsertPlanFilter("TANGGAL PEMBELIAN", ddlTanggalPembelian.SelectedValue, txtTanggalPembelian.Text, PlanId);
                }
            }
            if (checkboxTanggalPembelian.Checked)
            {
                plan.InsertPlanFilter("TANGGAL PEMBELIAN", ddlTanggalPembelian.SelectedValue, txtTanggalPembelian.Text, PlanId);
                if (checkboxTanggalSTNK.Checked == false)
                {
                    plan.InsertPlanFilter("TANGGAL STNK", ddlTanggalSTNK.SelectedValue, txtTanggalSTNK.Text, PlanId);
                }
            }
            if (checkboxServiceTerakhir.Checked)
            {
                plan.InsertPlanFilter("SERVICE TERAKHIR", ddlServiceTerakhir.SelectedValue, txtServieTerakhir.Text, PlanId);
            }
            if (checkboxJumlahService.Checked)
            {
                plan.InsertPlanFilter("JUMLAH SERVICE", ddlJumlahService.SelectedValue, txtJumlahService.Text, PlanId);
            }
            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Sukses', 'success');", true);

        }

        protected void gvPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewPlan")
            {
                string strEval = e.CommandArgument.ToString();
                string strPlanId = strEval.Split('|')[0];
                string strBranch = strEval.Split('|')[1];
                string strB = strBranch.Insert(0, "'");
                string strR = strB.Replace(",", "', '");
                string strA = strR.Insert(strR.Length - 0, "'");
                HiddenBranchSplit.Value = strA;
                HiddenPlanIsMedia.Value = strEval.Split('|')[2];
                HiddenPlanMedia.Value = strEval.Split('|')[3];
                HiddenScheduledMessage.Value = strEval.Split('|')[4];
                HiddenScheduledMessageTime.Value = strEval.Split('|')[5];

                ShowData(strPlanId);
                //Showdata form Header View Plan
                showPlanView(strPlanId);
                //GV for ViewPlan data Key
                BindDataViewPlanOperator(strPlanId);
                //GV for DataCustomer
                BindDataViewCats(strA);
            }
            if(e.CommandName == "EditPlan")
            {
                hiddenplanedit.Value = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalPlanEdit').modal('show');", true);
                PopulateBranchEdit();
                ShowDataEdit(e.CommandArgument.ToString());
            }
            if (e.CommandName == "Delete")
            {
                string strEval = e.CommandArgument.ToString();
                plan.DeletePlan(strEval);
                plan.DeletePlanOperator(strEval);
                Response.Redirect(Request.RawUrl, true);
            }
        }

        protected void gvPlan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            foreach (GridViewRow row in gvPlan.Rows)
            {
                LinkButton button = (LinkButton)row.FindControl("btnDelete");
                Label label = (Label)row.FindControl("lblstatus");
                if (label.Text == "waiting for schedule")
                {
                    label.CssClass = "gradient-custom-card-1";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 150;
                    label.ToolTip = "Menunggu Jadwal";
                }
                if (label.Text == "Generate")
                {
                    label.CssClass = "gradient-custom-button-2";
                    label.ForeColor = Color.FromKnownColor(KnownColor.White);
                    label.Width = 70;
                    label.ToolTip = "Sudah di Eksekusi";

                    button.Enabled = true;
                    button.BackColor = Color.FromKnownColor(KnownColor.DarkGray);
                }
            }
        }

        protected void gvPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPlan.PageIndex = e.NewPageIndex;
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            else
            {
                BindDataAllBranch();
            }
        }

        protected void btnViewPlan_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalPlanView').modal('show');", true);
        }

        protected void btnViewGeneratePlan_Click(object sender, EventArgs e)
        {
            if (HiddenSumberData.Value == "H1")
            {
                GenerateDataFromH1(hiddenOperator.Value, HiddenValuesServiceTerakhir.Value, HiddenSumberData.Value, HiddenBranchSplit.Value);
                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + hiddenPlanId.Value + "'");
            }
            else if (HiddenSumberData.Value == "H2")
            {
                GenerateDataFromH2(hiddenOperator.Value, HiddenValuesServiceTerakhir.Value, HiddenSumberData.Value, HiddenBranchSplit.Value);
                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + hiddenPlanId.Value + "'");
            }
            else
            {
                //DataSource All H2,H2
                GenerateDataFromAll(hiddenOperator.Value, HiddenValuesServiceTerakhir.Value, HiddenSumberData.Value, HiddenBranchSplit.Value);
                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + hiddenPlanId.Value + "'");
            }
        }

        protected void gvDataCats_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Gridview Filter Params
            gvDataCats.PageIndex = e.NewPageIndex;
            BindDataViewCats(HiddenBranchSplit.Value);
            updModalView.Update();
        }

        private void PopulateBranch()
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string UserBranch = Convert.ToString(Session["UserBranch"]);
                    string Query = "select branch_id, branch_name from mst_branch where branch_id = '001' order by branch_id asc";
                    cmd.CommandText = Query;
                    cmd.Connection = con;
                    con.Open();
                    using (OracleDataReader odr = cmd.ExecuteReader())
                    {
                        while (odr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = odr["branch_name"].ToString();
                            item.Value = odr["branch_id"].ToString();
                            checkboxBranch.Items.Add(item);
                        }
                    }
                    con.Close();
                }
            }
        }

        private void PopulateBranchEdit()
        {
            using (OracleConnection con = new OracleConnection(ConnectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string UserBranch = Convert.ToString(Session["UserBranch"]);
                    string Query = "select branch_id, branch_name from mst_branch where branch_id = '001' order by branch_id asc";
                    cmd.CommandText = Query;
                    cmd.Connection = con;
                    con.Open();
                    using (OracleDataReader odr = cmd.ExecuteReader())
                    {
                        while (odr.Read())
                        {
                            ListItem item = new ListItem();
                            item.Text = odr["branch_name"].ToString();
                            item.Value = odr["branch_id"].ToString();
                            chkEditBranch.Items.Add(item);
                        }
                    }
                    con.Close();
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
                    cmd.CommandText = "select id_plan, plan_title, decode(interval, '1', 'Daily', '2', 'Weekly', '3', 'Monthly', '4', 'Custom') interval, replace(branch_id, ',',', ') branch_id, create_date, branch_name, decode(plan_status, '0', 'waiting for schedule', '1', 'Generate') plan_status, is_media, media_name, schedule_date, schedule_time, to_char(scheduled_message, 'DD/MM/YYYY') scheduled_message, scheduled_message_time from trx_whatsapp_plan where create_by = '"+Convert.ToString(Session["UserID"])+"' and plan_title like :plantitle order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("plantitle", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvPlan.DataSource = dt;
                        gvPlan.DataBind();
                    }
                }
            }
        }
        private void BindDataAllBranch()
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
                    cmd.CommandText = "select id_plan, plan_title, decode(interval, '1', 'Daily', '2', 'Weekly', '3', 'Monthly', '4', 'Custom') interval, replace(branch_id, ',',', ') branch_id, create_date, branch_name, decode(plan_status, '0', 'waiting for schedule', '1', 'Generate') plan_status, is_media, media_name, schedule_date, schedule_time, to_char(scheduled_message, 'DD/MM/YYYY') scheduled_message, scheduled_message_time from trx_whatsapp_plan where plan_title like :plantitle order by create_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("plantitle", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvPlan.DataSource = dt;
                        gvPlan.DataBind();
                    }
                }
            }
        }

        private void ShowData(string strPlanId)
        {
            string strPlan2;
            string[] strGetData2 = new string[40];
            strPlan2 = plan.GetPlanFilterSumberData(strPlanId);
            if (strPlan2 != null || strPlan2 != "")
            {
                strGetData2 = strPlan2.Split('#');
                hiddenOperator.Value = strGetData2[0];
                HiddenValues2.Value = strGetData2[1];
                HiddenSumberData.Value = strGetData2[2];
            }
        }

        private void BindDataViewPlanOperator(string strPlanId)
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand("select * from trx_whatsapp_plan_filter where id_plan = '" + strPlanId + "'"))
                {
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvPlanOperator.DataSource = dt;
                            gvPlanOperator.DataBind();
                        }
                    }
                }
            }
        }

        private void BindDataViewCats(string strBranch)
        {
            #region SumberData
            string QrSTNK = "";
            string QrPEMBELIAN = "";
            string QrSERVICE = "";
            string QrJUMLAHSERVICE = "";
            string strGatPlanSUMBERDATA = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SUMBER DATA'");
            string strGetPlanTANGGALSTNK = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL STNK'");
            string strGetPlanTANGGALPEMBELIAN = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL PEMBELIAN'");
            string strGetPlanSERVICETERAKHIR = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SERVICE TERAKHIR'");
            string strGetPlanJUMLAHSERVICE = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'JUMLAH SERVICE'");
            #endregion
            #region H1
            string vleSUMBERDATA = strGatPlanSUMBERDATA.Split('#')[1];
            if (vleSUMBERDATA == "H1")
            {
                //TanggalSTNK
                string oprTANGGALSTNK = "";
                string vleTANGGALSTNK = "";
                string QrWhere = "";
                if (strGetPlanTANGGALSTNK != "<##" && strGetPlanTANGGALSTNK != "=##" && strGetPlanTANGGALSTNK != "")
                {
                    oprTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[0];
                    vleTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[1];
                    QrWhere = @"where";
                    QrSTNK = @"and trunc(sysdate) " + oprTANGGALSTNK + " trunc(STNK_RECEIVE_DATE) + " + vleTANGGALSTNK + "";
                }
                //TanggalPembelian
                string oprTANGGALPEMBELIAN = "";
                string vleTANGGALPEMBELIAN = "";
                if (strGetPlanTANGGALPEMBELIAN != "<##" && strGetPlanTANGGALPEMBELIAN != "=##" && strGetPlanTANGGALPEMBELIAN != "")
                {
                    oprTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[0];
                    vleTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[1];
                    QrPEMBELIAN = @"and trunc(sysdate) " + oprTANGGALPEMBELIAN + " trunc(do_date) + " + vleTANGGALPEMBELIAN + "";
                }
            }
            #endregion
            #region H2
            if (vleSUMBERDATA == "H2")
            {
                //Service Terakhir
                string oprSERVICETERAKHIR = "";
                string vleSERVICETERAKHIR = "";
                if (strGetPlanSERVICETERAKHIR != "<##" && strGetPlanSERVICETERAKHIR != "=##" && strGetPlanSERVICETERAKHIR != "")
                {
                    oprSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[0];
                    vleSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[1];
                    //QrSERVICE = @"and trunc(sysdate) " + oprSERVICETERAKHIR + " trunc(vo_last_order) + " + vleSERVICETERAKHIR + "";
                    QrSERVICE = @"lastorder "+oprSERVICETERAKHIR+" (trunc(sysdate-"+vleSERVICETERAKHIR+")) and INVOICE_DATE "+oprSERVICETERAKHIR+" (trunc(sysdate-"+vleSERVICETERAKHIR+"))";
                }
                //Jumlah Service
                string oprJUMLAHSERVICE = "";
                string vleJUMLAHSERVICE = "";
                if (strGetPlanJUMLAHSERVICE != "<##" && strGetPlanJUMLAHSERVICE != "=##" && strGetPlanJUMLAHSERVICE != "")
                {
                    oprJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[0];
                    vleJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[1];
                    //QrJUMLAHSERVICE = @"and totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
                    if(strGetPlanSERVICETERAKHIR != "")
                    {
                        QrJUMLAHSERVICE = @"and totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
                    }
                    else
                    {
                        QrJUMLAHSERVICE = @"totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
                    }
                }
            }
            #endregion

            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                string strQuery = "";
                if (vleSUMBERDATA == "H1")
                {
                    strQuery = "select * from ccmsmobil.vi_datapenjualanh1 where branch_id in(" + strBranch + ") " + QrSTNK + "" + QrPEMBELIAN + "";
                    ((BoundField)gvDataCats.Columns[0]).DataField = "CUST_NAME";
                    ((BoundField)gvDataCats.Columns[1]).DataField = "PHONE_NO1";
                    showDataCount(strQuery.Replace("*", "count(*)"));
                }
                else if (vleSUMBERDATA == "H2")
                {
                    //strQuery = "select * from ccmsmobil.vi_selectcustomerh2 where vo_branch_id in(" + strBranch + ") " + QrSERVICE + " " + QrJUMLAHSERVICE + " and vo_phone_1 is not null";
                    strQuery = "SELECT * FROM VI_PLAN_H2 where "+QrSERVICE+" "+QrJUMLAHSERVICE+"";
                    ((BoundField)gvDataCats.Columns[0]).DataField = "OWNERNAME";
                    ((BoundField)gvDataCats.Columns[1]).DataField = "PHONE1";
                    showDataCount(strQuery.Replace("*", "count(*)"));
                }
                else
                {
                    strQuery = "select cust_name, phone_no1, 'H1' as Frm from ccmsmobil.vi_datapenjualanh1 " +
                        "where branch_id in(" + strBranch + ") " + QrSTNK + "" + QrPEMBELIAN + "" +
                        "union " +
                        "select vo_name, vo_phone_1, 'H2' as Frm from ccmsmobil.vi_selectcustomerh2 " +
                        "where vo_branch_id in(" + strBranch + ") " + QrSERVICE + "" + QrJUMLAHSERVICE + "";
                    ((BoundField)gvDataCats.Columns[0]).DataField = "CUST_NAME";
                    ((BoundField)gvDataCats.Columns[1]).DataField = "PHONE_NO1";
                    showDataCount(strQuery.Replace("*", "count(*)"));
                }

                using (OracleCommand cmd = new OracleCommand(strQuery))
                {
                    using (OracleDataAdapter sda = new OracleDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvDataCats.DataSource = dt;
                            gvDataCats.DataBind();
                        }
                    }
                }
            }
        }

        private void GenerateDataFromH1(string strOperator, string strValue, string strSumberData, string strBranch)
        {
            #region SumberData
            //SumberData
            string strGatPlanSUMBERDATA = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SUMBER DATA'");
            string strGetPlanTANGGALSTNK = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL STNK'");
            string strGetPlanTANGGALPEMBELIAN = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL PEMBELIAN'");
            string strSenderId = plan.GetPlanFilter("select session_devices from trx_whatsapp_plan where id_plan = '"+hiddenPlanId.Value+"'");
            #endregion
            #region H1
            string vleSUMBERDATA = strGatPlanSUMBERDATA.Split('#')[1];
            //TanggalSTNK
            string oprTANGGALSTNK = "";
            string vleTANGGALSTNK = "";
            string QrWhere = "";
            string QrSTNK = "";
            if (strGetPlanTANGGALSTNK != "=##" && strGetPlanTANGGALSTNK != "<##" && strGetPlanTANGGALSTNK != "")
            {
                oprTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[0];
                vleTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[1];
                QrWhere = @"where";
                QrSTNK = @"and trunc(sysdate) " + oprTANGGALSTNK + " trunc(STNK_RECEIVE_DATE) + " + vleTANGGALSTNK + "";
            }
            //TanggalPembelian
            string oprTANGGALPEMBELIAN = "";
            string vleTANGGALPEMBELIAN = "";
            string QrPEMBELIAN = "";
            if (strGetPlanTANGGALPEMBELIAN != "=##" && strGetPlanTANGGALPEMBELIAN != "<##" && strGetPlanTANGGALPEMBELIAN != "")
            {
                oprTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[0];
                vleTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[1];
                QrPEMBELIAN = @"and trunc(sysdate) " + oprTANGGALPEMBELIAN + " trunc(do_date) + " + vleTANGGALPEMBELIAN + "";
            }
            #endregion

            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

            #region List
            List<string> SenderId = new List<string>();
            List<string> Full_name = new List<string>();
            List<string> Cust_name = new List<string>();
            List<string> Number = new List<string>();
            List<string> Branch_id = new List<string>();
            List<string> Tanggal_Beli = new List<string>();
            List<string> Type_Kendaraan = new List<string>();
            List<string> Tanggal_STNK = new List<string>();
            List<string> Plat_No = new List<string>();
            List<string> Branch_Name = new List<string>();
            List<string> Birth_Date = new List<string>();
            List<string> Branch_Address = new List<string>();
            List<string> Branch_Phone = new List<string>();
            List<string> Doc_Ref = new List<string>();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string strUser = Convert.ToString(Session["UserID"]);
            #endregion

            string trxid = "Generate.H1." + DateTime.Now.ToString("ddMMyyyyhhmmss");
            string strGetData = "";
            OracleCommand oracleCommand = null;
            strGetData = @"SELECT distinct ccmsmobil.vi_datapenjualanh1.CUST_NAME, REPLACE(REPLACE(REPLACE(REPLACE(ccmsmobil.vi_datapenjualanh1.PHONE_NO1, ' ', ''), '-', ''), '+62', '0'), '+62 ', '0') AS PHONE_NO1, ccmsmobil.vi_datapenjualanh1.BRANCH_ID, do_date, product_name, stnk_receive_date, police_no, cabang, BIRTH_DATE, bintangdbamobil.mst_branch.ADDRESS1, bintangdbamobil.mst_branch.phone_no1 as branch_phone, ccmsmobil.vi_datapenjualanh1.do_id, ccmsmobil.vi_datapenjualanh1.nickname FROM ccmsmobil.vi_datapenjualanh1 INNER JOIN ccmsmobil.vi_selectcustomerh1 ON ccmsmobil.vi_datapenjualanh1.cust_id = ccmsmobil.vi_selectcustomerh1.cust_id INNER JOIN bintangdbamobil.mst_branch on ccmsmobil.vi_datapenjualanh1.branch_id = bintangdbamobil.mst_branch.branch_id where ccmsmobil.vi_datapenjualanh1.branch_id in(" + strBranch + ")" + QrSTNK + "" + QrPEMBELIAN + "";
            oracleCommand = new OracleCommand(strGetData, oracleConnection);
            OracleDataReader dataReader = oracleCommand.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        Full_name.Add(dataReader.GetString(0));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(12))
                    {
                        Cust_name.Add(dataReader.GetString(12));
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
                        SenderId.Add(dataReader.GetString(2));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(3))
                    {
                        Tanggal_Beli.Add(dataReader.GetString(3));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(4))
                    {
                        Type_Kendaraan.Add(dataReader.GetString(4));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(5))
                    {
                        Tanggal_STNK.Add(dataReader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(6))
                    {
                        Plat_No.Add(dataReader.GetString(6));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(7))
                    {
                        Branch_Name.Add(dataReader.GetString(7));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(8))
                    {
                        Birth_Date.Add(dataReader.GetString(8));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(9))
                    {
                        Branch_Address.Add(dataReader.GetString(9));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(10))
                    {
                        Branch_Phone.Add(dataReader.GetString(10));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(11))
                    {
                        Doc_Ref.Add(dataReader.GetString(11));
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
                        string strMessageReplace = "";
                        Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}|{full_name}");
                        Match findMatch = findValue.Match(lblMessageContentView.Text);
                        if (findMatch.Success)
                        {
                            strMessageReplace = lblMessageContentView.Text.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", Tanggal_Beli[i]).Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", Tanggal_STNK[i]).Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", "").Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]).Replace("{full_name}", Full_name[i]);
                        }
                        else
                        {
                            strMessageReplace = lblMessageContentView.Text;
                        }
                        string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + hiddenPlanId.Value + "' and wa_number = '" + Number[i] + "'");
                        if(CheckNumber != "")
                        {

                        }
                        else
                        {
                            plan.InsertGeneratePlan(strSenderId.Split('#')[0], date, strSumberData, lblViewPlan.Text, Number[i], strMessageReplace, hiddenPlanId.Value, Doc_Ref[i]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            //Add HeaderText
            plan.InsertTrxMessageHeader(trxid, lblViewPlan.Text, date, "", HiddenPlanMedia.Value, HiddenPlanIsMedia.Value, HiddenScheduledMessage.Value, HiddenScheduledMessageTime.Value, hiddenPlanId.Value, strUser);
        }
        private void GenerateDataFromH2(string strOperator, string strValue, string strSumberData, string strBranch)
        {
            #region SumberData
            //SumberData
            string strGetPlanSERVICETERAKHIR = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SERVICE TERAKHIR'");
            string strGetPlanJUMLAHSERVICE = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'JUMLAH SERVICE'");
            string strGetPlanJUMLAHSERVICE2 = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'JUMLAH SERVICE 2'");
            string strGetPlanBELUMKPB = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'BELUM KPB'");
            string strSenderId = plan.GetPlanFilter("select session_devices from trx_whatsapp_plan where id_plan = '" + hiddenPlanId.Value + "'");
            #endregion
            #region H2
            //Service Terakhir
            string oprSERVICETERAKHIR = "";
            string vleSERVICETERAKHIR = "";
            string QrSERVICE = "";
            if (strGetPlanSERVICETERAKHIR != "")
            {
                oprSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[0];
                vleSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[1];
                QrSERVICE = @"and trunc(sysdate) " + oprSERVICETERAKHIR + " trunc(vo_last_order) + " + vleSERVICETERAKHIR + "";
            }
            //Jumlah Service
            string oprJUMLAHSERVICE = "";
            string vleJUMLAHSERVICE = "";
            string QrJUMLAHSERVICE = "";
            if (strGetPlanJUMLAHSERVICE != "")
            {
                oprJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[0];
                vleJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[1];
                QrJUMLAHSERVICE = @"and totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
            }
            #endregion
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

            #region List
            List<string> SenderId = new List<string>();
            List<string> Full_name = new List<string>();
            List<string> Cust_name = new List<string>();
            List<string> Number = new List<string>();
            List<string> Branch_id = new List<string>();
            List<string> Tanggal_Beli = new List<string>();
            List<string> Type_Kendaraan = new List<string>();
            List<string> Tanggal_STNK = new List<string>();
            List<string> Plat_No = new List<string>();
            List<string> Branch_Name = new List<string>();
            List<string> Birth_Date = new List<string>();
            List<string> Branch_Address = new List<string>();
            List<string> Branch_Phone = new List<string>();
            List<string> Last_Service = new List<string>();
            List<string> Doc_Ref = new List<string>();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string strUser = Convert.ToString(Session["UserID"]);
            #endregion

            string trxid = "Generate.H2." + DateTime.Now.ToString("ddMMyyyyhhmmss");
            string strGetData = "";
            OracleCommand oracleCommand = null;
            strGetData = @"SELECT VO_NAME, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(VO_PHONE_1, ':62', '0'), ':0','0'), ' ', ''), '-', ''), '+62', '0'), '+62 ', '0'), '62', '0'),';','0') AS VO_PHONE_1, VO_BRANCH_ID, VM_DESCRIPTION, WO_REGISTRATION_NUMBER, VO_LAST_ORDER, VO_BIRTHDAY, BRANCH_NAME, bintangdbamobil.MST_BRANCH.ADDRESS1, bintangdbamobil.MST_BRANCH.PHONE_NO1 AS BRANCH_PHONE, ccmsmobil.VI_WORKORDERH2.WO_ID, nickname FROM ccmsmobil.VI_SELECTCUSTOMERH2 INNER JOIN ccmsmobil.VI_WORKORDERH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_WORKORDERH2.WO_OWNER_ID INNER JOIN bintangdbamobil.MST_BRANCH ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_BRANCH_ID = bintangdbamobil.MST_BRANCH.BRANCH_ID INNER JOIN ccmsmobil.VI_SELECTVEHICLEH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_OWNER_ID INNER JOIN ccmsmobil.MST_VEHICLEMODEL ON ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_MODEL_CODE = ccmsmobil.MST_VEHICLEMODEL.VM_MODEL_CODE where vo_branch_id in(" + strBranch + ")" + QrSERVICE + "" + QrJUMLAHSERVICE + "";
            oracleCommand = new OracleCommand(strGetData, oracleConnection);
            OracleDataReader dataReader = oracleCommand.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        Full_name.Add(dataReader.GetString(0));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(11))
                    {
                        Cust_name.Add(dataReader.GetString(11));
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
                        SenderId.Add(dataReader.GetString(2));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(3))
                    {
                        Type_Kendaraan.Add(dataReader.GetString(3));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(4))
                    {
                        Plat_No.Add(dataReader.GetString(4));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(5))
                    {
                        Last_Service.Add(dataReader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(6))
                    {
                        Birth_Date.Add(dataReader.GetString(6));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(7))
                    {
                        Branch_Name.Add(dataReader.GetString(7));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(8))
                    {
                        Branch_Address.Add(dataReader.GetString(8));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(9))
                    {
                        Branch_Phone.Add(dataReader.GetString(9));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(10))
                    {
                        Doc_Ref.Add(dataReader.GetString(10));
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
                        string strMessageReplace = "";
                        Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}|{full_name}");
                        Match findMatch = findValue.Match(lblMessageContentView.Text);
                        if (findMatch.Success)
                        {
                            strMessageReplace = lblMessageContentView.Text.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", "").Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", "").Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", Last_Service[i]).Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]).Replace("{full_name}", Full_name[i]);
                        }
                        else
                        {
                            strMessageReplace = lblMessageContentView.Text;
                        }
                        string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + hiddenPlanId.Value + "' and wa_number = '" + Number[i] + "'");
                        if (CheckNumber != "")
                        {

                        }
                        else
                        {
                            plan.InsertGeneratePlan(strSenderId.Split('#')[0], date, strSumberData, lblViewPlan.Text, Number[i], strMessageReplace, hiddenPlanId.Value, Doc_Ref[i]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            //Add HeaderText
            plan.InsertTrxMessageHeader(trxid, lblViewPlan.Text, date, "", HiddenPlanMedia.Value, HiddenPlanIsMedia.Value, HiddenScheduledMessage.Value, HiddenScheduledMessageTime.Value, hiddenPlanId.Value, strUser);
        }
        private void GenerateDataFromAll(string strOperator, string strValue, string strSumberData, string strBranch)
        {
            #region SumberData
            //SumberData
            string strGatPlanSUMBERDATA = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SUMBER DATA'");
            string strGetPlanTANGGALSTNK = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL STNK'");
            string strGetPlanTANGGALPEMBELIAN = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'TANGGAL PEMBELIAN'");
            string strGetPlanSERVICETERAKHIR = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'SERVICE TERAKHIR'");
            string strGetPlanJUMLAHSERVICE = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'JUMLAH SERVICE'");
            string strGetPlanJUMLAHSERVICE2 = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'JUMLAH SERVICE 2'");
            string strGetPlanBELUMKPB = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + hiddenPlanId.Value + "' and kunci = 'BELUM KPB'");
            #endregion
            #region H1
            string vleSUMBERDATA = strGatPlanSUMBERDATA.Split('#')[1];
            //TanggalSTNK
            string oprTANGGALSTNK = "";
            string vleTANGGALSTNK = "";
            string QrWhere = "";
            string QrSTNK = "";
            if (strGetPlanTANGGALSTNK != "")
            {
                oprTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[0];
                vleTANGGALSTNK = strGetPlanTANGGALSTNK.Split('#')[1];
                QrWhere = @"where";
                QrSTNK = @"and trunc(sysdate) " + oprTANGGALSTNK + " trunc(STNK_RECEIVE_DATE) + " + vleTANGGALSTNK + "";
            }
            //TanggalPembelian
            string oprTANGGALPEMBELIAN = "";
            string vleTANGGALPEMBELIAN = "";
            string QrPEMBELIAN = "";
            if (strGetPlanTANGGALPEMBELIAN != "")
            {
                oprTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[0];
                vleTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[1];
                QrPEMBELIAN = @"and trunc(sysdate) " + oprTANGGALPEMBELIAN + " trunc(do_date) + " + vleTANGGALPEMBELIAN + "";
            }
            #endregion
            #region H2
            //Service Terakhir
            string oprSERVICETERAKHIR = "";
            string vleSERVICETERAKHIR = "";
            string QrSERVICE = "";
            if (strGetPlanSERVICETERAKHIR != "")
            {
                oprSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[0];
                vleSERVICETERAKHIR = strGetPlanSERVICETERAKHIR.Split('#')[1];
                QrSERVICE = @"and trunc(sysdate) " + oprSERVICETERAKHIR + " trunc(vo_last_order) + " + vleSERVICETERAKHIR + "";
            }
            //Jumlah Service
            string oprJUMLAHSERVICE = "";
            string vleJUMLAHSERVICE = "";
            string QrJUMLAHSERVICE = "";
            if (strGetPlanJUMLAHSERVICE != "")
            {
                oprJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[0];
                vleJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[1];
                QrJUMLAHSERVICE = @"and totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
            }


            #endregion
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

            #region List
            List<string> SenderId = new List<string>();
            List<string> Full_name = new List<string>();
            List<string> Cust_name = new List<string>();
            List<string> Number = new List<string>();
            List<string> Branch_id = new List<string>();
            List<string> Tanggal_Beli = new List<string>();
            List<string> Type_Kendaraan = new List<string>();
            List<string> Tanggal_STNK = new List<string>();
            List<string> Plat_No = new List<string>();
            List<string> Branch_Name = new List<string>();
            List<string> Birth_Date = new List<string>();
            List<string> Branch_Address = new List<string>();
            List<string> Branch_Phone = new List<string>();
            List<string> Last_Service = new List<string>();
            List<string> Doc_Ref = new List<string>();
            string date = DateTime.Now.ToString("dd/MM/yyyy");
            string strUser = Convert.ToString(Session["UserID"]);
            #endregion

            string trxid = "Generate.ALL." + DateTime.Now.ToString("ddMMyyyyhhmmss");
            string strGetData = "";
            OracleCommand oracleCommand = null;
            #region DataQuery
            strGetData = "SELECT distinct ccmsmobil.vi_datapenjualanh1.CUST_NAME, " +
                "REPLACE(REPLACE(REPLACE(REPLACE(ccmsmobil.vi_datapenjualanh1.PHONE_NO1, ' ', ''), '-', ''), '+62', '0'), '+62 ', '0') AS PHONE_NO1, " +
                "ccmsmobil.vi_datapenjualanh1.BRANCH_ID, product_name, police_no, BIRTH_DATE, cabang, bintangdbamobil.MST_BRANCH.ADDRESS1, " +
                "bintangdbamobil.MST_BRANCH.phone_no1 as branch_phone, to_char(do_date, 'dd/mm/yyyy') do_date, to_char(stnk_receive_date, 'dd/mm/yyyy') as stnk_receive_date, " +
                "nvl(to_char('', 'dd/mm/yyyy'), '') as VO_LAST_ORDER, ccmsmobil.vi_datapenjualanh1.do_id as DOC_REF, ccmsmobil.vi_datapenjualanh1.nickname " +
                "FROM ccmsmobil.vi_datapenjualanh1 " +
                "INNER JOIN ccmsmobil.vi_selectcustomerh1 ON ccmsmobil.vi_datapenjualanh1.cust_id = ccmsmobil.vi_selectcustomerh1.cust_id " +
                "INNER JOIN bintangdbamobil.MST_BRANCH on ccmsmobil.vi_datapenjualanh1.branch_id = bintangdbamobil.MST_BRANCH.branch_id " +
                "WHERE ccmsmobil.vi_datapenjualanh1.branch_id in(" + strBranch + ")" + QrSTNK + "" + QrPEMBELIAN + "" +
                "UNION " +
                "SELECT distinct VO_NAME, " +
                "REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(VO_PHONE_1, ':62', '0'), ':0','0'), ' ', ''), '-', ''), '+62', '0'), '+62 ', '0'), '62', '0'),';','0') AS VO_PHONE_1, " +
                "VO_BRANCH_ID, VM_DESCRIPTION, WO_REGISTRATION_NUMBER, to_char(VO_BIRTHDAY, 'dd/mm/yyyy') VO_BIRTHDAY, BRANCH_NAME, " +
                "bintangdbamobil.MST_BRANCH.ADDRESS1, bintangdbamobil.MST_BRANCH.PHONE_NO1 AS BRANCH_PHONE, nvl(to_char('', 'dd/mm/yyyy'), '') as do_date, " +
                "nvl(to_char('', 'dd/mm/yyyy'), '') as stnk_receive_date, to_char(VO_LAST_ORDER, 'dd/mm/yyyy') as VO_LAST_ORDER, " +
                "ccmsmobil.VI_WORKORDERH2.WO_ID as DOC_REF, nickname " +
                "FROM ccmsmobil.VI_SELECTCUSTOMERH2 " +
                "INNER JOIN ccmsmobil.VI_WORKORDERH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_WORKORDERH2.WO_OWNER_ID " +
                "INNER JOIN bintangdbamobil.MST_BRANCH ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_BRANCH_ID = bintangdbamobil.MST_BRANCH.BRANCH_ID " +
                "INNER JOIN ccmsmobil.VI_SELECTVEHICLEH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_OWNER_ID " +
                "INNER JOIN ccmsmobil.MST_VEHICLEMODEL ON ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_MODEL_CODE = ccmsmobil.MST_VEHICLEMODEL.VM_MODEL_CODE " +
                "where vo_branch_id in(" + strBranch + ")" + QrSERVICE + "" + QrJUMLAHSERVICE + "";
            #endregion
            oracleCommand = new OracleCommand(strGetData, oracleConnection);
            OracleDataReader dataReader = oracleCommand.ExecuteReader();
            if (dataReader.HasRows)
            {
                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(13))
                    {
                        Cust_name.Add(dataReader.GetString(13));
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
                        SenderId.Add(dataReader.GetString(2));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(3))
                    {
                        Type_Kendaraan.Add(dataReader.GetString(3));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(4))
                    {
                        Plat_No.Add(dataReader.GetString(4));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(5))
                    {
                        Birth_Date.Add(dataReader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(6))
                    {
                        Branch_Name.Add(dataReader.GetString(6));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(7))
                    {
                        Branch_Address.Add(dataReader.GetString(7));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(8))
                    {
                        Branch_Phone.Add(dataReader.GetString(8));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(10))
                    {
                        Tanggal_STNK.Add(dataReader.GetString(10));
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                    if (!dataReader.IsDBNull(11))
                    {
                        Last_Service.Add(dataReader.GetString(11));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(9))
                    {
                        Tanggal_Beli.Add(dataReader.GetString(9));
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                    if (!dataReader.IsDBNull(12))
                    {
                        Doc_Ref.Add(dataReader.GetString(12));
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                    if (!dataReader.IsDBNull(13))
                    {
                        Full_name.Add(dataReader.GetString(13));
                    }
                    else
                    {
                        Console.WriteLine("");
                    }
                }
                dataReader.Close();
                for (int i = 0; i < Number.Count; i++)
                {
                    try
                    {
                        string strMessageReplace = "";
                        Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}|{full_name}");
                        Match findMatch = findValue.Match(lblMessageContentView.Text);
                        if (findMatch.Success)
                        {
                            strMessageReplace = lblMessageContentView.Text.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", Tanggal_Beli[i]).Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", Tanggal_STNK[i]).Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", Last_Service[i]).Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]).Replace("{full_name}", Full_name[i]);
                        }
                        else
                        {
                            strMessageReplace = lblMessageContentView.Text;
                        }
                        string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + hiddenPlanId.Value + "' and wa_number = '" + Number[i] + "'");
                        if (CheckNumber != "")
                        {

                        }
                        else
                        {
                            plan.InsertGeneratePlan(SenderId[i], date, strSumberData, lblViewPlan.Text, Number[i], strMessageReplace, hiddenPlanId.Value, Doc_Ref[i]);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            //Add HeaderText
            plan.InsertTrxMessageHeader(trxid, lblViewPlan.Text, date, "", HiddenPlanMedia.Value, HiddenPlanIsMedia.Value, HiddenScheduledMessage.Value, HiddenScheduledMessageTime.Value, hiddenPlanId.Value, strUser);
        }

        private void showPlanView(string strPlanId)
        {
            string strPlan;
            string[] strGetData = new string[40];
            strPlan = plan.GetPlanView(strPlanId);
            if (strPlan != null || strPlan != "")
            {
                strGetData = strPlan.Split('#');
                hiddenPlanId.Value = strGetData[0];
                lblViewPlan.Text = strGetData[1];
                lblViewPlanBadge.Text = strGetData[2];
                //lblCabangView.Text = strGetData[6];
                lblMessageContentView.Text = strGetData[5];
                lblSentDate.Text = strGetData[7].Split(' ')[0];
                HiddenPlanStatus.Value = strGetData[8];

                txtPlanTitleView.Text = strGetData[1];
                txtDevicesView.Text = strGetData[11];
                txtScheduledTimeView.Text = strGetData[10];
            }
            if (HiddenPlanStatus.Value != "0")
            {
                btnViewGeneratePlan.Visible = false;
            }
        }
        private void showDataCount(string strquery)
        {
            string strVa;
            string[] strGetData = new string[10];
            strVa = plan.DataCountRow(strquery);
            if(strVa != null || strVa != "")
            {
                strGetData = strVa.Split('#');
                lblCountRow.Text = strGetData[0] + " " + "Rows Available";
            }
        }

        private void ShowDataEdit(string id)
        {
            string strPlan;
            string[] strGetData = new string[60];
            strPlan = plan.GetPlanEdit(id);
            if (strPlan != null || strPlan != "")
            {
                strGetData = strPlan.Split('#');
                txtEditPlan.Text = strGetData[1];
                txtEditShceduledPesan.Text = strGetData[16];
                txtEditSecheduledWaktuPesan.Text = strGetData[17];
                txtEditPesan.Text = strGetData[5];
                chkEditInterval.SelectedValue = strGetData[2];
                ddlEditDevices.SelectedValue = strGetData[8];
                lblEditPlan.Text = strGetData[1];
                chkEditBranch.Items.FindByText(strGetData[7]).Selected = true;
                HiddenIsMedia.Value = strGetData[12];
                HiddenFileNameBeforUpdate.Value = strGetData[13];
            }

            string getSumberData = plan.getDataTable("select isi from trx_whatsapp_plan_filter where kunci = 'SUMBER DATA' and id_plan = '" + id+"'").Split('#')[0];
            string getTanggalStnk = plan.getDataTable("select isi from trx_whatsapp_plan_filter where kunci = 'TANGGAL STNK' and id_plan = '" + id + "'").Split('#')[0];
            string getTanggalPembelian = plan.getDataTable("select isi from trx_whatsapp_plan_filter where kunci = 'TANGGAL PEMBELIAN' and id_plan = '" + id + "'").Split('#')[0];
            string getServiceTerakhir = plan.getDataTable("select isi from trx_whatsapp_plan_filter where kunci = 'SERVICE TERAKHIR' and id_plan = '" + id + "'").Split('#')[0];
            string getJumlahService = plan.getDataTable("select isi from trx_whatsapp_plan_filter where kunci = 'JUMLAH SERVICE' and id_plan = '" + id + "'").Split('#')[0];
            

            if(getSumberData != "")
            {
                chkEditSumberData.Checked = true;
                ddlEditSumberData.SelectedValue = getSumberData.Trim();
                txtEditTanggalStnk.Text = getTanggalStnk.Trim();
                txtEditTanggalPembelian.Text = getTanggalPembelian.Trim();
                txtEditServiceTerakhir.Text = getServiceTerakhir.Trim();
                txtEditJumlahService.Text = getJumlahService.Trim();
                if (getSumberData == "H1")
                {
                    chkEditServiceTerakhir.Enabled = false;
                    ddlEditServiceTerakhir.Enabled = false;
                    txtEditServiceTerakhir.Enabled = false;

                    chkEditJumlahService.Enabled = false;
                    ddlEditJumlahService.Enabled = false;
                    txtEditJumlahService.Enabled = false;

                    if(getTanggalStnk != "")
                    {
                        chkEditTanggalStnk.Checked = true;
                    }

                    if(getTanggalPembelian != "")
                    {
                        chkEditTanggalBeli.Checked = true;
                    }
                }
                if(getSumberData == "H2")
                {
                    chkEditTanggalStnk.Enabled = false;
                    ddlEditTanggalStnk.Enabled = false;
                    txtEditTanggalStnk.Enabled = false;

                    chkEditTanggalBeli.Enabled = false;
                    ddlEditTanggalPembelian.Enabled = false;
                    txtEditTanggalPembelian.Enabled = false;

                    if(getServiceTerakhir != "")
                    {
                        chkEditServiceTerakhir.Checked = true;
                    }

                    if(getJumlahService != "")
                    {
                        chkEditJumlahService.Checked = true;
                    }
                }
                if(getSumberData == "ALL")
                {

                }
            }
        }

        #region BadgeEdit
        protected void btnEditTemplateName_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{name}";
        }

        protected void btnEditTemplateFullName_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{full_name}";
        }

        protected void btnEditTemplateTglBeli_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{tgl_beli}";
        }

        protected void btnEditTemplateTypeKendaraan_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{type_kendaraan}";
        }

        protected void btnEditTemplateTglStnk_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{tgl_stnk}";
        }

        protected void btnEditTemplatePlatNo_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{plat_no}";
        }

        protected void btnEditTemplateLastService_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{last_service}";
        }

        protected void btnEditTemplateBirthDate_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{birth_date}";
        }

        protected void btnEditTemplateBranchName_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{branch_name}";
        }

        protected void btnEditTemplateBranchAddress_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{branch_address}";
        }

        protected void btnEditTemplateBranchPhone_Click(object sender, EventArgs e)
        {
            txtEditPesan.Text = txtEditPesan.Text + "{branch_phone}";
        }
        #endregion

        protected void btnEditPlan_Click(object sender, EventArgs e)
        {
            string UploadFileName = "";
            string strIsMedia = "";
            string strUser = Convert.ToString(Session["UserID"]);
            if(Convert.ToString(Session["IsFileNameUpdate"]) != "")
            {
                UploadFileName = Convert.ToString(Session["IsFileNameUpdate"]);
            }
            else
            {
                UploadFileName = HiddenFileNameBeforUpdate.Value;
            }
            if (chkEditIsMedia.Checked)
            {
                strIsMedia = "1";
            }
            else
            {
                if(HiddenIsMedia.Value == "0")
                {
                    strIsMedia = "0";
                }
                else
                {
                    strIsMedia = "1";
                }
            }
            plan.UpdatePlan(hiddenplanedit.Value, txtEditPlan.Text, chkEditInterval.SelectedValue, chkEditBranch.SelectedValue, txtEditPesan.Text, chkEditBranch.SelectedItem.Text, strIsMedia, UploadFileName, txtEditShceduledPesan.Text, txtEditSecheduledWaktuPesan.Text, ddlEditDevices.SelectedValue, strUser);
            if(ddlEditSumberData.SelectedValue == "H1")
            {
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '"+ddlEditTanggalStnk.SelectedValue+"', isi = '"+txtEditTanggalStnk.Text+"' where kunci = 'TANGGAL STNK' and id_plan = '" + hiddenplanedit.Value + "'");
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '"+ddlEditTanggalPembelian.SelectedValue+"', isi = '"+txtEditTanggalPembelian.Text+"' where kunci = 'TANGGAL PEMBELIAN' and id_plan = '"+hiddenplanedit.Value+"'");
            }
            if (ddlEditSumberData.SelectedValue == "H2")
            {
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '"+ddlEditServiceTerakhir.SelectedValue+"', isi = '"+txtEditServiceTerakhir.Text+"'  where kunci = 'SERVICE TERAKHIR' and id_plan = '" + hiddenplanedit.Value + "'");
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '"+ddlEditJumlahService.SelectedValue+"', isi = '"+txtEditJumlahService.Text+"' where kunci = 'JUMLAH SERVICE' and id_plan = '" + hiddenplanedit.Value + "'");
            }
            if (ddlEditSumberData.SelectedValue == "ALL")
            {
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '" + ddlEditTanggalStnk.SelectedValue + "', isi = '" + txtEditTanggalStnk.Text + "' where kunci = 'TANGGAL STNK' and id_plan = '" + hiddenplanedit.Value + "'");
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '" + ddlEditTanggalPembelian.SelectedValue + "', isi = '" + txtEditTanggalPembelian.Text + "' where kunci = 'TANGGAL PEMBELIAN' and id_plan = '" + hiddenplanedit.Value + "'");
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '" + ddlEditServiceTerakhir.SelectedValue + "', isi = '" + txtEditServiceTerakhir.Text + "'  where kunci = 'SERVICE TERAKHIR' and id_plan = '" + hiddenplanedit.Value + "'");
                plan.getDataTable(@"update trx_whatsapp_plan_filter set operator = '" + ddlEditJumlahService.SelectedValue + "', isi = '" + txtEditJumlahService.Text + "' where kunci = 'JUMLAH SERVICE' and id_plan = '" + hiddenplanedit.Value + "'");
            }
            Session["IsFileNameUpdate"] = "";
        }

        protected void chkEditIsMedia_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEditIsMedia.Checked)
            {
                ajaxfileupload1.Visible = true;
                Label8.Visible = true;
            }
            else
            {
                ajaxfileupload1.Visible = false;
                Label8.Visible = false;
            }
        }

        protected void ajaxfileupload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string ChangeName = "";
            string FileName = e.FileName;
            ChangeName = Guid.NewGuid() + FileName;
            string fileNameWithPath = Server.MapPath("~/Media/") + ChangeName;
            ajaxfileupload1.SaveAs(fileNameWithPath);
            Session["IsFileNameUpdate"] = ChangeName;

        }
        private void ReduceImageSize(double scaleFactor, Stream sourcePath, string targetPath)
        {
            using (var image = System.Drawing.Image.FromStream(sourcePath))
            {
                var newWidth = (int)(image.Width * scaleFactor);
                var newHeight = (int)(image.Height * scaleFactor);
                var thumbnailImg = new Bitmap(newWidth, newHeight);
                var thumbGraph = Graphics.FromImage(thumbnailImg);
                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                thumbGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                var imageRectangle = new Rectangle(0, 0, newWidth, newHeight);
                thumbGraph.DrawImage(image, imageRectangle);
                thumbnailImg.Save(targetPath, image.RawFormat);
            }
        }

        protected void ddlViewAllBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlViewAllBranch.SelectedValue == "false")
            {
                BindData();
            }
            if(ddlViewAllBranch.SelectedValue == "true")
            {
                BindDataAllBranch();
            }
        }

        protected void ddlSessionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlSessionId.SelectedValue != "")
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
                                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('" + ddlSessionId.SelectedItem.Text + " tidak terhubung', 'warning', 'Harap hubungkan device terlebih dahulu');", true);
                                ddlSessionId.SelectedValue = "";
                            }
                        }
                    }
                }
                catch(Exception ex)
                {

                }
            }
        }
    }
}