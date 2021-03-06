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
using Newtonsoft.Json;

namespace EPA.Project.WebSite.Controllers
{
    public class FixedPollutionControlInController : BaseController
    {
        FixedPollutionControlInService Service;
        FixedPollutionControlService fpcService;

        public FixedPollutionControlInController()
        {
            Service = new FixedPollutionControlInService();
            fpcService = new FixedPollutionControlService();
        }

        public class classd
        {
            public string key { get; set; }
            public string value { get; set; }
        }

        public class classz
        {
            public List<classd> data { get; set; }
        }

        [HttpPost]
        public ActionResult ReOrder(string s)
        {
            classz d = JsonConvert.DeserializeObject<classz>(s);
            bool isSuccess = Service.ReOrder(d);
            return Json(new { isSuccess = isSuccess });
        }

        #region CRUD
        //GET: /FixedPollutionControlIn/List
        public ActionResult List(FixedPollutionControlInListModel criteria)
        {
            try
            {
                FixedPollutionControlInListModel model = Service.GetList(User.Identity.Name, criteria);

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
            FixedPollutionControlInModel entity = Service.NewInstance();
            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(null);

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [HttpPost]
        public ActionResult Create(FixedPollutionControlInModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.FixedPollutionControlId != -1)
                {
                    try
                    {
                        string ErrMsgs = string.Empty;

                        if (Service.Create(
                            User.Identity.Name,
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
                else
                {
                    ModelState.AddModelError("message", "請選擇分類");
                }
            }

            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(model.FixedPollutionControlId.ToString());
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        public ActionResult Edit(int id, string Search, int page = 1)
        {
            FixedPollutionControlInModel model = Service.Get(User.Identity.Name, id);
            model.Search = Search;
            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(model.FixedPollutionControlId.ToString());
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
        public ActionResult Update(FixedPollutionControlInModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.FixedPollutionControlId != -1)
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
                else
                {
                    ModelState.AddModelError("message", "請選擇分類");
                }
            }

            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(model.FixedPollutionControlId.ToString());
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
