CREATE TABLE [system].[ErrorLog] (
    [ErrorLogID]   BIGINT        IDENTITY (1, 1) NOT NULL,
    [ModuleName]   VARCHAR (250) NOT NULL,
    [ErrorMessage] VARCHAR (MAX) NOT NULL,
    [StackTrace]   VARCHAR (MAX) NULL,
    [OccurredDate] DATETIME      DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED ([ErrorLogID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ErrorLog_OccurredDate]
    ON [system].[ErrorLog]([OccurredDate] DESC);

