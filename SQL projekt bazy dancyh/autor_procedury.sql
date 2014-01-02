USE muzeum 
GO

CREATE PROCEDURE [dbo].[GetAuthors]
AS
BEGIN
    SELECT id_autora, nazwa_autora, data_urodzenia, data_smierci, opis_autora
    From dbo.Autor 
END
GO

CREATE PROCEDURE [dbo].[UpdateAuthor]
(
  @id_autora int,
  @nazwa_autora nvarchar(50),
  @data_urodzenia nvarchar(150),
  @data_smierci nvarchar(50),
  @opis_autora nvarchar(50)
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    UPDATE dbo.Autor
    SET 
		nazwa_autora = @nazwa_autora, data_urodzenia = @data_urodzenia, data_smierci = @data_smierci,
		opis_autora = @opis_autora
	WHERE id_autora=@id_autora
	COMMIT TRANSACTION    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
		ROLLBACK
END CATCH
GO

CREATE PROCEDURE [dbo].[AddAuthor]
(
  @nazwa_autora nvarchar(50),
  @data_urodzenia nvarchar(150),
  @data_smierci nvarchar(50),
  @opis_autora nvarchar(50),
  @id_autora int OUTPUT
)
AS
BEGIN TRY
	BEGIN TRANSACTION
    INSERT dbo.Autor  (nazwa_autora,data_urodzenia,data_smierci, opis_autora)
    VALUES (@nazwa_autora, @data_urodzenia, @data_smierci, @opis_autora);
    COMMIT TRANSACTION
    SET @id_autora = SCOPE_IDENTITY();
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