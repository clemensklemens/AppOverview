using System.Threading.Tasks;

namespace AppOverview.Model.Interfaces
{
    public interface IUserAuthService
    {
        User GetUserNameAndPermissions(); // Now returns IsKnownUser as well
    }
}
