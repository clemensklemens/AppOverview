using System.ComponentModel;

namespace AppOverview.Model
{
    public class TechnologyDTO
    {        
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public TechnologyDTO() { }
    }   
}