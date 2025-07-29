namespace AppOverview.Model
{
    public interface IDepartmentService
    {
        Task<DepartmentDTO> AddDepartmentAsync(DepartmentDTO department);
        Task<IEnumerable<DepartmentDTO>> GetDepartmentsAsync();
        Task UpdateDepartmentAsync(DepartmentDTO department);
    }
}
