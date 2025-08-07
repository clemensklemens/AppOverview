using AppOverview.Model;
using AppOverview.Model.Interfaces;
using System.DirectoryServices.AccountManagement;

namespace AppOverview.Service
{
    public class UserAuthServiceWindows : IUserAuthService
    {
        private readonly string _adGroupName = "testgroup";

        public User GetUserNameAndPermissions()
        {
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            using var context = new PrincipalContext(ContextType.Domain);
            using var userPrincipal = UserPrincipal.FindByIdentity(context, userName) ?? throw new ServiceException($"User '{userName}' not found in Active Directory.");

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
