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
    public class ImportantLinkService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public ImportantLinkModel NewInstance()
        {
            ImportantLinkModel newOne = new ImportantLinkModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public ImportantLinkListModel GetList(string CurrentUserName, ImportantLinkListModel Page)
        {
           var o_query = from p in basedb.important_link
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            var query = o_query.OrderByDescending(p => p.create_time);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new ImportantLinkModel()
                    {
						Id = o_entity.id,
						BannerImg = o_entity.banner_img,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        Title = o_entity.title
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, ImportantLinkModel model, out string ErrMsgs)
        {
            if (model.BannerImgFile != null)
            {
                Library.Utils.SaveFile<ImportantLinkModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "BannerImg", model.BannerImgFile);
            }

            ErrMsgs = string.Empty;

            important_link dbEntity = new important_link();
			dbEntity.id = model.Id;
			dbEntity.banner_img = model.BannerImg;
			dbEntity.url = model.Url;
			dbEntity.create_time = model.CreateTime;
            dbEntity.title = model.Title;
            basedb.important_link.Add(dbEntity);

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

		public ImportantLinkModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.important_link
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new ImportantLinkModel()
                    {
						Id = o_entity.id,
						BannerImg = o_entity.banner_img,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        Title = o_entity.title
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, ImportantLinkModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.BannerImgFile != null)
            {
                Library.Utils.SaveFile<ImportantLinkModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "BannerImg", model.BannerImgFile);
            }

            try
            {
                important_link o_entity = new important_link()
                {
					id = model.Id,
					banner_img = model.BannerImg,
					url = model.Url,
					create_time = model.CreateTime,
                    title = model.Title
                };
				
                basedb.important_link.Attach(o_entity);
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

            var o_delete = from p in basedb.important_link
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.important_link.Remove(row);
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