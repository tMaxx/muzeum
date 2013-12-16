create database muzeum_v4
GO
USE muzeum_v4
GO

sp_addtype string, 'varchar(50)'
GO
/*Dodanie tabeli sala */
CREATE TABLE Lokalizacja(
	id_lokalizacji int IDENTITY NOT NULL PRIMARY KEY,
	nazwa_lokalizacji string NOT NULL,
	opis_lokalizacji  VARCHAR(150) NULL,
)
GO
CREATE TABLE Sala(
	id_sali int IDENTITY NOT NULL PRIMARY KEY,
	id_lokalizacji int NOT NULL REFERENCES Lokalizacja(id_lokalizacji),
	nazwa_sali string NOT NULL,
	opis_sali VARCHAR(150) NOT NULL,
)
GO
CREATE TABLE Bilet(
	id_biletu int IDENTITY NOT NULL PRIMARY KEY,
	cena_biletu smallmoney NOT NULL,
	nazwa_biletu string NOT NULL,
)
GO
CREATE TABLE Organizator(
	id_organizatora int IDENTITY NOT NULL PRIMARY KEY,
	nazwa_organizatora  string NOT NULL,
	miasto_organizatora  string NOT NULL,
	e_mail_organizatora  string NOT NULL,
	telefon_organizatora string NOT NULL,
)
GO
CREATE TABLE Ekspozycja(
	id_ekspozycji int IDENTITY NOT NULL PRIMARY KEY,
	nazwa_ekspozycji string NOT NULL,
	id_organizatora INT NOT NULL FOREIGN KEY REFERENCES Organizator(id_organizatora),
	id_lokalizacji INT NOT NULL FOREIGN KEY REFERENCES Lokalizacja(id_lokalizacji),
	opis_ekspozycji VARCHAR(150) NOT NULL,
)
GO
CREATE TABLE Sprzedaz(
	id_sprzedazy int IDENTITY NOT NULL PRIMARY KEY,
	id_ekspozycji int NOT NULL FOREIGN KEY REFERENCES Ekspozycja(id_ekspozycji),
	id_biletu int NOT NULL FOREIGN KEY REFERENCES Bilet(id_biletu),
)
GO
CREATE TABLE Autor (
   id_autora  INT IDENTITY NOT NULL PRIMARY KEY,
   nazwa_autora string NULL,
   data_urodzenia DATE  NULL,
   data_smierci DATE  NULL,
   opis_autora  VARCHAR(150) NULL,
)
GO
CREATE TABLE Wlasciciel(
	id_wlasciciela INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa_wlasciciela  string  NULL,
	miasto_wlasciciela  string  NULL,
	kraj_wlasciciela  string NULL,
	email_wlasciciela  string NULL,
	telefon_wlasciciela  string NULL,
)
GO
CREATE TABLE Eksponat  (
	id_eksponatu  INT IDENTITY NOT NULL   PRIMARY KEY,
	nazwa_eksponatu string NOT NULL,
	opis_eksponatu  VARCHAR(150) NOT NULL,
	id_wlasciciela  INT NOT NULL FOREIGN KEY REFERENCES Wlasciciel(id_wlasciciela), 
	id_autora  INT NOT NULL FOREIGN KEY REFERENCES Autor(id_autora),
)
GO
CREATE TABLE Prezentacje  (
   id_prezentacji  INT IDENTITY NOT NULL PRIMARY KEY,
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
