using AutoMapper;
using API.Entity;
using API.Helper;
using API.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using static API.Entity.CommonEntity;


public class AutomapperMappingProfile : Profile {
    public AutomapperMappingProfile() {

        //Common Dropdown
        //CreateMap<List<dynamic>, List<DropDown>>();

        Functions functions = new Functions();

        CreateMap<tbl_P3_SaleProjection_Uploaded, SalesForecastEntity.ImportProjection>()
            .ForMember(dest => dest.ID, opts => opts.MapFrom(src => src.PK_ProjectedSalesID))
            .ForMember(dest=>dest.Depot,opts=>opts.MapFrom(src=>src.DepotName))
            .ForMember(dest => dest.Division, opts => opts.MapFrom(src => src.DivisionName));

        CreateMap<SalesForecastEntity.ImportProjection,tbl_P3_SaleProjection_Uploaded>()
            .ForMember(dest => dest.PK_ProjectedSalesID, opts => opts.MapFrom(src => src.ID))
            .ForMember(dest => dest.DepotName, opts => opts.MapFrom(src => src.Depot))
            .ForMember(dest => dest.DivisionName, opts => opts.MapFrom(src => src.Division));

        CreateMap<tbl_P3_Production_PhysicianSample_AGG, ProductionPlan.ImportExcel_PhysicianSamplePlan>();
        CreateMap<ProductionPlan.ImportExcel_PhysicianSamplePlan, tbl_P3_Production_PhysicianSample_AGG>();

        //CreateMap<tbl_P3_Production_FactoryProductionTarget_AGG, ProductionPlan.ImportExcel_FactoryProductionTarget>();
        //CreateMap<ProductionPlan.ImportExcel_FactoryProductionTarget, tbl_P3_Production_FactoryProductionTarget_AGG>();

        CreateMap<tbl_P3_SecondarySaleTransDetails_Uploaded, SalesForecastEntity.Import_SecondarySales>();
        CreateMap<SalesForecastEntity.Import_SecondarySales, tbl_P3_SecondarySaleTransDetails_Uploaded>();

        CreateMap<tbl_P3_Master_Divisionwise_Product, MasterSetupEntity.Divisionwise_ProductEntity>();
        CreateMap<MasterSetupEntity.Divisionwise_ProductEntity, tbl_P3_Master_Divisionwise_Product>();

        CreateMap<tbl_P3_SaleProjection_SalesTeam, SalesForecastEntity.SaleProjection_SalesTeam>();
        CreateMap<SalesForecastEntity.SaleProjection_SalesTeam, tbl_P3_SaleProjection_SalesTeam>();

    }
}