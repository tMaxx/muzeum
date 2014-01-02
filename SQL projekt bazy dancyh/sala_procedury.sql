USE muzeum 
GO

CREATE PROCEDURE [dbo].[GetHalls]
AS
BEGIN
    SELECT S.id_sali, L.nazwa_lokalizacji, S.nazwa_sali, S.opis_sali
    From dbo.Sala As S
	Join dbo.Lokalizacja as L 
		On S.id_lokalizacji = L.id_lokalizacji
END
GO

CREATE PROCEDURE [dbo].[UpdateHall]
(
  @id_sali int,
  @nazwa_lokalizacji nvarchar(50),
  @nazwa_sali nvarchar(50),
  @opis_sali nvarchar(150)
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    BEGIN
        INSERT INTO dbo.Lokalizacja(nazwa_lokalizacji,opis_lokalizacji) VALUES (@nazwa_lokalizacji,'bd')
    END
    UPDATE dbo.Sala
    SET 
		nazwa_sali=@nazwa_sali, opis_sali=@opis_sali,
		id_lokalizacji=(SELECT id_lokalizacji FROM dbo.Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
	WHERE id_sali=@id_sali
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[AddHall]
(
  @nazwa_lokalizacji nvarchar(50),
  @nazwa_sali nvarchar(50),
  @opis_sali nvarchar(150),
  @id_sali int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
	 IF NOT EXISTS (SELECT 1 FROM Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    BEGIN
        INSERT INTO dbo.Lokalizacja(nazwa_lokalizacji,opis_lokalizacji) VALUES (@nazwa_lokalizacji,'bd')
    END
	DECLARE @LocID int
	SET @LocID = (SELECT id_lokalizacji FROM dbo.Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    INSERT dbo.Sala(id_lokalizacji,nazwa_sali,opis_sali)
    VALUES (@LocID, @nazwa_sali, @opis_sali);
    COMMIT TRANSACTION
    SET @id_sali = SCOPE_IDENTITY();
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