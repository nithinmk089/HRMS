CREATE TABLE [learning].[Enrollment] (
    [EnrollmentID]     BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]         BIGINT        NOT NULL,
    [EmployeeID]       BIGINT        NOT NULL,
    [CourseID]         BIGINT        NOT NULL,
    [EnrollmentDate]   DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [EnrollmentStatus] NVARCHAR (50) DEFAULT ('Active') NOT NULL,
    [CreatedBy]        BIGINT        NOT NULL,
    [CreatedDate]      DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]       BIGINT        NULL,
    [ModifiedDate]     DATETIME2 (7) NULL,
    [DeletedBy]        BIGINT        NULL,
    [DeletedDate]      DATETIME2 (7) NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]       ROWVERSION    NOT NULL,
    CONSTRAINT [PK_Enrollment] PRIMARY KEY CLUSTERED ([EnrollmentID] ASC),
    CONSTRAINT [FK_Enrollment_Course] FOREIGN KEY ([CourseID]) REFERENCES [learning].[Course] ([CourseID]),
    CONSTRAINT [FK_Enrollment_Employee] FOREIGN KEY ([EmployeeID]) REFERENCES [hr].[Employee] ([EmployeeID]),
    CONSTRAINT [FK_Enrollment_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

