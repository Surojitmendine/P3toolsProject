----- ForYear=2020 And ForMonth=12
Select  * from tbl_P3_PrimarySaleTransDetails_Uploaded Where ForYear=2020 And ForMonth=12 Order by PK_SalesTransactionID Desc
Select  * from tbl_P3_SecondarySaleTransDetails_Uploaded Where ForYear=2020 And ForMonth=11 Order by DivisionName Desc

 --Update tbl_P3_PrimarySaleTransDetails_Uploaded Set IsProcessed=0 Where ForYear=2020 And ForMonth=12
--Update tbl_P3_SecondarySaleTransDetails_Uploaded Set IsProcessed=0 Where ForYear=2020 And ForMonth=11

-- Checkm Aggregated ---
Select ForYear, ForMonth, Count(*) from tbl_P3_AGG_SaleTransaction Where ForYear=2020 Group by ForYear, ForMonth
Select  * from tbl_P3_AGG_SaleTransaction Where ForYear=2020 And ForMonth=12 Order by PK_AGG_SaleID Desc
Select  * from tbl_P3_AGG_SaleTransaction  Order by PK_AGG_SaleID Desc
-- Delete tbl_P3_AGG_SaleTransaction Where ForYear=2020 And ForMonth=11

Select ProjectionForYear, ProjectionForMonth, Count(*) from tbl_P3_SaleProjection_Uploaded  Group by ProjectionForYear, ProjectionForMonth
Select * from tbl_P3_SaleProjection_Uploaded Where ProjectionForYear=2021 And ProjectionForMonth=1
-- Delete from tbl_P3_SaleProjection_Uploaded Where ProjectionForYear=2021 And ProjectionForMonth=1

-- SalesTeam ---
Select ForYear, ForMonth, Count(*) from tbl_P3_SaleProjection_SalesTeam Group by ForYear, ForMonth
Select  * from tbl_P3_SaleProjection_SalesTeam Where ForYear=2021 And ForMonth=1 Order by PK_SalesTeamProjectionID Desc
-- Delete tbl_P3_SaleProjection_SalesTeam Where ForYear=2021 And ForMonth=1


-- Sales Forecasting Comparison ---
Select ForecastingForYear, ForecastingForMonth, Count(*) from tbl_P3_SaleForecastingComparison Group by ForecastingForYear, ForecastingForMonth
Select  * from tbl_P3_SaleForecastingComparison Where ForecastingForYear=2021 And ForecastingForMonth=1 Order by PK_SaleComparisonID Desc

----Delete tbl_P3_SaleForecastingComparison Where ForecastingForYear=2021 And ForecastingForMonth=1 
	Select dbo.udf_GetPreviousMonthDate(2021, 1, -2)

Select   Count(*) from tbl_P3_SaleProjection_Uploaded where ForecastingType='Marketing'  And (ProjectionForYear=Year('2020-11-30') And ProjectionForMonth=Month('2020-11-30')) 