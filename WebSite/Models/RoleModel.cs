using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Library;
using System.Web.UI.WebControls;

namespace EPA.Project.WebSite.Models
{
    public class RoleListModel : PagerModel
    {
        public string Search { get; set; }
		public List<RoleModel> Data { get; set; }
    }

    public class RoleModel : EditModePage
    {
		[Display(Name = "角色編號")]
		[Required(ErrorMessage="必填")]
		public string Id { get; set; }

		[Display(Name = "角色名稱")]
		[Required(ErrorMessage="必填")]
		public string Name { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime Createdate { get; set; }

		[Display(Name = "修改時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime Updatedate { get; set; }

		[Display(Name = "建立者")]
		public string Createuserid { get; set; }

		[Display(Name = "修改者")]
		public string Updateuserid { get; set; }

        public List<PermissionModel> PermissionList { get; set; }

        public List<string> PermissionMappingList { get; set; }

        public RoleModel()
        {
			Id = Utils.GetObjectId();
			Createdate = DateTime.Now;
			Updatedate = DateTime.Now;

        }
    }
}