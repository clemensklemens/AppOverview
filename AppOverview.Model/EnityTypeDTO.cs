using System.ComponentModel;

namespace AppOverview.Model
{
    public class EnityTypeDTO
    {        
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Browsable(false)]
        public string ColorHex { get; set; }
        public bool IsActive { get; set; }

        public EnityTypeDTO(int id, string name, string description, string colorHex, bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            ColorHex = colorHex;
            IsActive = isActive;
        }
    }
}
