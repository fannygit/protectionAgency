using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Library;

namespace EPA.Project.WebSite.Models
{
    public class PermissionListModel : PagerModel
    {
        public string Search { get; set; }
		public List<PermissionModel> Data { get; set; }
    }

    public class PermissionModel : EditModePage
    {
		[Display(Name = "權限編號")]
		[Required(ErrorMessage="必填")]
		public string Id { get; set; }

		[Display(Name = "權限名稱")]
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
		[Required(ErrorMessage="必填")]
		public string Createuserid { get; set; }

		[Display(Name = "修改者")]
		[Required(ErrorMessage="必填")]
		public string Updateuserid { get; set; }



        public PermissionModel()
        {
			Id = Utils.GetObjectId();
			Createdate = DateTime.Now;
			Updatedate = DateTime.Now;

        }
    }
}