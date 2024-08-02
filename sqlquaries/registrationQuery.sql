CREATE PROCEDURE RegisterUserFIX
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


    IF @IsBoss = 1
    
	BEGIN
        INSERT INTO company (id, cName, bigBoss, country) VALUES (@CompanyId, @CompanyName, NULL, @Country);
    END
    ELSE
    BEGIN
        INSERT INTO company (id, cName) VALUES (@CompanyId, @CompanyName);
    END

    
    SET @UserId = NEWID();

    INSERT INTO users (id, uLogin, uPassword, companyId, isBoss, phoneNumber, fName) VALUES (@UserId, @Login, @Password, @CompanyId, @IsBoss, @PhoneNumber, @Name);

    
	IF @IsBoss = 1
    BEGIN
        UPDATE company SET bigBoss = @UserId WHERE id = @CompanyId;
    END

    COMMIT TRANSACTION;
END;
