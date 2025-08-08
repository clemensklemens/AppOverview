using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppOverview.Data.Models;

public partial class EntityType
{
    public int EntityTypeId { get; set; }

    public string Name { get; set; } = null!;

    public string Apperance { get; set; } = null!;

    public string LastUser { get; set; } = null!;

    public DateTime? LastChange { get; set; } = null!;

    public bool Active { get; set; }

    public virtual ICollection<Entity> Entities { get; set; } = new List<Entity>();
}
