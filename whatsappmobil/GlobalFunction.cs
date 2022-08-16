using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace whatsappmobil
{
    public class GlobalFunction
    {
        public static string ResolveUrlApplicationPath()
        {
            return ResolveUrl("~/");
        }

        public static string ResolveUrlAccess(string relativeUrl)
        {
            if (HttpContext.Current != null)
            {
                System.Web.UI.Page p = HttpContext.Current.Handler as System.Web.UI.Page;
                if (p != null)
                {
                    string httphost = p.Request.ServerVariables["HTTP_HOST"];
                    string urlhost = p.Request.Url.ToString().Substring(0, p.Request.Url.ToString().IndexOf(httphost) + httphost.Length);
                    string returnurl = p.ResolveUrl(relativeUrl);

                    return p.ResolveUrl(returnurl);
                }
                else
                    //throw new InvalidOperationException("Unable to Resolve: Not in a Page Context");
                    return "";
            }
            else
                //throw new InvalidOperationException("Unable to Resolve: Not in a HttpContext");
                return "";
        }
        public static string ResolveUrl(string relativeUrl)
        {
            if (HttpContext.Current != null)
            {
                System.Web.UI.Page p = HttpContext.Current.Handler as System.Web.UI.Page;
                if (p != null)
                {
                    string httphost = p.Request.ServerVariables["HTTP_HOST"];
                    string urlhost = p.Request.Url.ToString().Substring(0, p.Request.Url.ToString().IndexOf(httphost) + httphost.Length);
                    string returnurl = p.ResolveUrl(relativeUrl);
                    returnurl = urlhost + returnurl;
                    return p.ResolveUrl(returnurl);
                }
                else
                    //throw new InvalidOperationException("Unable to Resolve: Not in a Page Context");
                    return "";
            }
            else
                //throw new InvalidOperationException("Unable to Resolve: Not in a HttpContext");
                return "";
        }
        public static void DisablePageCaching()
        {
            //Used for disabling page caching 
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
            HttpContext.Current.Response.CacheControl = "private";
            HttpContext.Current.Response.AddHeader("pragma", "no-cache");
            HttpContext.Current.Response.AddHeader("pragma", "no-store");
            HttpContext.Current.Response.CacheControl = "no-cache";
            HttpContext.Current.Response.Cache.SetNoServerCaching();
        }
    }
}