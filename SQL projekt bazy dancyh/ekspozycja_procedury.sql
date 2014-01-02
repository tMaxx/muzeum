USE muzeum 
GO

CREATE PROCEDURE [dbo].[GetExpositions]
AS
BEGIN
    SELECT 
	E.id_ekspozycji, 
	E.nazwa_ekspozycji, 
	O.nazwa_organizatora, 
	L.nazwa_lokalizacji, 
	E.opis_ekspozycji, 
	(SELECT COUNT(*) FROM Sprzedaz S WHERE E.id_ekspozycji= id_ekspozycji) as sprzedane_bilety,
	
	(SELECT ISNULL(SUM(B.cena_biletu),0) FROM Sprzedaz S 
	Join dbo.Bilet As B ON S.id_biletu = B.id_biletu
	Where S.id_ekspozycji = E.id_ekspozycji) AS zysk
	
	From dbo.Ekspozycja AS E
	JOIN dbo.Organizator AS O
		ON E.id_organizatora = O.id_organizatora
	JOIN dbo.Lokalizacja AS L
		ON E.id_lokalizacji = L.id_lokalizacji
END
GO

CREATE PROCEDURE [dbo].[UpdateExposition]
(
  @id_ekspozycji int,
  @nazwa_ekspozycji nvarchar(50),
  @nazwa_organizatora nvarchar(150),
  @nazwa_lokalizacji nvarchar(50),
  @opis_ekspozycji nvarchar(50)
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Organizator WHERE nazwa_organizatora=@nazwa_organizatora)
    BEGIN
        INSERT INTO dbo.Organizator(nazwa_organizatora,miasto_organizatora,e_mail_organizatora,telefon_organizatora) VALUES (@nazwa_organizatora,'bd','bd','bd')
    END
	IF NOT EXISTS (SELECT 1 FROM Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    BEGIN
        INSERT INTO dbo.Lokalizacja(nazwa_lokalizacji,opis_lokalizacji) VALUES (@nazwa_lokalizacji,'bd')
    END
    UPDATE dbo.Ekspozycja
    SET 
		nazwa_ekspozycji=@nazwa_ekspozycji, opis_ekspozycji=@opis_ekspozycji,
		id_organizatora=(SELECT id_organizatora FROM dbo.Organizator WHERE nazwa_organizatora=@nazwa_organizatora),
		id_lokalizacji=(SELECT id_lokalizacji FROM dbo.Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
	WHERE id_ekspozycji=@id_ekspozycji
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[DeleteExposition]
(
  @id_ekspozycji int
 )
AS
BEGIN TRY
    BEGIN TRANSACTION
			DELETE FROM dbo.Ekspozycja WHERE id_ekspozycji = @id_ekspozycji;
	COMMIT	    
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

Create PROCEDURE [dbo].[AddExposition]
(
  @nazwa_ekspozycji nvarchar(50),
  @nazwa_organizatora nvarchar(150),
  @nazwa_lokalizacji nvarchar(50),
  @opis_ekspozycji nvarchar(50),
  @id_ekspozycji int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Organizator WHERE nazwa_organizatora=@nazwa_organizatora)
    BEGIN
        INSERT INTO dbo.Organizator(nazwa_organizatora,miasto_organizatora,e_mail_organizatora,telefon_organizatora) VALUES (@nazwa_organizatora,'bd','bd','bd')
    END
	IF NOT EXISTS (SELECT 1 FROM Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    BEGIN
		INSERT INTO dbo.Lokalizacja(nazwa_lokalizacji,opis_lokalizacji) VALUES (@nazwa_lokalizacji,'bd')
    END
    DECLARE @OrgID int
	DECLARE @LocID int
    SET @OrgID =(SELECT id_organizatora FROM dbo.Organizator WHERE nazwa_organizatora=@nazwa_organizatora)
	SET	@LocID = (SELECT id_lokalizacji FROM dbo.Lokalizacja WHERE nazwa_lokalizacji=@nazwa_lokalizacji)
    INSERT dbo.Ekspozycja(nazwa_ekspozycji,opis_ekspozycji,id_organizatora, id_lokalizacji)
    VALUES (@nazwa_ekspozycji, @opis_ekspozycji, @OrgID, @LocID);
    COMMIT TRANSACTION
    SET @id_ekspozycji = SCOPE_IDENTITY();
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
