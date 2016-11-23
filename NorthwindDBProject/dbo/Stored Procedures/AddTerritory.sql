CREATE PROCEDURE [dbo].[AddTerritory] 
(
	@TerritoryDescription nchar(50), 
	@RegionID int, 
	@TerritoryID nvarchar(20)
)

AS
INSERT INTO [dbo].[Territories] 
(
	[TerritoryDescription], 
	[RegionID], 
	[TerritoryID]
)

VALUES 
(
	@TerritoryDescription, 
	@RegionID,
	@TerritoryID
)
