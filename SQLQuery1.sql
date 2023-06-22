/****** Скрипт для команды SelectTopNRows из среды SSMS  ******/
SELECT TOP (1000) [IdDebt]
      ,[Summ]
      ,[Date]
      ,[userId]
      ,[Status]
      ,[DateOfEnd]
  FROM [DSR_Practice_Debts].[dbo].[Debts]



SELECT * FROM Debts WHERE CAST(Debts.DateOfEnd AS datetime2) < CAST(GETDATE() AS smalldatetime)

UPDATE Debts SET Status = 'Открыт' WHERE IdDebt = 1;
SELECT * FROM Debts;

SELECT * FROM Users;

TRUNCATE TABLE Users

DROP TABLE Debts;
DROP TABLE Users;

SET IDENTITY_INSERT Users ON;
INSERT INTO Users (Id, Email, Password) VALUES (5, '5L^??$^pk8Wt}R@', '5L^??$^pk8Wt}R@');

DELETE Users WHERE Id = 2
INSERT INTO Debts VALUES (345, '2023-01-01', '2023-02-02', 'Открыт', 2, '')

DROP TABLE Debts

SET IDENTITY_INSERT Users ON;
INSERT INTO Users VALUES (1, '1', '1')

ALTER TABLE Debts DROP COLUMN userId;
TRUNCATE TABLE Users

DROP TABLE Users

SELECT * FROM Users
SELECT * FROM Debts

CREATE TABLE Debts (
                IdDebt INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                Summ INT NOT NULL,
                Date datetime2 NOT NULL,
                DateOfEnd datetime2 NOT NULL,
                Status nvarchar(max) NOT NULL,
                userId int FOREIGN KEY REFERENCES Users(Id),
                RealDateEnd datetime2 NULL)



ALTER TABLE Debts DROP COLUMN userId;

SELECT 1 FROM information_schema.COLUMNS
WHERE TABLE_NAME = 'Debts'
AND COLUMN_NAME = 'userId'

CREATE TABLE Debts (
	IdDebt INT NOT NULL PRIMARY KEY IDENTITY(1, 1)
)


CREATE PROCEDURE IDENTITY_Table_OFF
@Name nvarchar(13)
                                as
                                BEGIN
                                    SET IDENTITY_INSERT @Name ON
                                END


CREATE PROCEDURE CreateUsers
AS
BEGIN
	CREATE TABLE Users (
                Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                Email nvarchar(max) NOT NULL,
                Password nvarchar(max) NOT NULL)
END


CREATE PROCEDURE CreateDebts
AS
BEGIN
	CREATE TABLE Debts (
                IdDebt INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
                Summ INT NOT NULL,
                Date datetime2 NOT NULL,
                DateOfEnd datetime2 NOT NULL,
                Status nvarchar(max) NOT NULL,
                userId INT FOREIGN KEY REFERENCES Users(Id),
                RealDateEnd datetime2 NULL)
END