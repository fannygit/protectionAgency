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
    public class RelatedFileDownloadInfoController : BaseController
    {
        RelatedFileDownloadInfoService Service;
        RelatedFileDownloadService top1Service;
        public RelatedFileDownloadInfoController()
        {
            Service = new RelatedFileDownloadInfoService();
            top1Service = new RelatedFileDownloadService();
        }

        #region CRUD
        //GET: /RelatedFileDownloadInfo/List
        public ActionResult List(RelatedFileDownloadInfoListModel criteria)
        {
            try
            {
                RelatedFileDownloadInfoListModel model = Service.GetList(User.Identity.Name, criteria);
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
            RelatedFileDownloadInfoModel entity = Service.NewInstance();
            ViewData["FirstLevelItems"] = top1Service.GetCategoryTitle(null);
            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(RelatedFileDownloadInfoModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;
                    if (model.RelatedFileDownloadId != -1)
                    {
                        if (model.SubFileFile != null)
                        {
                            if (Service.Create(User.Identity.Name, model, out ErrMsgs))
                            {
                                return RedirectToAction("List", new { page = model.page });
                            }
                        }
                        else
                        {
                            ErrMsgs = "請選擇檔案";
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

            ViewData["FirstLevelItems"] = top1Service.GetCategoryTitle(model.RelatedFileDownloadId.ToString());
            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        public ActionResult Edit(int id, string Search, int page = 1)
        {
            RelatedFileDownloadInfoModel model = Service.Get(User.Identity.Name, id);
            ViewData["FirstLevelItems"] = top1Service.GetCategoryTitle(model.RelatedFileDownloadId.ToString());
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
        public ActionResult Update(RelatedFileDownloadInfoModel model)
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
                        ViewData["FirstLevelItems"] = top1Service.GetCategoryTitle(model.RelatedFileDownloadId.ToString());
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
