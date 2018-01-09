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
    public class FixedPollutionControlInListModel : PagerModel
    {
		public string Search { get; set; }

		public List<FixedPollutionControlInModel> Data { get; set; }
    }

    public class FixedPollutionControlInModel : EditModePage
    {
        public string CategoryTitle { get; set; }

        [Display(Name = "固定污染源管制編號")]
        [Required(ErrorMessage = "必填")]
        public int FixedPollutionControlId { get; set; }

		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

        [Display(Name = "側欄標題")]
		[Required(ErrorMessage="必填")]
		public string Title { get; set; }

		[Display(Name = "連結")]
		public string Url { get; set; }

		[Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }
        
        [Display(Name = "ICON圖片")]
        public string Img { get; set; }
        public HttpPostedFileBase ImgFile { get; set; }

        public int orderField { get; set; }

        public FixedPollutionControlInModel()
        {
			CreateTime = DateTime.Now;

        }
    }
}