CREATE TABLE [learning].[CourseCategory] (
    [CourseCategoryID] BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT         NOT NULL,
    [CategoryCode]     NVARCHAR (50)  NOT NULL,
    [CategoryName]     NVARCHAR (100) NOT NULL,
    [ParentCategoryID] BIGINT         NULL,
    [Description]      NVARCHAR (500) NULL,
    [IsActive]         BIT            DEFAULT ((1)) NOT NULL,
    [CreatedBy]        BIGINT         NOT NULL,
    [CreatedDate]      DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT         NULL,
    [ModifiedDate]     DATETIME2 (7)  NULL,
    [DeletedBy]        BIGINT         NULL,
    [DeletedDate]      DATETIME2 (7)  NULL,
    [IsDeleted]        BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION     NOT NULL,
    CONSTRAINT [PK_CourseCategory] PRIMARY KEY CLUSTERED ([CourseCategoryID] ASC),
    CONSTRAINT [FK_CourseCategory_Parent] FOREIGN KEY ([ParentCategoryID]) REFERENCES [learning].[CourseCategory] ([CourseCategoryID]),
    CONSTRAINT [FK_CourseCategory_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_CourseCategory_Name]
    ON [learning].[CourseCategory]([CategoryName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE NONCLUSTERED INDEX [IX_CourseCategory_Code]
    ON [learning].[CourseCategory]([CategoryCode] ASC) WHERE ([IsDeleted]=(0));

