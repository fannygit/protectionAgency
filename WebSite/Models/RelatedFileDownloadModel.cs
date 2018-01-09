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
    public class RelatedFileDownloadListModel : PagerModel
    {
		public string Search { get; set; }

		public List<RelatedFileDownloadModel> Data { get; set; }
    }

    public class RelatedFileDownloadModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "分類標題")]
		[Required(ErrorMessage="必填")]
		public string CategoryTitle { get; set; }

		[Display(Name = "分類標題檔案下載")]
		public string CategoryDownload { get; set; }

		[Display(Name = "標題")]
		public string Title { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }

        public int OrderField { get; set; }

        public RelatedFileDownloadModel()
        {
			CreateTime = DateTime.Now;

        }
    }
}