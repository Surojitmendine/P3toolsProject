using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using API.Models;

namespace API.Context
{
    public partial class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnalyticalReport_PerformRange> AnalyticalReport_PerformRange { get; set; }
        public virtual DbSet<TblProdmast> TblProdmast { get; set; }
        public virtual DbSet<closingstock> closingstock { get; set; }
        public virtual DbSet<salesdata> salesdata { get; set; }
        public virtual DbSet<tbl_Master_Month> tbl_Master_Month { get; set; }
        public virtual DbSet<tbl_Master_Product> tbl_Master_Product { get; set; }
        //public virtual DbSet<tbl_Master_Product_Mapping> tbl_Master_Product_Mapping { get; set; }
        public virtual DbSet<tbl_Master_ProductCategory> tbl_Master_ProductCategory { get; set; }
        public virtual DbSet<tbl_P3_AGG_SaleTransaction> tbl_P3_AGG_SaleTransaction { get; set; }
        public virtual DbSet<tbl_P3_Master_CalculationFormula> tbl_P3_Master_CalculationFormula { get; set; }
        public virtual DbSet<tbl_P3_Master_DepotNameMapping> tbl_P3_Master_DepotNameMapping { get; set; }
        public virtual DbSet<tbl_P3_Master_Divisionwise_Product> tbl_P3_Master_Divisionwise_Product { get; set; }
        public virtual DbSet<tbl_P3_PrimarySaleTransDetails_Uploaded> tbl_P3_PrimarySaleTransDetails_Uploaded { get; set; }
        public virtual DbSet<tbl_P3_Production_DepotStock_AGG> tbl_P3_Production_DepotStock_AGG { get; set; }
        public virtual DbSet<tbl_P3_Production_DepotStock_Raw> tbl_P3_Production_DepotStock_Raw { get; set; }
        public virtual DbSet<tbl_P3_Production_FactoryProductionTarget_AGG> tbl_P3_Production_FactoryProductionTarget_AGG { get; set; }
        public virtual DbSet<tbl_P3_Production_Forecasting_Product> tbl_P3_Production_Forecasting_Product { get; set; }
        public virtual DbSet<tbl_P3_Production_Forecasting_SKU> tbl_P3_Production_Forecasting_SKU { get; set; }
        public virtual DbSet<tbl_P3_Production_Forecasting_Temp> tbl_P3_Production_Forecasting_Temp { get; set; }
        public virtual DbSet<tbl_P3_Production_PhysicianSample_AGG> tbl_P3_Production_PhysicianSample_AGG { get; set; }
        public virtual DbSet<tbl_P3_Production_WIPStock_AGG> tbl_P3_Production_WIPStock_AGG { get; set; }
        public virtual DbSet<tbl_P3_Production_WIPStock_Raw> tbl_P3_Production_WIPStock_Raw { get; set; }
        public virtual DbSet<tbl_P3_SaleForecasting> tbl_P3_SaleForecasting { get; set; }
        public virtual DbSet<tbl_P3_SaleForecastingComparison> tbl_P3_SaleForecastingComparison { get; set; }
        public virtual DbSet<tbl_P3_SaleForecasting_SwillClosingStock> tbl_P3_SaleForecasting_SwillClosingStock { get; set; }
        public virtual DbSet<tbl_P3_SaleProjection_SalesTeam> tbl_P3_SaleProjection_SalesTeam { get; set; }
        public virtual DbSet<tbl_P3_SaleProjection_Uploaded> tbl_P3_SaleProjection_Uploaded { get; set; }
        public virtual DbSet<tbl_P3_SecondarySaleTransDetails_ClosingStock_bak> tbl_P3_SecondarySaleTransDetails_ClosingStock_bak { get; set; }
        public virtual DbSet<tbl_P3_SecondarySaleTransDetails_Uploaded> tbl_P3_SecondarySaleTransDetails_Uploaded { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_RoleClaims> tbl_SYS_AspNet_RoleClaims { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_Roles> tbl_SYS_AspNet_Roles { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_UserClaims> tbl_SYS_AspNet_UserClaims { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_UserInformation> tbl_SYS_AspNet_UserInformation { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_UserLogins> tbl_SYS_AspNet_UserLogins { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_UserRoles> tbl_SYS_AspNet_UserRoles { get; set; }
        public virtual DbSet<tbl_SYS_AspNet_UserTokens> tbl_SYS_AspNet_UserTokens { get; set; }
        public virtual DbSet<tbl_SYS_Master_Menu> tbl_SYS_Master_Menu { get; set; }
        public virtual DbSet<tbl_SYS_UserPermission> tbl_SYS_UserPermission { get; set; }

        public virtual DbSet<factory_closing_stock> factory_closing_stock_Insert_Status { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AnalyticalReport_PerformRange>(entity =>
            {
                entity.HasKey(e => e.PK_PerformerID);

                entity.Property(e => e.PerformName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TypeName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TblProdmast>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.prodcode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.prodcuct_name).HasMaxLength(200);
            });

            modelBuilder.Entity<closingstock>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.BATCH)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DEPOT)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DIVISION)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EXPDATE)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MFGDATE)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NRV).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.PACK)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PRODUCTNAME)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.STK)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.STOCKVALUE).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.TYPE)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<salesdata>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Billamount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.Billdate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Customercode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Customername)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DIVISION)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Packsize)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Productname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StockLocation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TaxableAmt).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.productcode)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_Master_Month>(entity =>
            {
                entity.HasKey(e => e.PK_MonthID);

                entity.Property(e => e.MonthName)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ShortMonthName)
                    .HasMaxLength(5)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_Master_Product>(entity =>
            {
                entity.HasKey(e => e.PK_ProductID);

                entity.Property(e => e.BatchSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FactorValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NRVEffectiveRateFrom).HasColumnType("smalldatetime");

                entity.Property(e => e.NRVRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductUOM)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TallyProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TallyUOM)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FK_ProductCategory)
                    .WithMany(p => p.tbl_Master_Product)
                    .HasForeignKey(d => d.FK_ProductCategoryID)
                    .HasConstraintName("FK_tbl_Master_Product_tbl_Master_ProductCategory");
            });

            //modelBuilder.Entity<tbl_Master_Product_Mapping>(entity =>
            //{
            //    entity.HasKey(e => e.PK_ProductID);

                
            //    entity.Property(e => e.ProductCode)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.ProductName)
            //        .HasMaxLength(100)
            //        .IsUnicode(false);

            //    entity.Property(e => e.PackUnit)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.Property(e => e.Category)
            //        .HasMaxLength(50)
            //        .IsUnicode(false);

            //    entity.Property(e => e.TallyProductName)
            //        .HasMaxLength(100)
            //        .IsUnicode(false);

            //    entity.Property(e => e.TallyUOM)
            //        .HasMaxLength(10)
            //        .IsUnicode(false);

            //    entity.HasOne(d => d.FK_ProductCategory)
            //        .WithMany(p => p.tbl_Master_Product_Mapping)
            //        .HasForeignKey(d => d.FK_ProductCategoryID)
            //        .HasConstraintName("FK_tbl_Master_Product_tbl_Master_ProductCategory");
            //});

            modelBuilder.Entity<tbl_Master_ProductCategory>(entity =>
            {
                entity.HasKey(e => e.PK_ProductCategoryID);

                entity.Property(e => e.CategoryDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_AGG_SaleTransaction>(entity =>
            {
                entity.HasKey(e => e.PK_AGG_SaleID);

                entity.HasIndex(e => new { e.ForYear, e.ForMonth, e.DivisionName, e.DepotName, e.ProductName, e.PackUnit })
                    .HasName("ix_AGG_SaleTransaction");

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FK_ClientID).HasDefaultValueSql("((1))");

                entity.Property(e => e.ForecastingType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.GrossAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.TotalDiscountAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalFreeSampleQTY).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalNetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalTaxAmount).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<tbl_P3_Master_CalculationFormula>(entity =>
            {
                entity.HasKey(e => e.PK_ID);

                entity.Property(e => e.Condition1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Condition2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ForecastingType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FormulaType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FormulaValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Remarks)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_Master_DepotNameMapping>(entity =>
            {
                entity.HasKey(e => e.PK_DepotMapID);

                entity.Property(e => e.P3DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SwillDepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_Master_Divisionwise_Product>(entity =>
            {
                entity.HasKey(e => e.PK_ID);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_PrimarySaleTransDetails_Uploaded>(entity =>
            {
                entity.HasKey(e => e.PK_SalesTransactionID)
                    .HasName("PK_tbl_P3_SaleTransDetails_Uploaded");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DiscountAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FK_ClientID).HasDefaultValueSql("((1))");

                entity.Property(e => e.ForMonth).HasDefaultValueSql("((0))");

                entity.Property(e => e.ForYear).HasDefaultValueSql("((0))");

                entity.Property(e => e.FreeSampleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GrossAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.NetAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SaleDate).HasColumnType("datetime");

                entity.Property(e => e.SalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TaxAmount)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_Production_DepotStock_AGG>(entity =>
            {
                entity.HasKey(e => e.PK_AggDepotStockID)
                    .HasName("PK_tbl_P3_Production_AGG_DepotStock");

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StockDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_P3_Production_DepotStock_Raw>(entity =>
            {
                entity.HasKey(e => e.PK_DepotStockRawID)
                    .HasName("PK_tbl_P3_Production_DepotStock");

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductGroup)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StockDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<tbl_P3_Production_FactoryProductionTarget_AGG>(entity =>
            {
                entity.HasKey(e => e.PK_FactoryProductionTargetID)
                    .HasName("PK_tbl_P3_Production_FactoryProductionTargetAGG");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FactoryProductionTargetDate).HasColumnType("datetime");

                entity.Property(e => e.FinalUnits_QTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_Production_Forecasting_Product>(entity =>
            {
                entity.HasKey(e => e.PK_ProductionForecastProductID)
                    .HasName("PK_tbl_P3_Production_ForecastingProduct");

                entity.Property(e => e.BatchSize).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ChargeableVolume_InLtr).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FactorValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FinalChargeableVolume_InLtr).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Volumn).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.WIPQTY).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<tbl_P3_Production_Forecasting_SKU>(entity =>
            {
                entity.HasKey(e => e.PK_ProductionForecastSKUID)
                    .HasName("PK_tbl_P3_Production_Forecasting");

                entity.Property(e => e.BatchSize)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ChargeableVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FactorForecastQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FactorValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FinalChargeableVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCategory)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionForecastQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductionForecastVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_Production_Forecasting_Temp>(entity =>
            {
                entity.HasKey(e => e.PK_TempSKUID);

                entity.Property(e => e.BatchSize)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ChargeableVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FactorForecastQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FactorValue)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FinalChargeableVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductionForecastQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductionForecastVolume_InLtr)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WIPQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_Production_PhysicianSample_AGG>(entity =>
            {
                entity.HasKey(e => e.PK_PhysicianSampleID);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PhysicianSampleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_Production_WIPStock_AGG>(entity =>
            {
                entity.HasKey(e => e.PK_WIPStockAGGID);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StockDate).HasColumnType("datetime");

                entity.Property(e => e.WIPStock_QTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_Production_WIPStock_Raw>(entity =>
            {
                entity.HasKey(e => e.PK_WIPStockRawID);

                entity.Property(e => e.BatchNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StockDate).HasColumnType("datetime");

                entity.Property(e => e.WIPStock_QTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_SaleForecasting>(entity =>
            {
                entity.HasKey(e => e.PK_SaleForecastingID);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrentDate).HasColumnType("datetime");

                entity.Property(e => e.Current_SalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Customer)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ForecastingDate).HasColumnType("datetime");

                entity.Property(e => e.ForecastingType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Frequency)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NRVRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Next_ProjectionSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Projected_SaleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProjectionValue)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_SaleForecastingComparison>(entity =>
            {
                entity.HasKey(e => e.PK_SaleComparisonID);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DifferencePersentage)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsAutoCalculate).HasDefaultValueSql("((0))");

                entity.Property(e => e.Logistics_ProjectionSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Marketing_ProjectedSaleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NRVRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NextMonth_FinialForecastingQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NextMonth_ForecastingQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectionValue)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Sales_ProjectedSaleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_SaleForecasting_SwillClosingStock>(entity =>
            {
                entity.HasKey(e => e.PK_SwillID);

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ForMonth).HasDefaultValueSql("((0))");

                entity.Property(e => e.ForYear).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SyncDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<tbl_P3_SaleProjection_SalesTeam>(entity =>
            {
                entity.HasKey(e => e.PK_SalesTeamProjectionID);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ForYear).HasDefaultValueSql("((2020))");

                entity.Property(e => e.ForecastingType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsManual).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.NRVRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PrimaryTotalSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectedTotalSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProjectionDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectionValue)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TotalClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<tbl_P3_SaleProjection_Uploaded>(entity =>
            {
                entity.HasKey(e => e.PK_ProjectedSalesID)
                    .HasName("PK_tbl_P3_SaleProjection");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ForecastingType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NRVRate)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectedAproxQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProjectedTotalSalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProjectionDate).HasColumnType("datetime");

                entity.Property(e => e.ProjectionForYear).HasDefaultValueSql("((2020))");

                entity.Property(e => e.ProjectionValue)
                    .HasColumnType("decimal(18, 3)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SalesPersonCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SalesPersonName)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_SecondarySaleTransDetails_ClosingStock_bak>(entity =>
            {
                entity.HasKey(e => e.PK_SSClosingStockID)
                    .HasName("PK_tbl_P3_SecondarySaleTransDetails_ClosingStock");

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FK_ClientID).HasDefaultValueSql("((1))");

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UOM)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_P3_SecondarySaleTransDetails_Uploaded>(entity =>
            {
                entity.HasKey(e => e.PK_SalesTransactionID);

                entity.Property(e => e.ClosingStockQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DepotName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DivisionName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FK_ClientID).HasDefaultValueSql("((1))");

                entity.Property(e => e.FreeSampleQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HQ)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.IsProcessed).HasDefaultValueSql("((0))");

                entity.Property(e => e.PackUnit)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ProductCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ProductName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.SalesQTY)
                    .HasColumnType("decimal(18, 2)")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UOM)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<tbl_SYS_AspNet_RoleClaims>(entity =>
            {
                entity.HasOne(d => d.FK_Role)
                    .WithMany(p => p.tbl_SYS_AspNet_RoleClaims)
                    .HasForeignKey(d => d.FK_RoleID)
                    .HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_Roles>(entity =>
            {
                entity.HasKey(e => e.PK_RoleID)
                    .HasName("PK_AspNetRoles");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserClaims>(entity =>
            {
                entity.HasOne(d => d.FK_User)
                    .WithMany(p => p.tbl_SYS_AspNet_UserClaims)
                    .HasForeignKey(d => d.FK_UserID)
                    .HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserInformation>(entity =>
            {
                entity.HasKey(e => e.PK_UserID)
                    .HasName("PK_AspNetUsers");

                entity.Property(e => e.Device_UUID)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DisplayUserName)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FK_DateKey_CreatedOn).HasDefaultValueSql("((((rtrim(datepart(year,getdate()))+replicate('0',(2)-len(rtrim(datepart(month,getdate())))))+rtrim(datepart(month,getdate())))+replicate('0',(2)-len(rtrim(datepart(day,getdate())))))+rtrim(datepart(day,getdate())))");

                entity.Property(e => e.FK_TimeKey_CreatedOn).HasDefaultValueSql("((datepart(hour,getdate())*(3600)+datepart(minute,getdate())*(60))+datepart(second,getdate()))");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserImagePath)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                    .HasName("PK_AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.HasOne(d => d.FK_User)
                    .WithMany(p => p.tbl_SYS_AspNet_UserLogins)
                    .HasForeignKey(d => d.FK_UserID)
                    .HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserRoles>(entity =>
            {
                entity.HasKey(e => new { e.PK_UserID, e.PK_RoleID });

                entity.HasOne(d => d.PK_Role)
                    .WithMany(p => p.tbl_SYS_AspNet_UserRoles)
                    .HasForeignKey(d => d.PK_RoleID)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId");

                entity.HasOne(d => d.PK_User)
                    .WithMany(p => p.tbl_SYS_AspNet_UserRoles)
                    .HasForeignKey(d => d.PK_UserID)
                    .HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId");
            });

            modelBuilder.Entity<tbl_SYS_AspNet_UserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                    .HasName("PK_AspNetUserTokens");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.tbl_SYS_AspNet_UserTokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
            });

            modelBuilder.Entity<tbl_SYS_Master_Menu>(entity =>
            {
                entity.HasKey(e => e.PK_MenuID);

                entity.Property(e => e.PK_MenuID).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.MenuName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MenuType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("smalldatetime");

                entity.Property(e => e.ModuleName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PageRedirect)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RelatedSPName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RelatedTableName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.StoreProcedureName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.TablesName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ViewOrder).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.FK_MenuID_ParentNavigation)
                    .WithMany(p => p.InverseFK_MenuID_ParentNavigation)
                    .HasForeignKey(d => d.FK_MenuID_Parent)
                    .HasConstraintName("FK_tbl_SYS_Master_Menu_tbl_SYS_Master_Menu");
            });

            modelBuilder.Entity<tbl_SYS_UserPermission>(entity =>
            {
                entity.HasKey(e => e.PK_RoleAccessID);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("smalldatetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");

                entity.Property(e => e.IsShowMenuInGroup).HasDefaultValueSql("((1))");

                entity.Property(e => e.ModifiedDate).HasColumnType("smalldatetime");

                entity.Property(e => e.Permission_Del).HasDefaultValueSql("((0))");

                entity.Property(e => e.Permission_Edit).HasDefaultValueSql("((0))");

                entity.Property(e => e.Permission_Export).HasDefaultValueSql("((0))");

                entity.Property(e => e.Permission_Import).HasDefaultValueSql("((1))");

                entity.Property(e => e.Permission_New).HasDefaultValueSql("((1))");

                entity.Property(e => e.Permission_Others).HasDefaultValueSql("((1))");

                entity.Property(e => e.Permission_Print).HasDefaultValueSql("((0))");

                entity.Property(e => e.Permission_Report).HasDefaultValueSql("((1))");

                entity.Property(e => e.Permission_View).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.FK_Menu)
                    .WithMany(p => p.tbl_SYS_UserPermission)
                    .HasForeignKey(d => d.FK_MenuID)
                    .HasConstraintName("FK_tbl_SYS_UserPermission_tbl_SYS_AspNet_Roles");
            });



            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
