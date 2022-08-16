using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using whatsappmobil.ssl;

namespace whatsappmobil.scheduler
{
    public class ScheduledPlan : IJob
    {
        FunctionPlan plan = new FunctionPlan();
        public async Task Execute(IJobExecutionContext context)
        {
            ScheduledLoopPlan();
        }
        private void ScheduledLoopPlan()
        {
            OracleConnection oracleConnection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]);
            oracleConnection.Open();
            try
            {
                #region List
                List<string> id = new List<string>();
                List<string> plantitle = new List<string>();
                List<string> branch = new List<string>();
                List<string> message = new List<string>();
                List<string> planstatus = new List<string>();
                List<string> branchform = new List<string>();
                List<string> ismedia = new List<string>();
                List<string> medianame = new List<string>();
                List<string> scheduledate = new List<string>();
                List<string> scheduletime = new List<string>();
                List<string> schedulemessage = new List<string>();
                List<string> schedulemessagetime = new List<string>();
                List<string> sessiondevices = new List<string>();
                List<string> create_by = new List<string>();
                string date = DateTime.Now.ToString("dd/MM/yyyy");
                #endregion

                OracleCommand oracleCommand = null;
                string strGetData = @"select id_plan, plan_title, interval, branch_id, create_date, message_content, is_survey, branch_name, session_devices, sent_date, plan_status, branch_from, is_media, nvl(media_name, ' ') media_name, create_by from trx_whatsapp_plan where plan_status = '0' order by create_date desc";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                        {
                            id.Add(dataReader.GetString(0));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(1))
                        {
                            plantitle.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(5))
                        {
                            message.Add(dataReader.GetString(5));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            branch.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(12))
                        {
                            ismedia.Add(dataReader.GetString(12));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(13))
                        {
                            medianame.Add(dataReader.GetString(13));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(8))
                        {
                            sessiondevices.Add(dataReader.GetString(8));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(14))
                        {
                            create_by.Add(dataReader.GetString(14));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        string strGatPlanSUMBERDATA = plan.GetPlanFilter("select isi from trx_whatsapp_plan_filter where id_plan = '" + id[i] + "' and kunci = 'SUMBER DATA'").Split('#')[0];

                        if (strGatPlanSUMBERDATA == "H1")
                        {
                            try
                            {
                                GenerateFromH1(id[i], message[i], branch[i], strGatPlanSUMBERDATA, plantitle[i], date, medianame[i], ismedia[i], "", "", sessiondevices[i], create_by[i]);
                                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + id[i] + "'");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (strGatPlanSUMBERDATA == "H2")
                        {
                            try
                            {
                                GenerateFromH2(id[i], message[i], branch[i], strGatPlanSUMBERDATA, plantitle[i], date, medianame[i], ismedia[i], "", "", sessiondevices[i], create_by[i]);
                                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + id[i] + "'");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (strGatPlanSUMBERDATA == "ALL")
                        {
                            try
                            {
                                string trxid = "Generate.ALL." + DateTime.Now.ToString("ddMMyyyyhhmmss");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
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

        private void GenerateFromH1(string strId, string strMessage, string strBranch, string strSumberdata, string strMessageTitle, string strScheduledDate, string strMedia, string strIsmedia, string strscheduledmessage, string strscheduledmessagetime, string strSessionDevices, string strCreateBy)
        {
            OracleConnection oracleConnection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]);
            oracleConnection.Open();
            try
            {
                #region SumberData
                string strGatPlanSUMBERDATA = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'SUMBER DATA'");
                string strGetPlanTANGGALSTNK = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'TANGGAL STNK'");
                string strGetPlanTANGGALPEMBELIAN = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'TANGGAL PEMBELIAN'");
                #endregion
                #region H1
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

                #region List
                List<string> SenderId = new List<string>();
                List<string> nick_Name = new List<string>();
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
                            Cust_name.Add(dataReader.GetString(0));
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
                        if (!dataReader.IsDBNull(12))
                        {
                            nick_Name.Add(dataReader.GetString(12));
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
                            Match findMatch = findValue.Match(strMessage);
                            if (findMatch.Success)
                            {
                                strMessageReplace = strMessage.Replace("{full_name}", Cust_name[i]).Replace("{tgl_beli}", Tanggal_Beli[i]).Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", Tanggal_STNK[i]).Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", "").Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]).Replace("{name}", nick_Name[i]);
                            }
                            else
                            {
                                strMessageReplace = strMessage;
                            }
                            string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + trxid + "' and wa_number = '" + Number[i] + "'");
                            if (CheckNumber != "")
                            {

                            }
                            else
                            {
                                try
                                {
                                    plan.InsertGeneratePlan(strSessionDevices, strScheduledDate, strSumberdata, strMessageTitle, Number[i], strMessageReplace, trxid, Doc_Ref[i]);
                                }
                                catch(Exception ex)
                                {

                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                plan.InsertTrxMessageHeader(trxid, strMessageTitle, strScheduledDate, "", strMedia, strIsmedia, strscheduledmessage, strscheduledmessagetime, strId, strCreateBy);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
        private void GenerateFromH2(string strId, string strMessage, string strBranch, string strSumberdata, string strMessageTitle, string strScheduledDate, string strMedia, string strIsmedia, string strscheduledmessage, string strscheduledmessagetime, string strSessionDevices, string strCreateBy)
        {
            OracleConnection oracleConnection = new OracleConnection(ConfigurationManager.AppSettings["ConnectionStringH2"]);
            oracleConnection.Open();
            try
            {
                #region SumberData
                //SumberData
                string strGetPlanSERVICETERAKHIR = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'SERVICE TERAKHIR'");
                string strGetPlanJUMLAHSERVICE = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'JUMLAH SERVICE'");
                string strGetPlanJUMLAHSERVICE2 = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'JUMLAH SERVICE 2'");
                string strGetPlanBELUMKPB = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'BELUM KPB'");
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
                    QrSERVICE = @"lastorder " + oprSERVICETERAKHIR + " (trunc(sysdate-" + vleSERVICETERAKHIR + ")) and INVOICE_DATE " + oprSERVICETERAKHIR + " (trunc(sysdate-" + vleSERVICETERAKHIR + "))";
                }
                //Jumlah Service
                string oprJUMLAHSERVICE = "";
                string vleJUMLAHSERVICE = "";
                string QrJUMLAHSERVICE = "";
                if (strGetPlanJUMLAHSERVICE != "")
                {
                    oprJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[0];
                    vleJUMLAHSERVICE = strGetPlanJUMLAHSERVICE.Split('#')[1];
                    if (strGetPlanSERVICETERAKHIR != "")
                    {
                        QrJUMLAHSERVICE = @"and totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
                    }
                    else
                    {
                        QrJUMLAHSERVICE = @"totalservice " + oprJUMLAHSERVICE + " " + vleJUMLAHSERVICE + "";
                    }
                }
                #endregion

                #region List
                List<string> Full_Name = new List<string>();
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
                #endregion

                string trxid = "Generate.H2." + DateTime.Now.ToString("ddMMyyyyhhmmss");
                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select ownername, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(phone1, ':62', '0'), ':0','0'), ' ', ''), '-', ''), '+62', '0'), '+62 ', '0'), '62', '0'),';','0') phone1, wo_branch_id, vm_description, no_polisi, lastorder, vo_birthday, branch_name, address1, phone_no1, totalservice, woid, nickname FROM VI_PLAN_H2 where "+QrSERVICE+" "+QrJUMLAHSERVICE+ " and phone1 is not null";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(0))
                        {
                            Full_Name.Add(dataReader.GetString(0));
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
                            Match findMatch = findValue.Match(strMessage);
                            if (findMatch.Success)
                            {
                                strMessageReplace = strMessage.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", "").Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", "").Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", Last_Service[i]).Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]).Replace("{full_name}", Full_Name[i]);
                            }
                            else
                            {
                                strMessageReplace = strMessage;
                            }
                            plan.InsertGeneratePlan(strSessionDevices, strScheduledDate, strSumberdata, strMessageTitle, Number[i], strMessageReplace, trxid, Doc_Ref[i]);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                plan.InsertTrxMessageHeader(trxid, strMessageTitle, strScheduledDate, "", strMedia, strIsmedia, strscheduledmessage, strscheduledmessagetime, strId, strCreateBy);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
    }
}