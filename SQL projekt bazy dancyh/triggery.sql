USE muzeum
GO

CREATE TRIGGER DeleteExhibitTrigger ON Eksponat
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @ID int 
	SET @ID = (Select id_eksponatu from DELETED)
	
	DELETE Prezentacje
    FROM Prezentacje P
    Where P.id_eksponatu = @ID

	DELETE Przypisanie
    FROM Przypisanie P
    Where P.id_eksponatu = @ID

    DELETE Eksponat
    FROM DELETED D
    INNER JOIN Eksponat T ON T.id_eksponatu = @ID
END
GO

ALTER TRIGGER DeletePresentationTrigger ON Prezentacje
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @ID int 
	DECLARE @IDExp int 
	DECLARE @IDExh int 

	SET @ID = (Select id_prezentacji from DELETED)
	SET @IDExp = (Select id_ekspozycji from DELETED)
	SET @IDExh = (Select id_eksponatu from DELETED)

	DECLARE @NUMBER_EXHIBITS int 
	SET @NUMBER_EXHIBITS = (Select COUNT(id_eksponatu) from Prezentacje P
					where P.id_eksponatu = @IDExh)
	
	DECLARE @NUMBER_EXPOSITIONS int 
	SET @NUMBER_EXPOSITIONS = (Select COUNT(id_ekspozycji) from Prezentacje P
					where P.id_ekspozycji = @IDExp)

	IF(@NUMBER_EXHIBITS <= 1 OR @NUMBER_EXPOSITIONS <= 1)
	BEGIN
		DELETE Przypisanie
		FROM Przypisanie P
		Where P.id_ekspozycji = @IDExp and P.id_eksponatu = @IDExh
	END

    DELETE Prezentacje
    FROM DELETED D
    INNER JOIN Prezentacje T ON T.id_prezentacji = @ID
END
GO

CREATE TRIGGER DeleteExpositionTrigger ON Ekspozycja
INSTEAD OF DELETE
AS
BEGIN
	DECLARE @ID int 
	SET @ID = (Select id_ekspozycji from DELETED)
	
	DELETE Prezentacje
    FROM Prezentacje P
    Where P.id_ekspozycji = @ID

	DELETE Przypisanie
    FROM Przypisanie P
    Where P.id_ekspozycji = @ID

	DELETE Sprzedaz
    FROM Sprzedaz S
    Where S.id_ekspozycji = @ID

    DELETE Ekspozycja
    FROM DELETED D
    INNER JOIN Ekspozycja T ON T.id_ekspozycji = @ID
END
GO

CREATE TRIGGER DuplicateExhibitTrigger ON Eksponat
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_eksponatu FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Eksponat E
			where UPPER(E.nazwa_eksponatu) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateExpositionTrigger ON Ekspozycja
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_ekspozycji FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Ekspozycja E
			where UPPER(E.nazwa_ekspozycji) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateAuthorTrigger ON Autor
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_autora FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Autor A
			where UPPER(A.nazwa_autora) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateOwnerTrigger ON Wlasciciel
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_wlasciciela FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Wlasciciel w
			where UPPER(w.nazwa_wlasciciela) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateLocationTrigger ON Lokalizacja
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_lokalizacji FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Lokalizacja L
			where UPPER(L.nazwa_lokalizacji) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateHallTrigger ON Sala
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_sali FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Sala S
			where UPPER(S.nazwa_sali) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO

CREATE TRIGGER DuplicateOrgTrigger ON Organizator
AFTER INSERT
AS
BEGIN
	DECLARE @NAME string 
	SET		@NAME = UPPER((SELECT nazwa_organizatora FROM INSERTED))
	DECLARE	@NUMBER int 
	SET		@NUMBER = (Select COUNT(*) from Organizator O
			where UPPER(O.nazwa_organizatora) = @NAME)
	IF(1 < @NUMBER)
	BEGIN
	RAISERROR ('Duplikaty', 16, 1)
	END
END 
GO