CREATE TABLE [learning].[Skill] (
    [SkillID]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]      BIGINT         NOT NULL,
    [SkillCode]     NVARCHAR (50)  NOT NULL,
    [SkillName]     NVARCHAR (100) NOT NULL,
    [SkillCategory] NVARCHAR (100) NOT NULL,
    [CreatedBy]     BIGINT         NOT NULL,
    [CreatedDate]   DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]    BIGINT         NULL,
    [ModifiedDate]  DATETIME2 (7)  NULL,
    [DeletedBy]     BIGINT         NULL,
    [DeletedDate]   DATETIME2 (7)  NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]    ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Skill] PRIMARY KEY CLUSTERED ([SkillID] ASC),
    CONSTRAINT [FK_Skill_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

