using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BowTie.View.Models;
using System.Web.Security;
using BowTie.BLL.DTO;
using BowTie.BLL.Interfaces;
using BowTie.BLL.Services;

namespace BowTie.View.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        private IUserService db = new UsersService();
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            return db.GetRolesForUser(username);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool result = false;
            try
            {
                result = db.IsUserInRole(username, roleName);
            }
            catch (Exception e)
            {
                return false;
            }
            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}