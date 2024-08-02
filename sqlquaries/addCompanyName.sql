CREATE TRIGGER UpdateCompanyName
ON users
AFTER INSERT, UPDATE
AS
BEGIN
    UPDATE u
    SET u.companyName = c.cName
    FROM users u
    INNER JOIN company c ON u.companyId = c.id
    WHERE u.companyId IS NOT NULL;
END;