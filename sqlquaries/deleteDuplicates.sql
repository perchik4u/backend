USE [bend_bd]
GO
/****** Object:  Trigger [dbo].[DeleteDuplicates]    Script Date: 03.08.2024 1:42:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TRIGGER [dbo].[DeleteDuplicates]
ON [dbo].[company]
AFTER INSERT
AS
BEGIN

    ALTER TABLE users NOCHECK CONSTRAINT ALL;
    ALTER TABLE company NOCHECK CONSTRAINT ALL;

    -- Удаление дубликатов из таблицы company
    WITH CTE1 AS (
        SELECT *,
               ROW_NUMBER() OVER (PARTITION BY cName ORDER BY (SELECT NULL)) AS rn
        FROM company
    )
    DELETE FROM CTE1
    WHERE rn > 1;

    -- Удаление дубликатов из таблицы users
    WITH CTE2 AS (
        SELECT *,
               ROW_NUMBER() OVER (PARTITION BY fName ORDER BY (SELECT NULL)) AS rn
        FROM users
    )
    DELETE FROM CTE2
    WHERE rn > 1;

	ALTER TABLE users CHECK CONSTRAINT ALL;
    ALTER TABLE company CHECK CONSTRAINT ALL;
END;