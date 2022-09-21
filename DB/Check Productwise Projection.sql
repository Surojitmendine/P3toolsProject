-- Last 3 Month Primary Sale --
Select ForYear,ForMonth, DivisionName,   DepotName, ProductName, PackUnit,  sum(FreeSampleQTY+ SalesQTY) as [PrimarySale] From tbl_P3_PrimarySaleTransDetails_Uploaded
where  DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200'
and ForYear=2020 and ForMonth In (8,7,6) Group by  ForYear,DivisionName , DepotName , ProductName, PackUnit, ForMonth
-- Current Month Last Year  Primary Sale --
Select ForYear, ForMonth, DivisionName,   DepotName,ProductName, PackUnit, sum(FreeSampleQTY+ SalesQTY) as [PrimarySale] From tbl_P3_PrimarySaleTransDetails_Uploaded
where  DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200'
and ForYear=2019 and ForMonth In (9) Group by  DivisionName , DepotName ,ProductName, PackUnit, ForMonth, ForYear

-- Prev to Prev Seconday Sales Stock --
Select ForYear, ForMonth, DivisionName,  DepotName, ProductName, PackUnit, sum(FreeSampleQTY+SalesQTY) as [SecondaySales], sum(ClosingStockQTY) as [ClosingStock]  From tbl_P3_SecondarySaleTransDetails_Uploaded
where  DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200' and ForYear=2020 and ForMonth=7
Group by  ForYear, ForMonth, DivisionName,  DepotName, ProductName, PackUnit


-- Average  --
Select ForecastingType, ForYear, ForMonth, DivisionName,  DepotName, Sum(TotalSalesQTY + TotalFreeSampleQTY) as [TotalSalesQTY]  From tbl_P3_AGG_SaleTransaction 
where DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200' and ForYear=2020 and ForMonth=8
Group by ForecastingType, ForYear, ForMonth, DivisionName,  DepotName 

--- Sales Team Projection ---
Select DivisionName, DepotName, ProductName, PackUnit, PrimaryTotalSalesQTY, TotalClosingStockQTY, ProjectedTotalSalesQTY 
From tbl_P3_SaleProjection_SalesTeam where DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200' and ForYear=2020 and ForMonth=9

---- OUTPUT Table -----
Select ForecastingType, ProjectionForYear, ProjectionForMonth, DivisionName, DepotName, ProductName, PackUnit, Sum(ProjectedTotalSalesQTY) 
From tbl_P3_SaleProjection_Uploaded where DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200' and ProjectionForYear=2020 and ProjectionForMonth=9
Group by ForecastingType, ProjectionForYear, ProjectionForMonth, DivisionName,  DepotName, ProductName, PackUnit ,ForecastingType

Select Top(10) * from tbl_P3_AGG_SaleTransaction where DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200'And ForYear=2020 And ForMonth=8




Select ForYear,ForMonth, DivisionName, DepotName,HQ, ProductName,PackUnit,  Round((Sum(ISNULL(TotalSalesQTY,0) + ISNULL(TotalFreeSampleQTY,0))/3 *0.6),2)   as [AvgTotalSalesQTY],
Sum(TotalSalesQTY+TotalFreeSampleQTY)
 From tbl_P3_AGG_SaleTransaction 	
where ForecastingType='Logistics' and DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200'
And 
(ForYear=2020 And ForMonth=8 Or ForYear=2020 And ForMonth=7 Or ForYear=2020 And ForMonth=6)
Group By ForYear, ForMonth, DivisionName, DepotName,HQ, ProductName,PackUnit 
order by ForYear, ForMonth,DivisionName, DepotName, HQ, ProductName,PackUnit


Select ForYear,ForMonth, DivisionName, DepotName, HQ, ProductName,PackUnit,  Round((Sum(ISNULL(TotalSalesQTY,0) + ISNULL(TotalFreeSampleQTY,0)) *0.4),2) as [LastYearTotalSalesQTY],
Sum(TotalSalesQTY+TotalFreeSampleQTY)
 From tbl_P3_AGG_SaleTransaction 	
where ForecastingType='Logistics' and DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200'
And 
(ForYear=2019 And ForMonth=9)
Group By ForYear, ForMonth, DivisionName, DepotName, HQ, ProductName,PackUnit 
order by ForYear, ForMonth,DivisionName, DepotName, HQ,  ProductName,PackUnit



Select *  From tbl_P3_AGG_SaleTransaction 
where DivisionName='PHOENIX' and DepotName='AGTL' And ProductName='CARMOZYME' and PackUnit='200' and ForYear=2020 and ForMonth=8
Group by ForecastingType, ForYear, ForMonth, DivisionName,  DepotName 

--	Update tbl_P3_PrimarySaleTransDetails_Uploaded Set IsProcessed=0 Where IsProcessed=1
--	Update tbl_P3_SecondarySaleTransDetails_Uploaded Set IsProcessed=0 Where IsProcessed=1












--Delete tbl_P3_SaleForecastingComparison
--Delete tbl_P3_SaleForecasting
--	Truncate Table tbl_P3_SaleProjection_Uploaded
--Delete tbl_P3_SaleProjection_SalesTeam
--Truncate Table  tbl_P3_AGG_SaleTransaction
--Delete tbl_P3_SecondarySaleTransDetails_ClosingStock
--Delete tbl_P3_SecondarySaleTransDetails_Uploaded
--Delete tbl_P3_PrimarySaleTransDetails_Uploaded