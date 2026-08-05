CREATE TABLE [hr].[EmployeeQualification] (
    [EmployeeQualificationID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]                BIGINT         NOT NULL,
    [EmployeeID]              BIGINT         NOT NULL,
    [QualificationType]       NVARCHAR (100) NOT NULL,
    [Institution]             NVARCHAR (200) NOT NULL,
    [University]              NVARCHAR (200) NULL,
    [YearOfPassing]           INT            NOT NULL,
    [Percentage]              DECIMAL (5, 2) NULL,
    [CreatedBy]               BIGINT         NOT NULL,
    [CreatedDate]             DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]              BIGINT         NULL,
    [ModifiedDate]            DATETIME2 (7)  NULL,
    [DeletedBy]               BIGINT         NULL,
    [DeletedDate]             DATETIME2 (7)  NULL,
    [IsDeleted]               BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]              ROWVERSION     NOT NULL,
    CONSTRAINT [PK_EmployeeQualification] PRIMARY KEY CLUSTERED ([EmployeeQualificationID] ASC),
    CONSTRAINT [FK_EmployeeQualification_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_EmployeeQualification_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Qualification_Employee]
    ON [hr].[EmployeeQualification]([EmployeeID] ASC) WHERE ([IsDeleted]=(0));

