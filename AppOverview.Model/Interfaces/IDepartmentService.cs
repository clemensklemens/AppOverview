using AppOverview.Model.DTOs;

namespace AppOverview.Model.Interfaces
{
    public interface IDepartmentService
    {
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department);
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();
        Task UpdateDepartmentAsync(DepartmentDTO department);
    }
}
