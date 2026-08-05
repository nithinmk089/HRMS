CREATE TABLE [offboarding].[ExperienceLetterRequest] (
    [ExperienceLetterRequestID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]                  BIGINT        NOT NULL,
    [EmployeeID]                BIGINT        NOT NULL,
    [RequestDate]               DATE          NOT NULL,
    [GeneratedDate]             DATE          NULL,
    [Status]                    NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]                 BIGINT        NOT NULL,
    [CreatedDate]               DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]                BIGINT        NULL,
    [ModifiedDate]              DATETIME2 (7) NULL,
    [DeletedBy]                 BIGINT        NULL,
    [DeletedDate]               DATETIME2 (7) NULL,
    [IsDeleted]                 BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]                ROWVERSION    NOT NULL,
    CONSTRAINT [PK_ExperienceLetterRequest] PRIMARY KEY CLUSTERED ([ExperienceLetterRequestID] ASC),
    CONSTRAINT [FK_ExperienceLetterRequest_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_ExperienceLetterRequest_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

