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
    public class RelatedFileDownloadService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public RelatedFileDownloadModel NewInstance()
        {
            RelatedFileDownloadModel newOne = new RelatedFileDownloadModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<SelectListItem> GetCategoryTitle(string defaultValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = (from s in basedb.related_file_download
                         select s).ToList();

            foreach(var i in items)
            {
                list.Add(new SelectListItem() { Text = i.category_title, Value = i.id.ToString(), Selected= i.id.ToString().Equals(defaultValue) });
            }

            if (string.IsNullOrEmpty(defaultValue) || defaultValue=="-1")
            {
                //加入「請選擇」
                list.Insert(0, new SelectListItem() { Text = "請選擇", Value = "-1" });
            }
            return list;
        }

        public bool ReOrder(EPA.Project.WebSite.Controllers.RelatedFileDownloadController.classz result)
        {
            try
            {
                foreach (var i in result.data)
                {
                    int ii = Convert.ToInt32(i.key);
                    var o_query = (from p in basedb.related_file_download
                                   where p.id == ii
                                   select p).FirstOrDefault();
                    if (o_query != null)
                    {
                        o_query.orderfield = Convert.ToInt32(i.value);
                    }
                    basedb.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public RelatedFileDownloadListModel GetList(string CurrentUserName, RelatedFileDownloadListModel Page)
        {
           var o_query = from p in basedb.related_file_download
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.category_title.Contains(Page.Search));
            }

            var query = o_query.OrderBy(p => p.orderfield);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new RelatedFileDownloadModel()
                    {
						Id = o_entity.id,
						CategoryTitle = o_entity.category_title,
						CategoryDownload = o_entity.category_download,
						Title = o_entity.title,
						CreateTime = o_entity.create_time,
                        OrderField = o_entity.orderfield,
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, RelatedFileDownloadModel model, out string ErrMsgs)
        {


            ErrMsgs = string.Empty;

            related_file_download dbEntity = new related_file_download();
			dbEntity.id = model.Id;
			dbEntity.category_title = model.CategoryTitle;
			dbEntity.category_download = model.CategoryDownload;
			dbEntity.title = model.Title;
			dbEntity.create_time = model.CreateTime;
            dbEntity.orderfield = model.OrderField;

            basedb.related_file_download.Add(dbEntity);

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

		public RelatedFileDownloadModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.related_file_download
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new RelatedFileDownloadModel()
                    {
						Id = o_entity.id,
						CategoryTitle = o_entity.category_title,
						CategoryDownload = o_entity.category_download,
						Title = o_entity.title,
						CreateTime = o_entity.create_time,
                        OrderField = o_entity.orderfield
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, RelatedFileDownloadModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;



            try
            {
                related_file_download o_entity = new related_file_download()
                {
					id = model.Id,
					category_title = model.CategoryTitle,
					category_download = model.CategoryDownload,
					title = model.Title,
					create_time = model.CreateTime,
                    orderfield = model.OrderField
                };
				
                basedb.related_file_download.Attach(o_entity);
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

            var o_delete = from p in basedb.related_file_download
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.related_file_download.Remove(row);
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