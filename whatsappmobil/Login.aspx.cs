using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using whatsappmobil.ssl;

namespace whatsappmobil
{
    public partial class Login : System.Web.UI.Page
    {
        Functionlogin sdl = new Functionlogin();
        string myKey;
        TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
        MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            String strUserID;
            String strPassword;

            int intCheck = 0;
            try
            {

                strUserID = txtUserID.Text.ToString().Trim();
                strPassword = Encrypt(txtPassword.Text.ToString().Trim());

                if ((strUserID.IndexOf("'") >= 0) || (strPassword.IndexOf("'") >= 0))
                {
                    Session.Clear();
                    txtUserID.Text = "";
                    return;
                }

                intCheck = sdl.CekData(strUserID, strPassword);
                if (intCheck == 1)
                {
                    string strResult = sdl.getDataTable("SELECT user_name, user_id, user_branch_id from mst_user where user_id = '" + strUserID + "'");
                    string[] strID = new string[4];
                    if (strResult != "")
                    {
                        strID = strResult.Split('#');
                        Session["UserName"] = strID[0];
                        Session["UserID"] = strID[1];
                        Session["UserBranch"] = strID[2];
                        //Session["UserCompany"] = strID[3];
                    }


                    int timeout = 36000000;
                    string userData = "WhatsappH2";
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                          1,
                          txtUserID.Text,
                          System.DateTime.Now,
                          System.DateTime.Now.AddMinutes(timeout), // Should be timout from web.config
                          false,
                          userData,
                          FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
                    string url = "";
                    if (Request.QueryString["ReturnUrl"] != null)
                    {
                        url = FormsAuthentication.GetRedirectUrl(txtUserID.Text, false);
                        Uri uri = Request.Url;
                        url = uri.Scheme.Replace("https", "http") + "://" + uri.Host + ":" + uri.Port + url;
                        Response.Redirect(url, true);
                    }
                    else
                    {
                        url = FormsAuthentication.GetRedirectUrl(txtUserID.Text, false);
                        //url = "Dashboard.aspx";
                        Response.Redirect(url, true);
                    }
                }
                else
                {
                    Session.Clear();
                    ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "alert('Username/Password Tidak Sesuai', 'error');", true);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string Encrypt(string myString)
        {
            myKey = "Ind0cy63R";
            cryptDES3.Key = cryptMD5Hash.ComputeHash(ASCIIEncoding.ASCII.GetBytes(myKey));
            cryptDES3.Mode = CipherMode.ECB;
            ICryptoTransform desdencrypt = cryptDES3.CreateEncryptor();
            Byte[] buff = ASCIIEncoding.ASCII.GetBytes(myString);
            return Convert.ToBase64String(desdencrypt.TransformFinalBlock(buff, 0, buff.Length));
        }
    }
}