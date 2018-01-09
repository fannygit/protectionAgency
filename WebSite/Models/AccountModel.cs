using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using EPA.Project.WebSite.Enums;

namespace EPA.Project.WebSite.Models
{
    /// <summary>
    /// 列表頁
    /// </summary>
    public class AccountListModel : PagerModel
    {
        public string Search { get; set; }

        public List<AccountModel> Data { get; set; }
    }

    /// <summary>
    /// 帳號
    /// </summary>
    public class AccountModel : EditModePage
    {
        public string IP { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage="必填")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "必填")]
        [Display(Name = "密碼")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "與密碼不相符")]
        public string RePassword { get; set; }

        [Display(Name = "狀態")]
        public int Status { get; set; }


        [Display(Name = "建立時間")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "最後登入時間")]
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 取得 Role
        /// </summary>
        /// <returns></returns>
        [Display(Name = "角色")]
        public string Role { get; set; }
       
        /// <summary>
        /// 取得 Role (帳號擁有的多個角色: 預留欄位)
        /// </summary>
        /// <returns></returns>
        [Display(Name = "角色")]
        public List<string> RoleList { get; set; }


        //角色下拉選項
        public List<System.Web.Mvc.SelectListItem> RoleDropDownList { get; set; }

        /// <summary>
        /// 網站角色判斷
        /// </summary>
        /// <param name="Roles"></param>
        /// <returns></returns>
        public bool IsInRoles(List<string> _Roles)
        {
            if (_Roles == null) return false;
            if (_Roles.Count() == 0) return false;
            foreach (var Role in _Roles)
            {
                if (RoleList != null)
                {
                    if (RoleList.Contains(Role))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

    }
}