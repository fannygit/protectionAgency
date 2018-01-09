/*
ModelName
dbModelName
KeyFieldType
KeyEexpression
DaoKeyFieldName
UpdateFields
Mapping_Fields_A
Mapping_Fields_B
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using System.Data.Objects;

using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.DbContext;

namespace EPA.Project.WebSite.Service
{
    public class NewsBulletinInfoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public NewsBulletinInfoModel NewInstance()
        {
            NewsBulletinInfoModel newOne = new NewsBulletinInfoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public NewsBulletinInfoListModel GetList(string CurrentUserName, NewsBulletinInfoListModel Page)
        {
           var o_query = from p in basedb.news_bulletin_info
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            var query = o_query.OrderByDescending(p => p.id);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new NewsBulletinInfoModel()
                    {
						Id = o_entity.id,
						NewBulletinId = o_entity.new_bulletin_id,
                        CategoryTitle = (from p in basedb.news_bulletin
                                                where p.id == o_entity.new_bulletin_id
                                                select p.title).FirstOrDefault(),
						Title = o_entity.title,
						Url = o_entity.url,
                        Blog = o_entity.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
						CreateTime = o_entity.create_time,

                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, NewsBulletinInfoModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            news_bulletin_info dbEntity = new news_bulletin_info();
			dbEntity.id = model.Id;
			dbEntity.new_bulletin_id = model.NewBulletinId;
			dbEntity.title = model.Title;
			dbEntity.url = model.Url;
			dbEntity.blog = model.Blog;
			dbEntity.create_time = model.CreateTime;


            basedb.news_bulletin_info.Add(dbEntity);

            try
            {

                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;
        }

		public NewsBulletinInfoModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.news_bulletin_info
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new NewsBulletinInfoModel()
                    {
						Id = o_entity.id,
						NewBulletinId = o_entity.new_bulletin_id,
						Title = o_entity.title,
						Url = o_entity.url,
                        Blog = o_entity.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
						CreateTime = o_entity.create_time,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, NewsBulletinInfoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                news_bulletin_info o_entity = new news_bulletin_info()
                {
					id = model.Id,
					new_bulletin_id = model.NewBulletinId,
					title = model.Title,
					url = model.Url,
					blog = model.Blog,
					create_time = model.CreateTime,

                };
				
                basedb.news_bulletin_info.Attach(o_entity);
				basedb.Entry(o_entity).Property(x => x.create_time).IsModified = true;

                basedb.Entry(o_entity).State = EntityState.Modified;
                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    ErrMsgs = ex.InnerException.Message;
                }
                else
                {
                    ErrMsgs = ex.Message;
                }
            }

            return ErrMsgs.Length == 0;    
        }

        public bool Delete(string userName, int? id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.news_bulletin_info
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.news_bulletin_info.Remove(row);
            }

            try
            {
                basedb.SaveChanges();
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }

            return ErrorMsg.Length == 0;
        }

        
   }
}