using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace EPA.Project.WebSite.Library.Principal
{
    /// <summary>
    /// 網站登入者的基本資料
    /// </summary>
    public class WebSiteUser
    {
        public string Id { get; set; }
        public string AccountType { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string[] Roles { get; set; }
    }

    /// <summary>
    /// http://primaryobjects.com/CMS/Article147.aspx
    /// </summary>
    public class EPAPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public EPAPrincipal(IIdentity identity)
        {

            Identity = identity;
        }

        public WebSiteUser User { get; set; }

        public bool IsInRole(string role)
        {
            if (User != null)
            {
                if (User.Roles != null)
                {
                    if (User.Roles.Length > 0)
                    {
                        return User.Roles.Contains(role);
                    }
                }
            }
            return false;
        }
    }
}