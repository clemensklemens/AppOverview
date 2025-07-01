using System.ComponentModel;

namespace AppOverview.Model
{
    public class DepartmentDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public DepartmentDTO(int id, string name, bool isActive)
        {
            Id = id;
            Name = name;
            IsActive = isActive;
        }
    }
}
