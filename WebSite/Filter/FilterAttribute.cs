using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using MvcPaging;

namespace EPA.Project.WebSite.Filter
{
    public class AllowedIpOnlyAttribute : FilterAttribute, IAuthorizationFilter
    {
        private List<string> ipList = new List<string>();
        //建構式接收以逗號或分號分隔的IP清單，限定存取來源
        //TODO: 如要方便事後修改，可擴充成由config讀取IP清單，但會增加被破解風險
        public AllowedIpOnlyAttribute(string allowedIps)
        {
            ipList = allowedIps.Split(',', ';').ToList();
            var configs = WebConfigurationManager.AppSettings["ipconfig"];
            if (!string.IsNullOrEmpty(configs))
            {
                foreach (var ip in configs.Split(',', ';').ToList())
                {
                    ipList.Add(ip);
                }
            }
        }
        #region IAuthorizationFilter Members
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            //實作OnAuthorization，當來源IP不在清單上，彈出錯誤
            string clientIp = filterContext.HttpContext.Request.UserHostAddress;
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            // Get the IP  
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            
            if (ipList.Contains(clientIp) || ipList.Contains(myIP) || ipList.Contains("*"))
            {

            }
            else
            {
                //throw new ApplicationException("Disallowed Client IP!");
                filterContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(
                                new
                                {
                                    controller = "WB",
                                    action = "Index"
                                }));
            }

        }
        #endregion
    }
    //限定本機存取為AllowedIpOnlyAttribute的特殊情境，限定IP=::1或127.0.0.1
    public class LocalhostOnlyAttribute : AllowedIpOnlyAttribute
    {
        public LocalhostOnlyAttribute()
            : base("::1;127.0.0.1")
        {
        }
    }
}