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
    public class FixedPollutionControlInfoController : BaseController
    {
        FixedPollutionControlInfoService Service;
        FixedPollutionControlService fpcService;

        public FixedPollutionControlInfoController()
        {
            Service = new FixedPollutionControlInfoService();
            fpcService = new FixedPollutionControlService();
        }

        #region CRUD
        //GET: /FixedPollutionControlInfo/List
        public ActionResult List(FixedPollutionControlInfoListModel criteria)
        {
            try
            {
                FixedPollutionControlInfoListModel model = Service.GetList(User.Identity.Name, criteria);

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
            FixedPollutionControlInfoModel entity = Service.NewInstance();
            var list = fpcService.GetCategoryTitle(null);
            ViewData["FirstLevelItems"] = list;
            int id =0;
            if (list.Count>1)
            {
                id = Convert.ToInt16(list[1].Value);
            }
            ViewData["SecondLevelItems"] = fpcService.GetCategoryTitleIN(null, id);

            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FixedPollutionControlInfoModel model)
        {
            string ErrMsgs = string.Empty;
            if (model.radionValue == "url")
            {
                model.Blog = "";
            }
            else if (model.radionValue == "blog")
            {
                model.Url = "";
            }
            else 
            {
                ModelState.AddModelError("message", "請選擇類型");
            }


            if (ModelState.IsValid)
            {
                try
                {

                    if (model.FixedPollutionControlId != -1)
                    {
                        if (model.InId != -1)
                        {
                            if (Service.Create(User.Identity.Name, model, out ErrMsgs))
                            {
                                return RedirectToAction("List", new { page = model.page });
                            }
                        }
                        else
                        {
                            ErrMsgs = "請選擇分類";
                        }
                    }
                    else
                    {
                        ErrMsgs = "請選擇分類";
                    }
                    ModelState.AddModelError("message", ErrMsgs);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }



            ViewData["SecondLevelItems"] = fpcService.GetCategoryTitleIN(model.InId.ToString(), model.FixedPollutionControlId);
            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(model.FixedPollutionControlId.ToString());
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }

        [HttpPost]
        public ActionResult GetLabel2(string nowkey, string key)
        {
            int id = Convert.ToInt16(key);
            List<SelectListItem> objcity = fpcService.GetCategoryTitleIN(nowkey, id);
            return Json(objcity);
        }

        public ActionResult Edit(int id, string Search, int page = 1)
        {
            FixedPollutionControlInfoModel model = Service.Get(User.Identity.Name, id);
            model.Search = Search;
            ViewData["FirstLevelItems"] = fpcService.GetCategoryTitle(model.FixedPollutionControlId.ToString());
            ViewData["SecondLevelItems"] = fpcService.GetCategoryTitleIN(model.InId.ToString(), model.FixedPollutionControlId);
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
        public ActionResult Update(FixedPollutionControlInfoModel model)
        {
            string ErrMsgs = string.Empty;
            if (model.radionValue == "url")
            {
                model.Blog = "";
            }
            else if (model.radionValue == "blog")
            {
                model.Url = "";
            }
            else
            {
                ModelState.AddModelError("message", "請選擇類型");
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (model.FixedPollutionControlId != -1)
                    {
                        if (model.InId != -1)
                        {
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
                        else
                        {
                            ModelState.AddModelError("message", "請選擇分類");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("message", "請選擇分類");
                    }

					
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("message", ex.Message);
                }
            }

            ViewData["SecondLevelItems"] = fpcService.GetCategoryTitleIN(model.InId.ToString(), model.FixedPollutionControlId);
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
