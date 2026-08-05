CREATE TABLE [learning].[CourseContent] (
    [CourseContentID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [CourseID]        BIGINT         NOT NULL,
    [ContentTitle]    NVARCHAR (200) NOT NULL,
    [ContentType]     NVARCHAR (50)  NOT NULL,
    [ContentUrl]      NVARCHAR (500) NULL,
    [SequenceNo]      INT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_CourseContent] PRIMARY KEY CLUSTERED ([CourseContentID] ASC),
    CONSTRAINT [FK_CourseContent_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_CourseContent_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

