using System.ComponentModel;

namespace AppOverview.Model
{
    public struct TechnologyDTO
    {
        [Browsable(false)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public TechnologyDTO(int id, string name, string description, bool isActive)
        {
            Id = id;
            Name = name;
            Description = description;
            IsActive = isActive;
        }
    }   
}
