using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages.Html;

namespace JamZoo.Project.WebSite.Service
{
    public class WBService : BaseService
    {
        public List<NewsModel> GetNewsBulletinList()
        {
            List<NewsModel> list = new List<NewsModel>();
            var items = (from s in basedb.news_bulletin
                         select s).ToList();

            foreach (var i in items)
            {
                list.Add(new NewsModel() { Text = i.title, Value = i.id, Url = i.url, IsUrl = !string.IsNullOrEmpty(i.url) });
            }
            return list;
        }

        public Dictionary<MenuModel, Dictionary<int, string>> GetFixedPollutionList()
        {
            Dictionary<MenuModel, Dictionary<int, string>> result = new Dictionary<MenuModel, Dictionary<int, string>>();
            var items = (from s in basedb.fixed_pollution_control
                         orderby s.orderfield
                         select s).ToList();
            //in2--in7
            int index = 2;
            foreach (var i in items)
            {
                MenuModel key = new MenuModel() {
                    Text = i.title,
                    Value = i.id.ToString(),
                    Css = "in" + index.ToString(),
                    Img = i.img
                };
                index++;
                if (index == 7) index = 2;

                var d = (from p in basedb.fixed_pollution_control_in
                         where p.fixed_pollution_control_id == i.id 
                         orderby p.orderfield
                         select p);

                Dictionary<int, string> values = d.Select(t => new { t.id, t.title }).ToDictionary(t => t.id, t => t.title);
                
                result.Add(key, values);
            }          
            return result;
        }


        public List<SystemLinkModel> GetSystemLinkLists()
        {
            List<SystemLinkModel> list = new List<SystemLinkModel>();
            var items = (from s in basedb.system_link
                         orderby s.orderfield
                         select s).ToList();

            foreach (var i in items)
            {
                SystemLinkModel d = new SystemLinkModel();
                d.Title = i.title;
                d.Url = i.url;
                list.Add(d);
            }
            return list;
        }

        public List<SelectListItem> GetRelatedFileDownloadList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = (from s in basedb.related_file_download
                         orderby s.orderfield
                         select s).ToList();

            foreach (var i in items)
            {
                list.Add(new SelectListItem() { Text = i.category_title, Value = i.id.ToString() });
            }
            return list;
        }

        public NewsBulletinInfoModel GetNesInfo(string info_id)
        {
            int nfid = Convert.ToInt32(info_id);

            var d = (from p in basedb.news_bulletin_info
                     where p.id == nfid
                     select p).FirstOrDefault();

            if (d != null)
            {
                NewsBulletinInfoModel info = new NewsBulletinInfoModel()
                {
                    Id=d.id,
                    CreateTime=d.create_time,
                    Title=d.title,
                    Blog = d.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
                    Url=d.url,
                    NewBulletinId = d.new_bulletin_id,
                    CategoryTitle = (from p in basedb.news_bulletin
                                    where p.id == d.new_bulletin_id
                                    select p.title).FirstOrDefault()
                };
                return info;
            }
            else
            {
                return null;
            }
        }

        public string GetNesTitle(string new_bulletin_id)
        {
            int nbid = Convert.ToInt32(new_bulletin_id);

            var d = (from p in basedb.news_bulletin
                     where p.id == nbid
                     select p).FirstOrDefault();

            if (d != null)
            {
                return d.title;
            }
            else
            {
                return string.Empty;
            }
        }

        public Dictionary<FixedPollutionControlInModel, Dictionary<int, string>> GetFixedPollutionControlInList(string f1)
        {
            Dictionary<FixedPollutionControlInModel, Dictionary<int, string>> list = new Dictionary<FixedPollutionControlInModel, Dictionary<int, string>>();
            int fd1 = Convert.ToInt32(f1);
            var o_query = from o_entity in basedb.fixed_pollution_control_in
                          where o_entity.fixed_pollution_control_id == fd1
                          select o_entity;

            foreach (var o_inid in o_query)
            {
                //key
                FixedPollutionControlInModel newModel = new FixedPollutionControlInModel()
                {
                    Id = o_inid.id,
                    Title = o_inid.title,
                    Url = o_inid.url,
                    CreateTime = o_inid.create_time,
                    orderField = o_inid.orderfield,
                    FixedPollutionControlId = o_inid.fixed_pollution_control_id,
                    Img = o_inid.img,
                    CategoryTitle = (from p in basedb.fixed_pollution_control
                                     where p.id == o_inid.fixed_pollution_control_id
                                     select p.title).FirstOrDefault(),
                };
                //value Dictionary<int, string>

                var d = (from p in basedb.fixed_pollution_control_info
                         where p.fixed_pollution_control_id == fd1 && p.inid == o_inid.id
                         orderby p.orderfield
                         select p);

                Dictionary<int, string> values = d.Select(t => new { t.id, t.title }).ToDictionary(t => t.id, t => t.title);

                list.Add(newModel, values);
            }

                          

            return list;
        }

