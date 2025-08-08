using System;
using System.Collections.Generic;

namespace AppOverview.Data.Models;

public partial class Technology
{
    public int TechnologyId { get; set; }

    public string Name { get; set; } = null!;

    public string LastUser { get; set; } = null!;

    public DateTime? LastChange { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();
}
