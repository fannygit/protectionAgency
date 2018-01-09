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
using System.Web.UI.WebControls;
using EPA.Project.WebSite.Library;

namespace EPA.Project.WebSite.Service
{
    public class RoleService : BaseService
    {
        /// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public RoleModel NewInstance()
        {
            RoleModel newOne = new RoleModel();
            //newOne.Id = Library.Utils.GetObjectId();
            //newOne.CreateDate = DateTime.Now;
            return newOne;
        }



        /// <summary>
        /// 初始化後台角色
        /// </summary>
        public void InitRole(out string errorMsg)
        {
            try
            {
                foreach (var role in basedb.T_Role)
                {
                    basedb.T_Role.Remove(role);
                }

                foreach (var mapping in basedb.T_Account_Role_Mapping)
                {
                    basedb.T_Account_Role_Mapping.Remove(mapping);
                }

                basedb.SaveChanges();

                RoleModel roleModel = new RoleModel();
                roleModel.Id = INIT_ROLE;
                roleModel.Createdate = DateTime.Now;
                roleModel.Createuserid = INIT_ACCOUNT;
                roleModel.Name = INIT_ROLE;
                roleModel.Updatedate = DateTime.Now;
                roleModel.Updateuserid = INIT_ACCOUNT;
                roleModel.PermissionMappingList = new List<string>() { INTI_PERMISSION };

                Create(INIT_ACCOUNT, roleModel, out errorMsg);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }

        }

        /// <summary>
        /// 取得角色下拉列表
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public List<SelectListItem> GetRoleList(string RoleId)
        {
            List<SelectListItem> ItemList = new List<SelectListItem>();

            var o_query = from p in basedb.T_Role
                          select new
                          {
                              p.Id,
                              p.Name
                          };

            foreach (var p in o_query)
            {
                ItemList.Add(new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id,
                    Selected = p.Id.Equals(RoleId)
                });
            }

            return ItemList.ToList();
        }


        public RoleListModel GetList(string CurrentUserName, RoleListModel Page)
        {
            var o_query = from p in basedb.T_Role
                          select p;

            if (!string.IsNullOrEmpty(Page.Search))
            {

            }

            o_query = o_query.OrderBy(p => p.CreateDate);
            try
            {
                Page.TotalRecords = o_query.Select(p => p.Id).Count();
                Page.Data = o_query.Skip(Page.Skip).Take(Page.Take).Select(o_entity =>
                    new RoleModel()
                    {
                        Id = o_entity.Id,
                        Name = o_entity.Name,
                        Createdate = o_entity.CreateDate,
                        Updatedate = o_entity.UpdateDate,
                        Createuserid = o_entity.CreateUserId,
                        Updateuserid = o_entity.UpdateUserId,
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }



        public bool Create(string UserName, RoleModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            T_Role dbEntity = new T_Role();

            dbEntity.Id = model.Id;
            dbEntity.Name = model.Name;
            dbEntity.CreateDate = model.Createdate;
            dbEntity.UpdateDate = model.Updatedate;
            dbEntity.CreateUserId = UserName;
            dbEntity.UpdateUserId = UserName;
            basedb.T_Role.Add(dbEntity);

            //角色所屬的權限Mapping
            if (model.PermissionMappingList.Count() != 0)
            {
                CreateByPermission(UserName, dbEntity.Id, model.PermissionMappingList, out ErrMsgs);
            }

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

        public void CreateByPermission(string nowLoginUser, string RoleId, List<string> PermissionIdList, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;
            foreach (var pid in PermissionIdList)
            {
                T_Role_Permission_Mapping dbEntityMapping = new T_Role_Permission_Mapping();

                dbEntityMapping.Id = Library.Utils.GetObjectId();
                dbEntityMapping.RoleId = RoleId;
                dbEntityMapping.PermissionId = pid;
                dbEntityMapping.CreateUserId = nowLoginUser;
                dbEntityMapping.CreateDate = DateTime.Now;
                basedb.T_Role_Permission_Mapping.Add(dbEntityMapping);
            }

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

        }

        public RoleModel Get(string userId, string id)
        {
            var o_entity = (from p in basedb.T_Role
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new RoleModel()
                    {
                        Id = o_entity.Id,
                        Name = o_entity.Name,
                        Createdate = o_entity.CreateDate,
                        Updatedate = o_entity.UpdateDate,
                        Createuserid = o_entity.CreateUserId,
                        Updateuserid = o_entity.UpdateUserId,

                    };

                return model;
            }
            return null;
        }

        public bool Update(string userId, string userAccount, RoleModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            var Query = (from p in basedb.T_Role
                         where p.Id == model.Id
                         select p).FirstOrDefault();

            if (Query != null)
            {
                Query.Name = model.Name;
                Query.UpdateUserId = userAccount;
                Query.UpdateDate = DateTime.Now;

                var QueryDel = from p in basedb.T_Role_Permission_Mapping
                                where p.RoleId == Query.Id
                                select p;

                foreach (var p in QueryDel)
                {
                    basedb.T_Role_Permission_Mapping.Remove(p);
                }

                foreach (var p in model.PermissionMappingList)
                {
                    T_Role_Permission_Mapping dbEntityMapping = new T_Role_Permission_Mapping();

                    dbEntityMapping.Id = Library.Utils.GetObjectId();
                    dbEntityMapping.RoleId = model.Id;
                    dbEntityMapping.PermissionId = p;
                    dbEntityMapping.CreateUserId = userAccount;
                    dbEntityMapping.CreateDate = DateTime.Now;

                    basedb.T_Role_Permission_Mapping.Add(dbEntityMapping);
                }
            }

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

        public bool Delete(string userName, string id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.T_Role
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.T_Role.Remove(row);
            }

            var QueryDel = from p in basedb.T_Role_Permission_Mapping
                           where p.RoleId == id
                           select p;

            foreach (var p in QueryDel)
            {
                basedb.T_Role_Permission_Mapping.Remove(p);
            }

            var AccountRoleDel = from p in basedb.T_Account_Role_Mapping
                                 where p.RoleId == id
                                 select p;

            foreach (var p in AccountRoleDel)
            {
                basedb.T_Account_Role_Mapping.Remove(p);
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

        /// <summary>
        /// 取得Name列表
        /// 最後修改者(Lucien)
        /// </summary>
        /// <returns></returns>
        public List<PermissionModel> GetPermissions()
        {
            List<PermissionModel> ItemList = new List<PermissionModel>();

            var o_query = from p in basedb.T_Permission
                          select new
                          {
                              p.Id,
                              p.Name
                          };

            foreach (var p in o_query)
            {
                ItemList.Add(new PermissionModel()
                {
                    Id = p.Id,
                    Name = p.Name
                });
            }

            return ItemList.ToList();
        }

        /// <summary>
        /// 取得被選擇的Name列表
        /// 最後修改者(Lucien)
        /// </summary>
        /// <returns></returns>
        public List<string> GetPermissionsChoose(string RoleId)
        {
            List<string> ItemList = new List<string>();

            var o_query = from p in basedb.T_Role_Permission_Mapping
                          where p.RoleId == RoleId
                          select p.PermissionId;

            foreach (var p in o_query)
            {
                ItemList.Add(p);
            }

            return ItemList.ToList();
        }
    }
}