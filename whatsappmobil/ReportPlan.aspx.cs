using OfficeOpenXml;
using OfficeOpenXml.Style;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
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
    public partial class ReportPlan : System.Web.UI.Page
    {
        FunctionReport Freport = new FunctionReport();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ddlViewAllBranch.SelectedValue == "false")
                {
                    BindDataReport();
                }
                else
                {
                    BindDataReportAll();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindDataReport();
            }
            else
            {
                BindDataReportAll();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringH2"];
            OracleConnection oracleConnection = new OracleConnection(ConnectionString);

            oracleConnection.Open();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Worksheets.Add("Report");
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets["Report"];
                ws.Column(1).Width = 14.43;
                DataTable dt = new DataTable();
                dt.Columns.Add("SENT_DATE");
                dt.Columns.Add("WA_NUMBER");
                //dt.Columns.Add("MESSAGE_CONTENT");
                dt.Columns.Add("STATUS");

                int rowIndex = 1;
                int colIndex = 1;

                List<string> SentDate = new List<string>();
                List<string> Number = new List<string>();
                List<string> Message = new List<string>();
                List<string> Status = new List<string>();

                string strGetData = "";
                OracleCommand cmdGetData = null;
                strGetData = "select sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, trxid, session_notic from trx_whatsapp_message";
                cmdGetData = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = cmdGetData.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(1))
                        {
                            SentDate.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            SentDate.Add("");
                        }

                        if (!dataReader.IsDBNull(4))
                        {
                            Number.Add(dataReader.GetString(4));
                        }
                        else
                        {
                            Number.Add("");
                        }

                        if (!dataReader.IsDBNull(6))
                        {
                            Status.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Status.Add("");
                        }
                    }
                }
                dataReader.Close();
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];
                    cell.Style.WrapText = false;
                    cell.Style.Font.Bold = true;
                    cell.AutoFitColumns();
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    var border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell.Value = dc.ColumnName;

                    colIndex++;
                }
                rowIndex++;
                for (int i = 0; i < Number.Count; i++)
                {
                    for (colIndex = 1; colIndex <= dt.Columns.Count; colIndex++)
                    {
                        if (colIndex == 1)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (SentDate[i]).ToString();
                        }
                        if (colIndex == 2)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (Number[i]).ToString();
                        }
                        if (colIndex == 3)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (Status[i]).ToString();
                        }
                    }
                    rowIndex++;
                }
                Byte[] bin = excelPackage.GetAsByteArray();
                string file = "";
                if (file == "")
                {
                    try
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("Content-Disposition", "attachment;filename=ExportMessage_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(bin);
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There is an error : " + ex + "')", true);
                    }
                }
            }
        }

        protected void gvReportPlan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                string strEval = e.CommandArgument.ToString();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Pop", "$('#ModalViewReport').modal('show');", true);
                if(ddlViewAllBranch.SelectedValue == "false")
                {
                    BindDataReportView(strEval.Split(' ')[0]);
                }
                else
                {
                    BindDataReportViewAllBranch(strEval.Split(' ')[0]);
                }
                hiddenTrxId.Value = strEval.Split(' ')[0];
                lblViewHeader.Text = "";
            }
            if (e.CommandName == "Resent")
            {
                string strEval = e.CommandArgument.ToString();
                Resent(strEval.Split(' ')[0]);
            }
            if(e.CommandName == "Export")
            {
                string strEval = e.CommandArgument.ToString();
                ExportDay(strEval.Split(' ')[0]);
            }
        }

        protected void gvReportPlan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReportPlan.PageIndex = e.NewPageIndex;
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindDataReport();
            }
            else
            {
                BindDataReportAll();
            }
        }

        protected void gvReportPlan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = Regex.Replace(e.Row.Cells[0].Text, txtSearch.Text.Trim(), delegate (Match match)
                {
                    return string.Format("<span style = 'background-color:#D9EDF7'>{0}</span>", match.Value);
                }, RegexOptions.IgnoreCase);
            }
            foreach (GridViewRow row in gvReportPlan.Rows)
            {
                string Status = row.Cells[1].Text;
                LinkButton button = (LinkButton)row.FindControl("btnResent");
                if (Status == "0")
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

        protected void btnSearchView_Click(object sender, EventArgs e)
        {
            //BindDataReportView(hiddenTrxId.Value);
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindDataReportView(hiddenTrxId.Value);
            }
            else
            {
                BindDataReportViewAllBranch(hiddenTrxId.Value);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {

        }

        protected void gvViewReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[6].Text == "Tidak Terkirim")
                {
                    e.Row.Attributes.CssStyle.Value = "background-color: #B7E5DD; color: black";
                }
            }
        }

        protected void gvViewReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvViewReport.PageIndex = e.NewPageIndex;
            //BindDataReportView(hiddenTrxId.Value);
            if (ddlViewAllBranch.SelectedValue == "false")
            {
                BindDataReportView(hiddenTrxId.Value);
            }
            else
            {
                BindDataReportViewAllBranch(hiddenTrxId.Value);
            }
        }

        private void BindDataReport()
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    string strQuey = "";
                    if (txtSearch.Text.Trim() == "")
                    {
                        strQuey = "where bb.create_by = '"+Convert.ToString(Session["UserID"])+"'";
                    }
                    else
                    {
                        strSearch = txtSearch.Text.Trim();
                        strQuey = "where bb.create_by = '"+Convert.ToString(Session["UserID"])+"' and aa.sent_date like to_date(:sentdate, 'dd/mm/yyyy')";
                    }
                    //cmd.CommandText = "select * from ( select aa.sent_date, aa.status_session, aa.status_session as col from trx_whatsapp_message aa " + strQuey + ") pivot(count(status_session) for col in ('0' as tidak_terkirim, '1' as terkirim, '2' as gagal))";
                    cmd.CommandText = "select * from ( select aa.sent_date, aa.status_session, aa.status_session as col from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid "+strQuey+ ") pivot(count(status_session) for col in ('0' as tidak_terkirim, '1' as terkirim, '2' as gagal)) order by sent_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("sentdate", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvReportPlan.DataSource = dt;
                        gvReportPlan.DataBind();
                    }
                }
            }
        }
        private void BindDataReportAll()
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    string strQuey = "";
                    if (txtSearch.Text.Trim() == "")
                    {
                        //strQuey = "where bb.create_by = '" + Convert.ToString(Session["UserID"]) + "'";
                    }
                    else
                    {
                        strSearch = txtSearch.Text.Trim();
                        strQuey = "where aa.sent_date like to_date(:sentdate, 'dd/mm/yyyy')";
                    }
                    //cmd.CommandText = "select * from ( select aa.sent_date, aa.status_session, aa.status_session as col from trx_whatsapp_message aa " + strQuey + ") pivot(count(status_session) for col in ('0' as tidak_terkirim, '1' as terkirim, '2' as gagal))";
                    cmd.CommandText = "select * from ( select aa.sent_date, aa.status_session, aa.status_session as col from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid " + strQuey + ") pivot(count(status_session) for col in ('0' as tidak_terkirim, '1' as terkirim, '2' as gagal)) order by sent_date desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("sentdate", strSearch));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvReportPlan.DataSource = dt;
                        gvReportPlan.DataBind();
                    }
                }
            }
        }

        private void BindDataReportView(string strTrxId)
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    string strSearchName = "";
                    if (txtSearchView.Text.Trim() == "" && txtSearchViewName.Text.Trim() == "")
                    {
                        strSearch = "%%";
                        strSearchName = "%%";
                    }
                    else
                    {
                        strSearch = "%" + txtSearchView.Text.Trim() + "%";
                        strSearchName = "%" + txtSearchViewName.Text.Trim() + "%";
                    }
                    //cmd.CommandText = "select sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, trxid, session_notic from trx_whatsapp_message where sent_date = to_date('" + strTrxId + "', 'dd/mm/yyyy') and wa_number like :wanumber AND message_content like :message order by status_session desc";
                    cmd.CommandText = "select aa.sender_id, aa.sent_date, aa.type_message, aa.message_title, aa.wa_number, aa.message_content, decode(aa.status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, aa.trxid, aa.session_notic from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where sent_date = to_date('" + strTrxId + "', 'dd/mm/yyyy') and wa_number like :wanumber AND message_content like :message and bb.create_by = '"+Convert.ToString(Session["UserID"])+"' order by status_session desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("wanumber", strSearch));
                    cmd.Parameters.Add(new OracleParameter("message", strSearchName));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvViewReport.DataSource = dt;
                        gvViewReport.DataBind();
                    }
                }
            }
        }
        private void BindDataReportViewAllBranch(string strTrxId)
        {
            using (OracleConnection con = new OracleConnection(ConnectionStringH2))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    string strSearch = "";
                    string strSearchName = "";
                    if (txtSearchView.Text.Trim() == "" && txtSearchViewName.Text.Trim() == "")
                    {
                        strSearch = "%%";
                        strSearchName = "%%";
                    }
                    else
                    {
                        strSearch = "%" + txtSearchView.Text.Trim() + "%";
                        strSearchName = "%" + txtSearchViewName.Text.Trim() + "%";
                    }
                    //cmd.CommandText = "select sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, trxid, session_notic from trx_whatsapp_message where sent_date = to_date('" + strTrxId + "', 'dd/mm/yyyy') and wa_number like :wanumber AND message_content like :message order by status_session desc";
                    cmd.CommandText = "select aa.sender_id, aa.sent_date, aa.type_message, aa.message_title, aa.wa_number, aa.message_content, decode(aa.status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, aa.trxid, aa.session_notic from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where sent_date = to_date('" + strTrxId + "', 'dd/mm/yyyy') and wa_number like :wanumber AND message_content like :message order by status_session desc";
                    cmd.Connection = con;
                    cmd.Parameters.Add(new OracleParameter("wanumber", strSearch));
                    cmd.Parameters.Add(new OracleParameter("message", strSearchName));
                    DataTable dt = new DataTable();
                    using (OracleDataAdapter sda = new OracleDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                        gvViewReport.DataSource = dt;
                        gvViewReport.DataBind();
                    }
                }
            }
        }

        private void Resent(string strTrxId)
        {
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringH2"];
            OracleConnection oracleConnection = new OracleConnection(ConnectionString);
            oracleConnection.Open();

            List<string> SenderId = new List<string>();
            List<string> SentDate = new List<string>();
            List<string> WhatsappNumber = new List<string>();
            List<string> Message = new List<string>();
            List<string> StatusSession = new List<string>();
            List<string> WaMedia = new List<string>();

            string strGetData = "";
            OracleCommand oracleCommand = null;
            strGetData = @"select distinct trx_whatsapp_message.sender_id, sent_date, type_message, message_title, wa_number, message_content, trx_whatsapp_message.status_session, trx_whatsapp_message.trxid, session_notic, nvl(trx_whatsapp_header.wa_media, ' ') wa_media, push_name from trx_whatsapp_message inner join trx_whatsapp_header on trx_whatsapp_message.trxid = trx_whatsapp_header.trxid where sent_date = to_date('" + strTrxId + "','dd/mm/yyyy') and trx_whatsapp_message.status_session = '0'";
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
                        SentDate.Add(dataReader.GetString(1));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(4))
                    {
                        WhatsappNumber.Add(dataReader.GetString(4));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(5))
                    {
                        Message.Add(dataReader.GetString(5));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(6))
                    {
                        StatusSession.Add(dataReader.GetString(6));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(9))
                    {
                        WaMedia.Add(dataReader.GetString(9));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                }
                dataReader.Close();
                for (int i = 0; i < WhatsappNumber.Count; i++)
                {
                    try
                    {
                        if (WaMedia[i] != " ")
                        {
                            string strFilePath = Server.MapPath("Media/" + WaMedia[i]);
                            using (var client = new HttpClient())
                            {
                                Sender sen = new Sender { sender = SenderId[i], number = WhatsappNumber[i], caption = Message[i], file = strFilePath };
                                client.BaseAddress = new Uri("http://192.168.100.1:9001/");
                                var response = client.PostAsJsonAsync("send-media", sen).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    Freport.UpdateStatusMessageReport(SentDate[i], WhatsappNumber[i]);
                                }
                                else
                                {
                                    if (response.StatusCode.ToString() == "422")
                                    {
                                        Freport.UpdateSessionNoticReport(SentDate[i], WhatsappNumber[i], "The number is not registered");
                                    }
                                }
                            }
                        }
                        else
                        {
                            using (var client = new HttpClient())
                            {
                                Sender sen = new Sender { sender = SenderId[i], number = WhatsappNumber[i], message = Message[i] };
                                client.BaseAddress = new Uri("http://192.168.100.1:9001/");
                                var response = client.PostAsJsonAsync("send-message", sen).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    Freport.UpdateStatusMessageReport(SentDate[i], WhatsappNumber[i]);
                                }
                                else
                                {
                                    if (response.StatusCode.ToString() == "422")
                                    {
                                        Freport.UpdateSessionNoticReport(SentDate[i], WhatsappNumber[i], "The number is not registered");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        private void ExportDay(string strDate)
        {
            string ConnectionString = ConfigurationManager.AppSettings["ConnectionStringH2"];
            OracleConnection oracleConnection = new OracleConnection(ConnectionString);

            oracleConnection.Open();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Worksheets.Add("Report");
                ExcelWorksheet ws = excelPackage.Workbook.Worksheets["Report"];
                ws.Column(1).Width = 14.43;
                DataTable dt = new DataTable();
                dt.Columns.Add("SENT_DATE");
                dt.Columns.Add("WA_NUMBER");
                //dt.Columns.Add("MESSAGE_CONTENT");
                dt.Columns.Add("STATUS");

                int rowIndex = 1;
                int colIndex = 1;

                List<string> SentDate = new List<string>();
                List<string> Number = new List<string>();
                List<string> Message = new List<string>();
                List<string> Status = new List<string>();

                string strGetData = "";
                OracleCommand cmdGetData = null;
                if(ddlViewAllBranch.SelectedValue == "false")
                {
                    strGetData = "select aa.sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(aa.status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, aa.trxid, session_notic, bb.create_by from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where create_by = '"+Convert.ToString(Session["UserID"])+"' and sent_date = to_date('" + strDate + "', 'DD/MM/YYYY')";
                }
                else
                {
                    strGetData = "select aa.sender_id, sent_date, type_message, message_title, wa_number, message_content, decode(aa.status_session, '0', 'Tidak Terkirim', '1', 'Terkirim', '2', 'Gagal') status_session, aa.trxid, session_notic, bb.create_by from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where sent_date = to_date('" + strDate + "', 'DD/MM/YYYY')";
                }
                cmdGetData = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = cmdGetData.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(1))
                        {
                            SentDate.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            SentDate.Add("");
                        }

                        if (!dataReader.IsDBNull(4))
                        {
                            Number.Add(dataReader.GetString(4));
                        }
                        else
                        {
                            Number.Add("");
                        }

                        if (!dataReader.IsDBNull(6))
                        {
                            Status.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Status.Add("");
                        }
                    }
                }
                dataReader.Close();
                foreach (DataColumn dc in dt.Columns)
                {
                    var cell = ws.Cells[rowIndex, colIndex];
                    cell.Style.WrapText = false;
                    cell.Style.Font.Bold = true;
                    cell.AutoFitColumns();
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    var fill = cell.Style.Fill;
                    fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    var border = cell.Style.Border;
                    border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell.Value = dc.ColumnName;

                    colIndex++;
                }
                rowIndex++;
                for (int i = 0; i < Number.Count; i++)
                {
                    for (colIndex = 1; colIndex <= dt.Columns.Count; colIndex++)
                    {
                        if (colIndex == 1)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (SentDate[i]).ToString();
                        }
                        if (colIndex == 2)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (Number[i]).ToString();
                        }
                        if (colIndex == 3)
                        {
                            var cell = ws.Cells[rowIndex, colIndex];

                            cell.Style.WrapText = false;
                            cell.Style.Font.Bold = false;
                            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            var border = cell.Style.Border;
                            border.BorderAround(ExcelBorderStyle.Thin);

                            cell.Value = (Status[i]).ToString();
                        }
                    }
                    rowIndex++;
                }
                Byte[] bin = excelPackage.GetAsByteArray();
                string file = "";
                if (file == "")
                {
                    try
                    {
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("Content-Disposition", "attachment;filename=ExportMessage_" + DateTime.Now.ToString("ddMMyyyyhhmmss") + ".xls");
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.BinaryWrite(bin);
                        Response.End();
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('There is an error : " + ex + "')", true);
                    }
                }
            }
        }

        protected void ddlViewAllBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ddlViewAllBranch.SelectedValue == "false")
            {
                BindDataReport();
            }
            else
            {
                BindDataReportAll();
            }
        }
    }
}