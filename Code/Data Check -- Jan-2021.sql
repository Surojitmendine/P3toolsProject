-- Data Upload Check --
Select * from tbl_P3_PrimarySaleTransDetails_Uploaded where ForYear=2020 And ForMonth=12
Select  * from tbl_P3_SecondarySaleTransDetails_Uploaded where ForYear=2020 And ForMonth=11


Select  * from tbl_P3_AGG_SaleTransaction where ForYear=2020 And ForMonth=12 --and year(saledate)=2021
Select  * from tbl_P3_SaleProjection_Uploaded where  year(ProjectionDate)=2021 order by PK_ProjectedSalesID desc


Select Top(100)  * from tbl_P3_SaleProjection_SalesTeam Order by PK_SalesTeamProjectionID Desc  -- where ForYear=2020 And ForMonth=11 and year(saledate)=2021

--Delete tbl_P3_SaleProjection_Uploaded where  year(ProjectionDate)=2021
--Delete from tbl_P3_AGG_SaleTransaction where ForYear=2020 And ForMonth=11 and year(saledate)=2021

Select Top(5000) * from tbl_P3_AGG_SaleTransaction order by PK_AGG_SaleID Desc


--Update tbl_P3_PrimarySaleTransDetails_Uploaded Set IsProcessed=0 where ForYear=2020 And ForMonth=12

--Update tbl_P3_SecondarySaleTransDetails_Uploaded Set IsProcessed=0 where ForYear=2020 And ForMonth=11