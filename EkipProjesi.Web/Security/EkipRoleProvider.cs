using EkipProjesi.Data;
using System;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace EkipProjesi.Web.Security
{
    public class EkipRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            using (var _db = new EKIPEntities())
            {
                int userid = (int)HttpContext.Current.Session["UserID"];
                var user = _db.Kullanicilar.SingleOrDefault(u => u.ID == userid);
                var kullaniciId = _db.Kullanicilar.Where(u => u.ID == userid).Select(u => u.ID).FirstOrDefault();
                var userRoles = _db.KullaniciErisimBilgileri.Where(r => r.KullaniciID == userid).Select(r=>r.ErisimKoduID.ToString().Trim()).ToArray();
                username = user.KullaniciAdi;

                if (userRoles != null)
                    return userRoles.ToArray();
                else
                    return new string[] { };
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
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