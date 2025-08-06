using System.ComponentModel;

namespace AppOverview.Model.DTOs
{
    public class DepartmentDTO
    {        
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public DepartmentDTO() { }
    }
}
