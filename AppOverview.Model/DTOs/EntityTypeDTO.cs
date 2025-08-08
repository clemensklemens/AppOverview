using System.ComponentModel;

namespace AppOverview.Model.DTOs
{
    public class EntityTypeDTO
    {                
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;   
        public string ColorHex { get; set; } = "#000000";
        public bool IsActive { get; set; } = true;

        public EntityTypeDTO() { }
    }
}
