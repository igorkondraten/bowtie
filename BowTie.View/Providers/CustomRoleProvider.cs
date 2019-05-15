using System;
using System.Linq;
using System.Web.Security;
using BowTie.BLL.Interfaces;
using Ninject;

namespace BowTie.View.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public IUserService db { get; set; }

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
            using (var scope = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                db = scope.GetService(typeof(IUserService)) as IUserService;
                return db.GetRolesForUser(username).ToArray();
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var scope = System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver.BeginScope())
            {
                db = scope.GetService(typeof(IUserService)) as IUserService;
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