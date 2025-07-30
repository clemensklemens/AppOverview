namespace AppOverview.Model
{
    public class EntityDTO
    {        
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;        
        public int TypeId { get; set; } = 0;
        public string Type { get; set; } = string.Empty;        
        public int DepartmentId { get; set; } = 0;
        public string Department { get; set; } = string.Empty;        
        public int TechnologyId { get; set; } = 0;
        public string Technology { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#000000";
        public bool IsActive { get; set; } = true;        
        public List<EntityDTO> Dependencies { get; set; } = new List<EntityDTO>(); //maybe also needs relation type

        public EntityDTO() { }
    }
}
