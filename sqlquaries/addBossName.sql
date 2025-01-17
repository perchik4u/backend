USE [bend_bd]
GO
/****** Object:  Trigger [dbo].[UpdateBossName]    Script Date: 03.08.2024 2:19:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[UpdateBossName]
ON [dbo].[company]
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE c
    SET c.bossName = u.fName
    FROM company c
    INNER JOIN users u ON c.bigBoss = u.id
    WHERE c.bigBoss IS NOT NULL;
END;