CREATE VIEW [dbo].[CountCategories]
	AS
	SELECT COUNT(*) AS CategoryCount
	FROM [dbo].[Categories]
	