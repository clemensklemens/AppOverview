using System;
using System.Collections.Generic;

namespace AppOverview.Data.Models;

public partial class Relation
{
    public int RelationId { get; set; }

    public string? Description { get; set; }

    public int SourceEntityId { get; set; }

    public int TargetEntityId { get; set; }

    public string LastUser { get; set; } = null!;

    public DateTime? LastChange { get; set; } = null!;

    public bool Active { get; set; }

    public virtual Entity SourceEntity { get; set; } = null!;

    public virtual Entity TargetEntity { get; set; } = null!;
}
