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

DROP TABLE Debts;