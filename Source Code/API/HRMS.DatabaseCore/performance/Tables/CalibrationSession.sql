CREATE TABLE [performance].[CalibrationSession] (
    [CalibrationSessionID] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TenantID]             BIGINT        NOT NULL,
    [SessionDate]          DATETIME2 (7) NOT NULL,
    [FacilitatorID]        BIGINT        NOT NULL,
    [SessionStatus]        NVARCHAR (50) DEFAULT ('Draft') NOT NULL,
    [CreatedBy]            BIGINT        NOT NULL,
    [CreatedDate]          DATETIME2 (7) DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy]           BIGINT        NULL,
    [ModifiedDate]         DATETIME2 (7) NULL,
    [DeletedBy]            BIGINT        NULL,
    [DeletedDate]          DATETIME2 (7) NULL,
    [IsDeleted]            BIT           DEFAULT ((0)) NOT NULL,
    [RowVersion]           ROWVERSION    NOT NULL,
    CONSTRAINT [PK_CalibrationSession] PRIMARY KEY CLUSTERED ([CalibrationSessionID] ASC),
    CONSTRAINT [FK_CalibrationSession_Tenant] FOREIGN KEY ([TenantID]) REFERENCES [security].[Tenant] ([TenantID])
);

