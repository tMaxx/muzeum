CREATE DATABASE IF NOT EXISTS muzeum_v2;
GO

USE muzeum_v2;
GO

CREATE TABLE IF NOT EXISTS Lokalizacja (
	id_lokalizacji INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Sala (
	id_sali INT IDENTITY NOT NULL PRIMARY KEY,
	id_lokalizacji INT NOT NULL REFERENCES Lokalizacja(id_lokalizacji),
	nazwa VARCHAR(45) NOT NULL,
	opis VARCHAR(100) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Bilety (
	id_biletu INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Sprzedaz (
	id_sprzedazy INT IDENTITY NOT NULL PRIMARY KEY,
	id_biletow INT NOT NULL FOREIGN KEY REFERENCES Bilety(id_biletu),
	nazwa VARCHAR(45) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Organizator (
	id_organizatora INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
	miasto VARCHAR(45) NOT NULL,
	e_mail VARCHAR(45) NOT NULL,
	telefon VARCHAR(45) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Autor (
	id_autora INT IDENTITY NOT NULL PRIMARY KEY,
	imie VARCHAR(45) NOT NULL,
	nazwisko VARCHAR(45) NULL,
	opis VARCHAR(45) NULL,
);

CREATE TABLE IF NOT EXISTS Wlasciciel (
	id_wlasciciela INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
	miasto VARCHAR(45) NOT NULL,
	kraj VARCHAR(45) NULL,
	e_mail VARCHAR(45) NULL,
	telefon VARCHAR(45) NULL,
);

CREATE TABLE IF NOT EXISTS Ekspozycja (
	id_ekspozycji INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
	id_sprzedazy INT NOT NULL FOREIGN KEY REFERENCES Sprzedaz(id_sprzedazy),
	id_organizatora INT NOT NULL FOREIGN KEY REFERENCES Organizator(id_organizatora),
	id_lokalizacji INT NOT NULL FOREIGN KEY REFERENCES Lokalizacja(id_lokalizacji),
	opis VARCHAR(45) NOT NULL,
);

CREATE TABLE IF NOT EXISTS Eksponat (
	id_eksponatu INT IDENTITY NOT NULL PRIMARY KEY,
	nazwa VARCHAR(45) NOT NULL,
	opis VARCHAR(100) NOT NULL,
	id_wlasciciela INT NOT NULL FOREIGN KEY REFERENCES Wlasciciel(id_wlasciciela), 
	id_autora INT NOT NULL FOREIGN KEY REFERENCES Autor(id_autora),
);

CREATE TABLE IF NOT EXISTS Prezentacje (
	id_prezentacje INT IDENTITY NOT NULL PRIMARY KEY,
	data_rozpoczecia DATE NOT NULL,
	data_zakonczenia DATE NOT NULL,
	id_eksponatu INT NOT NULL FOREIGN KEY REFERENCES Eksponat(id_eksponatu),
	id_ekspozycji INT NOT NULL FOREIGN KEY REFERENCES Ekspozycja(id_ekspozycji),
	id_sali INT NOT NULL FOREIGN KEY REFERENCES Sala(id_sali),
);

CREATE TABLE IF NOT EXISTS Przypisanie (
	id_przypisania INT IDENTITY NOT NULL PRIMARY KEY,
	id_eksponatu INT NOT NULL FOREIGN KEY REFERENCES Eksponat(id_eksponatu),
	id_ekspozycji INT NOT NULL FOREIGN KEY REFERENCES Ekspozycja(id_ekspozycji),
);
