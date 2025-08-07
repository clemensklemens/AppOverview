using System.Threading.Tasks;

namespace AppOverview.Model.Interfaces
{
    public interface IActiveDirectoryUserService
    {
        /// <summary>
        /// Checks if the given username is a member of the AD group "testgroup" and returns a User object with IsAdmin set accordingly.
        /// </summary>
        /// <param name="username">The username to check (e.g., sAMAccountName).</param>
        /// <returns>User object with Name and IsAdmin properties set.</returns>
        User GetUserWithAdminStatus(string username);
    }
}
