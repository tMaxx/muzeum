USE muzeum 
GO

CREATE PROCEDURE [dbo].[GetTickets]
AS
BEGIN
    SELECT id_biletu, cena_biletu, nazwa_biletu
    From dbo.Bilet 
END
GO

Create PROCEDURE [dbo].[AddSale]
(
	@nazwa_biletu string,
	@cena_biletu string,
	@nazwa_ekspozycji string,
	@id_sprzedazy int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    IF NOT EXISTS (SELECT 1 FROM Bilet WHERE nazwa_biletu=@nazwa_biletu)
    BEGIN
        INSERT INTO dbo.Bilet(cena_biletu,nazwa_biletu) VALUES (@cena_biletu,@nazwa_biletu)
    END
    DECLARE @TicID int
	DECLARE @ExpID int
    SET @TicID =(SELECT id_biletu FROM dbo.Bilet WHERE nazwa_biletu=@nazwa_biletu)
	SET @ExpID =(SELECT id_ekspozycji FROM dbo.Ekspozycja WHERE nazwa_ekspozycji=@nazwa_ekspozycji)
    INSERT dbo.Sprzedaz(id_biletu,id_ekspozycji)
    VALUES (@TicID, @ExpID);
    COMMIT TRANSACTION
    SET @id_sprzedazy = SCOPE_IDENTITY();
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