using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.BLL.Services;
using System.Web.Helpers;
using System.Web.Security;
using BowTie.View.Models.AuthModels;

namespace BowTie.View.Controllers
{
    public class AccountController : Controller
    {
        IUserService service = new UsersService();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                bool success = false;
                try
                {
                    success = service.LoginUser(model.Name, model.Password);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                if (success)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("Index", "Default");
                }
                else
                {
                    ModelState.AddModelError("", "Ім'я або пароль введено невірно");
                }
            }
            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO user = null;
                try
                {
                    user = service.GetUser(model.Name);
                }
                catch (Exception e)
                {
                    return new HttpStatusCodeResult(500, e.Message);
                }
                if (user == null)
                {
                    
                    if (service.RegisterUser(model.Name, model.Email, model.Password) != 0)
                    {
                        FormsAuthentication.SetAuthCookie(model.Name, true);
                        return RedirectToAction("Index", "Default");
                    }
                }
                else
                    ModelState.AddModelError("", "Користувач із заданим ім'ям вже існує");
            }
            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Default");
        }
    }
}