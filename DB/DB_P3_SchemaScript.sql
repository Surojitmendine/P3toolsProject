USE [P3]
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GetLastYearMonth]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[udf_GetLastYearMonth]
(
	-- Add the parameters for the function here
	@Year as Int=2020,
	@Month as Int=7
)
RETURNS DATE
AS
BEGIN
	-- Declare the return variable here
	Declare @PreviousMonthDate as DATE
	Declare @CurrentDate as DATE


	Set @CurrentDate=LTrim(RTrim(Str(@Month))) + '/01/' +  LTrim(RTrim(Str(@Year))) 

	-- DATEPART(m, DATEADD(m, -1, getdate()))
	Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -12, @CurrentDate)-12, -12)

	RETURN @PreviousMonthDate	
END

--	Select dbo.udf_GetLastYearMonth(2020,7)
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GetPreviousMonthDate]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[udf_GetPreviousMonthDate]
(
	-- Add the parameters for the function here
	@Year as Int=2020,
	@Month as Int=8,
	@NoofMonth as Int=0
)
RETURNS DATE
AS
BEGIN
	-- Declare the return variable here
	Declare @PreviousMonthDate as DATE
	Declare @CurrentDate as DATE


	Set @CurrentDate=LTrim(RTrim(Str(@Month))) + '/01/' +  LTrim(RTrim(Str(@Year))) 
	if (@NoofMonth=0)
		Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -1, @CurrentDate)-1, -1)
	if (@NoofMonth=-1)
		Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -1, @CurrentDate)-1, -1)
	Else If (@NoofMonth=-2)
		Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -2, @CurrentDate)-2, -2)
	Else If (@NoofMonth=-3)
		Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -3, @CurrentDate)-3, -1)
	Else
		Set @PreviousMonthDate=  GetDate()

	RETURN @PreviousMonthDate	
END

