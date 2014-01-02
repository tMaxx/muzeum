USE muzeum
GO

CREATE PROCEDURE [dbo].[GetExhibits]
AS
BEGIN
    SELECT E.id_eksponatu, E.nazwa_eksponatu,  E.opis_eksponatu,  A.nazwa_autora, O.nazwa_wlasciciela
    From dbo.Eksponat AS E
	JOIN dbo.Wlasciciel AS O
		ON E.id_wlasciciela = O.id_wlasciciela
	JOIN dbo.Autor AS A
		ON E.id_autora = A.id_autora
END
GO

Create PROCEDURE [dbo].[UpdateExhibit]
(
  @id_eksponatu int,
  @nazwa_eksponatu nvarchar(50),
  @opis_eksponatu nvarchar(150),
  @autor_eksponatu nvarchar(50),
  @wlasciciel_eksponatu nvarchar(50)
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Autor WHERE nazwa_autora=@autor_eksponatu)
    BEGIN
        INSERT INTO dbo.Autor(nazwa_autora,data_urodzenia,data_smierci,opis_autora) VALUES (@autor_eksponatu,'1000-01-01','1000-01-01','bd')
    END
	IF NOT EXISTS (SELECT 1 FROM Wlasciciel WHERE nazwa_wlasciciela=@wlasciciel_eksponatu)
    BEGIN
        INSERT INTO dbo.Wlasciciel(nazwa_wlasciciela,miasto_wlasciciela,kraj_wlasciciela,email_wlasciciela,telefon_wlasciciela) VALUES (@wlasciciel_eksponatu,'bd','bd','bd','bd')
    END
    UPDATE dbo.Eksponat
    SET 
		nazwa_eksponatu=@nazwa_eksponatu, opis_eksponatu=@opis_eksponatu,
		id_autora=(SELECT id_autora FROM dbo.Autor WHERE nazwa_autora=@autor_eksponatu),
		id_wlasciciela=(SELECT id_wlasciciela FROM dbo.Wlasciciel WHERE nazwa_wlasciciela=@wlasciciel_eksponatu)
	WHERE id_eksponatu=@id_eksponatu
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[DeleteExhibit]
(
  @id_eksponatu int
 )
AS
BEGIN TRY
    BEGIN TRANSACTION
			DELETE FROM dbo.Eksponat WHERE id_eksponatu = @id_eksponatu;
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

Create PROCEDURE [dbo].[AddExhibit]
(
  @nazwa_eksponatu nvarchar(50),
  @opis_eksponatu nvarchar(150),
  @autor_eksponatu nvarchar(50),
  @wlasciciel_eksponatu nvarchar(50),
  @id_eksponatu int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Autor WHERE nazwa_autora=@autor_eksponatu)
    BEGIN
                INSERT INTO dbo.Autor(nazwa_autora,data_urodzenia,data_smierci,opis_autora) VALUES (@autor_eksponatu,'1000-01-01','1000-01-01','bd')
    END
	IF NOT EXISTS (SELECT 1 FROM Wlasciciel WHERE nazwa_wlasciciela=@wlasciciel_eksponatu)
    BEGIN
        INSERT INTO dbo.Wlasciciel(nazwa_wlasciciela,miasto_wlasciciela,kraj_wlasciciela,email_wlasciciela,telefon_wlasciciela) VALUES (@wlasciciel_eksponatu,'bd','bd','bd','bd')
    END
    DECLARE @AuthorID int
	DECLARE @OwnerID int
    SET @AuthorID = (SELECT id_autora FROM dbo.Autor WHERE nazwa_autora=@autor_eksponatu)
	SET	@OwnerID = (SELECT id_wlasciciela FROM dbo.Wlasciciel WHERE nazwa_wlasciciela=@wlasciciel_eksponatu)
    INSERT dbo.Eksponat(nazwa_eksponatu,opis_eksponatu,id_autora, id_wlasciciela)
    VALUES (@nazwa_eksponatu, @opis_eksponatu, @AuthorID, @OwnerID);
    COMMIT TRANSACTION
    SET @id_eksponatu = SCOPE_IDENTITY();
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

Create PROCEDURE [dbo].[AddPresentation]
(
  @data_rozpoczecia DATE,
  @data_zakonczenia DATE,
  @nazwa_eksponatu string,
  @nazwa_ekspozycji string,
  @nazwa_sali string,
  @id_prezentacji int OUTPUT
) 
AS
BEGIN TRY
	BEGIN TRANSACTION
	DECLARE @ExhibitID int
	DECLARE @ExpositionID int
	DECLARE @HallID int
	SET @ExhibitID = (SELECT id_eksponatu FROM dbo.Eksponat WHERE nazwa_eksponatu=@nazwa_eksponatu)
	SET @ExpositionID = (SELECT id_ekspozycji FROM dbo.Ekspozycja WHERE nazwa_ekspozycji=@nazwa_ekspozycji)
	SET @HallID = (SELECT id_sali FROM dbo.Sala WHERE nazwa_sali=@nazwa_sali)
		
		IF NOT EXISTS (SELECT 1 FROM Przypisanie WHERE id_eksponatu = @ExhibitID AND id_ekspozycji = @ExpositionID)
		BEGIN
                INSERT INTO dbo.Przypisanie(id_eksponatu, id_ekspozycji) VALUES (@ExhibitID,@ExpositionID)
		END
    
	INSERT dbo.Prezentacje(data_rozpoczecia,data_zakonczenia,id_eksponatu,id_ekspozycji,id_sali)
    VALUES (@data_rozpoczecia,@data_zakonczenia,@ExhibitID,@ExpositionID,@HallID);
    COMMIT TRANSACTION
    SET @id_prezentacji = SCOPE_IDENTITY();
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

