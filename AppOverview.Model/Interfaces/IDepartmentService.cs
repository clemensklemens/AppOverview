using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IDepartmentService
    {
        Task<IdNameDTO> AddDepartmentAsync(IdNameDTO department, string userName);
        Task<IEnumerable<IdNameDTO>> GetDepartmentsAsync();
        Task UpdateDepartmentAsync(IdNameDTO department, string userName);
    }
}
