﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Library;

namespace EPA.Project.WebSite.Models
{
    public class NewsBulletinListModel : PagerModel
    {
		public string Search { get; set; }

		public List<NewsBulletinModel> Data { get; set; }
    }

    public class NewsBulletinModel : EditModePage
    {
		[Display(Name = "編號")]
		[Required(ErrorMessage="必填")]
		[RegularExpression("\\d+", ErrorMessage="格式錯誤")]
		public int Id { get; set; }

		[Display(Name = "分類標題")]
		[Required(ErrorMessage="必填")]
		public string Title { get; set; }

		[Display(Name = "連結")]
		public string Url { get; set; }

        [Display(Name = "建立時間")]
		[Required(ErrorMessage="必填")]
		[DataType(DataType.DateTime), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
		public DateTime CreateTime { get; set; }



        public NewsBulletinModel()
        {
			CreateTime = DateTime.Now;

        }
    }
}