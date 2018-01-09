using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using EPA.Project.WebSite.DbContext;

namespace EPA.Project.WebSite.Service
{
    public class BaseService
    {
        public Entities basedb;

        public string INIT_ACCOUNT = "EPA";
        public string INIT_ROLE = "InitAdmin";
        public string INTI_PERMISSION = "Admin";

        public BaseService()
        {
            basedb = new Entities();
        }
    }
}