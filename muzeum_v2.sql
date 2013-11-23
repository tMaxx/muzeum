Create database muzeum_v2
go

USE muzeum_v2
GO

sp_addtype string, 'varchar(50)'
GO
/*Dodanie tabeli sala */
CREATE TABLE Lokalizacja(
	id_lokalizacji int IDENTITY NOT NULL PRIMARY KEY,
	nazwa string NOT NULL,

)
GO
CREATE TABLE Sala(
	id_sali int IDENTITY NOT NULL PRIMARY KEY,
	id_lokalizacji int NOT NULL REFERENCES Lokalizacja(id_lokalizacji),
	nazwa string NOT NULL,
	opis string NOT NULL,
)
GO

CREATE TABLE Bilety(
	id_biletu int IDENTITY NOT NULL PRIMARY KEY,
	nazwa string NOT NULL,

)
GO

CREATE TABLE Sprzedaz(
	id_sprzedazy int IDENTITY NOT NULL PRIMARY KEY,
	id_biletow int NOT NULL FOREIGN KEY REFERENCES Bilety(id_biletu),
	nazwa string NOT NULL,

)
GO
CREATE TABLE Organizator(
	id_organizatora int IDENTITY NOT NULL PRIMARY KEY,
	nazwa  VARCHAR(45) NOT NULL,
   miasto  VARCHAR(45) NOT NULL,
   e_mail  VARCHAR(45) NOT NULL,
   telefon  VARCHAR(45) NOT NULL,
)
GO
CREATE TABLE Autor (
   id_autora  INT IDENTITY NOT NULL PRIMARY KEY,
   imie  VARCHAR(45) NOT NULL,
   nazwisko  VARCHAR(45) NULL,
   opis  VARCHAR(45) NULL,
)
GO
CREATE TABLE Wlasciciel(
  id_wlasciciela INT IDENTITY NOT NULL PRIMARY KEY,
   nazwa  VARCHAR(45) NOT NULL,
   miasto  VARCHAR(45) NOT NULL,
   kraj  VARCHAR(45) NULL,
   e_mail  VARCHAR(45) NULL,
  telefon  VARCHAR(45) NULL,
)
GO
CREATE TABLE Ekspozycja(
	id_ekspozycji int IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
	id_sprzedazy  INT NOT NULL FOREIGN KEY REFERENCES Sprzedaz(id_sprzedazy),
	id_organizatora INT NOT NULL FOREIGN KEY REFERENCES Organizator(id_organizatora),
	id_lokalizacji INT NOT NULL FOREIGN KEY REFERENCES Lokalizacja(id_lokalizacji),
	opis  VARCHAR(45) NOT NULL,
)
GO
CREATE TABLE Eksponat  (
id_eksponatu  INT IDENTITY NOT NULL   PRIMARY KEY,
nazwa  VARCHAR(45) NOT NULL,
opis  VARCHAR(100) NOT NULL,
id_wlasciciela  INT NOT NULL FOREIGN KEY REFERENCES Wlasciciel(id_wlasciciela), 
id_autora  INT NOT NULL FOREIGN KEY REFERENCES Autor(id_autora),
)
GO
CREATE TABLE Prezentacje  (
   id_prezentacje  INT IDENTITY NOT NULL PRIMARY KEY,
   data_rozpoczecia  DATE NOT NULL,
   data_zakonczenia  DATE NOT NULL,
   id_eksponatu  INT NOT NULL FOREIGN KEY REFERENCES Eksponat(id_eksponatu),
   id_ekspozycji  INT NOT NULL FOREIGN KEY REFERENCES Ekspozycja(id_ekspozycji),
   id_sali  INT NOT NULL FOREIGN KEY REFERENCES Sala(id_sali),
)
GO
CREATE TABLE Przypisanie  (
  id_przypisania  INT IDENTITY NOT NULL PRIMARY KEY,
	id_eksponatu  INT NOT NULL FOREIGN KEY REFERENCES Eksponat(id_eksponatu),
   id_ekspozycji  INT NOT NULL FOREIGN KEY REFERENCES Ekspozycja(id_ekspozycji),
)
GO