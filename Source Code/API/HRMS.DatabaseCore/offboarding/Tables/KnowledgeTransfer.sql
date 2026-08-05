CREATE TABLE [offboarding].[KnowledgeTransfer] (
    [KnowledgeTransferID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]            BIGINT        NOT NULL,
    [EmployeeID]          BIGINT        NOT NULL,
    [SuccessorEmployeeID] BIGINT        NULL,
    [KTDate]              DATE          NULL,
    [KTStatus]            NVARCHAR (50) DEFAULT ('Pending') NOT NULL,
    [CreatedBy]           BIGINT        NOT NULL,
    [CreatedDate]         DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]          BIGINT        NULL,
    [ModifiedDate]        DATETIME2 (7) NULL,
    [DeletedBy]           BIGINT        NULL,
    [DeletedDate]         DATETIME2 (7) NULL,
    [IsDeleted]           BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]          ROWVERSION    NOT NULL,
    CONSTRAINT [PK_KnowledgeTransfer] PRIMARY KEY CLUSTERED ([KnowledgeTransferID] ASC),
    CONSTRAINT [FK_KnowledgeTransfer_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_KnowledgeTransfer_Successor] FOREIGN KEY ([SuccessorEmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_KnowledgeTransfer_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

