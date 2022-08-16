using Oracle.ManagedDataAccess.Client;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using whatsappmobil.ssl;
using whatsappmobil.sender;

namespace whatsappmobil.ssl
{
    public class Scheduler : IJob
    {
        FunctionPlan plan = new FunctionPlan();
        string ConnectionStringH2 = ConfigurationManager.AppSettings["ConnectionStringH2"];
        public async Task Execute(IJobExecutionContext context)
        {
            //ScheduledPlan();
            //ScheduledMessagePlan();
            ScheduledGroup();
        }
        public void ScheduledPlan()
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

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
            #endregion

            string strGetData = "";
            OracleCommand oracleCommand = null;
            strGetData = @"select id_plan, plan_title, interval, branch_id, create_date, message_content, is_survey, branch_name, session_devices, sent_date, plan_status, branch_from, is_media, nvl(media_name, ' ') media_name, to_char(schedule_date, 'DD/MM/YYYY') schedule_date, schedule_time, to_char(scheduled_message, 'DD/MM/YYYY') scheduled_message, scheduled_message_time from trx_whatsapp_plan where plan_status = '0' order by create_date desc";
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
                    if (!dataReader.IsDBNull(14))
                    {
                        scheduledate.Add(dataReader.GetString(14));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(15))
                    {
                        scheduletime.Add(dataReader.GetString(15));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(16))
                    {
                        schedulemessage.Add(dataReader.GetString(16));
                    }
                    else
                    {
                        Console.WriteLine("No rows found");
                    }
                    if (!dataReader.IsDBNull(17))
                    {
                        schedulemessagetime.Add(dataReader.GetString(17));
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
                }
                dataReader.Close();
                for (int i = 0; i < id.Count; i++)
                {
                    string strGatPlanSUMBERDATA = plan.GetPlanFilter("select isi from trx_whatsapp_plan_filter where id_plan = '" + id[i] + "' and kunci = 'SUMBER DATA'");
                    string sumberdata = strGatPlanSUMBERDATA.Split('#')[0];
                    DateTime today = DateTime.UtcNow.Date;
                    string datetime = today.ToString("dd/MM/yyyy");
                    string times = DateTime.Now.ToString("HH:mm");
                    string resultdate = datetime + " " + times;

                    string scheduled = scheduledate[i];
                    string time = scheduletime[i].Replace(":", ".");
                    string result = scheduled + " " + time;

                    string strB = branch[i].Insert(0, "'");
                    string strR = strB.Replace(",", "', '");
                    string strA = strR.Insert(strR.Length - 0, "'");

                    if (resultdate == result)
                    {
                        if (sumberdata == "H1")
                        {
                            try
                            {
                                GenerateFromH1(id[i], message[i], strA, sumberdata, plantitle[i], scheduledate[i], medianame[i], ismedia[i], schedulemessage[i], schedulemessagetime[i], sessiondevices[i]);
                                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + id[i] + "'");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (sumberdata == "H2")
                        {
                            try
                            {
                                GenerateFromH2(id[i], message[i], strA, sumberdata, plantitle[i], scheduledate[i], medianame[i], ismedia[i], schedulemessage[i], schedulemessagetime[i], sessiondevices[i]);
                                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + id[i] + "'");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        if (sumberdata == "All")
                        {
                            try
                            {
                                GenerateFromAll(id[i], message[i], strA, sumberdata, plantitle[i], scheduledate[i], medianame[i], ismedia[i], schedulemessage[i], schedulemessagetime[i], sessiondevices[i]);
                                plan.ExecuteQuery("update trx_whatsapp_plan set plan_status = '1' where id_plan = '" + id[i] + "'");
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            oracleConnection.Close();
        }
        private void GenerateFromH1(string strId, string strMessage, string strBranch, string strSumberdata, string strMessageTitle, string strScheduledDate, string strMedia, string strIsmedia, string strscheduledmessage, string strscheduledmessagetime, string strSessionDevices)
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
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
                if (strGetPlanTANGGALSTNK != "=##" && strGetPlanTANGGALSTNK != "<##")
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
                if (strGetPlanTANGGALPEMBELIAN != "=##" && strGetPlanTANGGALPEMBELIAN != "<##")
                {
                    oprTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[0];
                    vleTANGGALPEMBELIAN = strGetPlanTANGGALPEMBELIAN.Split('#')[1];
                    QrPEMBELIAN = @"and trunc(sysdate) " + oprTANGGALPEMBELIAN + " trunc(do_date) + " + vleTANGGALPEMBELIAN + "";
                }
                #endregion



                #region List
                List<string> SenderId = new List<string>();
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

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"SELECT distinct ccmsmobil.vi_datapenjualanh1.CUST_NAME, REPLACE(REPLACE(REPLACE(REPLACE(ccmsmobil.vi_datapenjualanh1.PHONE_NO1, ' ', ''), '-', ''), '+62', '0'), '+62 ', '0') AS PHONE_NO1, ccmsmobil.vi_datapenjualanh1.BRANCH_ID, do_date, product_name, stnk_receive_date, police_no, cabang, BIRTH_DATE, bintangdbamobil.mst_branch.ADDRESS1, bintangdbamobil.mst_branch.phone_no1 as branch_phone, ccmsmobil.vi_datapenjualanh1.do_id FROM ccmsmobil.vi_datapenjualanh1 INNER JOIN ccmsmobil.vi_selectcustomerh1 ON ccmsmobil.vi_datapenjualanh1.cust_id = ccmsmobil.vi_selectcustomerh1.cust_id INNER JOIN bintangdbamobil.mst_branch on ccmsmobil.vi_datapenjualanh1.branch_id = bintangdbamobil.mst_branch.branch_id where ccmsmobil.vi_datapenjualanh1.branch_id in(" + strBranch + ")" + QrSTNK + "" + QrPEMBELIAN + "";
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
                    }
                    dataReader.Close();
                    for (int i = 0; i < Number.Count; i++)
                    {
                        try
                        {
                            string strMessageReplace = "";
                            Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}");
                            Match findMatch = findValue.Match(strMessage);
                            if (findMatch.Success)
                            {
                                strMessageReplace = strMessage.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", Tanggal_Beli[i]).Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", Tanggal_STNK[i]).Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", "").Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]);
                            }
                            else
                            {
                                strMessageReplace = strMessage;
                            }
                            string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + strId + "' and wa_number = '" + Number[i] + "'");
                            if (CheckNumber != "")
                            {

                            }
                            else
                            {
                                plan.InsertGeneratePlan(strSessionDevices, strScheduledDate, strSumberdata, strMessageTitle, Number[i], strMessageReplace, strId, Doc_Ref[i]);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //plan.InsertTrxMessageHeader(strId, strMessageTitle, strScheduledDate, "", strMedia, strIsmedia, strscheduledmessage, strscheduledmessagetime);
                //oracleConnection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
        private void GenerateFromH2(string strId, string strMessage, string strBranch, string strSumberdata, string strMessageTitle, string strScheduledDate, string strMedia, string strIsmedia, string strscheduledmessage, string strscheduledmessagetime, string strSessionDevices)
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
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

                #region List
                List<string> SenderId = new List<string>();
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

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"SELECT VO_NAME, REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(VO_PHONE_1, ':62', '0'), ':0','0'), ' ', ''), '-', ''), '+62', '0'), '+62 ', '0'), '62', '0'),';','0') AS VO_PHONE_1, VO_BRANCH_ID, VM_DESCRIPTION, WO_REGISTRATION_NUMBER, VO_LAST_ORDER, VO_BIRTHDAY, BRANCH_NAME, bintangdbamobil.MST_BRANCH.ADDRESS1, bintangdbamobil.MST_BRANCH.PHONE_NO1 AS BRANCH_PHONE, ccmsmobil.VI_WORKORDERH2.WO_ID FROM ccmsmobil.VI_SELECTCUSTOMERH2 INNER JOIN ccmsmobil.VI_WORKORDERH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_WORKORDERH2.WO_OWNER_ID INNER JOIN bintangdbamobil.MST_BRANCH ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_BRANCH_ID = bintangdbamobil.MST_BRANCH.BRANCH_ID INNER JOIN ccmsmobil.VI_SELECTVEHICLEH2 ON ccmsmobil.VI_SELECTCUSTOMERH2.VO_ID = ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_OWNER_ID INNER JOIN ccmsmobil.MST_VEHICLEMODEL ON ccmsmobil.VI_SELECTVEHICLEH2.VEHICLE_MODEL_CODE = ccmsmobil.MST_VEHICLEMODEL.VM_MODEL_CODE where vo_branch_id in(" + strBranch + ")" + QrSERVICE + "" + QrJUMLAHSERVICE + "";
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
                            Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}");
                            Match findMatch = findValue.Match(strMessage);
                            if (findMatch.Success)
                            {
                                strMessageReplace = strMessage.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", "").Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", "").Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", Last_Service[i]).Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]);
                            }
                            else
                            {
                                strMessageReplace = strMessage;
                            }
                            string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + strId + "' and wa_number = '"+Number[i]+"'");
                            if (CheckNumber != "")
                            {

                            }
                            else
                            {
                                plan.InsertGeneratePlan(strSessionDevices, strScheduledDate, strSumberdata, strMessageTitle, Number[i], strMessageReplace, strId, Doc_Ref[i]);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //plan.InsertTrxMessageHeader(strId, strMessageTitle, strScheduledDate, "", strMedia, strIsmedia, strscheduledmessage, strscheduledmessagetime);
                //oracleConnection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
        private void GenerateFromAll(string strId, string strMessage, string strBranch, string strSumberdata, string strMessageTitle, string strScheduledDate, string strMedia, string strIsmedia, string strscheduledmessage, string strscheduledmessagetime, string strSessionDevices)
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();
            try
            {
                #region SumberData
                //SumberData
                string strGatPlanSUMBERDATA = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'SUMBER DATA'");
                string strGetPlanTANGGALSTNK = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'TANGGAL STNK'");
                string strGetPlanTANGGALPEMBELIAN = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'TANGGAL PEMBELIAN'");
                string strGetPlanSERVICETERAKHIR = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'SERVICE TERAKHIR'");
                string strGetPlanJUMLAHSERVICE = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'JUMLAH SERVICE'");
                string strGetPlanJUMLAHSERVICE2 = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'JUMLAH SERVICE 2'");
                string strGetPlanBELUMKPB = plan.GetPlanFilter("select operator, isi from trx_whatsapp_plan_filter where id_plan = '" + strId + "' and kunci = 'BELUM KPB'");
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

                #region List
                List<string> SenderId = new List<string>();
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
                #endregion List

                string strGetData = "";
                OracleCommand oracleCommand = null;
                #region DataQuery
                strGetData = "SELECT distinct ccmsmobil.vi_datapenjualanh1.CUST_NAME, " +
                    "REPLACE(REPLACE(REPLACE(REPLACE(ccmsmobil.vi_datapenjualanh1.PHONE_NO1, ' ', ''), '-', ''), '+62', '0'), '+62 ', '0') AS PHONE_NO1, " +
                    "ccmsmobil.vi_datapenjualanh1.BRANCH_ID, product_name, police_no, BIRTH_DATE, cabang, bintangdbamobil.MST_BRANCH.ADDRESS1, " +
                    "bintangdbamobil.MST_BRANCH.phone_no1 as branch_phone, to_char(do_date, 'dd/mm/yyyy') do_date, to_char(stnk_receive_date, 'dd/mm/yyyy') as stnk_receive_date, " +
                    "nvl(to_char('', 'dd/mm/yyyy'), '') as VO_LAST_ORDER, ccmsmobil.vi_datapenjualanh1.do_id as DOC_REF " +
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
                    "ccmsmobil.VI_WORKORDERH2.WO_ID as DOC_REF " +
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
                    }
                    dataReader.Close();
                    for (int i = 0; i < Number.Count; i++)
                    {
                        try
                        {
                            string strMessageReplace = "";
                            Regex findValue = new Regex(@"{name}|{tgl_beli}|{type_kendaraan}|{tgl_stnk}|{plat_no}|{last_service}|{birth_date}|{branch_name}|{branch_address}|{branch_phone}");
                            Match findMatch = findValue.Match(strMessage);
                            if (findMatch.Success)
                            {
                                strMessageReplace = strMessage.Replace("{name}", Cust_name[i]).Replace("{tgl_beli}", Tanggal_Beli[i]).Replace("{type_kendaraan}", Type_Kendaraan[i]).Replace("{tgl_stnk}", Tanggal_STNK[i]).Replace("{plat_no}", Plat_No[i]).Replace("{last_service}", Last_Service[i]).Replace("{birth_date}", Birth_Date[i]).Replace("{branch_name}", Branch_Name[i]).Replace("{branch_address}", Branch_Address[i]).Replace("{branch_phone}", Branch_Phone[i]);
                            }
                            else
                            {
                                strMessageReplace = strMessage;
                            }
                            string CheckNumber = plan.GetPlanFilter("select wa_number from trx_whatsapp_message where trxid = '" + strId + "' and wa_number = '" + Number[i] + "'");
                            if (CheckNumber != "")
                            {

                            }
                            else
                            {
                                plan.InsertGeneratePlan(strSessionDevices, strScheduledDate, strSumberdata, strMessageTitle, Number[i], strMessageReplace, strId, Doc_Ref[i]);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //plan.InsertTrxMessageHeader(strId, strMessageTitle, strScheduledDate, "", strMedia, strIsmedia, strscheduledmessage, strscheduledmessagetime);
                //oracleConnection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
        private void ScheduledMessagePlan()
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

            try
            {
                List<string> id = new List<string>();
                List<string> title = new List<string>();
                List<string> status = new List<string>();
                List<string> media = new List<string>();
                List<string> ismedia = new List<string>();
                List<string> scheduledate = new List<string>();
                List<string> scheduletime = new List<string>();

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select trxid, wa_header, wa_date, decode(status_session, '0', 'Open', '1', 'Close') as status_session, sender_id, to_char(scheduled_date, 'DD/MM/YYYY') scheduled_date, scheduled_time, is_media, nvl(wa_media, ' ') wa_media from trx_whatsapp_header order by create_date desc";
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
                            title.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            status.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(5))
                        {
                            scheduledate.Add(dataReader.GetString(5));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(6))
                        {
                            scheduletime.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(7))
                        {
                            ismedia.Add(dataReader.GetString(7));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(8))
                        {
                            media.Add(dataReader.GetString(8));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        DateTime today = DateTime.UtcNow.Date;
                        string datetime = today.ToString("dd/MM/yyyy");
                        string times = DateTime.Now.ToString("HH:mm");
                        string resultdate = datetime + " " + times;

                        string scheduled = scheduledate[i];
                        string time = scheduletime[i].Replace(":", ".");
                        string result = scheduled + " " + time;

                        if (resultdate == result)
                        {
                            FetchMessage(id[i], media[i]);
                            plan.ExecuteQuery(@"update trx_whatsapp_header set status_session = '1' where trxid = '" + id[i] + "'");
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
        private void FetchMessage(string strid, string strmedia)
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();
            try
            {
                List<string> id = new List<string>();
                List<string> senderid = new List<string>();
                List<string> type = new List<string>();
                List<string> messagetitle = new List<string>();
                List<string> number = new List<string>();
                List<string> message = new List<string>();
                List<string> status = new List<string>();

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select * from trx_whatsapp_message where trxid = '" + strid + "' and status_session = '0'";
                oracleCommand = new OracleCommand(strGetData, oracleConnection);
                OracleDataReader dataReader = oracleCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        if (!dataReader.IsDBNull(7))
                        {
                            id.Add(dataReader.GetString(7));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(0))
                        {
                            senderid.Add(dataReader.GetString(0));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(2))
                        {
                            type.Add(dataReader.GetString(2));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            messagetitle.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(4))
                        {
                            number.Add(dataReader.GetString(4));
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
                        if (!dataReader.IsDBNull(6))
                        {
                            status.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        if (strmedia == " ")
                        {
                            ScheduleSendChat(senderid[i], number[i], message[i], id[i]);
                        }
                        else
                        {
                            string filefile = Path.Combine(HttpRuntime.AppDomainAppPath, @"Media\" + strmedia);
                            ScheduleSendMedia(senderid[i], number[i], message[i], filefile, id[i]);
                        }
                    }
                }
                //oracleConnection.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }

        }
        private void ScheduleSendChat(string senderid, string number, string message, string strid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderSingleChat SenderSingleChat = new SenderSingleChat { sender = senderid, number = number, message = message };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-message");
                    var response = client.PostAsJsonAsync("", SenderSingleChat).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        plan.getDataTable(@"update trx_whatsapp_message set status_session = '1' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            plan.getDataTable(@"update trx_whatsapp_message set session_notic = 'number is not registered' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                plan.getDataTable(@"update trx_whatsapp_message set session_notic = '" + ex + "' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trx_id = '" + strid + "'");
            }
        }
        private void ScheduleSendMedia(string senderid, string number, string caption, string file, string strid)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderSingleChatMedia SenderSingleChatMedia = new SenderSingleChatMedia { sender = senderid, number = number, caption = caption, file = file };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-media");
                    var response = client.PostAsJsonAsync("", SenderSingleChatMedia).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        plan.getDataTable(@"update trx_whatsapp_message set status_session = '1' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            plan.getDataTable(@"update trx_whatsapp_message set session_notic = 'number is not registered' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trxid = '" + strid + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                plan.getDataTable(@"update trx_whatsapp_message set session_notic = '" + ex + "' where sender_id = '" + senderid + "' and wa_number = '" + number + "' and trx_id = '" + strid + "'");
            }
        }
        private void ScheduledGroup()
        {
            OracleConnection oracleConnection = new OracleConnection(ConnectionStringH2);
            oracleConnection.Open();

            try
            {
                #region List
                List<string> id = new List<string>();
                List<string> title = new List<string>();
                List<string> groupname = new List<string>();
                List<string> sender = new List<string>();
                List<string> message = new List<string>();
                List<string> branch = new List<string>();
                List<string> ismedia = new List<string>();
                List<string> media = new List<string>();
                List<string> scheduledate = new List<string>();
                List<string> scheduledtime = new List<string>();
                #endregion

                string strGetData = "";
                OracleCommand oracleCommand = null;
                strGetData = @"select trxid, title_blast, group_name, sender_id, message_content, branch_id, is_media, create_date, create_by, notification, blast_date, blast_status, media_name, to_char(scheduled_date, 'DD/MM/YYYY') scheduled_date, scheduled_time from trx_whatsapp_message_group where blast_status = '0' order by create_date desc";
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
                            title.Add(dataReader.GetString(1));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(2))
                        {
                            groupname.Add(dataReader.GetString(2));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(3))
                        {
                            sender.Add(dataReader.GetString(3));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(4))
                        {
                            message.Add(dataReader.GetString(4));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(5))
                        {
                            branch.Add(dataReader.GetString(5));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(6))
                        {
                            ismedia.Add(dataReader.GetString(6));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(12))
                        {
                            media.Add(dataReader.GetString(12));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(13))
                        {
                            scheduledate.Add(dataReader.GetString(13));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                        if (!dataReader.IsDBNull(14))
                        {
                            scheduledtime.Add(dataReader.GetString(14));
                        }
                        else
                        {
                            Console.WriteLine("No rows found");
                        }
                    }
                    dataReader.Close();
                    for (int i = 0; i < id.Count; i++)
                    {
                        DateTime today = DateTime.UtcNow.Date;
                        string datetime = today.ToString("dd/MM/yyyy");
                        string times = DateTime.Now.ToString("HH:mm");
                        string resultdate = datetime + " " + times;

                        string scheduled = scheduledate[i];
                        string time = scheduledtime[i].Replace(":", ".");
                        string result = scheduled + " " + time;

                        if (resultdate == result)
                        {
                            SendGroupMessage(sender[i], groupname[i], message[i], id[i]);
                        }
                    }
                }
                //oracleConnection.Close();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                oracleConnection.Close();
            }
        }
        private void SendGroupMessage(string sender, string groupname, string message, string id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    SenderGroupChat sen = new SenderGroupChat { sender = sender, name = groupname, message = message };
                    client.BaseAddress = new Uri("http://192.168.100.1:9001/send-group-message");
                    var response = client.PostAsJsonAsync("", sen).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        plan.getDataTable(@"UPDATE TRX_WHATSAPP_MESSAGE_GROUP SET BLAST_STATUS = '1' WHERE TRXID = '" + id + "'");
                    }
                    else
                    {
                        if (response.StatusCode.ToString() == "422")
                        {
                            plan.getDataTable(@"UPDATE TRX_WHATSAPP_MESSAGE_GROUP SET BLAST_STATUS = '2', NOTIFICATION = 'GRUP TIDAK DITEMUKAN' WHERE TRXID = '" + id + "'");
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