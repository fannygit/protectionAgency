using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


using EPA.Project.WebSite.Models;
using EPA.Project.WebSite.Service;
using EPA.Project.WebSite.ViewModels;
using EPA.Project.WebSite.Enums;
using EPA.Project.WebSite.Filter;
using EPA.Project.WebSite.Library.Principal;
using EPA.Project.WebSite.Library;

namespace EPA.Project.WebSite.Controllers
{
    public class AccountController : Controller
    {
        AccountService Service = new AccountService();
        PermissionService permissionService = new PermissionService();
        RoleService roleService = new RoleService();

        public ActionResult test(string t)
        {
            if (string.IsNullOrEmpty(t))
                t = "zxcv1234";
            return Content(Utils.ToHash256(t, null));
        }
        public ActionResult testip()
        {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            return Content(myIP);
        }

        public AccountController()
        {
            Service = new AccountService();
        }

        

        #region CRUD
        //GET: /Account/List
        [Authorize]
        public ActionResult List(AccountListModel criteria)
        {
            try
            {
                AccountListModel model = Service.GetList(User.Identity.Name, criteria);

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("message", ex.Message);
            }

            return View(criteria);
        }

        [Authorize]
        public ActionResult Add(int page = 1)
        {
            AccountModel entity = Service.NewInstance();
            entity.RoleDropDownList = roleService.GetRoleList("");
            entity.page = page;
            entity.Mode = EditPageMode.Create;
            return View(entity);
        }

        [Authorize]
        public ActionResult Change(int page = 1)
        {
            AccountModel model = Service.GetFirstOrDefault(User.Identity.Name);
            model.Mode = EditPageMode.Update;
            model.RoleDropDownList = roleService.GetRoleList(model.Id);
            model.page = page;
            return View("Add", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(AccountModel model)
        {
            if (ModelState.IsValid)
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

            model.Mode = EditPageMode.Create;
            return View("Add", model);
        }


        [Authorize]
        public ActionResult Edit(string id, int page = 1)
        {
            if (string.IsNullOrEmpty(id))
            { 
                id = User.Identity.Name;
            }
            AccountModel model = Service.Get(User.Identity.Name, id);
            model.Mode = EditPageMode.Update;
            model.RoleDropDownList = roleService.GetRoleList(id);
            model.page = page;
            return View("Add", model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Update(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string ErrMsgs = string.Empty;

                    if (Service.Update(User.Identity.Name, model, out ErrMsgs))
                    {
                        //return RedirectToAction("List", new { page = model.page });
                        return RedirectToAction("Info");
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

        [Authorize]
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


        [AllowedIpOnly("118.163.26.49")]
        public ActionResult Info()
        {
            return View();
        }

        #region 認證模組

        // 登入頁
        // GET: /Account/Login

        [AllowedIpOnly("118.163.26.49")]
        public ActionResult Login()
        {
            LoginPage Model = new LoginPage();

            return View("Index", Model);
        }

        // 登入驗證
        // POST: /Account/Login
        [HttpPost]

        [AllowedIpOnly("118.163.26.49")]
        public ActionResult Login(LoginPage Model, string returnUrl)
        {
            string errorMsg = string.Empty;
            if (ModelState.IsValid)
            {
                AccountModel user = null;
                bool isSuccess = Service.Authentication(
                    Model.Account, 
                    Model.Password,
                    out user,
                    out errorMsg);

                if (isSuccess)
                {
                    //要存在 cookie 的 user data
                    WebSiteUser userData = new WebSiteUser()
                    {
                        Id = user.Id,
                        Account = user.Account,
                        Roles = user.RoleList.Select(p => p.ToString()).ToArray()
                    };

                    bool isCookiePersistent = false;
                    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    string jsonAccountModel = serializer.Serialize(userData);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1,
                              user.Id, DateTime.Now, DateTime.Now.AddMinutes(60), isCookiePersistent, jsonAccountModel);

                    string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

                    if (true == isCookiePersistent)
                        authCookie.Expires = authTicket.Expiration;
                    Response.Cookies.Add(authCookie);
                    //Response.Redirect(FormsAuthentication.GetRedirectUrl(Model.Account, false));
                    Response.Redirect("~/Account/Info");
                }

            }

            ModelState.AddModelError("message", errorMsg);

            return View("index", Model);
        }


        // 登出
        // GET: /Account/Signout

        [AllowedIpOnly("118.163.26.49")]
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Account/Login");
        }

        #endregion
        

    }
}
