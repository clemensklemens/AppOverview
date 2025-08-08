using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppOverview.Model.DTOs
{
    //small DTO for Id and Name used for dropdowns and lists
    public class IdNameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public IdNameDTO() { }
    }
}
