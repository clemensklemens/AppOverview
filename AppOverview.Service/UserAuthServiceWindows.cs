using AppOverview.Model;
using AppOverview.Model.Interfaces;
using System.DirectoryServices.AccountManagement;
using System.Runtime.Caching;

namespace AppOverview.Service
{
    public class UserAuthServiceWindows : IUserAuthService
    {
        private readonly string _adGroupName = "AppOverviewAdmin";
        private static readonly MemoryCache _cache = MemoryCache.Default;
        private static readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        public User GetUserNameAndPermissions()
        {
            User user = new User();
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            if (string.IsNullOrEmpty(userName))
            {
                return new User()
                {
                    Name = "Unknown",
                    IsAdmin = false,
                    IsKnownUser = false
                };
            }

            string cacheKey = $"UserAuth_{userName}";
            if (_cache.Get(cacheKey) is User cachedUser)
            {
                return cachedUser;
            }

            using var context = new PrincipalContext(ContextType.Domain);
            using var userPrincipal = UserPrincipal.FindByIdentity(context, userName);

            if (userPrincipal is null)
            {
                user = new User
                {
                    Name = userName,
                    IsAdmin = false,
                    IsKnownUser = false
                };
            }
            else
            {
                bool isAdmin = false;
                var group = GroupPrincipal.FindByIdentity(context, _adGroupName);
                if (group != null && userPrincipal.IsMemberOf(group))
                {
                    isAdmin = true;
                }
                user = new User
                {
                    Name = userPrincipal.SamAccountName,
                    IsAdmin = isAdmin,
                    IsKnownUser = true
                };
            }

            _cache.Set(cacheKey, user, new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(_cacheDuration) });
            return user;
        }
    }
}
