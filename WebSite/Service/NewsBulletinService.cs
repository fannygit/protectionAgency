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
using System.Web.Mvc;

namespace EPA.Project.WebSite.Service
{
    public class NewsBulletinService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public NewsBulletinModel NewInstance()
        {
            NewsBulletinModel newOne = new NewsBulletinModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<SelectListItem> GetCategoryTitle(string defaultValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = (from s in basedb.news_bulletin
                         select s).ToList();

            foreach (var i in items)
            {
                list.Add(new SelectListItem() { Text = i.title, Value = i.id.ToString(), Selected = i.id.ToString().Equals(defaultValue) });
            }

            if (string.IsNullOrEmpty(defaultValue) || defaultValue == "-1")
            {
                //加入「請選擇」
                list.Insert(0, new SelectListItem() { Text = "請選擇", Value = "-1" });
            }
            return list;
        }

        public NewsBulletinListModel GetList(string CurrentUserName, NewsBulletinListModel Page)
        {
           var o_query = from p in basedb.news_bulletin
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            var query = o_query.OrderBy(p => p.id);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new NewsBulletinModel()
                    {
						Id = o_entity.id,
						Title = o_entity.title,
						Url = o_entity.url,
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

        

        public bool Create(string UserName, NewsBulletinModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            news_bulletin dbEntity = new news_bulletin();
			dbEntity.id = model.Id;
			dbEntity.title = model.Title;
			dbEntity.url = model.Url;
			dbEntity.create_time = model.CreateTime;

            basedb.news_bulletin.Add(dbEntity);

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

		public NewsBulletinModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.news_bulletin
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new NewsBulletinModel()
                    {
						Id = o_entity.id,
						Title = o_entity.title,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, NewsBulletinModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                news_bulletin o_entity = new news_bulletin()
                {
					id = model.Id,
					title = model.Title,
					url = model.Url,
					create_time = model.CreateTime,

                };
				
                basedb.news_bulletin.Attach(o_entity);
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

            var o_delete = from p in basedb.news_bulletin
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.news_bulletin.Remove(row);
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