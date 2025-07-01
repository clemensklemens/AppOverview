using System;
using System.Collections.Generic;

namespace AppOverview.Data.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; } = null!;

    public string LastUser { get; set; } = null!;

    public string LastChange { get; set; } = null!;

    public int Active { get; set; }

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();
}
