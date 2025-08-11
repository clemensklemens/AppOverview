CREATE TABLE [Department] (
    [DepartmentID] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [LastUser] nvarchar(max) NOT NULL,
    [LastChange] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Department] PRIMARY KEY ([DepartmentID])
);
GO


CREATE TABLE [EntityType] (
    [EntityTypeID] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Apperance] nvarchar(max) NOT NULL,
    [LastUser] nvarchar(max) NOT NULL,
    [LastChange] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_EntityType] PRIMARY KEY ([EntityTypeID])
);
GO


CREATE TABLE [Technology] (
    [TechnologyID] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [LastUser] nvarchar(max) NOT NULL,
    [LastChange] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Technology] PRIMARY KEY ([TechnologyID])
);
GO


CREATE TABLE [Entity] (
    [EntityID] int NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NULL,
    [SourceControlURL] nvarchar(max) NULL,
    [Owner] nvarchar(max) NULL,
    [EntityTypeID] int NOT NULL,
    [DepartmentID] int NOT NULL,
    [TechnologyID] int NOT NULL,
    [LastUser] nvarchar(max) NOT NULL,
    [LastChange] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Entity] PRIMARY KEY ([EntityID]),
    CONSTRAINT [FK_Entity_Department_DepartmentID] FOREIGN KEY ([DepartmentID]) REFERENCES [Department] ([DepartmentID]),
    CONSTRAINT [FK_Entity_EntityType_EntityTypeID] FOREIGN KEY ([EntityTypeID]) REFERENCES [EntityType] ([EntityTypeID]),
    CONSTRAINT [FK_Entity_Technology_TechnologyID] FOREIGN KEY ([TechnologyID]) REFERENCES [Technology] ([TechnologyID])
);
GO


CREATE TABLE [Relation] (
    [RelationID] int NOT NULL,
    [Description] nvarchar(max) NULL,
    [SourceEntityID] int NOT NULL,
    [TargetEntityID] int NOT NULL,
    [LastUser] nvarchar(max) NOT NULL,
    [LastChange] datetime2 NULL,
    [Active] bit NOT NULL DEFAULT CAST(1 AS bit),
    CONSTRAINT [PK_Relation] PRIMARY KEY ([RelationID]),
    CONSTRAINT [FK_Relation_Entity_SourceEntityID] FOREIGN KEY ([SourceEntityID]) REFERENCES [Entity] ([EntityID]),
    CONSTRAINT [FK_Relation_Entity_TargetEntityID] FOREIGN KEY ([TargetEntityID]) REFERENCES [Entity] ([EntityID])
);
GO


CREATE UNIQUE INDEX [IX_Department_DepartmentID] ON [Department] ([DepartmentID]);
GO


CREATE UNIQUE INDEX [IX_Department_Name] ON [Department] ([Name]);
GO


CREATE INDEX [IX_Entity_Department] ON [Entity] ([DepartmentID], [Active]);
GO


CREATE UNIQUE INDEX [IX_Entity_EntityID] ON [Entity] ([EntityID]);
GO


CREATE INDEX [IX_Entity_EntityType] ON [Entity] ([EntityTypeID], [Active]);
GO


CREATE INDEX [IX_Entity_Technology] ON [Entity] ([TechnologyID]);
GO


CREATE UNIQUE INDEX [IX_EntityType_EntityTypeID] ON [EntityType] ([EntityTypeID]);
GO


CREATE UNIQUE INDEX [IX_EntityType_Name] ON [EntityType] ([Name]);
GO


CREATE UNIQUE INDEX [IX_Relation_RelationID] ON [Relation] ([RelationID]);
GO


CREATE INDEX [IX_Relation_Source] ON [Relation] ([SourceEntityID]);
GO


CREATE INDEX [IX_Relation_Target] ON [Relation] ([TargetEntityID]);
GO


CREATE UNIQUE INDEX [IX_Technology_Name] ON [Technology] ([Name]);
GO


