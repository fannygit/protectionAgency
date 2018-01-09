using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Providers;
using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.Service;

namespace EPA.Project.WebSite.Library.Security
{
    public class BasicRoleProvider : DefaultRoleProvider
    {
        // Emp SN
        public override string[] GetRolesForUser(string userid)
        {
            AccountService service = new AccountService();
            AccountModel m = service.Get("role provider", userid);

            if (m != null)
            {
                return m.RoleList.Select(p => p.ToString()).ToArray<string>();
            }

            return null;
        }
    }
}