/*
ModelName
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.Service;
using EPA.Project.WebSite.ViewModels;
using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Library.Principal;

namespace EPA.Project.WebSite.Controllers
{
    public class RoleController : BaseController
    {
        RoleService Service;

        public RoleController()
        {
            Service = new RoleService();
        }

        #region CRUD
        //GET: /Role/List
        public ActionResult List(RoleListModel criteria)
        {
            try
            {
                RoleListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            return View(criteria);
        }

        public ActionResult Add(int page = 1)
        {
            RoleModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            entity.PermissionList = Service.GetPermissions();
            entity.PermissionMappingList = Service.GetPermissionsChoose("");
            return View(entity);
        }

        [HttpPost]
        public ActionResult Create(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Create(
                        UserAccount, 
                        model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page });
                    }

                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }
            model.PermissionList = Service.GetPermissions();
            model.PermissionMappingList = Service.GetPermissionsChoose("");
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        public ActionResult Edit(string id, int page = 1)
        {
            RoleModel model = Service.Get(User.Identity.Name, id);
			if (model != null)
			{
                model.PermissionList = Service.GetPermissions();
                model.PermissionMappingList = Service.GetPermissionsChoose(id);
				model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }

        [HttpPost]
        public ActionResult Update(RoleModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

					string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page });
                    }
                    else
                    {
                        ModelState.AddModelError("message", "修改失敗:" + ErrMsgs);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }
            model.PermissionList = Service.GetPermissions();
            model.PermissionMappingList = Service.GetPermissionsChoose(model.Id);
            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        [HttpPost]
        public ActionResult Delete(string id, int page = 1)
        {
            try
            {
                string errorMsg = string.Empty;

                if (Service.Delete(User.Identity.Name, id, out errorMsg))
                {
                    TempData["delete"] = true;
                }
                return RedirectToAction("List", new { page = page });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }
            return RedirectToAction("List");
        }
        #endregion
       

    }
}
