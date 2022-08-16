using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class Dashboard : System.Web.UI.Page
    {
        FunctionDashboard dashboard = new FunctionDashboard();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetWhatsappMediaCount();
                GetWhatsappMessageCount();
                GetTotalTargetCount();
                GetTotalTargetPlan();
                GetChart(Convert.ToString(Session["UserID"]));
            }
        }

        private void GetWhatsappMediaCount()
        {
            string strheader;
            string[] strGetData = new string[40];
            strheader = dashboard.GetWhatsappMediaCount(Convert.ToString(Session["UserID"]));
            if (strheader != null || strheader != "")
            {
                strGetData = strheader.Split('#');
                lblSentMedia.Text = strGetData[0];
            }
        }

        private void GetWhatsappMessageCount()
        {
            string strheader;
            string[] strGetData = new string[40];
            strheader = dashboard.GetWhatsappMessageCount(Convert.ToString(Session["UserID"]));
            if (strheader != null || strheader != "")
            {
                strGetData = strheader.Split('#');
                lblSentMessage.Text = strGetData[0];
            }
        }

        private void GetTotalTargetCount()
        {
            string strheader;
            string[] strGetData = new string[40];
            strheader = dashboard.GetTotalTargetCount(Convert.ToString(Session["UserID"]));
            if (strheader != null || strheader != "")
            {
                strGetData = strheader.Split('#');
                lblTotalTarget.Text = strGetData[0];
            }
        }

        private void GetTotalTargetPlan()
        {
            string strheader;
            string[] strGetData = new string[40];
            strheader = dashboard.GetTotalTargetPlan(Convert.ToString(Session["UserID"]));
            if (strheader != null || strheader != "")
            {
                strGetData = strheader.Split('#');
                lblPlan.Text = strGetData[0];
            }
        }

        private void GetChart(string CreateBy)
        {
            string Januari = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '01' and create_by = '"+CreateBy+"' group by trunc(sent_date, 'MM')");
            string Februari = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '02' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Maret = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '03' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string April = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '04' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Mei = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '05' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Juni = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '06' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Juli = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '07' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Agustus = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '08' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string September = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '09' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Oktober = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '10' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string November = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '11' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string Desember = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_message aa inner join trx_whatsapp_header bb on aa.trxid = bb.trxid where to_char(sent_date, 'MM') = '12' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");

            lblJanuari.Value = Januari.Split('#')[0];
            lblFebruari.Value = Februari.Split('#')[0];
            lblMaret.Value = Maret.Split('#')[0];
            lblApril.Value = April.Split('#')[0];
            lblMei.Value = Mei.Split('#')[0];
            lblJuni.Value = Juni.Split('#')[0];
            lblJuli.Value = Juli.Split('#')[0];
            lblAgustus.Value = Agustus.Split('#')[0];
            lblSeptember.Value = September.Split('#')[0];
            lblOktober.Value = Oktober.Split('#')[0];
            lblNovember.Value = November.Split('#')[0];
            lblDesember.Value = Desember.Split('#')[0];

            string plJanuari = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '01' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plFebruari = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '02' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plMaret = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '03' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plApril = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '04' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plMei = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '05' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plJuni = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '06' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plJuli = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '07' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plAgustus = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '08' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plSeptember = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '09' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plOktober = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '10' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plNovember = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '11' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");
            string plDesember = dashboard.getDataTable(@"select count(*) countresult from trx_whatsapp_plan where to_char(sent_date, 'MM') = '12' and create_by = '" + CreateBy + "' group by trunc(sent_date, 'MM')");

            pJanuari.Value = plJanuari.Split('#')[0];
            pFebruari.Value = plFebruari.Split('#')[0];
            pMaret.Value = plMaret.Split('#')[0];
            pApril.Value = plApril.Split('#')[0];
            pMei.Value = plMei.Split('#')[0];
            pJuni.Value = plJuni.Split('#')[0];
            pJuli.Value = plJuli.Split('#')[0];
            pAgustus.Value = plAgustus.Split('#')[0];
            pSeptember.Value = plSeptember.Split('#')[0];
            pOktober.Value = plOktober.Split('#')[0];
            pNovember.Value = plNovember.Split('#')[0];
            pDesember.Value = plDesember.Split('#')[0];
        }
    }
}