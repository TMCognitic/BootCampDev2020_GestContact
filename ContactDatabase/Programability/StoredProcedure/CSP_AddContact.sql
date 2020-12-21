CREATE PROCEDURE [dbo].[CSP_AddContact]
	@LastName NVARCHAR(75), 
    @FirstName NVARCHAR(75), 
    @Email NVARCHAR(320), 
    @Phone NVARCHAR(20), 
    @BirthDate DATE
AS
BEGIN
	INSERT INTO Contact (LastName, FirstName, Email, Phone, BirthDate) 
    OUTPUT inserted.Id
    VALUES (@LastName, @FirstName, @Email, @Phone, @BirthDate);
END
