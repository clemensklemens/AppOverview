using System.ComponentModel;

namespace AppOverview.Model
{
    public class EntityDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Department { get; set; }
        public string Technology { get; set; }
        public string HexColor { get; set; }
        public bool IsActive { get; set; }
        [Browsable(false)]
        List<EntityDTO> Dependencies { get; set; } //maybe also needs relation type

        public EntityDTO(int id, string name, string description, string type, string department, string technology, string hexColor, bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = type;
            Department = department;
            Technology = technology;
            HexColor = hexColor;
            IsActive = isActive;
        }
    }
}
