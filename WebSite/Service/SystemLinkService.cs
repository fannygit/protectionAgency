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
    public class SystemLinkService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public SystemLinkModel NewInstance()
        {
            SystemLinkModel newOne = new SystemLinkModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public bool ReOrder(EPA.Project.WebSite.Controllers.SystemLinkController.classz result)
        {
            try
            {
                foreach (var i in result.data)
                {
                    int ii = Convert.ToInt32(i.key);
                    var o_query = (from p in basedb.system_link
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

        public SystemLinkListModel GetList(string CurrentUserName, SystemLinkListModel Page)
        {
           var o_query = from p in basedb.system_link
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
                    new SystemLinkModel()
                    {
						Id = o_entity.id,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        Title = o_entity.title,
                        OrderField = o_entity.orderfield
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, SystemLinkModel model, out string ErrMsgs)
        {
           
            ErrMsgs = string.Empty;

            system_link dbEntity = new system_link();
			dbEntity.id = model.Id;
			dbEntity.url = model.Url;
			dbEntity.create_time = model.CreateTime;
            dbEntity.title = model.Title;
            dbEntity.orderfield = model.OrderField;
            basedb.system_link.Add(dbEntity);

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

		public SystemLinkModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.system_link
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new SystemLinkModel()
                    {
						Id = o_entity.id,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        Title = o_entity.title,
                        OrderField = o_entity.orderfield
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, SystemLinkModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;
            

            try
            {
                system_link o_entity = new system_link()
                {
					id = model.Id,
					url = model.Url,
					create_time = model.CreateTime,
                    title = model.Title,
                    orderfield = model.OrderField
                };
				
                basedb.system_link.Attach(o_entity);
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

            var o_delete = from p in basedb.system_link
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.system_link.Remove(row);
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