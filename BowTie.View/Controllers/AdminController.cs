﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BowTie.View.Models;
using BowTie.BLL.DTO;
using BowTie.BLL.Services;
using BowTie.BLL.Interfaces;
using System.Net;

namespace BowTie.View.Controllers
{
    public class AdminController : Controller
    {
        IUserService service = new UsersService();

        [Authorize(Roles = "Адміністратор")]
        public ActionResult Users()
        {
            IEnumerable<UserViewModel> model;
            try
            {
                model = service.GetUsers().Select(u => new UserViewModel() { Id = u.Id, Name = u.Name, Role = u.Role.Name, Email = u.Email });
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
                user = service.GetUser(id.Value);
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
                roles = service.GetRoles().ToList();
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
                    service.EditUser(model.RoleId, model.Id, model.Name, model.Email, model.Password);
                    // Якщо користувач змінює своє ім'я, виконати вихід з аккаунту для оновлення cookie
                    if (service.GetUser(HttpContext.User.Identity.Name) == null)
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
                roles = service.GetRoles().ToList();
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