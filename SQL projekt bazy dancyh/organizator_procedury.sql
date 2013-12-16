USE muzeum_v4
GO

CREATE PROCEDURE [dbo].[GetOrgs]
AS
BEGIN
    SELECT id_organizatora, nazwa_organizatora, miasto_organizatora, e_mail_organizatora, telefon_organizatora
    From dbo.Organizator 
END
GO

CREATE PROCEDURE [dbo].[UpdateOrg]
(
	@id_organizatora int,
	@nazwa_organizatora nvarchar(50),
	@miasto_organizatora nvarchar(50),
	@e_mail_organizatora nvarchar(50),
	@telefon_organizatora nvarchar(50)
                    
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    UPDATE dbo.Organizator
    SET 
		nazwa_organizatora = @nazwa_organizatora, miasto_organizatora = @miasto_organizatora,
		e_mail_organizatora = @e_mail_organizatora,telefon_organizatora = @telefon_organizatora
	WHERE id_organizatora=@id_organizatora
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[AddOrg]
(
  
	@nazwa_organizatora nvarchar(50),
	@miasto_organizatora nvarchar(50),
	@telefon_organizatora nvarchar(50),
	@e_mail_organizatora nvarchar(50),
	@id_organizatora int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    INSERT dbo.Organizator(nazwa_organizatora, miasto_organizatora, e_mail_organizatora, telefon_organizatora)
    VALUES  (@nazwa_organizatora,@miasto_organizatora, @e_mail_organizatora, @telefon_organizatora);
    COMMIT TRANSACTION
    SET @id_organizatora = SCOPE_IDENTITY();
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
