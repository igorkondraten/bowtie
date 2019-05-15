using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using System.Net;

namespace BowTie.View.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Адміністратор")]
        public ActionResult Users()
        {
            IEnumerable<UserViewModel> model;
            try
            {
                model = _userService.GetUsers().Select(u => new UserViewModel() { Id = u.Id, Name = u.Name, Role = u.Role.Name, Email = u.Email });
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Адміністратор")]
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EditUserViewModel model = new EditUserViewModel();
            UserDTO user;
            try
            {
                user = _userService.GetUser(id.Value);
            }
            catch (ArgumentException e)
            {
                return new HttpStatusCodeResult(404, e.Message);
            }
            model.Email = user.Email;
            model.Id = user.Id;
            model.Name = user.Name;
            model.RoleId = user.RoleId;
            IEnumerable<RoleDTO> roles;
            try
            {
                roles = _userService.GetRoles().ToList();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Roles = new SelectList(roles, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Адміністратор")]
        public ActionResult EditUser(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new UserDTO()
                    {
                        Name = model.Name,
                        Password = model.Password,
                        Email = model.Email,
                        RoleId = model.RoleId,
                        Id = model.Id
                    };
                    _userService.EditUser(newUser);
                    if (_userService.GetUser(HttpContext.User.Identity.Name) == null)
                    {
                        return RedirectToAction("Logoff", "Account");
                    }
                }
                catch(ArgumentException e)
                {
                    return new HttpStatusCodeResult(404, e.Message);
                }
                return RedirectToAction("Users", "Admin");
            }
            IEnumerable<RoleDTO> roles;
            try
            {
                roles = _userService.GetRoles().ToList();
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500, e.Message);
            }
            model.Roles = new SelectList(roles, "Id", "Name");
            return View(model);
        }
    }
}