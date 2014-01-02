USE muzeum
GO

CREATE PROCEDURE [dbo].[GetOwners]
AS
BEGIN
    SELECT id_wlasciciela, nazwa_wlasciciela, miasto_wlasciciela, kraj_wlasciciela, email_wlasciciela, telefon_wlasciciela
    From dbo.Wlasciciel 
END
GO

CREATE PROCEDURE [dbo].[UpdateOwner]
(
	@id_wlasciciela int,
	@nazwa_wlasciciela nvarchar(50),
	@miasto_wlasciciela nvarchar(50),
	@kraj_wlasciciela nvarchar(50),
	@email_wlasciciela nvarchar(50),
	@telefon_wlasciciela nvarchar(50)
                    
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    UPDATE dbo.Wlasciciel
    SET 
		nazwa_wlasciciela = @nazwa_wlasciciela, miasto_wlasciciela = @miasto_wlasciciela, kraj_wlasciciela = @kraj_wlasciciela,
		email_wlasciciela = @email_wlasciciela,telefon_wlasciciela = @telefon_wlasciciela
	WHERE id_wlasciciela=@id_wlasciciela
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[AddOwner]
(
  
	@nazwa_wlasciciela nvarchar(50),
	@miasto_wlasciciela nvarchar(50),
	@kraj_wlasciciela nvarchar(50),
	@email_wlasciciela nvarchar(50),
	@telefon_wlasciciela nvarchar(50),
	@id_wlasciciela int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    INSERT dbo.Wlasciciel  (nazwa_wlasciciela, miasto_wlasciciela, kraj_wlasciciela, email_wlasciciela, telefon_wlasciciela)
    VALUES  (@nazwa_wlasciciela,@miasto_wlasciciela,@kraj_wlasciciela, @email_wlasciciela, @telefon_wlasciciela);
    COMMIT TRANSACTION
    SET @id_wlasciciela = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    ROLLBACK
	DECLARE @ErrMsg nvarchar(4000), @ErrSeverity int
	SELECT @ErrMsg = ERROR_MESSAGE(),
           @ErrSeverity = ERROR_SEVERITY()
	RAISERROR(@ErrMsg, @ErrSeverity, 1)
END CATCH
GO
