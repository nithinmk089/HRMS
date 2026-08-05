CREATE TABLE [learning].[EmployeeSkill] (
    [EmployeeSkillID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT        NOT NULL,
    [EmployeeID]      BIGINT        NOT NULL,
    [SkillID]         BIGINT        NOT NULL,
    [SkillLevel]      NVARCHAR (50) DEFAULT ('Beginner') NOT NULL,
    [CreatedBy]       BIGINT        NOT NULL,
    [CreatedDate]     DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT        NULL,
    [ModifiedDate]    DATETIME2 (7) NULL,
    [DeletedBy]       BIGINT        NULL,
    [DeletedDate]     DATETIME2 (7) NULL,
    [IsDeleted]       BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION    NOT NULL,
    CONSTRAINT [PK_EmployeeSkill] PRIMARY KEY CLUSTERED ([EmployeeSkillID] ASC),
    CONSTRAINT [FK_EmployeeSkill_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeSkill_Skill] FOREIGN KEY ([SkillID]) REFERENCES [learning].[Skill] ([SkillID]),
    CONSTRAINT [FK_EmployeeSkill_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

