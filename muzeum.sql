
/* dodanie nowej bazy */
IF DB_ID('muzeum') IS NOT NULL 
    DROP DATABASE muzeum;
GO

--DROP DATABASE ksiegarnia
CREATE DATABASE muzeum
GO

/* przejscie do bazy o zadanej nazwie */
USE muzeum
GO

/* definicja nowego typu danych */
sp_addtype string, 'varchar(50)'
GO

/*Dodanie tabeli sala */
CREATE TABLE sala(
	id_sali int IDENTITY NOT NULL PRIMARY KEY,
	nazwa string NOT NULL,
	pojemnosc string NULL
)
GO

/*dodanie tabeli wystawa*/
CREATE TABLE wystawa(
	id_wystawy int IDENTITY NOT NULL PRIMARY KEY,
	nazwa string NOT NULL,

)
GO

/*dodanie tabeli eksponat */
CREATE TABLE eksponat(
	id_eksponatu int IDENTITY NOT NULL PRIMARY KEY,
	nazwa string NOT NULL,	
	autor string NOT NULL,
	rok_prod int NOT NULL,
	cena float,
	id_sali int FOREIGN KEY REFERENCES sala(id_sali),
	id_wys int FOREIGN KEY REFERENCES wystawa(id_wystawy),
)
GO
	
	

/*Uzupełnianie tabel */
INSERT INTO sala VALUES ('Duża sala',20)
INSERT INTO sala VALUES ('Mała sala',10)
INSERT INTO sala VALUES ('Zaplecze',200)
GO

SELECT * FROM sala
GO

INSERT INTO wystawa VALUES ('II Wojna Światowa')
INSERT INTO wystawa VALUES ('Powstanie warszawskie')
GO

SELECT * FROM wystawa
GO

INSERT INTO eksponat VALUES ('AK-47','Dmitri Jukov',1988,2000,2,1)
INSERT INTO eksponat VALUES ('Granat','Jan Kowalski',1928,2000,1,1)
INSERT INTO eksponat VALUES ('Ołówek','Putin',2001,2000,2,2)
INSERT INTO eksponat VALUES ('długopis','Obama',1988,2000,2,2)
INSERT INTO eksponat VALUES ('garnek','Anonim',1988,2000,3,1)
GO

SELECT * FROM eksponat
GO