        public List<FixedPollutionControlInfoModel> GetFixedPollutionControlInfoList(int fd1, int fd2)
        {
            var d = (from p in basedb.fixed_pollution_control_info
                     where p.fixed_pollution_control_id == fd1 && p.inid ==fd2
                     orderby p.orderfield
                     select new FixedPollutionControlInfoModel()
                     {
                         Blog = p.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
                         Title = p.title,
                         Url = p.url,
                         Id = p.id,
                         FixedPollutionControlId = p.fixed_pollution_control_id,
                         InId = p.inid,
                         CreateTime = p.create_time,
                         CategoryTitle = (from s in basedb.fixed_pollution_control
                                          where s.id == p.fixed_pollution_control_id
                                          select s.title).FirstOrDefault(),
                         CategoryTitleIN = (from e in basedb.fixed_pollution_control_in
                                            where e.id == p.inid && e.fixed_pollution_control_id == p.fixed_pollution_control_id
                                            select e.title).FirstOrDefault(),
                     }).ToList();
            return d;
        }

        public List<ImportantLinkModel> GetImportantLinks(int skip, int take, out int TotalRecords)
        {
            var query = (from p in basedb.important_link
                         orderby p.create_time descending
                         select p);

            TotalRecords = query.Count();

            List<ImportantLinkModel> list = query.Skip(skip).Take(take).Select(p =>
                                            new ImportantLinkModel()
                                            {
                                                Title = p.title,
                                                CreateTime = p.create_time,
                                                Id = p.id,
                                                Url = p.url,
                                                BannerImg = p.banner_img
                                            }
                                            ).ToList();

            return list;
        }


        public string GetNewsFirst()
        {
            var query = (from p in basedb.news_bulletin_info
                orderby p.create_time descending
                select p).FirstOrDefault();
            if (query != null)
            {
                return query.id.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得最新公告LIST
        /// </summary>
        /// <returns></returns>
        public List<NewsBulletinInfoModel> GetNews(string new_bulletin_id, int skip, int take, out int TotalRecords)
        {
            int nbid = Convert.ToInt32(new_bulletin_id);
            var query = (from p in basedb.news_bulletin_info
                         where p.new_bulletin_id == nbid
                         orderby p.create_time descending
                         select p);

            TotalRecords = query.Count();
            List<NewsBulletinInfoModel> list = query.Skip(skip).Take(take).Select(p =>
                                            new NewsBulletinInfoModel()
                                            {
                                                Title = p.title,
                                                CreateTime = p.create_time,
                                                Id = p.id,
                                                Blog = p.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
                                                Url = p.url
                                            }
                                            ).ToList();           
            return list;
        }

        public List<RelatedFileDownloadInfoModel> GetDownloadList(string download_id, int skip, int take, out int TotalRecords)
        {
            int did = Convert.ToInt32(download_id);
            var query = (from p in basedb.related_file_download_info
                         where p.related_file_download_id == did
                         orderby p.create_time descending
                         select p);
            TotalRecords = query.Count();

            List<RelatedFileDownloadInfoModel> list = query.Skip(skip).Take(take).Select(p =>
                                           new RelatedFileDownloadInfoModel()
                                           {
                                               SubFile = p.sub_file,
                                               SubTitle = p.sub_title,
                                               RelatedFileDownloadId = p.related_file_download_id,
                                               CreateTime = p.create_time
                                           }
                                           ).ToList();          
            return list;
        }
        
    }
}