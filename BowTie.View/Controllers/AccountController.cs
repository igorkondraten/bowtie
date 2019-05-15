using System;
using System.Web.Mvc;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using System.Web.Security;
using BowTie.View.Models.AuthModels;

namespace BowTie.View.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool success;
            try
            {
                success = _userService.LoginUser(model.Name, model.Password);
            }
            catch (Exception e)
            {
                success = false;
            }
            if (!success)
            {
                ModelState.AddModelError("", "Ім'я або пароль введено невірно");
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(model.Name, true);
            return RedirectToAction("Index", "Default");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            UserDTO user;
            try
            {
                user = _userService.GetUser(model.Name);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            if (user != null)
            {
                ModelState.AddModelError("", "Користувач із заданим ім'ям вже існує");
                return View(model);
            }
            var newUser = new UserDTO()
            {
                Name = model.Name,
                Password = model.Password,
                Email = model.Email,
                RoleId = 2
            };
            if (_userService.RegisterUser(newUser) == 0)
                return View(model);
            FormsAuthentication.SetAuthCookie(model.Name, true);
            return RedirectToAction("Index", "Default");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Default");
        }
    }
}