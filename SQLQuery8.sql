USE [bend_bd]
GO
/****** Object:  StoredProcedure [dbo].[RegisterUserFIX]    Script Date: 25.07.2024 22:53:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[RegisterUserFIX]
    @Login NVARCHAR(50),
    @Password NVARCHAR(100),
	@Name NVARCHAR(25),
	@PhoneNumber NVARCHAR(25),
	@Country NVARCHAR(25),
    @IsBoss BIT,
	@CompanyName NVARCHAR(25)
AS
BEGIN
    BEGIN TRANSACTION;

    DECLARE @UserId UNIQUEIDENTIFIER;
    DECLARE @CompanyId UNIQUEIDENTIFIER;

    SET @CompanyId = NEWID();

    INSERT INTO company (id, cName, bigBoss, country) VALUES (@CompanyId, @CompanyName, NULL, @Country);
    
    SET @UserId = NEWID();

    INSERT INTO users (id, uLogin, uPassword, companyId, isBoss, phoneNumber, fName) VALUES (@UserId, @Login, @Password, @CompanyId, @IsBoss, @PhoneNumber, @Name);

    
	IF @IsBoss = 1
    BEGIN
        UPDATE company SET bigBoss = @UserId WHERE id = @CompanyId;
    END

    COMMIT TRANSACTION;
END;
