using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace EPA.Project.WebSite.Models
{
    public class MenuModel
    {
        public string Text { get; set; }
        public string Value { get; set; }
        public string Css { get; set; }

        public string Img { get; set; }
    }

    public class NewsModel
    {
        public string Text { get; set; }
        public int Value { get; set; }
        public bool IsUrl { get; set; }
        public string Url { get; set; }
    }


    public class WBModel : PagerModel
    {

        /// <summary>
        /// MENU
        /// </summary>
        public Dictionary<MenuModel, Dictionary<int, string>> FixedPollutionList { get; set; }
        /// <summary>
        /// MENU
        /// </summary>
        public List<SelectListItem> RelatedFileDownloadList { get; set; }
        /// <summary>
        /// MENU
        /// </summary>
        public List<NewsModel> NewsBulletinList { get; set; }

        public List<SystemLinkModel> SystemLinkLists { get; set; }

        public string download_id { get; set; }
        public List<RelatedFileDownloadInfoModel> DownloadInfoList { get; set; }
        public List<NewsBulletinInfoModel> News { get; set; }
        public List<ImportantLinkModel> ImportantLinkLists { get; set; }


        public string f1 { get; set; }
        public string f2 { get; set; }
        public string f3 { get; set; }


        public Dictionary<FixedPollutionControlInModel, Dictionary<int, string>> FixedPollutionControlInList { get; set; }
        public List<FixedPollutionControlInfoModel> FixedPollutionControlInfoList { get; set; }
        public FixedPollutionControlInfoModel FixedPollutionControlInfo { get; set; }//大標題/中標題



        public string new_bulletin_id { get; set; }
        public string NewsTitle { get; set; }

        public NewsBulletinInfoModel NewsInfo { get; set; }

    }
}