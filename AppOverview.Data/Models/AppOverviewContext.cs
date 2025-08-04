using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppOverview.Data.Models;

public partial class AppOverviewContext : DbContext
{
    public AppOverviewContext()
    {
    }

    public AppOverviewContext(DbContextOptions<AppOverviewContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<Relation> Relations { get; set; }

    public virtual DbSet<Technology> Technologies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\clemens\\source\\AppOverview\\AppOverview.Data\\AppOverview.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department>(entity =>
        {
            entity.ToTable("Department");

            entity.HasIndex(e => e.DepartmentId, "IX_Department_DepartmentID").IsUnique();

            entity.HasIndex(e => e.Name, "IX_Department_Name").IsUnique();

            entity.Property(e => e.DepartmentId)
                .ValueGeneratedNever()
                .HasColumnName("DepartmentID");
            entity.Property(e => e.Active).HasDefaultValue(1);
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.ToTable("Entity");

            entity.HasIndex(e => e.EntityId, "IX_Entity_EntityID").IsUnique();

            entity.HasIndex(e => new { e.DepartmentId, e.Active }, "IX_Entity_Department");

            entity.HasIndex(e => new { e.EntityTypeId, e.Active }, "IX_Entity_EntityType");

            entity.HasIndex(e => e.TechnologyId, "IX_Entity_Technology");

            entity.Property(e => e.EntityId)
                .ValueGeneratedNever()
                .HasColumnName("EntityID");
            entity.Property(e => e.Active).HasDefaultValue(1);
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.EntityTypeId).HasColumnName("EntityTypeID");
            entity.Property(e => e.SourceControlUrl).HasColumnName("SourceControlURL");
            entity.Property(e => e.SourceControlUrl).HasColumnName("Owner");
            entity.Property(e => e.TechnologyId).HasColumnName("TechnologyID");

            entity.HasOne(d => d.Department).WithMany(p => p.Entities)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.EntityType).WithMany(p => p.Entities)
                .HasForeignKey(d => d.EntityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Technology).WithMany(p => p.Entities)
                .HasForeignKey(d => d.TechnologyId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.ToTable("EntityType");

            entity.HasIndex(e => e.EntityTypeId, "IX_EntityType_EntityTypeID").IsUnique();

            entity.HasIndex(e => e.Name, "IX_EntityType_Name").IsUnique();

            entity.Property(e => e.EntityTypeId)
                .ValueGeneratedNever()
                .HasColumnName("EntityTypeID");
            entity.Property(e => e.Active).HasDefaultValue(1);
        });

        modelBuilder.Entity<Relation>(entity =>
        {
            entity.ToTable("Relation");

            entity.HasIndex(e => e.RelationId, "IX_Relation_RelationID").IsUnique();

            entity.HasIndex(e => e.SourceEntityId, "IX_Relation_Source");

            entity.HasIndex(e => e.TargetEntityId, "IX_Relation_Target");

            entity.Property(e => e.RelationId)
                .ValueGeneratedNever()
                .HasColumnName("RelationID");
            entity.Property(e => e.Active).HasDefaultValue(1);
            entity.Property(e => e.SourceEntityId).HasColumnName("SourceEntityID");
            entity.Property(e => e.TargetEntityId).HasColumnName("TargetEntityID");

            entity.HasOne(d => d.SourceEntity).WithMany(p => p.RelationSourceEntities)
                .HasForeignKey(d => d.SourceEntityId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.TargetEntity).WithMany(p => p.RelationTargetEntities)
                .HasForeignKey(d => d.TargetEntityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Technology>(entity =>
        {
            entity.ToTable("Technology");

            entity.HasIndex(e => e.Name, "IX_Technology_Name").IsUnique();

            entity.Property(e => e.TechnologyId)
                .ValueGeneratedNever()
                .HasColumnName("TechnologyID");
            entity.Property(e => e.Active).HasDefaultValue(1);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
