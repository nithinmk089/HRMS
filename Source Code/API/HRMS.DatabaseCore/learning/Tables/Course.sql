CREATE TABLE [learning].[Course] (
    [CourseID]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT          NOT NULL,
    [CourseCode]       NVARCHAR (50)   NOT NULL,
    [CourseName]       NVARCHAR (200)  NOT NULL,
    [CourseCategoryID] BIGINT          NOT NULL,
    [Description]      NVARCHAR (1000) NULL,
    [DurationMinutes]  INT             DEFAULT ((60)) NOT NULL,
    [CourseStatus]     NVARCHAR (50)   DEFAULT ('Draft') NOT NULL,
    [CreatedBy]        BIGINT          NOT NULL,
    [CreatedDate]      DATETIME2 (7)   DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT          NULL,
    [ModifiedDate]     DATETIME2 (7)   NULL,
    [DeletedBy]        BIGINT          NULL,
    [DeletedDate]      DATETIME2 (7)   NULL,
    [IsDeleted]        BIT             DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION      NOT NULL,
    CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED ([CourseID] ASC),
    CONSTRAINT [FK_Course_Category] FOREIGN KEY ([CourseCategoryID]) REFERENCES [learning].[CourseCategory] ([CourseCategoryID]),
    CONSTRAINT [FK_Course_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Course_Category]
    ON [learning].[Course]([CourseCategoryID] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_Course_Code]
    ON [learning].[Course]([CourseCode] ASC) WHERE ([IsDeleted]=(0));

