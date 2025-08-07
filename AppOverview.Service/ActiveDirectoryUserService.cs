using AppOverview.Model;
using AppOverview.Model.Interfaces;
using System.DirectoryServices.AccountManagement;

namespace AppOverview.Service
{
    public class ActiveDirectoryUserService : IActiveDirectoryUserService
    {
        private readonly string _adGroupName = "testgroup";

        public User GetUserWithAdminStatus(string username)
        {
            using var context = new PrincipalContext(ContextType.Domain);
            using var userPrincipal = UserPrincipal.FindByIdentity(context, username) ?? throw new ServiceException($"User '{username}' not found in Active Directory.");

            bool isAdmin = false;
            var group = GroupPrincipal.FindByIdentity(context, _adGroupName);
            if (group != null && userPrincipal.IsMemberOf(group))
            {
                isAdmin = true;
            }

            return new User
            {
                Name = userPrincipal.SamAccountName,
                IsAdmin = isAdmin
            };
        }
    }
}
