using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.Models;
using JamZoo.Project.WebSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamZoo.Project.WebSite.Controllers
{
    public class WBController : Controller
    {
        // GET: /WB/
        WBService Servcie = new WBService();

        public ActionResult Index()
        {
            WBModel m = new WBModel();
            m.Page = 1;

            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            int totalRecords = 0;
            m.News = Servcie.GetNews("2", m.Skip, m.Take, out totalRecords).Take(3).ToList();
            m.ImportantLinkLists = Servcie.GetImportantLinks(m.Skip, m.Take, out totalRecords);
            m.TotalRecords = totalRecords;

            TempData["title"] = "首頁";
            return View(m);
        }

        public ActionResult NewsInfo(string info_id)
        {
            WBModel m = new WBModel();
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.NewsInfo = Servcie.GetNesInfo(info_id);
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            TempData["title"] = "最新公告-"+m.NewsInfo.Title;
            return View(m);
        }

        public ActionResult NewsList(int page = 1, string new_bulletin_id = "2")
        {
            WBModel m = new WBModel();
            m.Page = page;
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            int totalRecords = 0;
            m.News = Servcie.GetNews(new_bulletin_id, m.Skip, m.Take, out totalRecords);
            m.TotalRecords = totalRecords;

            m.NewsTitle = Servcie.GetNesTitle(new_bulletin_id);
            m.new_bulletin_id = new_bulletin_id;

            TempData["title"] = "最新公告-" + m.NewsTitle;
            return View(m);
        }

        public ActionResult Download(int page = 1, string download_id = "1")
        {
            WBModel m = new WBModel();
            m.Page = page;

            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            int totalRecords = 0;
            m.DownloadInfoList = Servcie.GetDownloadList(download_id, m.Skip, m.Take, out totalRecords); 
            m.TotalRecords = totalRecords;

            m.download_id = download_id;
            TempData["title"] = "下載文件專區-"+ m.RelatedFileDownloadList.Where(p=>p.Value== download_id).LastOrDefault().Text;
            return View(m);
        }

        public ActionResult WebLink(int page = 1)
        {
            WBModel m = new WBModel();
            m.Page = page;

            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            int totalRecords = 0;
            m.ImportantLinkLists = Servcie.GetImportantLinks(m.Skip, m.Take, out totalRecords);
            m.TotalRecords = totalRecords;

            TempData["title"] = "重要連結";
            return View(m);
        }

        public ActionResult AddPage()
        {
            WBModel m = new WBModel();

            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            TempData["title"] = "各類管制與申報系統";
            return View(m);
        }

        public ActionResult AddPage2()
        {
            WBModel m = new WBModel();

            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            TempData["title"] = "相關法規與公告查詢";
            return View(m);
        }

        public ActionResult About(string f3, string f2, string f1 = "1")
        {
            WBModel m = new WBModel();
            //f1 list
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            //f2 list ??
            m.FixedPollutionControlInList = Servcie.GetFixedPollutionControlInList(f1);
            int fd1 = Convert.ToInt16(f1);
            int fd2 = Convert.ToInt16(f2);
            if (fd2 == 0)
            {
                if (m.FixedPollutionControlInList.Any())
                    fd2 = m.FixedPollutionControlInList.Where(p=>p.Key.FixedPollutionControlId==fd1).FirstOrDefault().Key.Id;
            }

            //f3 list & f3 sigle
            m.FixedPollutionControlInfoList = Servcie.GetFixedPollutionControlInfoList(fd1, fd2);

            int fd3 = Convert.ToInt16(f3);
            if (!string.IsNullOrEmpty(f3))
            {
                m.FixedPollutionControlInfo = m.FixedPollutionControlInfoList.Where(p => p.FixedPollutionControlId == fd1 && p.InId == fd2 && p.Id == fd3).FirstOrDefault();//GET
            }
            else
            {
                m.FixedPollutionControlInfo = m.FixedPollutionControlInfoList.Where(p => p.FixedPollutionControlId == fd1 && p.InId == fd2).FirstOrDefault();//GET
            }
            
            if (m.FixedPollutionControlInfo == null)//給預設值
                m.FixedPollutionControlInfo = new EPA.Project.WebSite.Models.FixedPollutionControlInfoModel();
            
            m.f1 = f1;
            m.f2 = f2;
            m.f3 = f3;

            if (!string.IsNullOrEmpty(f3) && !string.IsNullOrEmpty(f2) && !string.IsNullOrEmpty(f1))
            {
                TempData["title"] = m.FixedPollutionControlInfo.CategoryTitle + "-" +
                                m.FixedPollutionControlInfo.CategoryTitleIN + "-" + m.FixedPollutionControlInfo.Title;
            }
            else if (!string.IsNullOrEmpty(f2) && !string.IsNullOrEmpty(f1))
            {
                var f2list = m.FixedPollutionList.Where(p => p.Key.Value == f1).FirstOrDefault().Value;
                TempData["title"] = f2list.Where(p => p.Key == fd2).FirstOrDefault().Value;
            }
            else if (!string.IsNullOrEmpty(f1))
            {
                TempData["title"] = m.FixedPollutionList.Where(p => p.Key.Value == f1).FirstOrDefault().Key.Text;
            }

            
            return View(m);
        }

        public ActionResult Sitemap()
        {
            WBModel m = new WBModel();
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();

            TempData["title"] = "網站導覽";
            return View(m);
        }

        public ActionResult Open()
        {
            WBModel m = new WBModel();
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();
            TempData["title"] = "網站資料開放宣告";
            return View(m);
        }

        public ActionResult Privacy()
        {
            WBModel m = new WBModel();
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();
            TempData["title"] = "隱私權保護政策";
            return View(m);
        }

        public ActionResult Website()
        {
            WBModel m = new WBModel();
            m.FixedPollutionList = Servcie.GetFixedPollutionList();
            m.NewsBulletinList = Servcie.GetNewsBulletinList();
            m.RelatedFileDownloadList = Servcie.GetRelatedFileDownloadList();
            m.SystemLinkLists = Servcie.GetSystemLinkLists();
            TempData["title"] = "網站安全政策";
            return View(m);
        }
    }
}
