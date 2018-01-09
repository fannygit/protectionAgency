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
    public class RelatedFileDownloadInfoListModel : PagerModel
    {
		public string Search { get; set; }

		public List<RelatedFileDownloadInfoModel> Data { get; set; }
    }

    public class RelatedFileDownloadInfoModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "相關檔案下載編號")]
		[Required(ErrorMessage="必填")]
		public int RelatedFileDownloadId { get; set; }

        [Display(Name = "檔案")]
		public string SubFile { get; set; }
        public HttpPostedFileBase SubFileFile { get; set; }

		[Display(Name = "標題")]
		[Required(ErrorMessage="必填")]
		public string SubTitle { get; set; }

		[Display(Name = "排序")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int SubNo { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }



        public RelatedFileDownloadInfoModel()
        {
			CreateTime = DateTime.Now;

        }
    }
}