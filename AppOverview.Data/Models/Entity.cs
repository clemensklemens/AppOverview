using System;
using System.Collections.Generic;

namespace AppOverview.Data.Models;

public partial class Entity
{
    public int EntityId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? SourceControlUrl { get; set; }

    public int EntityTypeId { get; set; }

    public int DepartmentId { get; set; }

    public int TechnologyId { get; set; }

    public string LastUser { get; set; } = null!;

    public string LastChange { get; set; } = null!;

    public int Active { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual EntityType EntityType { get; set; } = null!;

    public virtual ICollection<Relation> RelationSourceEntities { get; set; } = new List<Relation>();

    public virtual ICollection<Relation> RelationTargetEntities { get; set; } = new List<Relation>();

    public virtual Technology Technology { get; set; } = null!;
}
