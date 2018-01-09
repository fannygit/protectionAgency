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
    public class RelatedFileDownloadInfoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public RelatedFileDownloadInfoModel NewInstance()
        {
            RelatedFileDownloadInfoModel newOne = new RelatedFileDownloadInfoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public RelatedFileDownloadInfoListModel GetList(string CurrentUserName, RelatedFileDownloadInfoListModel Page)
        {
           var o_query = from p in basedb.related_file_download_info
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
            }

            var query = o_query.OrderBy(p => p.id);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new RelatedFileDownloadInfoModel()
                    {
						Id = o_entity.id,
						RelatedFileDownloadId = o_entity.related_file_download_id,
						SubFile = o_entity.sub_file,
						SubTitle = o_entity.sub_title,
						SubNo = o_entity.sub_no,
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

        

        public bool Create(string UserName, RelatedFileDownloadInfoModel model, out string ErrMsgs)
        {

            if (model.SubFileFile != null)
            {
                Library.Utils.SaveFile<RelatedFileDownloadInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "SubFile", model.SubFileFile);
            }


            ErrMsgs = string.Empty;

            related_file_download_info dbEntity = new related_file_download_info();
			dbEntity.id = model.Id;
			dbEntity.related_file_download_id = model.RelatedFileDownloadId;
			dbEntity.sub_file = model.SubFile;
			dbEntity.sub_title = model.SubTitle;
			dbEntity.sub_no = model.SubNo;
			dbEntity.create_time = model.CreateTime;


            basedb.related_file_download_info.Add(dbEntity);

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

		public RelatedFileDownloadInfoModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.related_file_download_info
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new RelatedFileDownloadInfoModel()
                    {
						Id = o_entity.id,
						RelatedFileDownloadId = o_entity.related_file_download_id,
						SubFile = o_entity.sub_file,
						SubTitle = o_entity.sub_title,
						SubNo = o_entity.sub_no,
						CreateTime = o_entity.create_time,

                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, RelatedFileDownloadInfoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.SubFileFile != null)
            {
                Library.Utils.SaveFile<RelatedFileDownloadInfoModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "SubFile", model.SubFileFile);
            }


            try
            {
                related_file_download_info o_entity = new related_file_download_info()
                {
					id = model.Id,
					related_file_download_id = model.RelatedFileDownloadId,
					sub_file = model.SubFile,
					sub_title = model.SubTitle,
					sub_no = model.SubNo,
					create_time = model.CreateTime,

                };
				
                basedb.related_file_download_info.Attach(o_entity);
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

            var o_delete = from p in basedb.related_file_download_info
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.related_file_download_info.Remove(row);
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