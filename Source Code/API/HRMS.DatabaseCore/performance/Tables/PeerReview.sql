CREATE TABLE [performance].[PeerReview] (
    [PeerReviewID] BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]     BIGINT          NOT NULL,
    [ReviewerID]   BIGINT          NOT NULL,
    [EmployeeID]   BIGINT          NOT NULL,
    [ReviewText]   NVARCHAR (2000) NOT NULL,
    [CreatedBy]    BIGINT          NOT NULL,
    [CreatedDate]  DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]   BIGINT          NULL,
    [ModifiedDate] DATETIME2 (7)   NULL,
    [DeletedBy]    BIGINT          NULL,
    [DeletedDate]  DATETIME2 (7)   NULL,
    [IsDeleted]    BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]   ROWVERSION      NOT NULL,
    CONSTRAINT [PK_PeerReview] PRIMARY KEY CLUSTERED ([PeerReviewID] ASC),
    CONSTRAINT [FK_PeerReview_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_PeerReview_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

