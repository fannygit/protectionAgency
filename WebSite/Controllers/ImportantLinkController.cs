﻿/*
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
using System.Drawing;

namespace EPA.Project.WebSite.Controllers
{
    public class ImportantLinkController : BaseController
    {
        ImportantLinkService Service;

        public ImportantLinkController()
        {
            Service = new ImportantLinkService();
        }

        #region CRUD
        //GET: /ImportantLink/List
        public ActionResult List(ImportantLinkListModel criteria)
        {
            try
            {
                ImportantLinkListModel model = Service.GetList(User.Identity.Name, criteria);

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
            ImportantLinkModel entity = Service.NewInstance();

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ImportantLinkModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    //if (EPA.Project.WebSite.Library.Utils.SaveAndCheckImg(model.BannerImgFile, 300, 200, out ErrMsgs, out imgName))
                    //{
                        if (Service.Create(
                            User.Identity.Name,
                            model, out ErrMsgs))
                        {
                            return RedirectToAction("List", new { page = model.page });
                        }
                    //}
                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
                
            }

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        public ActionResult Edit(int id, string Search, int page = 1)
        {
            ImportantLinkModel model = Service.Get(User.Identity.Name, id);
            model.Search = Search;
			if (model != null)
			{
				model.Mode = EditPageMode.Update;
				return View("Add", model);
			}
			else
			{
				return Content("無此資料");
			}
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(ImportantLinkModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

					string UserAccount = (null != UserManager.User) ? UserManager.User.Account : "N/A";

                    if (Service.Update(User.Identity.Name, UserAccount, model, out ErrMsgs))
                    {
                        return RedirectToAction("List", new { page = model.page, Search = model.Search });
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

            model.Mode = EditPageMode.Update;
            return View("Add", model);
        }

        public ActionResult Delete(int id, int page = 1)
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
