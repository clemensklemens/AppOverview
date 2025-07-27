namespace AppOverview.Model
{
    public interface IDepartmentService
    {
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department);
        Task<DepartmentDTO> GetDepartmentAsync(int id);
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task UpdateDepartmentAsync(DepartmentDTO department);
    }
}
