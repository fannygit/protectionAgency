//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EPA.Project.WebSite.DbContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_Account
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string CreateUserID { get; set; }
        public string UpdateUserID { get; set; }
        public string IP { get; set; }
    }
}
