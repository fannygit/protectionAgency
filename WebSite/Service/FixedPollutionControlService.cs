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
    public class FixedPollutionControlService : BaseService
    {
		/// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public FixedPollutionControlModel NewInstance()
        {
            FixedPollutionControlModel newOne = new FixedPollutionControlModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }

        public List<SelectListItem> GetCategoryTitleIN(string defaultValue, int id)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = (from s in basedb.fixed_pollution_control_in
                         where s.fixed_pollution_control_id == id
                         orderby s.orderfield
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

        public List<SelectListItem> GetCategoryTitle(string defaultValue)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var items = (from s in basedb.fixed_pollution_control
                         orderby s.orderfield 
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

        public bool ReOrder(EPA.Project.WebSite.Controllers.FixedPollutionControlController.classz result)
        {
            try
            {
                foreach (var i in result.data)
                {
                    int ii = Convert.ToInt32(i.key);
                    var o_query = (from p in basedb.fixed_pollution_control
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

        public FixedPollutionControlListModel GetList(string CurrentUserName, FixedPollutionControlListModel Page)
        {
           var o_query = from p in basedb.fixed_pollution_control
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.title.Contains(Page.Search));
            }

            var query = o_query.OrderBy(p => p.orderfield);

            try
            {
                Page.TotalRecords = query.Select(p => p.id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new FixedPollutionControlModel()
                    {
						Id = o_entity.id,
						Title = o_entity.title,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        orderField = o_entity.orderfield,
                        Img = o_entity.img,
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        

        public bool Create(string UserName, FixedPollutionControlModel model, out string ErrMsgs)
        {
            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<FixedPollutionControlModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            ErrMsgs = string.Empty;

            fixed_pollution_control dbEntity = new fixed_pollution_control();
			dbEntity.id = model.Id;
			dbEntity.title = model.Title;
			dbEntity.url = model.Url;
			dbEntity.create_time = model.CreateTime;
            dbEntity.orderfield = model.orderField;
            dbEntity.img = model.Img;
            basedb.fixed_pollution_control.Add(dbEntity);

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

		public FixedPollutionControlModel Get(string userId, int? id)
        {
            var o_entity = (from p in basedb.fixed_pollution_control
                            where p.id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new FixedPollutionControlModel()
                    {
						Id = o_entity.id,
						Title = o_entity.title,
						Url = o_entity.url,
						CreateTime = o_entity.create_time,
                        orderField = o_entity.orderfield,
                        Img = o_entity.img
                    };

                return model;
            }
            return null;
        }

		public bool Update(string userId, string userAccount, FixedPollutionControlModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            if (model.ImgFile != null)
            {
                Library.Utils.SaveFile<FixedPollutionControlModel>(model, HttpContext.Current.Server.MapPath("~/App_Data/UploadFile"), "Img", model.ImgFile);
            }

            try
            {
                fixed_pollution_control o_entity = new fixed_pollution_control()
                {
					id = model.Id,
					title = model.Title,
					url = model.Url,
					create_time = model.CreateTime,
                    orderfield = model.orderField,
                    img = model.Img
                };
				
                basedb.fixed_pollution_control.Attach(o_entity);
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

            var o_delete = from p in basedb.fixed_pollution_control
                           where p.id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.fixed_pollution_control.Remove(row);
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