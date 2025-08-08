using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department, string userName);
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();
        Task UpdateDepartmentAsync(DepartmentDTO department, string userName);
    }
}
