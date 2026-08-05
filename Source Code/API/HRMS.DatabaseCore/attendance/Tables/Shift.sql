CREATE TABLE [attendance].[Shift] (
    [ShiftID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [TenantID]        BIGINT         NOT NULL,
    [ShiftCode]       NVARCHAR (50)  NOT NULL,
    [ShiftName]       NVARCHAR (100) NOT NULL,
    [ShiftType]       NVARCHAR (50)  NOT NULL,
    [StartTime]       TIME (7)       NOT NULL,
    [EndTime]         TIME (7)       NOT NULL,
    [GraceInMinutes]  INT            DEFAULT ((0)) NOT NULL,
    [GraceOutMinutes] INT            DEFAULT ((0)) NOT NULL,
    [IsFlexible]      BIT            DEFAULT ((0)) NOT NULL,
    [CreatedBy]       BIGINT         NOT NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]      BIGINT         NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    [DeletedBy]       BIGINT         NULL,
    [DeletedDate]     DATETIME2 (7)  NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [RowVersion]      ROWVERSION     NOT NULL,
    CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED ([ShiftID] ASC),
    CONSTRAINT [FK_Shift_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);


GO
CREATE NONCLUSTERED INDEX [IX_Shift_Name]
    ON [attendance].[Shift]([ShiftName] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UQ_Shift_ShiftCode]
    ON [attendance].[Shift]([TenantID] ASC, [ShiftCode] ASC) WHERE ([IsDeleted]=(0));

