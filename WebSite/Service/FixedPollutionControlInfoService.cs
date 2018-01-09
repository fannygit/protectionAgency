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
    public class FixedPollutionControlInfoService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public FixedPollutionControlInfoModel NewInstance()
        {
            FixedPollutionControlInfoModel newOne = new FixedPollutionControlInfoModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public FixedPollutionControlInfoListModel GetList(string CurrentUserName, FixedPollutionControlInfoListModel Page)
        {
           var o_query = from p in basedb.fixed_pollution_control_info
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            var query = o_query.OrderBy(p => p.fixed_pollution_control_id).ThenBy(p => p.inid).ThenBy(p => p.orderfield);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new FixedPollutionControlInfoModel()
                    {
						Id = o_entity.id,
                        FixedPollutionControlId = o_entity.fixed_pollution_control_id,
                        CategoryTitle = (from p in basedb.fixed_pollution_control
                                         where p.id == o_entity.fixed_pollution_control_id
                                         select p.title).FirstOrDefault(),
                        CategoryTitleIN = (from p in basedb.fixed_pollution_control_in
                                           where p.id == o_entity.inid
                                           select p.title).FirstOrDefault(),
                        Type = string.IsNullOrEmpty(o_entity.url)? "文章" : "超連結",
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

        

        public bool Create(string UserName, FixedPollutionControlInfoModel model, out string ErrMsgs)
        {

           

            ErrMsgs = string.Empty;

            fixed_pollution_control_info dbEntity = new fixed_pollution_control_info();
			dbEntity.id = model.Id;
            dbEntity.fixed_pollution_control_id = model.FixedPollutionControlId;
			dbEntity.title = model.Title;
			dbEntity.url = model.Url;
			dbEntity.blog = model.Blog;
			dbEntity.create_time = model.CreateTime;
            dbEntity.inid = model.InId;
            dbEntity.orderfield = model.orderField;
            basedb.fixed_pollution_control_info.Add(dbEntity);

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

		public FixedPollutionControlInfoModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.fixed_pollution_control_info
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new FixedPollutionControlInfoModel()
                    {
						Id = o_entity.id,
                        FixedPollutionControlId = o_entity.fixed_pollution_control_id,
						Title = o_entity.title,
						Url = o_entity.url,
                        Blog = o_entity.blog.Replace("http://52.187.122.112", "https://ernet.epa.gov.tw/"),
						CreateTime = o_entity.create_time,
                        InId= o_entity.inid,
                        orderField = o_entity.orderfield
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, FixedPollutionControlInfoModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            try
            {
                fixed_pollution_control_info o_entity = new fixed_pollution_control_info()
                {
					id = model.Id,
                    fixed_pollution_control_id = model.FixedPollutionControlId,
					title = model.Title,
					url = model.Url,
					blog = model.Blog,
					create_time = model.CreateTime,
                    inid = model.InId,
                    orderfield = model.orderField
                };
				
                basedb.fixed_pollution_control_info.Attach(o_entity);
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

            var o_delete = from p in basedb.fixed_pollution_control_info
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.fixed_pollution_control_info.Remove(row);
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