USE muzeum_v4
GO

CREATE PROCEDURE [dbo].[GetLocations]
AS
BEGIN
    SELECT id_lokalizacji, nazwa_lokalizacji, opis_lokalizacji
    From dbo.Lokalizacja 
END
GO

CREATE PROCEDURE [dbo].[UpdateLocation]
(
	  @id_lokalizacji int,
	  @nazwa_lokalizacji nvarchar(50),
	  @opis_lokalizacji nvarchar(150)
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    UPDATE dbo.Lokalizacja
    SET 
	nazwa_lokalizacji=@nazwa_lokalizacji, opis_lokalizacji=@opis_lokalizacji
	WHERE id_lokalizacji=@id_lokalizacji
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO


CREATE PROCEDURE [dbo].[AddLocation]
(
	@nazwa_lokalizacji nvarchar(50),
	@opis_lokalizacji nvarchar(150),
	@id_lokalizacji int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    INSERT dbo.Lokalizacja(nazwa_lokalizacji, opis_lokalizacji)
    VALUES (@nazwa_lokalizacji,@opis_lokalizacji);
    COMMIT TRANSACTION
    SET @id_lokalizacji = SCOPE_IDENTITY();
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