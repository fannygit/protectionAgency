using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;

using System.Web.Mvc;


using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.DbContext;
using EPA.Project.WebSite.Library;
using System.Data.Entity;


namespace EPA.Project.WebSite.Service
{
    public class AccountService : BaseService
    {

        /// <summary>
        /// 取得帳號列表
        /// </summary>
        /// <param name="CurrentUserName">目前使用者ID</param>
        /// <param name="Page">分頁</param>
        /// <returns></returns>
        public AccountListModel GetList(string CurrentUserName, AccountListModel Page)
        {
            var o_query = from p in basedb.T_Account
                          join mapping in basedb.T_Account_Role_Mapping on p.Account equals mapping.AccountId
                          join r in basedb.T_Role on mapping.RoleId equals r.Id
                          select new
                          {
                              p=p,
                              r=r,
                          };

            if (!string.IsNullOrEmpty(Page.Search))
            {
                o_query = o_query.Where(p => p.p.Account.Contains(Page.Search));
            }

            var query = o_query.OrderBy(p => p.p.Account);

            try
            {
                Page.TotalRecords = query.Select(p => p.p.Id).Count();
                Page.Data = query.Skip(Page.Skip).Take(Page.Take).Select(p =>
                    new AccountModel()
                    {
                        Id = p.p.Id,
                        Account = p.p.Account,
                        CreateDate = p.p.CreateDate,
                        LastLoginTime = p.p.LastLoginTime,
                        Status = p.p.Status,
                        Role = p.r.Name,
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Page;
        }

        /// <summary>
        /// 驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="User"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool Authentication(
            string Account,
            string Password,
            out AccountModel User,
            out string errorMsg 
            )
        {
            errorMsg = string.Empty;
            string accountId = string.Empty;
            bool isSuccess = Validate(Account, Password, out accountId, out errorMsg);

            User = Get("system", accountId); 

            return errorMsg.Length == 0;
        }

        bool DataValidate(AccountModel model, out string message)
        {
            message = string.Empty;

            if (basedb.T_Account.Where(p => p.Account == model.Account).Select(p => p.Id).Count() > 0)
            {
                message = "此帳號已存在";
            }

            return message.Length == 0;
        }

        /// <summary>
        /// 建立一筆全新的實體，給予初始值
        /// </summary>
        /// <returns></returns>
        public AccountModel NewInstance()
        {
            AccountModel newOne = new AccountModel();
            newOne.Id = Library.Utils.GetObjectId();
            newOne.CreateDate = DateTime.Now;
            newOne.Status = 1;
            return newOne;
        }

        public bool Create(string UserName, AccountModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            //1.Validate
            if (DataValidate(model, out ErrMsgs))
            {
                T_Account dbEntity = new T_Account();
                dbEntity.Id = model.Id;
                dbEntity.Account = model.Account;

                //實作 Hash256 驗證
                dbEntity.Password = model.Password.ToHash256();

                //預設啟用
                dbEntity.Status = 1;
                dbEntity.Name = model.Name;
                dbEntity.IP = model.IP;
                dbEntity.LastLoginTime = DateTime.Now;
                dbEntity.CreateDate = DateTime.Now;
                dbEntity.UpdateDate = DateTime.Now;
                dbEntity.CreateUserID = model.Account;
                dbEntity.UpdateUserID = model.Account;

                //2.Add User to Roles
                if (model.RoleList != null)
                {
                    if (model.RoleList.Count() > 0)
                    {
                        AddUserToRoles(UserName,dbEntity.Id, model.RoleList);
                    }
                }

                basedb.T_Account.Add(dbEntity);

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

            return ErrMsgs.Length == 0;
        }

        public AccountModel GetByAccount(string Account)
        {
            var o_entity = (from p in basedb.T_Account
                            where p.Account == Account
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AccountModel()
                {
                    Id = o_entity.Id,
                    Account = o_entity.Account,
                    CreateDate = o_entity.CreateDate,
                    LastLoginTime = o_entity.LastLoginTime,
                    Status = o_entity.Status,
                };

                model.RoleList = GetRolesByAccountId(model.Id);
                return model;
            }
            return null;
        }

        public AccountModel GetFirstOrDefault(string userId)
        {
            var o_entity = (from p in basedb.T_Account
                            where p.Id == userId
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AccountModel()
                {
                    Id = o_entity.Id,
                    Account = o_entity.Account,
                    CreateDate = o_entity.CreateDate,
                    LastLoginTime = o_entity.LastLoginTime,
                    Status = o_entity.Status,
                    IP = o_entity.IP,
                    Name = o_entity.Name,

                };

                model.RoleList = GetRolesByAccountId(model.Id);
                return model;
            }
            return null;
        }

        public AccountModel Get(string userId, string id)
        {
            var o_entity = (from p in basedb.T_Account
                            where p.Id == id
                            select p).FirstOrDefault();

            if (o_entity != null)
            {
                var model = new AccountModel()
                    {
                        Id = o_entity.Id,
                        Account = o_entity.Account,
                        CreateDate = o_entity.CreateDate,
                        LastLoginTime = o_entity.LastLoginTime,
                        Status = o_entity.Status,
                        IP = o_entity.IP,
                        Name = o_entity.Name,

                    };

                model.RoleList = GetRolesByAccountId(model.Id);
                return model;
            }
            return null;
        }

        public bool Update(string userName, AccountModel model, out string ErrMsgs)
        {
            ErrMsgs = string.Empty;

            try
            {
                T_Account o_entity = new T_Account()
                {
                    Id = model.Id,
                    Password = model.Password.ToHash256(),
                    Account = model.Account,
                    CreateDate = model.CreateDate,
                    Status = 1,
                    CreateUserID = userName,
                    UpdateUserID = userName,
                    UpdateDate = DateTime.Now
                };

                //for entity object
                //basedb.CreateObjectSet<T_Account>().Attach(o_entity);
                //basedb.ObjectStateManager.GetObjectStateEntry(o_entity).SetModifiedProperty("Password");
                //basedb.ObjectStateManager.ChangeObjectState(o_entity, EntityState.Modified);

                basedb.T_Account.Attach(o_entity);
                //basedb.Entry(o_entity).Property(x => x.Status).IsModified = true;
                basedb.Entry(o_entity).Property(x => x.CreateDate).IsModified = true;

                //ClearRoles(model.Id);
                //AddUserToRoles(userName, model.Id, model.RoleList);

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

        public bool Delete(string userName, string id, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;

            var o_delete = from p in basedb.T_Account
                           where p.Id == id
                           select p;

            foreach (var row in o_delete)
            {
                basedb.T_Account.Remove(row);
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

        #region 其他函式

        /// <summary>
        /// 初始化最高管理者
        /// </summary>
        /// <param name="data"></param>
        /// <param name="errorMsg"></param>
        public AccountModel InitAdmin(out string errorMsg)
        {
            AccountModel model = NewInstance();
            model.Id = INIT_ACCOUNT;
            model.Account = INIT_ACCOUNT;
            model.Password = "zxcv1234";
            model.Status = 1;
            model.Name = INIT_ACCOUNT;
            model.IP = "210.61.142.43";
            model.RoleList = new List<string>() { INIT_ROLE };

            Create(INIT_ACCOUNT, model, out errorMsg);

            return model;
        }

        /// <summary>
        /// 基本驗證
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="Password"></param>
        /// <param name="AccountId"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public bool Validate(
            string Account,
            string Password,
            out string AccountId,
            out string ErrorMsg 
            )
        {
            ErrorMsg = string.Empty;
            AccountId = string.Empty;

            var dbUser = (from p in basedb.T_Account
                          where p.Account == Account
                          select p).FirstOrDefault();

            if (dbUser == null)
            {
                ErrorMsg = "無此帳號";
            }
            else
            {
                string Hash256Pwd = Password.ToHash256();

                if (dbUser.Password != Hash256Pwd)
                {
                    ErrorMsg = "密碼錯誤";
                }

                else if (dbUser.Status != 1)
                {
                    ErrorMsg = "帳號停用中,請洽管理員";
                }

                else
                {
                    try
                    {
                        AccountId = dbUser.Id;
                        dbUser.LastLoginTime = DateTime.Now;
                        basedb.SaveChanges();
                    }catch(Exception e)
                    {
                        ErrorMsg = "帳號發生異常,請洽管理員";
                    }
                }
            }

            return ErrorMsg.Length == 0;
        }

        /// <summary>
        /// 清除所有角色
        /// </summary>
        /// <param name="AccountId"></param>
        public void ClearRoles(string AccountId)
        {
            var accountRoleMappings = from p in basedb.T_Account_Role_Mapping
                                    where p.AccountId == AccountId
                                    select p;

            foreach (var mapping in accountRoleMappings)
            {
                basedb.T_Account_Role_Mapping.Remove(mapping);
            }
        }


        /// <summary>
        /// 將使用者加到角色
        /// </summary>
        /// <param name="AccountId"></param>
        /// <param name="RoleNameList"></param>
        /// <returns></returns>
        public bool AddUserToRoles(string nowLoginUser, string AccountId, List<string> RoleIdList)
        {
            foreach (string rid in RoleIdList)
            {
                T_Account_Role_Mapping mapping = new T_Account_Role_Mapping();
                mapping.Id = Utils.GetObjectId();
                mapping.RoleId = rid;
                mapping.AccountId = AccountId;
                mapping.CreateDate = DateTime.Now;
                mapping.CreateUserId = nowLoginUser;
                basedb.T_Account_Role_Mapping.Add(mapping);
                basedb.SaveChanges();
            }

            return true;
        }


        /// <summary>
        /// 依 Account Id 取得角色
        /// </summary>
        /// <param name="AccountId"></param>
        /// <returns></returns>
        public List<String> GetRolesByAccountId(string AccountId)
        {
            List<String> RoleList = new List<string>();
            var roles = from a in basedb.T_Account
                                join roleMapping in basedb.T_Account_Role_Mapping on a.Id equals roleMapping.AccountId
                                join pMapping in basedb.T_Role_Permission_Mapping on roleMapping.RoleId equals pMapping.RoleId
                                join permission in basedb.T_Permission on pMapping.PermissionId equals permission.Id
                                where a.Id == AccountId
                                select permission.Name;

            foreach (var r in roles)
            {
                RoleList.Add(r);
            }

            return RoleList;
        }
        #endregion
    }
}