--	Select dbo.udf_GetPreviousMonthDate(2020,8, -1)
--	Select dbo.udf_GetPreviousMonthDate(2020,8, -2)
--	Select dbo.udf_GetPreviousMonthDate(2020,8, -3)
--	Select dbo.udf_GetPreviousMonthDate(2020,8, 0)
GO
/****** Object:  UserDefinedFunction [dbo].[udf_GetPrevMonth]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[udf_GetPrevMonth]
(
	-- Add the parameters for the function here
	@Year as Int=2020,
	@Month as Int=8
)
RETURNS DATE
AS
BEGIN
	-- Declare the return variable here
	Declare @PreviousMonthDate as DATE
	Declare @CurrentDate as DATE


	Set @CurrentDate=LTrim(RTrim(Str(@Month))) + '/01/' +  LTrim(RTrim(Str(@Year))) 

	-- DATEPART(m, DATEADD(m, -1, getdate()))
	Set @PreviousMonthDate=  DATEADD(MONTH, DATEDIFF(MONTH, -1, @CurrentDate)-1, -1)

	RETURN @PreviousMonthDate	
END

--	Select dbo.udf_GetPrevMonth(2020,8)
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_MonthName]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Description:	Returm Date in 01-Jan-2014 Format
-- =============================================
CREATE FUNCTION [dbo].[ufn_MonthName]
(
	-- Add the parameters for the function here
	 @SDate datetime
)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @MonthName as varchar(50)

	-- Add the T-SQL statements to compute the return value here
	SELECT @MonthName= DATENAME(month, DATEADD(month, 0, CAST(@SDate AS datetime)))

	-- Return the result of the function
	RETURN @MonthName

	-- Select dbo.ufn_MonthName(GETDATE())
END



GO
/****** Object:  Table [dbo].[AnalyticalReport_PerformRange]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AnalyticalReport_PerformRange](
	[PK_PerformerID] [int] IDENTITY(1,1) NOT NULL,
	[TypeName] [varchar](50) NULL,
	[PerformName] [varchar](50) NULL,
	[LowerValue] [int] NULL,
	[UpperValue] [int] NULL,
	[OrderBy] [int] NULL,
 CONSTRAINT [PK_AnalyticalReport_PerformRange] PRIMARY KEY CLUSTERED 
(
	[PK_PerformerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[closingstock]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[closingstock](
	[DIVISION] [varchar](10) NULL,
	[DEPOT] [varchar](100) NULL,
	[STK] [varchar](50) NULL,
	[PRODUCTNAME] [varchar](100) NULL,
	[PACK] [varchar](100) NULL,
	[BATCH] [varchar](100) NULL,
	[MFGDATE] [varchar](100) NULL,
	[EXPDATE] [varchar](100) NULL,
	[CLOSINGBALANCE] [int] NULL,
	[NRV] [numeric](18, 2) NULL,
	[STOCKVALUE] [numeric](18, 2) NULL,
	[DAYSEXP] [int] NULL,
	[TYPE] [varchar](100) NULL,
	[DAYSFRMANUFAC] [bigint] NULL,
	[WEIGTDAYS] [bigint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[salesdata]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[salesdata](
	[DIVISION] [varchar](10) NULL,
	[StockLocation] [varchar](100) NULL,
	[Billdate] [varchar](50) NULL,
	[Customercode] [varchar](100) NULL,
	[Customername] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[Productname] [varchar](100) NULL,
	[Packsize] [varchar](100) NULL,
	[productcode] [varchar](100) NULL,
	[Qtymade] [bigint] NULL,
	[FreeQty] [bigint] NULL,
	[Billamount] [numeric](18, 2) NULL,
	[TaxableAmt] [numeric](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Master_Month]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Master_Month](
	[PK_MonthID] [int] IDENTITY(1,1) NOT NULL,
	[MonthName] [varchar](20) NULL,
	[ShortMonthName] [varchar](5) NULL,
 CONSTRAINT [PK_tbl_Master_Month] PRIMARY KEY CLUSTERED 
(
	[PK_MonthID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_Master_Product]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_Master_Product](
	[PK_ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [int] NULL,
	[ProductType] [varchar](50) NULL,
	[ProductCategory] [varchar](50) NULL,
	[ProductUOM] [varchar](50) NULL,
	[FactorValue] [decimal](18, 2) NULL,
	[BatchSize] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_Master_Product] PRIMARY KEY CLUSTERED 
(
	[PK_ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_AGG_SaleTransaction]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_AGG_SaleTransaction](
	[PK_AGG_SaleID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_ClientID] [int] NULL,
	[IsProcessed] [bit] NULL,
	[SaleDate] [datetime] NULL,
	[ForYear] [int] NULL,
	[ForMonth] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[CustomerCode] [varchar](50) NULL,
	[CustomerName] [varchar](250) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[TotalSalesQTY] [decimal](18, 2) NULL,
	[TotalFreeSampleQTY] [decimal](18, 2) NULL,
	[GrossAmount] [decimal](18, 2) NULL,
	[TotalDiscountAmount] [decimal](18, 2) NULL,
	[TotalTaxAmount] [decimal](18, 2) NULL,
	[TotalNetAmount] [decimal](18, 2) NULL,
	[ForecastingType] [varchar](50) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_AGG_SaleTransaction] PRIMARY KEY CLUSTERED 
(
	[PK_AGG_SaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Master_CalculationFormula]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Master_CalculationFormula](
	[PK_ID] [int] IDENTITY(1,1) NOT NULL,
	[FormulaType] [varchar](50) NULL,
	[Condition1] [varchar](50) NULL,
	[Condition2] [varchar](50) NULL,
	[FormulaValue] [decimal](18, 2) NULL,
	[Remarks] [varchar](250) NULL,
	[ForecastingType] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_P3_Master_CalculationFormula] PRIMARY KEY CLUSTERED 
(
	[PK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Master_DepotNameMapping]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Master_DepotNameMapping](
	[PK_DepotMapID] [int] IDENTITY(1,1) NOT NULL,
	[P3DepotName] [varchar](100) NULL,
	[SwillDepotName] [varchar](100) NULL,
 CONSTRAINT [PK_tbl_P3_Master_DepotNameMapping] PRIMARY KEY CLUSTERED 
(
	[PK_DepotMapID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Master_Divisionwise_Product]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Master_Divisionwise_Product](
	[PK_ID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [smalldatetime] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
 CONSTRAINT [PK_tbl_P3_Master_Divisionwise_Product] PRIMARY KEY CLUSTERED 
(
	[PK_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded](
	[PK_SalesTransactionID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ClientID] [int] NULL,
	[IsProcessed] [bit] NULL,
	[ForYear] [int] NULL,
	[ForMonth] [int] NULL,
	[SaleDate] [datetime] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[CustomerCode] [varchar](50) NULL,
	[CustomerName] [varchar](250) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[SalesQTY] [decimal](18, 2) NULL,
	[FreeSampleQTY] [decimal](18, 2) NULL,
	[GrossAmount] [decimal](18, 2) NULL,
	[DiscountAmount] [decimal](18, 2) NULL,
	[TaxAmount] [decimal](18, 2) NULL,
	[NetAmount] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_SaleTransDetails_Uploaded] PRIMARY KEY CLUSTERED 
(
	[PK_SalesTransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_DepotStock_AGG]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_DepotStock_AGG](
	[PK_AggDepotStockID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[StockDate] [datetime] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_AGG_DepotStock] PRIMARY KEY CLUSTERED 
(
	[PK_AggDepotStockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_DepotStock_Raw]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_DepotStock_Raw](
	[PK_DepotStockRawID] [int] IDENTITY(1,1) NOT NULL,
	[StockDate] [datetime] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[BatchNo] [varchar](50) NULL,
	[ProductGroup] [varchar](50) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_DepotStock] PRIMARY KEY CLUSTERED 
(
	[PK_DepotStockRawID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_FactoryProductionTarget_AGG]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_FactoryProductionTarget_AGG](
	[PK_FactoryProductionTargetID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[FactoryProductionTargetDate] [datetime] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](50) NULL,
	[FinalUnits_QTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_FactoryProductionTargetAGG] PRIMARY KEY CLUSTERED 
(
	[PK_FactoryProductionTargetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_Forecasting_Product]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_Forecasting_Product](
	[PK_ProductionForecastProductID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductType] [varchar](50) NULL,
	[ProductCategory] [varchar](100) NULL,
	[ProductName] [varchar](100) NULL,
	[FactorValue] [decimal](18, 2) NULL,
	[Volumn] [decimal](18, 2) NULL,
	[WIPQTY] [decimal](18, 2) NULL,
	[ChargeableVolume_InLtr] [decimal](18, 2) NULL,
	[BatchSize] [decimal](18, 2) NULL,
	[FinalChargeableVolume_InLtr] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_Production_ForecastingProduct] PRIMARY KEY CLUSTERED 
(
	[PK_ProductionForecastProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_Forecasting_SKU]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_Forecasting_SKU](
	[PK_ProductionForecastSKUID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductType] [varchar](50) NULL,
	[ProductCategory] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [int] NULL,
	[ProductionForecastQTY] [decimal](18, 2) NULL,
	[FactorValue] [decimal](18, 2) NULL,
	[FactorForecastQTY] [decimal](18, 2) NULL,
	[ProductionForecastVolume_InLtr] [decimal](18, 2) NULL,
	[ChargeableVolume_InLtr] [decimal](18, 2) NULL,
	[BatchSize] [decimal](18, 2) NULL,
	[FinalChargeableVolume_InLtr] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_Production_Forecasting] PRIMARY KEY CLUSTERED 
(
	[PK_ProductionForecastSKUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_Forecasting_Temp]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_Forecasting_Temp](
	[PK_TempSKUID] [int] IDENTITY(1,1) NOT NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [int] NULL,
	[ProductionForecastQTY] [decimal](18, 2) NULL,
	[FactorValue] [decimal](18, 2) NULL,
	[FactorForecastQTY] [decimal](18, 2) NULL,
	[ProductionForecastVolume_InLtr] [decimal](18, 2) NULL,
	[WIPQTY] [decimal](18, 2) NULL,
	[ChargeableVolume_InLtr] [decimal](18, 2) NULL,
	[BatchSize] [decimal](18, 2) NULL,
	[FinalChargeableVolume_InLtr] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_Production_Forecasting_Temp] PRIMARY KEY CLUSTERED 
(
	[PK_TempSKUID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_PhysicianSample_AGG]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_PhysicianSample_AGG](
	[PK_PhysicianSampleID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[PhysicianSampleQTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_PhysicianSample_AGG] PRIMARY KEY CLUSTERED 
(
	[PK_PhysicianSampleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_WIPStock_AGG]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_WIPStock_AGG](
	[PK_WIPStockAGGID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[StockDate] [datetime] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[WIPStock_QTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_WIPStock_AGG] PRIMARY KEY CLUSTERED 
(
	[PK_WIPStockAGGID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_Production_WIPStock_Raw]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_Production_WIPStock_Raw](
	[PK_WIPStockRawID] [int] IDENTITY(1,1) NOT NULL,
	[StockDate] [datetime] NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[BatchNo] [varchar](50) NULL,
	[WIPStock_QTY] [decimal](18, 2) NULL,
	[IsProcessed] [bit] NULL,
 CONSTRAINT [PK_tbl_P3_Production_WIPStock_Raw] PRIMARY KEY CLUSTERED 
(
	[PK_WIPStockRawID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SaleForecasting]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SaleForecasting](
	[PK_SaleForecastingID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[CurrentDate] [datetime] NULL,
	[Frequency] [varchar](50) NULL,
	[ForecastingDate] [datetime] NULL,
	[ForecastingForMonth] [int] NULL,
	[ForecastingForYear] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[Customer] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[Projected_SaleQTY] [decimal](18, 2) NULL,
	[Current_SalesQTY] [decimal](18, 2) NULL,
	[Next_ProjectionSalesQTY] [decimal](18, 2) NULL,
	[ForecastingType] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_P3_SaleForecasting] PRIMARY KEY CLUSTERED 
(
	[PK_SaleForecastingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SaleForecasting_SwillClosingStock]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SaleForecasting_SwillClosingStock](
	[PK_SwillID] [int] IDENTITY(1,1) NOT NULL,
	[IsProcessed] [bit] NULL,
	[ForYear] [int] NULL,
	[ForMonth] [int] NULL,
	[SyncDate] [datetime] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_SaleForecasting_SwillClosingStock] PRIMARY KEY CLUSTERED 
(
	[PK_SwillID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SaleForecastingComparison]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SaleForecastingComparison](
	[PK_SaleComparisonID] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[FK_CreatedByID] [int] NULL,
	[ForecastingForMonth] [int] NULL,
	[ForecastingForYear] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](100) NULL,
	[PackUnit] [varchar](100) NULL,
	[Logistics_ProjectionSalesQTY] [decimal](18, 2) NULL,
	[Marketing_ProjectedSaleQTY] [decimal](18, 2) NULL,
	[Sales_ProjectedSaleQTY] [decimal](18, 2) NULL,
	[DifferencePersentage] [decimal](18, 2) NULL,
	[IsAutoCalculate] [bit] NULL,
	[NextMonth_ForecastingQTY] [decimal](18, 2) NULL,
	[NextMonth_FinialForecastingQTY] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_SaleForecastingComparison] PRIMARY KEY CLUSTERED 
(
	[PK_SaleComparisonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SaleProjection_SalesTeam]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam](
	[PK_SalesTeamProjectionID] [int] IDENTITY(1,1) NOT NULL,
	[IsManual] [bit] NULL,
	[IsProcessed] [bit] NULL,
	[ProjectionDate] [datetime] NULL,
	[ForMonth] [int] NULL,
	[ForYear] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[PrimaryTotalSalesQTY] [decimal](18, 2) NULL,
	[TotalClosingStockQTY] [decimal](18, 2) NULL,
	[ProjectedTotalSalesQTY] [decimal](18, 2) NULL,
	[ForecastingType] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_P3_SaleProjection_SalesTeam] PRIMARY KEY CLUSTERED 
(
	[PK_SalesTeamProjectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SaleProjection_Uploaded]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SaleProjection_Uploaded](
	[PK_ProjectedSalesID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectionDate] [datetime] NULL,
	[SalesPersonCode] [varchar](20) NULL,
	[SalesPersonName] [varchar](100) NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[CustomerCode] [varchar](50) NULL,
	[CustomerName] [varchar](250) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[ProjectionForMonth] [int] NULL,
	[ProjectionForYear] [int] NULL,
	[ProjectedTotalSalesQTY] [decimal](18, 2) NULL,
	[ProjectedAproxQTY] [decimal](18, 2) NULL,
	[ForecastingType] [varchar](50) NULL,
 CONSTRAINT [PK_tbl_P3_SaleProjection] PRIMARY KEY CLUSTERED 
(
	[PK_ProjectedSalesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SecondarySaleTransDetails_ClosingStock_bak]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_ClosingStock_bak](
	[PK_SSClosingStockID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ClientID] [int] NULL,
	[IsProcessed] [bit] NULL,
	[ForYear] [int] NULL,
	[ForMonth] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[CustomerCode] [varchar](50) NULL,
	[CustomerName] [varchar](250) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[UOM] [varchar](50) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_SecondarySaleTransDetails_ClosingStock] PRIMARY KEY CLUSTERED 
(
	[PK_SSClosingStockID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded](
	[PK_SalesTransactionID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ClientID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[IsProcessed] [bit] NULL,
	[ForYear] [int] NULL,
	[ForMonth] [int] NULL,
	[DivisionName] [varchar](100) NULL,
	[DepotName] [varchar](100) NULL,
	[HQ] [varchar](100) NULL,
	[CustomerCode] [varchar](50) NULL,
	[CustomerName] [varchar](250) NULL,
	[ProductCode] [varchar](50) NULL,
	[ProductName] [varchar](250) NULL,
	[PackUnit] [varchar](100) NULL,
	[UOM] [varchar](50) NULL,
	[FreeSampleQTY] [decimal](18, 2) NULL,
	[SalesQTY] [decimal](18, 2) NULL,
	[ClosingStockQTY] [decimal](18, 2) NULL,
 CONSTRAINT [PK_tbl_P3_SecondarySaleTransDetails_Uploaded] PRIMARY KEY CLUSTERED 
(
	[PK_SalesTransactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_RoleClaims]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_RoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_RoleID] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_Roles]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_Roles](
	[PK_RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[PK_RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_UserClaims]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_UserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FK_UserID] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_UserInformation]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_UserInformation](
	[PK_UserID] [int] IDENTITY(1,1) NOT NULL,
	[FK_UserID_CreatedBy] [int] NULL,
	[FK_DateKey_CreatedOn] [int] NULL,
	[FK_TimeKey_CreatedOn] [int] NULL,
	[IsDeleted] [bit] NULL,
	[IsActive] [bit] NULL,
	[FK_UserID_ModifiedBy] [int] NULL,
	[FK_DateKey_ModifiedOn] [int] NULL,
	[FK_TimeKey_ModifiedOn] [int] NULL,
	[FK_ClientID] [int] NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[Password] [varchar](20) NULL,
	[Title] [varchar](50) NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[Gender] [varchar](20) NULL,
	[MobileNo] [varchar](20) NULL,
	[DisplayUserName] [varchar](150) NULL,
	[UserImagePath] [varchar](500) NULL,
	[IsSuperAdmin] [bit] NULL,
	[Latitude] [varchar](50) NULL,
	[Longitude] [varchar](50) NULL,
	[Device_UUID] [varchar](50) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[PK_UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_UserLogins]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_UserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[FK_UserID] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_UserRoles]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_UserRoles](
	[PK_UserID] [int] NOT NULL,
	[PK_RoleID] [int] NOT NULL,
 CONSTRAINT [PK_tbl_SYS_AspNet_UserRoles] PRIMARY KEY CLUSTERED 
(
	[PK_UserID] ASC,
	[PK_RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_AspNet_UserTokens]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_AspNet_UserTokens](
	[UserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_Master_Menu]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_Master_Menu](
	[PK_MenuID] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [smalldatetime] NULL,
	[IsActive] [bit] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedOn] [smalldatetime] NULL,
	[ModuleName] [varchar](50) NULL,
	[MenuName] [varchar](100) NULL,
	[ViewOrder] [int] NULL,
	[FK_MenuID_Parent] [int] NULL,
	[MenuType] [varchar](50) NULL,
	[PageRedirect] [varchar](500) NULL,
	[TablesName] [varchar](500) NULL,
	[RelatedTableName] [varchar](500) NULL,
	[StoreProcedureName] [varchar](500) NULL,
	[RelatedSPName] [varchar](500) NULL,
 CONSTRAINT [PK_tbl_SYS_Master_Menu] PRIMARY KEY CLUSTERED 
(
	[PK_MenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbl_SYS_UserPermission]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_SYS_UserPermission](
	[PK_RoleAccessID] [int] IDENTITY(1,1) NOT NULL,
	[Createdby] [int] NULL,
	[CreatedDate] [smalldatetime] NULL,
	[IsActive] [bit] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [smalldatetime] NULL,
	[CompanyID] [int] NULL,
	[FK_RoleID] [int] NULL,
	[ModuleID] [int] NULL,
	[FK_MenuID] [int] NULL,
	[IsShowMenuInGroup] [bit] NULL,
	[Permission_New] [bit] NULL,
	[Permission_Edit] [bit] NULL,
	[Permission_Del] [bit] NULL,
	[Permission_View] [bit] NULL,
	[Permission_Print] [bit] NULL,
	[Permission_Export] [bit] NULL,
	[Permission_Import] [bit] NULL,
	[Permission_Report] [bit] NULL,
	[Permission_Others] [bit] NULL,
 CONSTRAINT [PK_tbl_SYS_UserPermission] PRIMARY KEY CLUSTERED 
(
	[PK_RoleAccessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TblProdmast]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TblProdmast](
	[prodcode] [varchar](50) NULL,
	[prodcuct_name] [nvarchar](200) NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_FK_ClientID]  DEFAULT ((1)) FOR [FK_ClientID]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_TotalSaleQTY]  DEFAULT ((0)) FOR [TotalSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_GrossAmount]  DEFAULT ((0)) FOR [GrossAmount]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_TotalDiscountAmount]  DEFAULT ((0)) FOR [TotalDiscountAmount]
GO
ALTER TABLE [dbo].[tbl_P3_AGG_SaleTransaction] ADD  CONSTRAINT [DF_tbl_P3_AGG_SaleTransaction_ClosingStockQTY]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Master_CalculationFormula] ADD  CONSTRAINT [DF_tbl_P3_Master_CalculationFormula_FormulaValue]  DEFAULT ((0)) FOR [FormulaValue]
GO
ALTER TABLE [dbo].[tbl_P3_Master_Divisionwise_Product] ADD  CONSTRAINT [DF_tbl_P3_Master_Divisionwise_Product_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_FK_ClientID]  DEFAULT ((1)) FOR [FK_ClientID]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_PrimarySaleTransDetails_Uploaded_ForYear]  DEFAULT ((0)) FOR [ForYear]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_PrimarySaleTransDetails_Uploaded_ForMonth]  DEFAULT ((0)) FOR [ForMonth]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_TotalSalesQTY]  DEFAULT ((0)) FOR [SalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_FreeSampleQTY]  DEFAULT ((0)) FOR [FreeSampleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_GrossAmount]  DEFAULT ((0)) FOR [GrossAmount]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_TotalDiscountAmount]  DEFAULT ((0)) FOR [DiscountAmount]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_TaxAmount]  DEFAULT ((0)) FOR [TaxAmount]
GO
ALTER TABLE [dbo].[tbl_P3_PrimarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleTransDetails_Uploaded_NetAmount]  DEFAULT ((0)) FOR [NetAmount]
GO
ALTER TABLE [dbo].[tbl_P3_Production_DepotStock_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_AGG_DepotStock_ClosingStock]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_DepotStock_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_DepotStock_AGG_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_Production_DepotStock_Raw] ADD  CONSTRAINT [DF_tbl_P3_Production_DepotStock_Raw_StockDate]  DEFAULT (getdate()) FOR [StockDate]
GO
ALTER TABLE [dbo].[tbl_P3_Production_DepotStock_Raw] ADD  CONSTRAINT [DF_tbl_P3_Production_DepotStock_ClosingStock]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_DepotStock_Raw] ADD  CONSTRAINT [DF_tbl_P3_Production_DepotStock_Raw_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_Production_FactoryProductionTarget_AGG] ADD  CONSTRAINT [DF_Table_1_WIPStock_QTY]  DEFAULT ((0)) FOR [FinalUnits_QTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_FactoryProductionTarget_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_FactoryProductionTargetAGG_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_Table_1_IsAutoCalculate]  DEFAULT ((0)) FOR [ProductionForecastQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_FactorValue]  DEFAULT ((0)) FOR [FactorValue]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_FactorForecastQTY]  DEFAULT ((0)) FOR [FactorForecastQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_ProductionForecastVolume_InLtr]  DEFAULT ((0)) FOR [ProductionForecastVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_ChargeableVolume_InLtr]  DEFAULT ((0)) FOR [ChargeableVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_BatchSize]  DEFAULT ((0)) FOR [BatchSize]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_SKU] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_FinalChargeableVolume_InLtr]  DEFAULT ((0)) FOR [FinalChargeableVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_ProductionForecastQTY]  DEFAULT ((0)) FOR [ProductionForecastQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_FactorValue]  DEFAULT ((0)) FOR [FactorValue]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_FactorForecastQTY]  DEFAULT ((0)) FOR [FactorForecastQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_ProductionForecastVolume_InLtr]  DEFAULT ((0)) FOR [ProductionForecastVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_WIPQTY]  DEFAULT ((0)) FOR [WIPQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_ChargeableVolume_InLtr]  DEFAULT ((0)) FOR [ChargeableVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_BatchSize]  DEFAULT ((0)) FOR [BatchSize]
GO
ALTER TABLE [dbo].[tbl_P3_Production_Forecasting_Temp] ADD  CONSTRAINT [DF_tbl_P3_Production_Forecasting_Temp_FinalChargeableVolume_InLtr]  DEFAULT ((0)) FOR [FinalChargeableVolume_InLtr]
GO
ALTER TABLE [dbo].[tbl_P3_Production_PhysicianSample_AGG] ADD  CONSTRAINT [DF_Table_1_WIPStock_QTY_1]  DEFAULT ((0)) FOR [PhysicianSampleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_PhysicianSample_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_PhysicianSample_AGG_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_Production_WIPStock_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_WIPStock_AGG_WIPStock_QTY]  DEFAULT ((0)) FOR [WIPStock_QTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_WIPStock_AGG] ADD  CONSTRAINT [DF_tbl_P3_Production_WIPStock_AGG_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_Production_WIPStock_Raw] ADD  CONSTRAINT [DF_Table_1_ClosingStock]  DEFAULT ((0)) FOR [WIPStock_QTY]
GO
ALTER TABLE [dbo].[tbl_P3_Production_WIPStock_Raw] ADD  CONSTRAINT [DF_tbl_P3_Production_WIPStock_Raw_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_Projected_SaleQTY]  DEFAULT ((0)) FOR [Projected_SaleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_Current_SalesQTY]  DEFAULT ((0)) FOR [Current_SalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_Next_ProjectionSalesQTY]  DEFAULT ((0)) FOR [Next_ProjectionSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting_SwillClosingStock] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_SwillClosingStock_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting_SwillClosingStock] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_SwillClosingStock_ForYear]  DEFAULT ((0)) FOR [ForYear]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting_SwillClosingStock] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_SwillClosingStock_ForMonth]  DEFAULT ((0)) FOR [ForMonth]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecasting_SwillClosingStock] ADD  CONSTRAINT [DF_tbl_P3_SaleForecasting_SwillClosingStock_SalesQTY]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_Next_ProjectionSalesQTY]  DEFAULT ((0)) FOR [Logistics_ProjectionSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_Projected_SaleQTY]  DEFAULT ((0)) FOR [Marketing_ProjectedSaleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_tbl_P3_SaleForecastingComparison_Marketing_ProjectedSaleQTY1]  DEFAULT ((0)) FOR [Sales_ProjectedSaleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_LastYear_SalesQTY]  DEFAULT ((0)) FOR [DifferencePersentage]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_Current_SalesQTY]  DEFAULT ((0)) FOR [IsAutoCalculate]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_LastPrevious_SalesQTY]  DEFAULT ((0)) FOR [NextMonth_ForecastingQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleForecastingComparison] ADD  CONSTRAINT [DF_Table_1_PreviousSalesQTY]  DEFAULT ((0)) FOR [NextMonth_FinialForecastingQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_IsManual]  DEFAULT ((0)) FOR [IsManual]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_ProjectionForYear]  DEFAULT ((2020)) FOR [ForYear]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_ProjectedTotalSalesQTY1]  DEFAULT ((0)) FOR [PrimaryTotalSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_ProjectedTotalSalesQTY2]  DEFAULT ((0)) FOR [TotalClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_SalesTeam] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_SalesTeam_ProjectedTotalSalesQTY]  DEFAULT ((0)) FOR [ProjectedTotalSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_Uploaded_ProjectionForYear]  DEFAULT ((2020)) FOR [ProjectionForYear]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_TotalSalesQTY]  DEFAULT ((0)) FOR [ProjectedTotalSalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SaleProjection_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SaleProjection_Uploaded_ProjectedTotalSalesQTY1]  DEFAULT ((0)) FOR [ProjectedAproxQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_ClosingStock_bak] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_ClosingStock_FK_ClientID]  DEFAULT ((1)) FOR [FK_ClientID]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_ClosingStock_bak] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_ClosingStock_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_ClosingStock_bak] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_ClosingStock_SalesQTY]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_FK_ClientID]  DEFAULT ((1)) FOR [FK_ClientID]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_IsProcessed]  DEFAULT ((0)) FOR [IsProcessed]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_FreeSampleQTY]  DEFAULT ((0)) FOR [FreeSampleQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_TotalSalesQTY]  DEFAULT ((0)) FOR [SalesQTY]
GO
ALTER TABLE [dbo].[tbl_P3_SecondarySaleTransDetails_Uploaded] ADD  CONSTRAINT [DF_tbl_P3_SecondarySaleTransDetails_Uploaded_ClosingStockQTY]  DEFAULT ((0)) FOR [ClosingStockQTY]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserInformation] ADD  CONSTRAINT [DF_tbl_SYS_AspNet_Users_FK_DateKey_CreatedOn]  DEFAULT ((((rtrim(datepart(year,getdate()))+replicate('0',(2)-len(rtrim(datepart(month,getdate())))))+rtrim(datepart(month,getdate())))+replicate('0',(2)-len(rtrim(datepart(day,getdate())))))+rtrim(datepart(day,getdate()))) FOR [FK_DateKey_CreatedOn]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserInformation] ADD  CONSTRAINT [DF_tbl_SYS_AspNet_Users_FK_TimeKey_CreatedOn]  DEFAULT ((datepart(hour,getdate())*(3600)+datepart(minute,getdate())*(60))+datepart(second,getdate())) FOR [FK_TimeKey_CreatedOn]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserInformation] ADD  CONSTRAINT [DF_tbl_SYS_AspNet_Users_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserInformation] ADD  CONSTRAINT [DF_tbl_SYS_AspNet_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbl_SYS_Master_Menu] ADD  CONSTRAINT [DF_tbl_SYS_Master_Menu_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_SYS_Master_Menu] ADD  CONSTRAINT [DF_tbl_SYS_Master_Menu_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbl_SYS_Master_Menu] ADD  CONSTRAINT [DF_tbl_SYS_Master_Menu_ViewOrder]  DEFAULT ((0)) FOR [ViewOrder]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_IsShowMenuInGroup]  DEFAULT ((1)) FOR [IsShowMenuInGroup]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_New]  DEFAULT ((1)) FOR [Permission_New]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Edit]  DEFAULT ((0)) FOR [Permission_Edit]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Del]  DEFAULT ((0)) FOR [Permission_Del]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_View]  DEFAULT ((1)) FOR [Permission_View]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Print]  DEFAULT ((0)) FOR [Permission_Print]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Export]  DEFAULT ((0)) FOR [Permission_Export]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Import]  DEFAULT ((1)) FOR [Permission_Import]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Report]  DEFAULT ((1)) FOR [Permission_Report]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] ADD  CONSTRAINT [DF_tbl_SYS_UserPermission_Permission_Others]  DEFAULT ((1)) FOR [Permission_Others]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_RoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([FK_RoleID])
REFERENCES [dbo].[tbl_SYS_AspNet_Roles] ([PK_RoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_RoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([FK_UserID])
REFERENCES [dbo].[tbl_SYS_AspNet_UserInformation] ([PK_UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([FK_UserID])
REFERENCES [dbo].[tbl_SYS_AspNet_UserInformation] ([PK_UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([PK_RoleID])
REFERENCES [dbo].[tbl_SYS_AspNet_Roles] ([PK_RoleID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([PK_UserID])
REFERENCES [dbo].[tbl_SYS_AspNet_UserInformation] ([PK_UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbl_SYS_AspNet_UserInformation] ([PK_UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbl_SYS_AspNet_UserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[tbl_SYS_Master_Menu]  WITH CHECK ADD  CONSTRAINT [FK_tbl_SYS_Master_Menu_tbl_SYS_Master_Menu] FOREIGN KEY([FK_MenuID_Parent])
REFERENCES [dbo].[tbl_SYS_Master_Menu] ([PK_MenuID])
GO
ALTER TABLE [dbo].[tbl_SYS_Master_Menu] CHECK CONSTRAINT [FK_tbl_SYS_Master_Menu_tbl_SYS_Master_Menu]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_tbl_SYS_UserPermission_tbl_SYS_AspNet_Roles] FOREIGN KEY([FK_MenuID])
REFERENCES [dbo].[tbl_SYS_Master_Menu] ([PK_MenuID])
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] CHECK CONSTRAINT [FK_tbl_SYS_UserPermission_tbl_SYS_AspNet_Roles]
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission]  WITH CHECK ADD  CONSTRAINT [FK_tbl_SYS_UserPermission_tbl_SYS_UserPermission] FOREIGN KEY([PK_RoleAccessID])
REFERENCES [dbo].[tbl_SYS_UserPermission] ([PK_RoleAccessID])
GO
ALTER TABLE [dbo].[tbl_SYS_UserPermission] CHECK CONSTRAINT [FK_tbl_SYS_UserPermission_tbl_SYS_UserPermission]
GO
/****** Object:  StoredProcedure [dbo].[Get_P3SalesForcastingData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[Get_P3SalesForcastingData]
  AS
  BEGIN
	SELECT 
		[ForecastingForMonth]
      , [ForecastingForYear]
	  , CASE WHEN [DivisionName] = 'EVA' THEN 'MAD' ELSE [DivisionName] END
      , [DepotName] 
	  , [ProductCode]
	  , [ProductName]
	  , [PackUnit]
	  , [NextMonth_FinialForecastingQTY]
	  FROM [P3].[dbo].[tbl_P3_SaleForecastingComparison]
  END
GO
/****** Object:  StoredProcedure [dbo].[GetP3SalesForcastingData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[GetP3SalesForcastingData]
  AS
  BEGIN
	SELECT 
		[ForecastingForMonth]
      , [ForecastingForYear]
      , [DepotName] 
	  , [ProductName]
	  , [PackUnit]
	  , [NextMonth_FinialForecastingQTY]
	  FROM [P3].[dbo].[tbl_P3_SaleForecastingComparison]
  END
GO
/****** Object:  StoredProcedure [dbo].[insertsalesdata]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[insertsalesdata]
@startdate varchar(50),
@endate varchar(50)
as
begin

EXECUTE [dbo].[getsalesdata] @startdate
			,@endate
end
GO
/****** Object:  StoredProcedure [dbo].[SALESTRANSACTION]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SALESTRANSACTION] 
	-- Add the parameters for the stored procedure here
	@startdate varchar(10)=null,
	@enddate varchar(10)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

    -- Insert statements for procedure here

	CREATE TABLE #TEMP (DIVISION varchar(10),
		StockLocation varchar(100),
			Billdate varchar(100),
				Customercode varchar(100),	Customername varchar(100),
					HQ varchar(100),	Productname varchar(100),
						Packsize varchar(100),	productcode	varchar(100),
						Qtymade bigint,	FreeQty bigint,	Billamount decimal(18,2),	TaxableAmt decimal(18,2)
						);


						BEGIN DISTRIBUTED TRANSACTION; 
						insert into #TEMP


	EXEC  [103.253.125.131,5000].[MendineMaster].[dbo].[Transactionbilltaxableamtautomation] @startdate,@enddate
	commit transaction
	select * from #TEMP

	DROP TABLE #TEMP;
END
GO
/****** Object:  StoredProcedure [dbo].[usp_01_LoadPrimary_SalesTransactionData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 06-04-2020
-- Description:	Load Daily Sales Data from Transaction System to P3 System
-- Date Format : DD/MM/YYYY
-- Exec usp_01_LoadPrimary_SalesTransactionData @StartDate='01/05/2020', @EndDate='30/06/2020'
-- =============================================
CREATE PROCEDURE [dbo].[usp_01_LoadPrimary_SalesTransactionData]
	-- Add the parameters for the stored procedure here
	@StartDate varchar(10) =null,
	@EndDate varchar(10) =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	SET NOCOUNT ON;

	-- Truncate Table tbl_P3_PrimarySaleTransDetails_Uploaded
	EXECUTE [dbo].[getsalesdata] @StartDate ,@EndDate
	
	Insert Into tbl_P3_PrimarySaleTransDetails_Uploaded 
	(DivisionName, DepotName, SaleDate, ForYear, ForMonth, CustomerCode, CustomerName, HQ, ProductName, PackUnit, ProductCode, SalesQTY, FreeSampleQTY, NetAmount, GrossAmount)
	
	Select  DIVISION, StockLocation, Billdate, Year(Billdate), Month(Billdate),  Customercode, Customername, HQ, Productname, Packsize, productcode, Qtymade, FreeQty, Billamount, TaxableAmt
	from salesdata

	Truncate Table salesdata
END
GO
/****** Object:  StoredProcedure [dbo].[usp_01_LoadSecondary_SalesTransactionData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 06-04-2020
-- Description:	Load Daily Sales Data from Transaction System to P3 System
-- Date Format : DD/MM/YYYY
-- Exec usp_01_LoadSecondary_SalesTransactionData @StartDate='01/01/2020', @EndDate='02/01/2020'
-- =============================================
Create PROCEDURE [dbo].[usp_01_LoadSecondary_SalesTransactionData]
	-- Add the parameters for the stored procedure here
	@StartDate varchar(10) =null,
	@EndDate varchar(10) =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	SET NOCOUNT ON;

	-- Truncate Table tbl_P3_SaleTransDetails_Uploaded

	Insert Into tbl_P3_SecondarySaleTransDetails_Uploaded
	(DivisionName, DepotName,  CustomerCode, CustomerName, HQ, ProductName, PackUnit, ProductCode, SalesQTY, FreeSampleQTY)
	EXEC  [103.253.125.131,5000].[MendineMaster].[dbo].[Transactionbilltaxableamtautomation] @startdate, @enddate

	
	--Insert Into tbl_P3_SaleTransDetails_Uploaded 
	--(DivisionName, DepotName, SaleDate, CustomerCode, CustomerName, HQ, ProductName, PackUnit, ProductCode, SalesQTY, FreeSampleQTY, NetAmount, GrossAmount)
	--Select * from salesdata


END
GO
/****** Object:  StoredProcedure [dbo].[usp_01_LoadSwill_ClosingData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sajal
-- Create date: 06-04-2020
-- Description:	Load Closing Stock Data from Swill System to P3 System
-- Date Format : MM/DD/YYYY
-- Exec usp_01_LoadSwill_ClosingData @StartDate='12/31/2020', @ForYear=2021, @ForMonth=1
-- =============================================
CREATE PROCEDURE [dbo].[usp_01_LoadSwill_ClosingData]
	-- Add the parameters for the stored procedure here
	@StartDate varchar(10) =null, @ForYear as int=2021, @ForMonth as int=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	SET NOCOUNT ON;

	EXECUTE [dbo].[getclosingstock] @StartDate
	
	Insert Into tbl_P3_SaleForecasting_SwillClosingStock 
	(ForYear, ForMonth, SyncDate, DivisionName, DepotName, ProductName, PackUnit, ClosingStockQTY)
	
	SELECT @ForYear, @ForMonth, GetDate(),  DIVISION, STK, PRODUCTNAME, PACK,  Sum(CLOSINGBALANCE) as [CLOSINGBALANCE] 	FROM     closingstock
	Group By DIVISION, STK, PRODUCTNAME, PACK

	--Truncate Table closingstock

	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='AGARTALA' Where DepotName='AGTL' 
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='BURDWAN' Where DepotName='BDWN'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='BEHALA' Where DepotName='BHLA'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='BARASAT' Where DepotName='BRST'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='CUTTACK' Where DepotName='CTCK'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='DUMDUM' Where DepotName='DMDM'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='GUWAHATI' Where DepotName='GHY'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='HOWRAH' Where DepotName='HWH'

	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='HOWRAH' Where DepotName='MDNP'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='MALDA' Where DepotName='MLDA'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='NEW ALIPORE' Where DepotName='NWLP'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='PATNA' Where DepotName='PTNA'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='SILIGURI' Where DepotName='SLG'
	Update  tbl_P3_SaleForecasting_SwillClosingStock Set DepotName='TAMLUK' Where DepotName='TMLK'
END
GO
/****** Object:  StoredProcedure [dbo].[usp_02_Load_MasterData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 07-Apr-2020
-- Exec usp_Load_MasterData 
-- =============================================
CREATE PROCEDURE [dbo].[usp_02_Load_MasterData]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	--  Truncate Table tbl_Master_Division
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-----   Division --------------------
	INSERT INTO tbl_Master_Division (DivisionName)	
	SELECT Distinct td.DivisionName FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct m.DivisionName FROM tbl_Master_Division m  WHERE m.DivisionName = td.DivisionName)

	-----   Depot Name --------------------
	INSERT INTO tbl_Master_Depot (DepotName)	
	SELECT Distinct td.DepotName FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct m.DepotName FROM tbl_Master_Depot m  WHERE m.DepotName = td.DepotName)

	-----   HQ --------------------
	INSERT INTO tbl_Master_HQ (HQName)	
	SELECT Distinct td.HQ FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct m.HQName FROM tbl_Master_HQ m  WHERE m.HQName = td.HQ)

	-----   Customer Name --------------------
	INSERT INTO tbl_Master_Customer (CustomerCode, CustomerName)	
	SELECT Distinct td.CustomerCode, td.CustomerName FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct td.CustomerCode, td.CustomerName FROM tbl_Master_Customer m  WHERE m.CustomerCode = td.CustomerCode And m.CustomerName = td.CustomerName)

	-----   Product Name --------------------
	INSERT INTO tbl_Master_Product (ProductCode, ProductName)	
	SELECT Distinct td.ProductCode, td.ProductName FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct td.ProductCode, td.ProductName FROM tbl_Master_Product m  WHERE m.ProductCode = td.ProductCode And m.ProductName = td.ProductName)

	-----   Pack Unit --------------------
	INSERT INTO tbl_Master_UOM (UOMCode, UOMName)	
	SELECT Distinct td.PackUnit, td.PackUnit FROM tbl_P3_SaleTransDetails_Uploaded as td
	WHERE NOT EXISTS
	(SELECT Distinct m.UOMCode FROM tbl_Master_UOM m  WHERE m.UOMCode = td.PackUnit)

--	SELECT  DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit,  Sum(SalesQTY)
--FROM     tbl_P3_SaleTransDetails_Uploaded
--WHERE  (SaleDate = '2019-04-09 00:00:00.000') 
--GROUP BY DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
--ORDER BY DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
END
GO
/****** Object:  StoredProcedure [dbo].[usp_02_Process_AggregateSaleData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 08-04-2020
-- Exec usp_02_Process_AggregateSaleData @YearNo=2020, @MonthNo=5


-- Exec usp_02_Process_AggregateSaleData @ForecastingType='Marketing'
-- =============================================
CREATE PROCEDURE [dbo].[usp_02_Process_AggregateSaleData]
	-- Add the parameters for the stored procedure here
	--@ForecastingType as varchar(50)
	@YearNo as Int=2020,
	@MonthNo as Int=0	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ForecastingType varchar(50)

    ------Start Insert Logistics Aggregate Value from Salrs Transaction --------
	Set @ForecastingType='Logistics'

	if (@ForecastingType='Logistics')
	BEGIN
		INSERT INTO tbl_P3_AGG_SaleTransaction (ForecastingType, SaleDate, ForYear, ForMonth,  DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit, TotalSalesQTY, TotalFreeSampleQTY, GrossAmount,
												TotalDiscountAmount, TotalTaxAmount, TotalNetAmount, ClosingStockQTY)
				  
		SELECT  @ForecastingType, SaleDate,Year(SaleDate), Month(SaleDate),  DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit,   Sum(Isnull(SalesQTY,0)), Sum(Isnull(FreeSampleQTY,0)), Sum(Isnull(GrossAmount,0)), Sum(Isnull(DiscountAmount,0)), 
					 Sum(TaxAmount), Sum(NetAmount), 0
		FROM     tbl_P3_PrimarySaleTransDetails_Uploaded as td 
		WHERE td.IsProcessed=0
		And NOT EXISTS
			(
				SELECT PK_AGG_SaleID FROM tbl_P3_AGG_SaleTransaction a  
					WHERE a.ForecastingType=@ForecastingType And a.SaleDate=td.SaleDate And a.DivisionName=td.DivisionName And a.DepotName=td.DepotName And 
						  a.HQ=td.HQ And a.ProductCode=td.ProductCode And a.ProductName=td.ProductCode And a.PackUnit=td.PackUnit And td.IsProcessed=0
			)
		GROUP BY SaleDate,DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
		ORDER BY SaleDate, DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
	
		Update tbl_P3_PrimarySaleTransDetails_Uploaded Set IsProcessed=1 Where IsProcessed=0
	END
	------End Insert Aggregate Value from Salrs Transaction --------


    ------Start Insert Marketing Aggregate Value from Salrs Transaction --------
	Set @ForecastingType='Marketing'

	if (@ForecastingType='Marketing')
	BEGIN
		INSERT INTO tbl_P3_AGG_SaleTransaction (ForecastingType, SaleDate, ForYear, ForMonth, DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit, TotalSalesQTY, TotalFreeSampleQTY, GrossAmount,
												TotalDiscountAmount, TotalTaxAmount, TotalNetAmount, ClosingStockQTY)
				  
		SELECT  @ForecastingType, GetDate(),ForYear, ForMonth, DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit,   Sum(Isnull(SalesQTY,0)),  Sum(Isnull(FreeSampleQTY,0)), 0, 0, 
					 0,0 , Sum(Isnull(ClosingStockQTY,0))
		FROM     tbl_P3_SecondarySaleTransDetails_Uploaded as td 
		WHERE td.IsProcessed=0 --And ForYear=@YearNo And  ForMonth=@MonthNo
		And NOT EXISTS
			(
				SELECT PK_AGG_SaleID FROM tbl_P3_AGG_SaleTransaction a  
					WHERE a.ForecastingType=@ForecastingType And a.ForYear=td.ForYear And a.ForMonth=td.ForMonth And a.DivisionName=td.DivisionName And a.DepotName=td.DepotName And 
						  a.HQ=td.HQ And a.ProductCode=td.ProductCode And a.ProductName=td.ProductCode And a.PackUnit=td.PackUnit And td.IsProcessed=0
			)
		GROUP BY ForYear,ForMonth, DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
		ORDER BY ForYear,ForMonth, DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
	
		Update tbl_P3_SecondarySaleTransDetails_Uploaded Set IsProcessed=1 Where IsProcessed=0
	END


	END
GO
/****** Object:  StoredProcedure [dbo].[usp_03_GenerateProjectionMonthlyData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 09-Apr-2020
-- Exec usp_03_GenerateProjectionMonthlyData @YearNo=2020,  @MonthNo=9, @IsReGenerated=1

--	1.	Logistics – Primary Sales.  Identify Weighted Average Sale [0.6 of last 3months' average + 0.4 of previous year current month]
--	2.	Marketing - Secondary Sales. 
--		•	HQ wise Secondary Sale Trend
--		•	Identify Average Sale
--		•	Capture Secondary Closing Stock
--		•	Calculate Secondary Projection = (Average Sale *2) Closing Stock
--Sales Team – Generate blank data set for sales team entry division wise  

-- ******** Need to Check Duplicate Data Issue for a Month and Year ****** 8-Apr-2020
-- =============================================
CREATE PROCEDURE [dbo].[usp_03_GenerateProjectionMonthlyData]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@IsReGenerated as bit =0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	Declare @ForecastingType as varchar(50)
	If (@IsReGenerated=1)
	Begin
		--Truncate Table tbl_P3_SaleProjection_Uploaded
		Delete tbl_P3_SaleProjection_Uploaded where ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo And ForecastingType='Logistics'
		Delete tbl_P3_SaleProjection_Uploaded where ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo And ForecastingType='Marketing'
	End
	
	Declare @PrevMonthLastDate as Date,
			@1stPreviousMonth as Date,
			@2ndPreviousMonth as Date,
			@3rdPreviousMonth as Date,
			@PrevYear as Int,
			@PrevMonth as Int,
			@NoofRows as Int=0
			
	
	Select @PrevMonthLastDate=dbo.udf_GetLastYearMonth(@YearNo, @MonthNo)
	Set @PrevYear =Year(@PrevMonthLastDate)
	Set @PrevMonth =Month(@PrevMonthLastDate)

	-- Previous Month : Month -1 
	Select @1stPreviousMonth=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -1)
	-- Previous Month : Month - 2 
	Select @2ndPreviousMonth=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -2)
	-- Previous Month : Month - 3
	Select @3rdPreviousMonth=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -3)
	
	Select  @NoofRows= Count(*) from tbl_P3_SaleProjection_Uploaded where ForecastingType='Logistics' And ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo 
	If (@NoofRows<=0)
	Begin
		Print 'Logistics'
		------ HQ wise Primary Sale Trend [last three month & previous year current month] 
		------ 1.	Logistics – Primary Sales.  Identify Weighted Average Sale [0.6 of last 3months' average + 0.4 of previous year current month]
		Select DivisionName, DepotName, HQ, ProductName,PackUnit,  Round((Sum(ISNULL(TotalSalesQTY,0) + ISNULL(TotalFreeSampleQTY,0))/3 *0.6),2)   as [AvgTotalSalesQTY]
		Into #tempPrevPrimaryTrans  
		From tbl_P3_AGG_SaleTransaction 	
		where ForecastingType='Logistics' 
		And (
			 ForYear=Year(@1stPreviousMonth) And ForMonth=Month(@1stPreviousMonth) 
			 Or ForYear=Year(@2ndPreviousMonth) And ForMonth=Month(@2ndPreviousMonth) 
			 Or ForYear=Year(@3rdPreviousMonth) And ForMonth=Month(@3rdPreviousMonth)
			 )
		--And DivisionName='PHOENIX' and DepotName='CTCK' And ProductName='AMIRID (TABLET)' and PackUnit='10'
		Group By DivisionName, DepotName, HQ, ProductName,PackUnit order by DivisionName, DepotName, HQ, ProductName,PackUnit

		---------- 1.	Logistics – Primary Sales.  0.4 of previous year current month]
		Select DivisionName, DepotName, HQ, ProductName,PackUnit, Round((Sum(ISNULL(TotalSalesQTY,0) + ISNULL(TotalFreeSampleQTY,0)) *0.4),2) as [LastYearTotalSalesQTY]
		Into #tempLastYearPrimaryTrans  
		From tbl_P3_AGG_SaleTransaction 	
		where ForecastingType='Logistics' 
		And (ForYear= @YearNo -1 And ForMonth= @MonthNo)
		--And DivisionName='PHOENIX' and DepotName='CTCK' And ProductName='AMIRID (TABLET)' and PackUnit='10'
		Group By DivisionName, DepotName, HQ, ProductName,PackUnit order by DivisionName, DepotName, HQ, ProductName,PackUnit

	-- Exec usp_03_GenerateProjectionMonthlyData @YearNo=2020,  @MonthNo=9, @IsReGenerated=1

		INSERT INTO tbl_P3_SaleProjection_Uploaded (ForecastingType,ProjectionDate, DivisionName, DepotName, HQ, ProductName, PackUnit, ProjectedTotalSalesQTY, ProjectionForMonth,
						ProjectionForYear )
		
		--https://community.spiceworks.com/topic/1689761-how-to-sum-union-d-queries

		Select 'Logistics' as [ForecastingType], GetDate() as [ProjectionDate], pp.DivisionName, pp.DepotName, pp.HQ,  pp.ProductName, pp.PackUnit, 
		IsNull(pp.AvgTotalSalesQTY,0)  as [LogisticsProjectionQTY], @MonthNo as [ProjectionMonth], @YearNo as [ProjectionYear]
		From #tempPrevPrimaryTrans as pp  

		Union all

		Select 'Logistics' as [ForecastingType], GetDate() as [ProjectionDate], lt.DivisionName, lt.DepotName, lt.HQ,  lt.ProductName, lt.PackUnit, 
		IsNull(lt.LastYearTotalSalesQTY,0)  as [LogisticsProjectionQTY], @MonthNo as [ProjectionMonth], @YearNo as [ProjectionYear]
		From #tempLastYearPrimaryTrans as lt 

	End

	Select  @NoofRows= Count(*) from tbl_P3_SaleProjection_Uploaded where ForecastingType='Marketing' And ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo 
	If (@NoofRows<=0)
	Begin
		Print 'Marketing'
		------Calculate Secondary Projection = (Last Month Secondary Sale * 2) - Last Month Closing Stock ------
		INSERT INTO tbl_P3_SaleProjection_Uploaded (ForecastingType,ProjectionDate, DivisionName, DepotName, HQ, ProductName, PackUnit, ProjectedTotalSalesQTY, ProjectionForMonth,
				ProjectionForYear )

		Select 'Marketing', GetDate(), MP.DivisionName, MP.DepotName, MP.HQ,  MP.ProductName, MP.PackUnit, 
		CASE WHEN MP.AvgTotalSalesQTY > 0 THEN MP.AvgTotalSalesQTY ELSE 0 END AS AvgTotalSalesQTY, @MonthNo, @YearNo
		From  	
			(
				Select DivisionName, DepotName, HQ,  ProductName, PackUnit , Sum(Isnull(TotalSalesQTY,0)* 2) - Sum(Isnull(ClosingStockQTY,0))   as [AvgTotalSalesQTY]
				From tbl_P3_AGG_SaleTransaction 
				where  ForecastingType='Marketing' And (ForYear=Year(@2ndPreviousMonth) And ForMonth=Month(@2ndPreviousMonth))
				Group By DivisionName, DepotName, HQ, ProductName,PackUnit
			) as MP  order by DivisionName, DepotName, HQ, ProductName,PackUnit

		--Select 'Marketing', GetDate(), DivisionName, DepotName, HQ,  ProductName,PackUnit,  Sum(Isnull(TotalSalesQTY,0)* 2) - Sum(Isnull(ClosingStockQTY,0))   as [AvgTotalSalesQTY],
		--@MonthNo, @YearNo
		--From tbl_P3_AGG_SaleTransaction 	
		--where  ForecastingType='Marketing' And (ForYear=Year(@2ndPreviousMonth) And ForMonth=Month(@2ndPreviousMonth))
		----And DivisionName='PHOENIX' and DepotName='CTCK' And ProductName='AMIRID (TABLET)' and PackUnit='10'
		--Group By DivisionName, DepotName, HQ, ProductName,PackUnit order by DivisionName, DepotName, HQ, ProductName,PackUnit
	End
	
	-- Exec usp_03_GenerateProjectionMonthlyData @YearNo=2020,  @MonthNo=9

END
GO
/****** Object:  StoredProcedure [dbo].[usp_03_GenerateProjectionMonthlyData_SalesTeam]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Author:		Sajal
-- Create date: 09-Apr-2020
-- Exec usp_03_GenerateProjectionMonthlyData_SalesTeam @YearNo=2021,  @MonthNo=6, @IsReGenerated=1

--Sales Team – Generate blank data set for sales team entry division wise  

-- ******** Need to Check Duplicate Data Issue for a Month and Year ****** 8-Apr-2020
-- =============================================
CREATE PROCEDURE [dbo].[usp_03_GenerateProjectionMonthlyData_SalesTeam]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@IsReGenerated as bit =0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	Declare @ForecastingType as varchar(50)
	If (@IsReGenerated=1)
	Begin
		--Truncate Table tbl_P3_SaleProjection_Uploaded
		Delete  tbl_P3_SaleProjection_SalesTeam where ForYear=@YearNo And ForMonth=@MonthNo And ForecastingType='SalesTeam'
	End
	
	Declare @PrevMonthLastDate as Date,
			@1stPreviousDate as Date,
			@2ndPreviousDate as Date,
			--@3rdPreviousDate as Date,
			@PrevYear as Int,
			@PrevMonth as Int,
			@NoofRows as Int=0
			
	
	Select @PrevMonthLastDate=dbo.udf_GetLastYearMonth(@YearNo, @MonthNo)
	Set @PrevYear =Year(@PrevMonthLastDate)
	Set @PrevMonth =Month(@PrevMonthLastDate)

	-- Previous Month : Month -1 
	Select @1stPreviousDate=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -1)
	-- Previous Month : Month - 2 
	Select @2ndPreviousDate=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -2)
	---- Previous Month : Month - 3
	--Select @3rdPreviousDate=dbo.udf_GetPreviousMonthDate(@YearNo, @MonthNo, -3)
	
	

	Select  @NoofRows= Count(*) from tbl_P3_SaleProjection_SalesTeam where ForecastingType='SalesTeam' And ForYear=@YearNo And ForMonth=@MonthNo 
	If (@NoofRows<=0)
	Begin

		Select c.DivisionName, c.DepotName,  c.ProductName, c.PackUnit,  Sum(Isnull(c.TotalSalesQTY,0))+ Sum(Isnull(c.TotalFreeSampleQTY,0)) as [TotalSalesQTY]
	   Into #tempSaleTransaction
	   From tbl_P3_AGG_SaleTransaction as c
	   Where c.ForecastingType='Logistics' And c.ForYear=Year(@1stPreviousDate)  And c.ForMonth=Month(@1stPreviousDate)
	   Group By c.DivisionName, c.DepotName,  c.ProductName, c.PackUnit
	   Order By c.DivisionName, c.DepotName,  c.ProductName, c.PackUnit

		Print 'SalesTeam @1stPreviousDate : ' + str(Year(@1stPreviousDate)) + '   Month : ' + str(Month(@1stPreviousDate))  --+ '   Date : ' + str(@1stPreviousDate) 
		Print @1stPreviousDate

		--- Call SP to Fetch Last Month Closing Stock 
		Exec getclosingstock @dt = @1stPreviousDate

		Print 'SalesTeam @@2ndPreviousDate : ' + str(Year(@2ndPreviousDate)) + '   Month : ' + str(Month(@2ndPreviousDate)) 
		Print @2ndPreviousDate

	   Select c.DivisionName as [DivisionName], c.DepotName as[DepotName],  c.ProductName, c.PackUnit as [PackUnit],  Sum(Isnull(c.ClosingStockQTY,0)) as [ClosingStockQTY]
	   Into #tempClosingStock
	   From tbl_P3_SecondarySaleTransDetails_Uploaded as c
	   Where c.ForYear=Year(@2ndPreviousDate)  And c.ForMonth=Month(@2ndPreviousDate)
	   Group By c.DivisionName, c.DepotName,  c.ProductName, c.PackUnit
	   Order By c.DivisionName, c.DepotName,  c.ProductName, c.PackUnit

	   --Select * from #tempSaleTransaction
	   --Select * from #tempClosingStock
		INSERT INTO tbl_P3_SaleProjection_SalesTeam (ForecastingType, ProjectionDate, ForMonth, ForYear, DivisionName, DepotName, ProductName, PackUnit, 
		PrimaryTotalSalesQTY, TotalClosingStockQTY, ProjectedTotalSalesQTY)

	   	Select 'SalesTeam' as [ForecastingType], GetDate() as [ProjectionDate],  @MonthNo as [ForMonth], @YearNo as [ForYear],  p.DivisionName, p.DepotName,  
		p.ProductName, p.PackUnit, Isnull(s.TotalSalesQTY,0) as [PrimarySalesQTY], Isnull(c.ClosingStockQTY,0) as [ClosingStockQTY],   0 as [ProjectionQTY]
		From tbl_P3_Master_Divisionwise_Product as p
		Left Outer Join #tempClosingStock as c On p.DivisionName= c.DivisionName And p.DepotName=c.DepotName  And p.ProductName=c.ProductName And p.PackUnit = c.PackUnit
		Left Outer Join #tempSaleTransaction as s On p.DivisionName= s.DivisionName And p.DepotName=s.DepotName  And p.ProductName=s.ProductName And p.PackUnit = s.PackUnit
		--Group By p.DivisionName, p.DepotName,  p.ProductName, p.PackUnit
		order by p.DivisionName, p.DepotName,  p.ProductName, p.PackUnit
	End

END
GO
/****** Object:  StoredProcedure [dbo].[usp_04_DownloadExcelProjectionMonthlyData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 09-Apr-2020
-- Exec usp_04_DownloadExcelProjectionMonthlyData @ForecastingType='Logistics', @YearNo=2020, @MonthNo=5,  @IsShowDummyProjectedQTY=1
-- Exec usp_04_DownloadExcelProjectionMonthlyData @ForecastingType='Marketing', @YearNo=2020, @MonthNo=5,  @IsShowDummyProjectedQTY=1


-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[usp_04_DownloadExcelProjectionMonthlyData]
	-- Add the parameters for the stored procedure here
	@ForecastingType as varchar(50),
	@YearNo as Int=2020,
	@MonthNo as Int=0,	
	@IsShowDummyProjectedQTY as bit=0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @MonthName as varchar(50)
	
	SELECT @MonthName= MonthName FROM tbl_Master_Month Where PK_MonthID=@MonthNo

	--Select @MonthName=   DATENAME(month, DATEADD(month, 1, GETDATE()))-- dbo.ufn_MonthName(@ProjectionMonth_mmddyyyy)--+ Ltrim(RTrim(Year(GETDATE()))

	Declare @ColumnName as varchar(100)
	Set @ColumnName ='[ProjectedQTYFor-'+ @MonthName +'-'+ Ltrim(RTrim(str(@YearNo))) +'-'+ @ForecastingType+']'

	DECLARE @SQL nvarchar(max)
	Set @SQL=''
	Set @SQL += 'SELECT  PK_ProjectedSalesID as [ID], ForecastingType as [Type],  HQ as [HQ],  DepotName as [Depot], DivisionName as [Division], ProductCode as [ProductCode], ProductName as [ProductName], PackUnit as [PackUnit], ' 
	Set @SQL += ' CASE WHEN '+ str(@IsShowDummyProjectedQTY) +'=1 THEN ProjectedAproxQTY  ELSE ProjectedTotalSalesQTY END as ' + @ColumnName + ''
	Set @SQL += ' FROM  tbl_P3_SaleProjection_Uploaded Where ForecastingType= '''+ @ForecastingType +''' And ProjectionForYear= '+ Str(@YearNo) +' And ProjectionForMonth= ' + Str(@MonthNo)
	Set @SQL += ' Order by HQ, DepotName, DivisionName, ProductName'

	Print @sql
	EXEC sp_sqlexec  @sql

	-- Exec usp_05_DownloadExcelProjectionMonthlyData @ProjectionMonth_mmddyyyy='05/01/2020', @IsShowDummyProjectedQTY=1

END
GO
/****** Object:  StoredProcedure [dbo].[usp_05_GenerateSalesForecasting]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 9-Apr-2020
-- Exec usp_05_GenerateSalesForecasting  @YearNo=2020,@MonthNo=9 , @CreatedByID=1, @IsShowDummyProjectedQTY=0

-- Exec usp_05_GenerateSalesForecasting @YearNo=2020,@MonthNo=7 , @CreatedByID=1, @IsShowDummyProjectedQTY=0

-- =============================================
CREATE PROCEDURE [dbo].[usp_05_GenerateSalesForecasting]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=0,	
	@CreatedByID as Int=1,
	@IsShowDummyProjectedQTY as bit=0
	--@IsRegenerateForecasting as bit=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	Declare @MonthName as varchar(50),
			@IsRegenerateForecasting as bit ,
			@ForecastingType as varchar(50)

	--Select @MonthName=   DATENAME(month, DATEADD(month, 1, GETDATE()))-- dbo.ufn_MonthName(@ProjectionMonth_mmddyyyy)--+ Ltrim(RTrim(Year(GETDATE()))
	SELECT @MonthName= MonthName FROM tbl_Master_Month Where PK_MonthID=@MonthNo
	

	Declare @varProjected_SaleQTY as decimal(18, 2)=0,
			@varLastYear_SalesQTY as decimal(18, 2)=0,
			@varLastPrevious_SalesQTY as decimal(18, 2)=0,
			@varPreviousSalesQTY as decimal(18, 2)=0,
			@varNext_ProjectionSalesQTY as decimal(18, 2)=0,
			@ForecastingMonth_mmddyyyy as Datetime,
			@varFormula_Projected as decimal(18, 2)=0,
			@varFormula_PreviousMonth as decimal(18, 2)=0,
			@varFormula_LastYear  as decimal(18, 2)=0

	Declare @StartDate as varchar(20),
			@EndDate as varchar(20)
	Set @ForecastingMonth_mmddyyyy= DATEADD(month, 1, GETDATE())


	DECLARE @cnt INT = 0;

	WHILE @cnt <= 1
	BEGIN	
		if (@cnt=1)
			Set @ForecastingType='Logistics'
		Else
			Set @ForecastingType='Marketing'

		--SELECT  @varFormula_Projected =FormulaValue FROM  tbl_P3_Master_CalculationFormula Where ForecastingType= @ForecastingType And FormulaType='SalesForecasting' And Condition1='Projected'
		--SELECT  @varFormula_PreviousMonth =FormulaValue FROM  tbl_P3_Master_CalculationFormula Where  ForecastingType= @ForecastingType And FormulaType='SalesForecasting' And Condition1='Previous' And Condition2='LastPrevious'
		--SELECT  @varFormula_LastYear =FormulaValue FROM  tbl_P3_Master_CalculationFormula Where  ForecastingType= @ForecastingType And FormulaType='SalesForecasting' And Condition1='LastYear'
		
		--if (@YearNo=Year(GetDate()) And @MonthNo=Month(GetDate())+1)
		--Begin
		--	Set @IsRegenerateForecasting=-1
		--End
		Set @IsRegenerateForecasting=1

		if (@IsRegenerateForecasting)=1
		Begin
			Print 'Process Forecasting'
			Delete tbl_P3_SaleForecasting Where ForecastingForYear=@YearNo And ForecastingForMonth=@MonthNo And  ForecastingType= @ForecastingType 

			INSERT INTO tbl_P3_SaleForecasting (ForecastingType, CreatedDate, FK_CreatedByID, CurrentDate,Frequency, ForecastingDate, ForecastingForMonth, ForecastingForYear,
												DivisionName, DepotName,ProductName, PackUnit)
				  
			SELECT  Distinct @ForecastingType, Getdate(), @CreatedByID, GetDate(), 'Monthly', @ForecastingMonth_mmddyyyy, @MonthNo, @YearNo,
							 DivisionName, DepotName, ProductName, PackUnit
			FROM     tbl_P3_AGG_SaleTransaction as AG
			WHERE NOT EXISTS
				(
					SELECT Distinct  DivisionName, DepotName,ProductName, PackUnit, @MonthNo, @YearNo
					FROM tbl_P3_SaleForecasting spu  
					WHERE spu.ForecastingType= @ForecastingType And spu.ForecastingForYear=@YearNo And spu.ForecastingForMonth=@MonthNo And spu.DivisionName=AG.DivisionName 
					And spu.DepotName=AG.DepotName And  spu.ProductName=AG.ProductCode And spu.PackUnit=AG.PackUnit
				)
		--- Update Projected QTY ----
		UPDATE ut SET ut.Projected_SaleQTY = t.ProjectedQTY
		FROM tbl_P3_SaleForecasting AS ut
		INNER JOIN
			(
				SELECT CASE WHEN @IsShowDummyProjectedQTY=1 THEN a.ProjectedAproxQTY  ELSE a.ProjectedTotalSalesQTY END as [ProjectedQTY],
				ProjectionForYear, ProjectionForMonth, DivisionName, DepotName, HQ, CustomerCode, CustomerName,  ProductName, PackUnit
				FROM tbl_P3_SaleProjection_Uploaded as a
				WHERE a.ForecastingType= @ForecastingType And a.ProjectionForYear=@YearNo And a.ProjectionForMonth=@MonthNo
				--GROUP BY DivisionName, DepotName, HQ, ProductCode, ProductName, PackUnit
			) t
		ON 
		t.ProjectionForYear= ut.ForecastingForYear And t.ProjectionForMonth= ut.ForecastingForMonth And
		t.DivisionName=ut.DivisionName And t.DepotName=ut.DepotName And  t.HQ=ut.HQ  And  t.ProductName=ut.ProductName And  t.PackUnit=ut.PackUnit
		WHERE 
		ut.ForecastingType= @ForecastingType And ut.ForecastingForYear=@YearNo And ut.ForecastingForMonth=@MonthNo And
		t.DivisionName=ut.DivisionName And t.DepotName=ut.DepotName And  t.HQ=ut.HQ  And t.ProductName=ut.ProductName And  t.PackUnit=ut.PackUnit 
	
		End-- if (@IsRegenerateForecasting)=1
   SET @cnt = @cnt + 1;
  END;

	--Exec usp_06_GenerateSalesForecastingComparison @ForecastingMonth_mmddyyyy=@ForecastingMonth_mmddyyyy, @CreatedByID=@CreatedByID



END
GO
/****** Object:  StoredProcedure [dbo].[usp_06_GenerateSalesForecastingComparison]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Exec usp_06_GenerateSalesForecastingComparison @YearNo=2020, @MonthNo=9, @CreatedByID=1

-- =============================================
CREATE PROCEDURE [dbo].[usp_06_GenerateSalesForecastingComparison]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@CreatedByID as Int=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ForecastingType as varchar(50)
	Declare
			@MonthName as varchar(50),
			@IsRegenerateForecasting as bit 

	--Set @MonthNo=Month(@ForecastingMonth_mmddyyyy)
	--Set @YearNo=Year(@ForecastingMonth_mmddyyyy)
	--Select @MonthName= dbo.ufn_MonthName(@ForecastingMonth_mmddyyyy)
	Select @MonthName=   DATENAME(month, DATEADD(month, 1, GETDATE()))

	Delete tbl_P3_SaleForecastingComparison Where ForecastingForYear=@YearNo And  ForecastingForMonth= @MonthNo
	------------------------- Logistics Projection ---------------------

	select DivisionName, DepotName, ProductName, PackUnit, sum(ProjectedTotalSalesQTY) as [LogisticsQTY]	
	Into #tempLogistics
	from tbl_P3_SaleProjection_Uploaded  Where ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo and ForecastingType='Logistics'
	Group by DivisionName, DepotName, ProductName, PackUnit Order by DivisionName, DepotName, ProductName, PackUnit
	------------------------- Marketing Projection ---------------------
	select DivisionName, DepotName, ProductName, PackUnit, sum(ProjectedTotalSalesQTY) as [MarketingQTY]	
	Into #tempMarketing
	from tbl_P3_SaleProjection_Uploaded  Where ProjectionForYear=@YearNo And ProjectionForMonth=@MonthNo and ForecastingType='Marketing'
	Group by DivisionName, DepotName, ProductName, PackUnit Order by DivisionName, DepotName, ProductName, PackUnit

	------------------------- SalesTeam Projection ---------------------
	select DivisionName, DepotName, ProductName, PackUnit, sum(ProjectedTotalSalesQTY) as [SalesTeamQTY]	
	Into #tempSalesTeam
	from tbl_P3_SaleProjection_SalesTeam  Where ForYear=@YearNo And ForMonth=@MonthNo 
	Group by DivisionName, DepotName, ProductName, PackUnit Order by DivisionName, DepotName, ProductName, PackUnit

	--Select * from #tempLogistics
	-------------------Insert Block -------------------------------------------------
	Insert Into tbl_P3_SaleForecastingComparison
	(CreatedDate,FK_CreatedByID, ForecastingForYear,ForecastingForMonth, DivisionName, DepotName, ProductCode,  ProductName, PackUnit, Logistics_ProjectionSalesQTY, 
	Marketing_ProjectedSaleQTY, Sales_ProjectedSaleQTY, DifferencePersentage, NextMonth_ForecastingQTY, NextMonth_FinialForecastingQTY)
	select  GETDATE(), @CreatedByID, @YearNo, @MonthNo,	p.DivisionName, p.DepotName, p.ProductCode, p.ProductName ,p.PackUnit, Round(isnull(l.LogisticsQTY,0),0) as [LogisticsQTY]
	, Round(IsNull(m.MarketingQTY,0),0) as [MarketingQTY], ISNULL(s.SalesTeamQTY,0) as [SalesTeamQTY], 
	CASE WHEN (s.SalesTeamQTY) - (l.LogisticsQTY) > 0 
	THEN Round(((((s.SalesTeamQTY) - (l.LogisticsQTY))/s.SalesTeamQTY) * 100),0) ELSE 0 END as [DeviationPersentage],

	--FLOOR(ABS(ISNULL(((s.SalesTeamQTY) - (l.LogisticsQTY)) / NULLIF((l.LogisticsQTY),0),0)*100))  as [DeviationPersentage],
	ISNULL(s.SalesTeamQTY,0) as [NextMonth_ForecastingQTY], ISNULL(s.SalesTeamQTY,0) as [NextMonth_FinialForecastingQTY]

	From tbl_P3_Master_Divisionwise_Product as P
	Left Outer Join #tempLogistics as l ON L.DivisionName=p.DivisionName and l.DepotName=p.DepotName and l.ProductName=p.ProductName and l.PackUnit=p.PackUnit
	Left Outer Join #tempMarketing as m ON m.DivisionName=p.DivisionName and m.DepotName=p.DepotName and m.ProductName=p.ProductName and m.PackUnit=p.PackUnit
	Left Outer Join #tempSalesTeam as s ON s.DivisionName=p.DivisionName and s.DepotName=p.DepotName and s.ProductName=p.ProductName and s.PackUnit=p.PackUnit
	Order by p.DivisionName, p.DepotName, p.ProductName, p.PackUnit


	Update tbl_P3_SaleForecastingComparison Set IsAutoCalculate= IIf (NextMonth_ForecastingQTY<0, 1, 0) Where ForecastingForYear=@YearNo And  ForecastingForMonth= @MonthNo

	-- =============================================
-- Exec usp_06_GenerateSalesForecastingComparison @ForecastingMonth_mmddyyyy='05/01/2020', @CreatedByID=1, @IsShowDummyProjectedQTY=0

END 
GO
/****** Object:  StoredProcedure [dbo].[usp_06_GenerateSalesForecastingComparison_bak]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Exec usp_06_GenerateSalesForecastingComparison @YearNo=2020, @MonthNo=7, @CreatedByID=1

-- =============================================
Create PROCEDURE [dbo].[usp_06_GenerateSalesForecastingComparison_bak]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@CreatedByID as Int=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ForecastingType as varchar(50)
	Declare
			@MonthName as varchar(50),
			@IsRegenerateForecasting as bit 

	--Set @MonthNo=Month(@ForecastingMonth_mmddyyyy)
	--Set @YearNo=Year(@ForecastingMonth_mmddyyyy)
	--Select @MonthName= dbo.ufn_MonthName(@ForecastingMonth_mmddyyyy)
	Select @MonthName=   DATENAME(month, DATEADD(month, 1, GETDATE()))


	Delete tbl_P3_SaleForecastingComparison Where ForecastingForYear=@YearNo And  ForecastingForMonth= @MonthNo


	Insert Into tbl_P3_SaleForecastingComparison
	(CreatedDate,FK_CreatedByID,  ForecastingForMonth, ForecastingForYear, DivisionName, DepotName,  ProductCode, ProductName, PackUnit, Logistics_ProjectionSalesQTY, 
            Marketing_ProjectedSaleQTY, DifferencePersentage, NextMonth_ForecastingQTY)

	SELECT GETDATE(), @CreatedByID,
	SFL.ForecastingForMonth, SFL.ForecastingForYear, SFL.DivisionName, SFL.DepotName, SFL.ProductCode,  SFL.ProductName, SFL.PackUnit,
	Sum(SFL.Next_ProjectionSalesQTY) as [LogisticsQTY], Sum(SFM.Next_ProjectionSalesQTY) as [MarketingQTY],
	 ----Difference based on Logistics and Marketing ---
	FLOOR(ABS(ISNULL((Sum(SFL.Next_ProjectionSalesQTY) - Sum(SFM.Next_ProjectionSalesQTY)) / NULLIF(sum(SFM.Next_ProjectionSalesQTY),0),0)*100))  as [Difference%],
	---- Forecasting 10%  ---
	IIF(FLOOR(ABS(ISNULL((SUM(SFL.Next_ProjectionSalesQTY) - Sum(SFM.Next_ProjectionSalesQTY)) / NULLIF(Sum(SFM.Next_ProjectionSalesQTY),0),0)*100)) <10, 
		Floor((Sum(SFL.Next_ProjectionSalesQTY) + Sum(SFM.Next_ProjectionSalesQTY))/2) , 0) as [NextMonthCalculatedForecasting]
	
	FROM     tbl_P3_SaleForecasting as SFL 
	Left Outer Join tbl_P3_SaleForecasting as SFM ON  
	SFL.ForecastingForMonth= SFM.ForecastingForMonth And SFL.ForecastingForYear= SFM.ForecastingForYear And SFL.DivisionName=SFM.DivisionName
	And SFL.DepotName=SFM.DepotName And SFL.HQ= SFM.HQ  And SFL.ProductCode= SFM.ProductCode And  SFL.ProductName= SFM.ProductName And SFL.PackUnit=SFM.PackUnit
	Where 	SFL.ForecastingType='Logistics' And SFM.ForecastingType='Marketing' 
	And SFL.ForecastingForMonth=@MonthNo And SFM.ForecastingForMonth=@MonthNo
	And SFL.ForecastingForYear= @YearNo And SFM.ForecastingForYear= @YearNo
	group by SFL.ForecastingForMonth, SFL.ForecastingForYear, SFL.DivisionName, SFL.DepotName,  SFL.Customer, SFL.ProductCode,  SFL.ProductName, SFL.PackUnit
	order by  DepotName, ProductName


	Update tbl_P3_SaleForecastingComparison Set NextMonth_FinialForecastingQTY=NextMonth_ForecastingQTY,  IsAutoCalculate= IIf (NextMonth_ForecastingQTY>0, 1, 0)
	Where ForecastingForYear=@YearNo And  ForecastingForMonth= @MonthNo

	--,IsAutoCalculate, NextMonth_FinialForecastingQTY
	-- =============================================
-- Exec usp_06_GenerateSalesForecastingComparison @ForecastingMonth_mmddyyyy='05/01/2020', @CreatedByID=1, @IsShowDummyProjectedQTY=0

END 
GO
/****** Object:  StoredProcedure [dbo].[Usp_LOAD_SalesTransactionData]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Sajal
-- Create date: 06-04-2020
-- Description:	Load Daily Sales Data from Transaction System to P3 System
-- Date Format : DD/MM/YYYY
-- Exec Usp_LOAD_SalesTransactionData @StartDate='01/01/2020', @EndDate='02/01/2020'
-- =============================================
CREATE PROCEDURE [dbo].[Usp_LOAD_SalesTransactionData]
	-- Add the parameters for the stored procedure here
	@StartDate varchar(10) =null,
	@EndDate varchar(10) =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	
	SET NOCOUNT ON;

	-- Truncate Table tbl_P3_SaleTransDetails_Uploaded

	Insert Into tbl_P3_SaleTransDetails_Uploaded 
	(DivisionName, DepotName, SaleDate, CustomerCode, CustomerName, HQ, ProductName, PackUnit, ProductCode, SalesQTY, FreeSampleQTY, NetAmount, GrossAmount)
	EXEC  [103.253.125.131,5000].[MendineMaster].[dbo].[Transactionbilltaxableamtautomation] @startdate, @enddate

	
	--Insert Into tbl_P3_SaleTransDetails_Uploaded 
	--(DivisionName, DepotName, SaleDate, CustomerCode, CustomerName, HQ, ProductName, PackUnit, ProductCode, SalesQTY, FreeSampleQTY, NetAmount, GrossAmount)
	--Select * from salesdata


END
GO
/****** Object:  StoredProcedure [dbo].[usp_Production_01_TransferDepotStockFromEasyReport]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




-- =============================================
-- Exec usp_Production_01_TransferDepotStockFromEasyReport @StartDate='05/01/2020', @EndDateTime='05/31/2020'

-- =============================================
CREATE PROCEDURE [dbo].[usp_Production_01_TransferDepotStockFromEasyReport]
	-- Add the parameters for the stored procedure here
	@StartDate as DateTime,
	@EndDateTime as Datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Truncate Table closingstock
	Truncate Table tbl_P3_Production_DepotStock_Raw
    -- Insert statements for procedure here
	EXEC  [P3].[dbo].[getclosingstock] 

	Print 'Ok'
	Insert Into tbl_P3_Production_DepotStock_Raw
	(StockDate, DivisionName, DepotName,  ProductName, PackUnit, BatchNo, ProductGroup, ClosingStockQTY, IsProcessed)
	(
		SELECT GETDATE(),  DIVISION, DEPOT, PRODUCTNAME, PACK, BATCH, TYPE ,CLOSINGBALANCE, 0
		FROM     closingstock
	)
END


-- Select * from tbl_P3_Production_DepotStock_Raw
GO
/****** Object:  StoredProcedure [dbo].[usp_Production_02_Process_AggregateDepotStock]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 08-04-2020
-- Exec usp_Production_02_Process_AggregateDepotStock @YearNo=2020, @MonthNo=5, @CreatedByID=1

-- =============================================
CREATE PROCEDURE [dbo].[usp_Production_02_Process_AggregateDepotStock]
	-- Add the parameters for the stored procedure here
	--@ForecastingType as varchar(50)
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@CreatedByID as Int=1

	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ForecastingType varchar(50)

    ------Start Insert Logistics Aggregate Value from Salrs Transaction --------

		INSERT INTO tbl_P3_Production_DepotStock_AGG (CreatedDate, FK_CreatedByID, StockDate, ForMonth, ForYear, DivisionName, DepotName, ProductCode, ProductName,
												PackUnit, ClosingStockQTY)
--		
		SELECT GETDATE(),@CreatedByID,  StockDate, @MonthNo, @YearNo,  DivisionName, DepotName, p.ProductCode, td.ProductName, td.PackUnit, Sum(ClosingStockQTY)
		FROM     tbl_P3_Production_DepotStock_Raw as td  
		Left Join tbl_Master_Product as P ON p.ProductName=td.ProductName
		WHERE td.IsProcessed=0
		And NOT EXISTS
			(
				SELECT PK_DepotStockRawID FROM tbl_P3_Production_DepotStock_Raw a  
					WHERE  a.DivisionName=td.DivisionName And a.DepotName=td.DepotName And a.ProductName=td.ProductCode And a.PackUnit=td.PackUnit And td.IsProcessed=0
			)
		GROUP BY StockDate,DivisionName, DepotName, p.ProductCode, td.ProductName, td.PackUnit
		ORDER BY DivisionName, DepotName, ProductName
	
		Update tbl_P3_Production_DepotStock_Raw Set IsProcessed=1 Where IsProcessed=0

END
GO
/****** Object:  StoredProcedure [dbo].[usp_Production_03_TransferWIPStockFromEasyReport]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- usp_Production_03_TransferWIPStockFromEasyReport @StartDate='05/01/2020', @EndDateTime='05/31/2020'

-- WIP Stock Transfer from Easy Report Database to P3 Database
-- =============================================
CREATE PROCEDURE [dbo].[usp_Production_03_TransferWIPStockFromEasyReport]
	-- Add the parameters for the stored procedure here
	@StartDate as DateTime=GetDate,
	@EndDateTime as Datetime=GetDate
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Delete tbl_P3_Production_WIPStock_Raw where IsProcessed=0

	Insert Into tbl_P3_Production_WIPStock_Raw(StockDate, ProductName, BatchNo, WIPStock_QTY, IsProcessed)
    (
		--SELECT GetDate(), sd.CompanyID, sd.StockDate, sd.StockItemName, sd.GodownName, sd.BatchName, sd.Quantity, sd.UOM, sd.Rate, sd.Amount, st.StockGroup
		SELECT GetDate(), sd.StockItemName,sd.BatchName, sd.Quantity, 0
		FROM   [EasyReports3.6].dbo.TD_Txn_StockDetails as sd
		INNER JOIN [EasyReports3.6].dbo.TD_Mst_StockItem as st ON sd.CompanyID = st.CompanyID 
		AND sd.StockItemName = st.StockItemName
		where sd.CompanyID=2 and StockGroup like 'ready to fill' and GodownName like 'Un Approved Store'  and  sd.Quantity>0
		--and sd.StockDate='2020-06-30 00:00:00.000'
		--and sd.StockDate=@EndDateTime
	)
END

--	Select * From tbl_P3_Production_WIPStock_Raw

--Select top(20) * from [EasyReports3.6].dbo.TD_Txn_StockDetails
GO
/****** Object:  StoredProcedure [dbo].[usp_Production_04_Process_AggregateWIPStock]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 08-04-2020
-- Exec usp_Production_04_Process_AggregateWIPStock @YearNo=2020, @MonthNo=5, @CreatedByID=1

-- =============================================
CREATE PROCEDURE [dbo].[usp_Production_04_Process_AggregateWIPStock]
	-- Add the parameters for the stored procedure here
	--@ForecastingType as varchar(50)
	@YearNo as Int=2020,
	@MonthNo as Int=0,
	@CreatedByID as Int=1

	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @ForecastingType varchar(50)

    ------Start Insert Logistics Aggregate Value from Salrs Transaction --------

		INSERT INTO tbl_P3_Production_WIPStock_AGG (CreatedDate, FK_CreatedByID, StockDate, ForMonth, ForYear, ProductCode, ProductName, WIPStock_QTY)
		
		SELECT GETDATE(),@CreatedByID,  StockDate, @MonthNo, @YearNo, Isnull(P.ProductCode,'') as [ProductCode], td.ProductName, SUM(WIPStock_QTY)
		FROM tbl_P3_Production_WIPStock_Raw as td  
		Left Join tbl_Master_Product as P ON p.ProductName=td.ProductName
		WHERE td.IsProcessed=0
		--And NOT EXISTS
		--	(
		--		SELECT PK_WIPStockRawID FROM tbl_P3_Production_WIPStock_Raw a  
		--			WHERE  a.ProductName=td.ProductName And td.IsProcessed=0
		--	)
		GROUP BY StockDate, p.ProductCode, td.ProductName
		ORDER BY  ProductName
	
		Update tbl_P3_Production_WIPStock_Raw Set IsProcessed=1 Where IsProcessed=0

END
GO
/****** Object:  StoredProcedure [dbo].[usp_Production_05_ProcessProductionPlanning]    Script Date: 7/1/2021 10:40:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		Sajal
-- Create date: 08-04-2020
-- Exec usp_Production_05_ProcessProductionPlanning @YearNo=2020, @MonthNo=6

-- =============================================
CREATE PROCEDURE [dbo].[usp_Production_05_ProcessProductionPlanning]
	-- Add the parameters for the stored procedure here
	@YearNo as Int=2020,
	@MonthNo as Int=6,
	@CreatedByID as Int=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @MonthName as varchar(50),
		@IsRegenerate as bit,
		@ForecastingMonth_mmddyyyy as Datetime,
		@StartDate as varchar(20),
		@EndDate as varchar(20),

		@PreviousMonthDate as Date, @PreviousMonth as Int, @CurrentYear as Int, @PreviousYear as Int,
		@LastPreviousMonthDate as Date, @LastPreviousMonth as Int, @LastPreviousYear as Int,
		@NextMonthDate as Date, @NextMonth as Int, @NextYear as Int,
		@AfterNextMonthDate as Date, @AfterNextMonth as Int, @AfterNextYear as Int

	Set @ForecastingMonth_mmddyyyy =Cast(Ltrim(RTrim(Str(@MonthNo))) +'/' +'01'+'/'+Ltrim(RTrim(Str(@YearNo)))  as date)
	------ Previous Month based on Projection Planing Month ----- X-1
	Set @PreviousMonthDate =   DATEADD(MONTH, DATEDIFF(MONTH, 0, @ForecastingMonth_mmddyyyy) - 1, 0)
	Set @PreviousMonth=MONTH(@PreviousMonthDate)
	Set @PreviousYear=Year(@PreviousMonthDate)
	------ Last Previous Month based on Projection Planing Month ----- X-2
	Set @ForecastingMonth_mmddyyyy =Cast(Ltrim(RTrim(Str(@MonthNo))) +'/' +'01'+'/'+Ltrim(RTrim(Str(@YearNo)))  as date)
	Set @LastPreviousMonthDate =   DATEADD(MONTH, DATEDIFF(MONTH, 0, @ForecastingMonth_mmddyyyy) - 2, 0)
	Set @LastPreviousMonth=MONTH(@LastPreviousMonthDate)
	Set @LastPreviousYear=Year(@LastPreviousMonthDate)
	--------------------------------------------------
	------ Next Month based on Projection Planing Month ----- X-2	
	Set @NextMonthDate =   DATEADD(MONTH, DATEDIFF(MONTH, 0, @ForecastingMonth_mmddyyyy) + 0, 0)
	Set @NextMonth=MONTH(@NextMonthDate)
	Set @NextYear=Year(@NextMonthDate)
	------ After Next Month based on Projection Planing Month ----- X-2	
	Set @AfterNextMonthDate =   DATEADD(MONTH, DATEDIFF(MONTH, 0, @ForecastingMonth_mmddyyyy) + 1, 0)
	Set @AfterNextMonth=MONTH(@AfterNextMonthDate)
	Set @AfterNextYear=Year(@AfterNextMonthDate)
	Print  '@NextMonth: '+  Ltrim(RTrim(Str(@NextMonth))) 
	Print  'AfterNextMonth: ' + Ltrim(RTrim(Str(@AfterNextMonth))) 

	------- Delete All Records From OUTPUT  Product and SKU if any -----------
	Delete From tbl_P3_Production_Forecasting_Product Where ForYear=@YearNo And ForMonth=@MonthNo
	Delete From tbl_P3_Production_Forecasting_SKU Where ForYear=@YearNo And ForMonth=@MonthNo
	Delete From tbl_P3_Production_Forecasting_Temp Where ForYear=@YearNo And ForMonth=@MonthNo
	
	---------- Product Cursor --------------
	IF CURSOR_STATUS('global','CURSOR_ProductName')>=-1
	--IF Cursor_Status('LOCAL','CURSOR_ProductName')> 0 
	BEGIN 
		CLOSE CURSOR_ProductName 
		DEALLOCATE CURSOR_ProductName 
	END
	Declare @ProductName  as varchar(200),
			@BatchSize as Decimal(18,2)=0, @ProductType  as varchar(20), @ProductCategory  as varchar(20),
			@ProductUOM  as varchar(20)
	Declare CURSOR_ProductName CURSOR local for	

	Select Distinct ProductName, BatchSize, ProductType, ProductCategory, ProductUOM
			from tbl_Master_Product Where ProductCategory='FG'-- And ProductName='CARMOZYME'
	OPEN CURSOR_ProductName
	FETCH NEXT FROM CURSOR_ProductName INTO  @ProductName, @BatchSize, @ProductType, @ProductCategory , @ProductUOM					
	WHILE(@@fetch_status = 0)
	BEGIN
	Declare @ProductDepot_OpeningStock as Decimal(18,2)=0,
			@PreviousMonth_StockInHand as Decimal(18,2)=0,
			@PreviousMonth_BoughtOutStock as Decimal(18,2)=0, -- BOP Product Type Stock
			@PreviousMonth_FactoryProductionTarget as Decimal(18,2)=0,
			@PreviousMonth_ProjectedSalesQTY as Decimal(18,2)=0,
			@PreviousMonth_ClosingStock as Decimal(18,2)=0,
			@PreviousMonth_StockInTransit as Decimal(18,2)=0,
			@PreviousMonth_ForecastedStock as Decimal(18,2)=0,
			@LastYearPreviousMonth_ActualSales as Decimal(18,2)=0,
			@ForecastedStockValidated as Decimal(18,2)=0,
			@ForecastedQuantityAll as Decimal(18,2)=0,
			@PhysicianSampleQTY_NextMonth as Decimal(18,2)=0,
			@PhysicianSampleQTY_AfterNextMonth as Decimal(18,2)=0,
			@PhysicianSamplePreviousMonth_ClosingStock as Decimal(18,2)=0,
			@PhysicianSample_TotalProjectionSales as Decimal(18,2)=0,
			@PhysicianSample_ForecastedStock as Decimal(18,2)=0,
			@PhysicianSample_ForecastedQuantity as Decimal(18,2)=0,
			@PhysicianSample_PackSize as Decimal(18,2)=0,
			@PhysicianSample_ProductionVolume as Decimal(18,2)=0,
			@TotalProductionVolume as Decimal(18,2)=0,
			@FinalForecastedProductionVolume  as Decimal(18,2)=0,
			@FinalChargeableVolume_InLtr  as Decimal(18,2)=0,
			@WIPInLtr  as Decimal(18,2)=0,
			@ActualChargeableVolume as Decimal(18,2)=0,
			@ModProductionVolume as Decimal(18,2)=0,
			@UnitFactor  as Decimal(18,2)=0,
			@ProductQuantityIP as Decimal(18,2)=0

			 Print  CHAR(13) +' Type : '+ @ProductType+ '  Product Name : '+  @ProductName +' Batch Size:'+ Ltrim(RTrim(Str(@BatchSize))) +' - Previous Year: '+ Ltrim(RTrim(Str(@PreviousYear))) +' '+'  Previous Month: '+ Ltrim(RTrim(Str(@PreviousMonth)))
												   +' - Next Year: '+ Ltrim(RTrim(Str(@NextYear))) +' '+'  Next Month: '+ Ltrim(RTrim(Str(@NextMonth)))
												   +' - AfterNext Year: '+ Ltrim(RTrim(Str(@AfterNextYear))) +' '+'  AfterNext Month: '+ Ltrim(RTrim(Str(@AfterNextMonth)))

	------------ SKU  Cursor --------------
			IF Cursor_Status('LOCAL','CURSOR_SKU')> 0 
			BEGIN 
				CLOSE CURSOR_SKU 
				DEALLOCATE CURSOR_SKU 
			END
			Declare @ProductCode as varchar(20), @SKUName  as varchar(200), @PackUnit as Int, @FactorValue as Decimal(18,2) 
			Declare CURSOR_SKU CURSOR local for	

			SELECT p.ProductCode, p.ProductName, P.PackUnit, p.FactorValue FROM  tbl_Master_Product as p Where  p.ProductName=@ProductName  Order by p.PackUnit
			OPEN CURSOR_SKU

			FETCH NEXT FROM CURSOR_SKU INTO  @ProductCode, @SKUName, @PackUnit, @FactorValue					
			WHILE(@@fetch_status = 0)
			BEGIN
				--Print  '		SKU Name : '+ @SKUName +'-'+ Str(@PackUnit)
				
				--Computation of “Closing Stock” for X – 1 month --- Depot Opening Stock for Previous Month Closing Stock		 
				Print  '	 Start: 1. Product Code : '+  @ProductCode +'  SKU NAME: '+ @SKUName +'  Pack Unit: '+ Ltrim(RTrim(Str(@PackUnit))) 

				SELECT @ProductDepot_OpeningStock += Isnull(Sum(ClosingStockQTY),0) FROM  tbl_P3_Production_DepotStock_AGG 
				Where ForMonth=@PreviousMonth And ForYear=@YearNo and ProductName=@SKUName And PackUnit=@PackUnit And IsProcessed=0
				Print  '			2. ProductDepot OpeningStock : -> '  + Ltrim(RTrim(Str(@ProductDepot_OpeningStock)))

				-- BOP Product Type Opening Stock for Previous Month Closing Stock
				IF (@ProductType='BOP')
				Begin
					SELECT @PreviousMonth_BoughtOutStock= IsNull(Sum(ClosingStockQTY),0) FROM  tbl_P3_Production_DepotStock_AGG 
					Where ForMonth=@PreviousMonth And ForYear=@PreviousYear and ProductName=@SKUName And PackUnit=@PackUnit And IsProcessed=0 
				End
				Print  '			3. PreviousMonth_BoughtOutStock  -> '  + Ltrim(RTrim(Str(@PreviousMonth_BoughtOutStock)))

				-- Previous Month Factory Production Target of (x-1)
				SELECT @PreviousMonth_FactoryProductionTarget=Isnull(Sum(FinalUnits_QTY),0)
				FROM tbl_P3_Production_FactoryProductionTarget_AGG
				Where ForMonth=@PreviousYear And ForMonth=@PreviousMonth And ProductName=@SKUName And PackUnit=@PackUnit
				Print  '			4. PreviousMonth FactoryProductionTarget  -> '  + Ltrim(RTrim(Str(@PreviousMonth_FactoryProductionTarget)))

				--Stock in Hand = Bought out Stock + Factory Production Target of (x-1)
				Set @PreviousMonth_StockInHand = @PreviousMonth_BoughtOutStock + @PreviousMonth_FactoryProductionTarget
				Print  '			5. PreviousMonth_StockInHand  : -> '  + Ltrim(RTrim(Str(@PreviousMonth_StockInHand)))

				--Projected Sales for (x-1) month
				SELECT @PreviousMonth_ProjectedSalesQTY= Isnull(Sum(NextMonth_FinialForecastingQTY),0) FROM tbl_P3_SaleForecastingComparison
				Where ForecastingForYear=@PreviousYear And ForecastingForMonth=@PreviousMonth and ProductName=@SKUName  And PackUnit=@PackUnit

				 Print  '			6. @PreviousMonth_ProjectedSalesQTY : -> '  + Ltrim(RTrim(Str(@PreviousMonth_ProjectedSalesQTY)))
				---- --𝐶𝑙𝑜𝑠𝑖𝑛𝑔𝑆𝑡𝑜𝑐𝑘 (X – 1 month) = [(𝑂𝑝𝑒𝑛𝑖𝑛𝑔𝑆𝑡𝑜𝑐𝑘 + Stock in Hand) – Projected Sales] (For X – 1 month)
				 Set @PreviousMonth_ClosingStock= ((@ProductDepot_OpeningStock + @PreviousMonth_StockInHand) - @PreviousMonth_ProjectedSalesQTY)

				 Print  '			7. PreviousMonth_ClosingStock :  -> '  + Ltrim(RTrim(Str(@PreviousMonth_ClosingStock)))
		
				----2nd Start :  Computation of “Final Forecasted Production Volume” for In-house Products (IP) to produce (in Litre) for X month:
				
				 If (@ProductType='IP')
				 Begin
					SELECT @PreviousMonth_StockInTransit=ISNULL(Sum(FinalChargeableVolume_InLtr), 0)
					FROM     tbl_P3_Production_Forecasting_SKU
					Where ForYear=@PreviousYear And ForMonth=@PreviousYear And ProductName=@SKUName And PackUnit=@PackUnit
					 --SELECT @PreviousMonth_StockInTransit= Isnull(Sum(WIPStock_QTY),0) FROM tbl_P3_Production_WIPStock_AGG
					 --where ForYear=@PreviousYear And ForMonth=@PreviousYear and IsProcessed=0 And ProductName=@SKUName
				 End
				  Print  '??? QUERY 	8. PreviousMonth_StockInTransit :  -> '  + Ltrim(RTrim(Str(@PreviousMonth_StockInTransit)))

				 --Forecasted Stock = (Factor * Projected Sales) – (Stock in Transit + Closing Stock)
				 Set @PreviousMonth_ForecastedStock = (@FactorValue * @PreviousMonth_ProjectedSalesQTY) - (@PreviousMonth_StockInTransit + @PreviousMonth_ClosingStock)
				 Print  '			9. ForecastedStock :  -> '  + Ltrim(RTrim(Str(@PreviousMonth_ForecastedStock)))

				 --Assumed Sales (X month) = Actual Sales (X month) of Last Year 		 
				 SELECT @LastYearPreviousMonth_ActualSales= IsNull(Sum(TotalSalesQTY),0) FROM tbl_P3_AGG_SaleTransaction
				 WHERE  (MONTH(SaleDate) = @PreviousMonth) AND (YEAR(SaleDate) = @PreviousYear-1) AND (ProductName = @SKUName) And PackUnit=@PackUnit
				 Print  '			10. LastYearPreviousMonth_ActualSales :  -> '  + Ltrim(RTrim(Str(@LastYearPreviousMonth_ActualSales)))

				 --Forecasted Stock (validated) = IF Forecasted Stock (X month) > Assumed Sales then “Forecasted Stock” otherwise “Assumed Sales” 
				 IF (@PreviousMonth_ForecastedStock > @LastYearPreviousMonth_ActualSales)
					Set @ForecastedStockValidated= @PreviousMonth_ForecastedStock
				 Else
					Set @ForecastedStockValidated= @LastYearPreviousMonth_ActualSales
				Print  '			11. ForecastedStockValidated :  -> '  + Ltrim(RTrim(Str(@ForecastedStockValidated)))
				--Forecasted Quantity (All) =∑_(k=0)〖(Forecasted Stock (validated))〗
				Set @ForecastedQuantityAll= @ForecastedStockValidated
				Print  '			12. ForecastedQuantityAll :  -> '  + Ltrim(RTrim(Str(@ForecastedQuantityAll)))

				Declare @ProductionVolumeIP as Decimal(18,2)

				--Production Volume (IP) = (Forecasted Quantity (All) * Pack Size) / 1000 (In Litre)
				if (@ProductType='IP')
				Begin
					Set @ProductionVolumeIP =IsNull((@ForecastedQuantityAll * @PackUnit) /1000,0)
				END
				Else
					Set @ProductionVolumeIP =0

				Print  '			13. ProductionVolumeIP :  -> '  + Ltrim(RTrim(Str(@ProductionVolumeIP)))
				Print  '			----------------------------------------------------------------------'
				--- SKU Wise Forecasting -----
				Print ' PackUnit :  -> '  + Ltrim(RTrim(Str(@PackUnit)))

				Insert Into tbl_P3_Production_Forecasting_Temp( ForMonth, ForYear, ProductCode, ProductName, PackUnit, ProductionForecastQTY, FactorValue,
							FactorForecastQTY, ProductionForecastVolume_InLtr, ChargeableVolume_InLtr, BatchSize, FinalChargeableVolume_InLtr)
				Values
						( @MonthNo, @YearNo, @ProductCode, @ProductName, @PackUnit, @ForecastedQuantityAll, @FactorValue,
							(@ForecastedQuantityAll * @FactorValue) , @ProductionVolumeIP, @ProductionVolumeIP, @BatchSize, (((@ForecastedQuantityAll * @FactorValue) *@PackUnit)/1000) )

			FETCH NEXT FROM CURSOR_SKU INTO  @ProductCode, @SKUName, @PackUnit, @FactorValue
			END
			CLOSE CURSOR_SKU 
			DEALLOCATE CURSOR_SKU 
	--||||||||||||||||||||||| END OF SKU SECTION |||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

	--------Computation of “Final Forecasted Production Volume” for Physician Sample (PS) to produce (in Litre) for X month:
	SELECT @PhysicianSampleQTY_NextMonth= IsNull(Sum(PhysicianSampleQTY),0) FROM tbl_P3_Production_PhysicianSample_AGG
	Where ForMonth=@NextMonth And ForYear=@NextYear And ProductName=@SKUName --And IsProcessed=0

	SELECT @PhysicianSampleQTY_AfterNextMonth= IsNull(Sum(PhysicianSampleQTY),0) FROM tbl_P3_Production_PhysicianSample_AGG
	Where ForMonth=@AfterNextMonth And ForYear=@AfterNextYear And ProductName=@SKUName --And IsProcessed=0
	Print  CHAR(13) +'===============  Production Volumn Calcution of '+ @SKUName +'==================='
	--Total Projection Sales (PS) = Projection for Month 1 + Projection for Month 2 + Projection for Month 3
	Set @PhysicianSample_TotalProjectionSales =@PhysicianSampleQTY_NextMonth +  @PhysicianSampleQTY_AfterNextMonth
	Print  'PS	    	14. PhysicianSample_TotalProjectionSales :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_TotalProjectionSales)))	

	-- Forecasted Stock (PS) = Total Projection Sales (PS) - Closing Stock (X – 1 month) (X month)	
	SELECT @PhysicianSamplePreviousMonth_ClosingStock= IsNull(Sum(PhysicianSampleQTY),0) FROM tbl_P3_Production_PhysicianSample_AGG
	Where ForMonth=@PreviousYear And ForYear=@PreviousMonth And ProductName=@SKUName --And IsProcessed=0

	Set @PhysicianSample_ForecastedStock= @PhysicianSample_TotalProjectionSales - @PhysicianSamplePreviousMonth_ClosingStock
	Print  'PS	    	15. Forecasted Stock (PS)  :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_ForecastedStock)))	

	------Forecasted Quantity (PS) =∑_(k=0)^n▒〖(Forecasted Stock (PS))〗[*where ‘n’ is the number of Depot(s)] 
	Set @PhysicianSample_ForecastedQuantity = @PhysicianSample_ForecastedStock
	Print  'PS	    	16. Forecasted Quantity (PS)  :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_ForecastedQuantity)))

	--Pack Size (PS) = Pack Size of respective Physician Sample
	
	SELECT @PhysicianSample_PackSize= PackUnit FROM tbl_P3_Production_PhysicianSample_AGG
	Where ForMonth=@AfterNextMonth And ForYear=@AfterNextYear And ProductName=@SKUName --And IsProcessed=0
	Print  'PS	    	17. Pack Size (PS) :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_PackSize)))

	--Production Volume (PS) = (Forecasted Quantity (PS) * Pack Size (PS) / 1000 (In Litre)
	Set @PhysicianSample_ProductionVolume = IsNull((@PhysicianSample_ForecastedQuantity * @PhysicianSample_PackSize)/100,0)
	Print  'PS	    	18. Production Volume (PS) :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_ProductionVolume)))

	--- Physician Sample SKU Wise Forecasting -----
	--Print ' PhysicianSample PackSize :  -> '  + Ltrim(RTrim(Str(@PhysicianSample_PackSize)))
	If(@PhysicianSample_PackSize>0)
	Begin
		Insert Into tbl_P3_Production_Forecasting_Temp( ForMonth, ForYear, ProductCode, ProductName, PackUnit, ProductionForecastQTY, FactorValue,
					FactorForecastQTY, ProductionForecastVolume_InLtr, ChargeableVolume_InLtr, BatchSize, FinalChargeableVolume_InLtr)
		Values
				( @MonthNo, @YearNo, '', @ProductName, @PhysicianSample_PackSize, @PhysicianSample_ForecastedQuantity, @FactorValue,
				(@PhysicianSample_ForecastedQuantity * @FactorValue) , @PhysicianSample_ProductionVolume, @PhysicianSample_ProductionVolume, @BatchSize,
				(((@ForecastedQuantityAll * @FactorValue) *@PackUnit)/1000))
	End
	--Calculation for Total Forecasted Volume (In Litre)
	--Total Production Volume = Production Volume (IP) + Production Volume (PS) (In Litre)
	Set @TotalProductionVolume=  @PhysicianSample_ProductionVolume + @ProductionVolumeIP
	Print  '			19. Total Production Volume :  -> '  + Ltrim(RTrim(Str(@TotalProductionVolume)))

	-- WIP = Work In Progress (In Litre)
	SELECT @WIPInLtr= Sum(WIPStock_QTY) FROM tbl_P3_Production_WIPStock_AGG
	Where ForMonth=@PreviousMonth And ForYear=@PreviousYear And ProductName=@ProductName

	--Final Forecasted Production Volume = Total Production Volume – WIP (In Litre)
	Set @FinalForecastedProductionVolume = @ProductionVolumeIP - @WIPInLtr 	
	Print  '			20. Final Forecasted Production Volume :  -> '  + Ltrim(RTrim(Str(@FinalForecastedProductionVolume)))

	--Actual Chargeable Volume = Final Forecasted Production Volume.		
	--IF MOD (Actual Chargeable Volume, Batch Size) = 0
	If (@BatchSize >0)
	Begin
		Select @ModProductionVolume= @FinalForecastedProductionVolume - @BatchSize *(FLOOR(@FinalForecastedProductionVolume/@BatchSize));
		Set @FinalChargeableVolume_InLtr= @FinalForecastedProductionVolume + @ModProductionVolume
		Set @ActualChargeableVolume= @FinalForecastedProductionVolume		
	End
	Print  '			21. Mod Volume :  -> '  + Ltrim(RTrim(Str(@ModProductionVolume)))

	--------- Insert Value PRODUCT Wise ------------------------
	Insert Into tbl_P3_Production_Forecasting_Product( CreatedDate, FK_CreatedByID, ForMonth, ForYear, ProductType, ProductCategory, ProductName,
								FactorValue, Volumn, WIPQTY, ChargeableVolume_InLtr, BatchSize,FinalChargeableVolume_InLtr)
    Values
	( GetDate(), @CreatedByID, @MonthNo, @YearNo, @ProductType, @ProductCategory, @ProductName,
								ISNULL(@FactorValue,0), ISNULL(@TotalProductionVolume,0), ISNULL(@WIPInLtr,0), ISNULL(@FinalForecastedProductionVolume,0), 
								ISNULL(@BatchSize,0), ISNULL(@FinalChargeableVolume_InLtr,0))
	-----------------------------------------------------------------
	----- LOOP For SKU Product for Forcasting Pack Size -------
		------------ SKU  Cursor --------------
		IF Cursor_Status('LOCAL','CURSOR_SKUPack')> 0 
		BEGIN 
			CLOSE CURSOR_SKUPack 
			DEALLOCATE CURSOR_SKUPack 
		END
		Declare @SKUPack_ProductCode as varchar(20), @SKU_Pack_PackUnit as Decimal(18,2)=0, @SKU_ProductionForecastQTY as Decimal(18,2)=0,@SKU_FactorValue as Decimal(18,2)=0,
				@SKU_FactorForecastQTY as Decimal(18,2)=0, @SKU_ProductionForecastVolume_InLtr as Decimal(18,2)=0, @SKU_ChargeableVolume_InLtr as Decimal(18,2)=0,
				@SKU_FinalChargeableVolume_InLtr as Decimal(18,2)=0				
		Declare CURSOR_SKUPack CURSOR local for	
		SELECT ProductCode, PackUnit, ProductionForecastQTY, FactorValue, FactorForecastQTY, ProductionForecastVolume_InLtr, 
			   ChargeableVolume_InLtr, FinalChargeableVolume_InLtr
		FROM     tbl_P3_Production_Forecasting_Temp Where ForMonth=@MonthNo And ForYear=@YearNo And ProductName=@ProductName order by PackUnit
		OPEN CURSOR_SKUPack

		FETCH NEXT FROM CURSOR_SKUPack INTO  @SKUPack_ProductCode,  @SKU_Pack_PackUnit,@SKU_ProductionForecastQTY, @SKU_FactorValue, @SKU_FactorForecastQTY,
											@SKU_ProductionForecastVolume_InLtr, @SKU_ChargeableVolume_InLtr, @SKU_FinalChargeableVolume_InLtr	
		WHILE(@@fetch_status = 0)
		BEGIN
			------Unit Factor = Forecasted Production Volume / Volume (In Litre)
			IF (@ProductType='IP')			
				Set @UnitFactor= @FinalChargeableVolume_InLtr /1000 
			Else
				Set @UnitFactor=0
			Print  '			22. Unit Factor :  -> '  + Ltrim(RTrim(Str(@UnitFactor)))
			
			------Product Quantity (IP) = (Actual Chargeable Volume – Production Volume (PS) + WIP) * Unit Factor
			IF (@ProductType='IP')
				Set @ProductQuantityIP= IsNull((@ActualChargeableVolume - (@PhysicianSample_ProductionVolume + @WIPInLtr)) * @UnitFactor,0)
			Else
				Set @ProductQuantityIP=0
			Print  '			23. Product Quantity IP :  -> '  + Ltrim(RTrim(Str(@ProductQuantityIP)))

			---------- INSERT Data in SKU Level ---------------------
			Insert Into tbl_P3_Production_Forecasting_SKU (CreatedDate, FK_CreatedByID, ForMonth, ForYear, ProductType, ProductCategory, ProductCode, ProductName,
						PackUnit, ProductionForecastQTY, FactorValue, FactorForecastQTY, ProductionForecastVolume_InLtr, ChargeableVolume_InLtr,
						BatchSize, FinalChargeableVolume_InLtr)
			Values
			(
				GETDATE(), @CreatedByID, @MonthNo, @YearNo, @ProductType, @ProductCategory, @SKUPack_ProductCode, @ProductName,
				@SKU_Pack_PackUnit, @SKU_ProductionForecastQTY, @SKU_FactorValue, @SKU_FactorForecastQTY, @SKU_ProductionForecastVolume_InLtr, @SKU_ChargeableVolume_InLtr,
				@BatchSize, @SKU_FinalChargeableVolume_InLtr
			)

			--------------------------------------------------------------
			FETCH NEXT FROM CURSOR_SKUPack INTO  @SKUPack_ProductCode,  @SKU_Pack_PackUnit,@SKU_ProductionForecastQTY, @SKU_FactorValue, @SKU_FactorForecastQTY,
									@SKU_ProductionForecastVolume_InLtr, @SKU_ChargeableVolume_InLtr, @SKU_FinalChargeableVolume_InLtr		
		END
		CLOSE CURSOR_SKUPack 
		DEALLOCATE CURSOR_SKUPack 
		-------------------------------------

	FETCH NEXT FROM CURSOR_ProductName INTO  @ProductName, @BatchSize, @ProductType, @ProductCategory , @ProductUOM		
	END
	CLOSE CURSOR_ProductName 
	DEALLOCATE CURSOR_ProductName 

END
-- Exec usp_Production_05_ProcessProductionPlanning @YearNo=2020, @MonthNo=6

--Select *  From tbl_P3_Production_Forecasting_Product Where ForYear=2020 And ForMonth=6
--Select *  From tbl_P3_Production_Forecasting_SKU Where ForYear=2020 And ForMonth=6
--Select *  From tbl_P3_Production_Forecasting_Temp Where ForYear=2020 And ForMonth=6
GO
