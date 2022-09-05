using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.sender;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class ImportMessage : System.Web.UI.Page
    {
        Function fcn = new Function();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnTemplateName_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{name}";
        }

        protected void btnTemplateFullName_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{full_name}";
        }

        protected void btnTemplateTglBeli_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{tgl_beli}";
        }

        protected void btnTemplateTypeKendaraan_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{type_kendaraan}";
        }

        protected void btnTemplateTglSTNK_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{tgl_stnk}";
        }

        protected void btnTemplatePlatNo_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{plat_no}";
        }

        protected void btnTemplateLastService_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{last_service}";
        }

        protected void btnTemplateBirthDate_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{birth_date}";
        }

        protected void btnTemplateBranchName_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{branch_name}";
        }

        protected void btnTemplateBranchAddress_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{branch_address}";
        }

        protected void btnTemplateBranchPhone_Click(object sender, EventArgs e)
        {
            txtMessage.Text = txtMessage.Text + "{branch_phone}";
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Regex findValueValidation = new Regex(@"{Name}|{Tgl_beli}|{Type_kendaraan}|{Tgl_stnk}|{Plat_no}|{Last_service}|{Birth_date}|{Branch_name}|{Branch_address}|{Branch_phone}|{Full_name}|{Tgl_Beli}|{Type_Kendaraan}|{Tgl_Stnk}|{Plat_No}|{Last_Service}|{Birth_Date}|{Branch_Name}|{Branch_Address}|{Branch_Phone}|{Full_Name}");
            Match findMatchValidation = findValueValidation.Match(txtMessage.Text);
            if (findMatchValidation.Success)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('PARAMETER {.....} TIDAK SESUAI, HURUF KECIL SEMUA exp: {full_name}!!!', 'warning');", true);
                return;
            }
            else
            {
                HSSFWorkbook hssfwb;
                #region Alert
                if (uploadfilemessage.FileName == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('File Kosong', 'warning');", true);
                    return;
                }
                if (txtTitle.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Judul Kosong', 'info');", true);
                    return;
                }
                if (txtScheduled.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Masukan Tanggal Scheduled', 'info');", true);
                    return;
                }
                if (txtScheduledTime.Text == "")
                {
                    //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Masukan Waktu Scheduled', 'info');", true);
                    //return;
                }
                if (txtMessage.Text == "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Masukan Pesan', 'info');", true);
                    return;
                }
                #endregion

                string TrxId = "Import." + DateTime.Now.ToString("ddMMyyyyhhmmss");
                string strUser = Convert.ToString(Session["UserID"]);
                string filename = Server.MapPath("~\\Upload") + "\\" + Guid.NewGuid() + "_" + uploadfilemessage.FileName;
                uploadfilemessage.SaveAs(filename);

                string filenameMedia = "";
                string changeName = "";
                string ismedia = "";
                string tanggal = txtScheduled.Text.Split(' ')[0];
                if (uploadMedia.HasFile)
                {
                    ismedia = "1";
                    //filenameMedia = Path.GetFileName(uploadMedia.PostedFile.FileName);
                    //changeName = Guid.NewGuid() + filenameMedia;
                    //uploadMedia.PostedFile.SaveAs(Server.MapPath("~/Media/" + changeName));

                    string filenamecomprese = Path.GetFileName(uploadMedia.PostedFile.FileName);
                    changeName = Guid.NewGuid() + filenamecomprese;
                    string targetPath = Server.MapPath("~/Media/" + changeName);
                    Stream strm = uploadMedia.PostedFile.InputStream;
                    var targetFile = targetPath;
                    ReduceImageSize(0.5, strm, targetFile);
                }
                else
                {
                    ismedia = "0";
                }

                if (uploadfilemessage.HasFile)
                {
                    try
                    {
                        fcn.InsertTrxMessageHeader(TrxId, txtTitle.Text, tanggal, "", changeName, ismedia, txtScheduled.Text, txtScheduledTime.Text, strUser);

                        using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            hssfwb = new HSSFWorkbook(file);
                            ISheet sheet = hssfwb.GetSheetAt(0);
                            for (int row = 1; row <= sheet.LastRowNum; row++)
                            {
                                if (sheet.GetRow(row) != null && sheet.GetRow(row).GetCell(0) != null)
                                {
                                    string WhatsappNumber = Convert.ToString(sheet.GetRow(row).GetCell(0)).Trim();
                                    string Name = Convert.ToString(sheet.GetRow(row).GetCell(1)).Trim();
                                    string FullName = Convert.ToString(sheet.GetRow(row).GetCell(2)).Trim();
                                    string TglBeli = Convert.ToString(sheet.GetRow(row).GetCell(3)).Trim();
                                    string Type = Convert.ToString(sheet.GetRow(row).GetCell(4)).Trim();
                                    string TglSTNK = Convert.ToString(sheet.GetRow(row).GetCell(5)).Trim();
                                    string PlatNo = Convert.ToString(sheet.GetRow(row).GetCell(6)).Trim();
                                    string LastService = Convert.ToString(sheet.GetRow(row).GetCell(7)).Trim();
                                    string BirthDate = Convert.ToString(sheet.GetRow(row).GetCell(8)).Trim();
                                    string BranchName = Convert.ToString(sheet.GetRow(row).GetCell(9)).Trim();
                                    string BranchAddress = Convert.ToString(sheet.GetRow(row).GetCell(10)).Trim();
                                    string BranchPhone = Convert.ToString(sheet.GetRow(row).GetCell(11)).Trim();

                                    string strMessageReplace = "";
                                    Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}|{full_name}");
                                    Match findMatch = findValue.Match(txtMessage.Text);
                                    if (findMatch.Success)
                                    {
                                        strMessageReplace = txtMessage.Text.Replace("{name}", Name).Replace("{tgl_beli}", TglBeli).Replace("{type_kendaraan}", Type).Replace("{tgl_stnk}", TglSTNK).Replace("{plat_no}", PlatNo).Replace("{last_service}", LastService).Replace("{birth_date}", BirthDate).Replace("{branch_name}", BranchName).Replace("{branch_address}", BranchAddress).Replace("{branch_phone}", BranchPhone).Replace("{full_name}", FullName);
                                    }
                                    else
                                    {
                                        strMessageReplace = txtMessage.Text;
                                    }
                                    if (WhatsappNumber.Length < 8)
                                    {

                                    }
                                    else
                                    {
                                        fcn.InsertWhatsappImport(ddlSessionId.SelectedValue, txtScheduled.Text, "IMPORT", txtTitle.Text, WhatsappNumber, strMessageReplace, "0", TrxId);
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Import Sukses', 'success');", true);
                    }
                    catch (Exception ex)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('" + ex + "', 'error');", true);
                    }
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnDownloadSec_Click(object sender, EventArgs e)
        {
            try
            {
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "attachment; filename=WhatsappImport.xls");
                Response.TransmitFile(Server.MapPath("~/assets/WhatsappImport.xls"));
                Response.End();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('" + ex + "', 'error');", true);
            }
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

        protected void ddlSessionId_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSessionId.SelectedValue != "")
            {
                try
                {
                    #region Baileys
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri("http://localhost:8000/sessions/status/?id="+ddlSessionId.SelectedValue+"");
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
                                    if (status != "connected")
                                    {
                                        //ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alerterror('Connected', 'info');", true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('"+ddlSessionId.SelectedItem.Text+" tidak terhubung', 'warning', 'Harap hubungkan device terlebih dahulu');", true);
                            ddlSessionId.SelectedValue = "";
                        }
                    }
                    #endregion

                    #region Whatsapp-web.js
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
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('" + ddlSessionId.SelectedItem.Text + " tidak terhubung', 'warning', 'Harap hubungkan device terlebih dahulu');", true);
                    //            ddlSessionId.SelectedValue = "";
                    //        }
                    //    }
                    //}
                    #endregion
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('ERROR', 'error');", true);
                }
            }
        }
    }
}