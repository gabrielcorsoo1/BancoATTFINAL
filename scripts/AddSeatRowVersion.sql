IF COL_LENGTH('dbo.Seats','RowVersion') IS NULL
BEGIN
    ALTER TABLE dbo.Seats ADD RowVersion rowversion NOT NULL;
END