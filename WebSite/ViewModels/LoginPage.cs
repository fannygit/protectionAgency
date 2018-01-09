using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EPA.Project.WebSite.Enums;

namespace EPA.Project.WebSite.ViewModels
{
    /// <summary>
    /// 登入頁面
    /// </summary>
    public class LoginPage
    {
        [Required(ErrorMessage="必填")]
        public string Account { get; set; }

        [Required(ErrorMessage="必填")]
        public string Password { get; set; }
    }
}