using System.Threading.Tasks;

namespace AppOverview.Model.Interfaces
{
    public interface IUserAuthService
    {
        User GetUserNameAndPermissions();
    }
}
