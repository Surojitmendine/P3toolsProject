using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using API.BusinessLogic;
using API.Context;
using API.Helper;
using API.IdentityModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using static API.Entity.MasterSetupEntity;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MasterSetupController : ApiBase
    {
        public static CultureInfo enGBcultureFormat = new CultureInfo("en-GB");
        private readonly IMapper _mapper;
        private MasterSetupLogic masterSetupLogic;
        readonly UserManager<ApplicationUser> userManager;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly Functions functions = new Functions();
        private readonly ExcelReader excelReader;
        public IConfiguration Configuration { get; set; }
        public String MSSQLConnection { get; set; }

        public MasterSetupController(ILoggerFactory loggerFactory, DBContext db, IMapper mapper, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IConfiguration configuration) : base(loggerFactory)
        {
            Configuration = configuration;
            MSSQLConnection = Configuration.GetConnectionString("MSSQLConnection").ToString();

            this._mapper = mapper;
            this.userManager = userManager;
            this.webHostEnvironment = webHostEnvironment;
            excelReader = new ExcelReader();

            this.masterSetupLogic = new MasterSetupLogic(db, mapper, userManager);
        }
        #region -- PRODUCT MASTER ---

        #region Product Master Search Fields
        [HttpGet]
        [SwaggerOperation(
                        Summary = "Division Product ",
                        Description = "Division Product ",
                        OperationId = "Division Product",
                        Tags = new[] { "Product" }
                    )]

        [SwaggerResponse(200, "Division Product")]
        [SwaggerResponse(204, "Division Product ", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> ProductMaster_SearchFields()
        {
            var records = await this.masterSetupLogic.ProductMaster_SearchFields();
            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Product Master", data = new { categories = records[0] } });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- List of Product --
        [HttpGet]
        [SwaggerOperation(
                          Summary = "Get List of all Product Type",
                          Description = "Get List of all Product Type",
                          OperationId = "ListProductType",
                          Tags = new[] { "Product" }
                      )]

        [SwaggerResponse(200, "Product Type found"/*,typeof(IEnumerable<Vendor>)*/)]
        [SwaggerResponse(204, "Product Type not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> List_ProductMaster()
        {
            var lstData = this.masterSetupLogic.List_ProductMaster();

            if (lstData != null && lstData.Count() > 0)
            {
                return Ok(new { success = 1, message = "ProductType list", data = lstData });
            }
            else if (lstData == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- Add New Product --
        [HttpPost]

        [SwaggerOperation(
                   Summary = "Add New Product",
                   Description = "Add New Product",
                   OperationId = "AddProduct",
                   Tags = new[] { "Product Setup" }
               )]
        [SwaggerResponse(201, "Product Created", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> AddNew_ProductMaster([FromBody, SwaggerParameter("Add Product's Details", Required = true)] ProductMaster Product)
        {
            var result = await this.masterSetupLogic.AddNew_ProductMaster(Product);
            if (result == true)
            {
                return Ok(new { success = 1, message = "Product Created successfully." });
            }
            else
            {
                return BadRequest(new { success = 0, message = "Product not created due to internal error." });
            }
        }
        #endregion

        #region -- Update New Product --
        [HttpPost]

        [SwaggerOperation(
                          Summary = "Update exsisting Product",
                          Description = "Update exsisting Product",
                          OperationId = "UpdateProduct",
                          Tags = new[] { "Product Setup" }
                      )]
        [SwaggerResponse(201, "Product Updated", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> Update_ProductMaster([FromBody, SwaggerParameter("Update exsisting Product's Details", Required = true)] ProductMaster Product)
        {
            var applicationUser = await this.GetCurrentUser();
            var result = await this.masterSetupLogic.Update_ProductMaster(Product, applicationUser);

            if (result == true)
            {
                return Ok(new { success = 1, message = "Product update successfully." });
            }
            else
            {
                return BadRequest(new { success = 0, message = "Product not update due to internal error." });
            }
        }
        #endregion

        #region -- Get by ID --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get Product by ID",
                         Description = "Get Product by ID",
                         OperationId = "GetProductByID",
                         Tags = new[] { "Product Setup" }
                     )]
        [SwaggerResponse(201, "Product found", typeof(Divisionwise_ProductEntity))]
        [SwaggerResponse(204, "Product not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> GetByID_ProductMaster([FromQuery, SwaggerParameter("Product's ID", Required = true)] Int32 ProductID)
        {
            var Product = await this.masterSetupLogic.GetByID_ProductMaster(ProductID);

            if (Product != null)
            {
                return Ok(new { success = 1, message = "Product found", data = Product });
            }
            else if (Product == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion
        #endregion

        #region-- PRODUCT MAPPING MASTER --
        #region -- Update Product Mapping --
        [HttpPost]

        [SwaggerOperation(
                   Summary = "Update Product Mapping",
                   Description = "Update Product Mapping",
                   OperationId = "UpdateProductMapping",
                   Tags = new[] { "Update Product Mapping Setup" }
               )]
        [SwaggerResponse(201, "Product Mapping Updated", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public IActionResult Update_ProductMappingMaster([FromBody, SwaggerParameter("Add Product's Details", Required = true)] ProductMasterMapping Product)
        {
            MasterSetupLogic.Update_ProductMappingMaster(MSSQLConnection, Product);
            return Ok(new { data = "Primary Sales Data Transfer Successfully !!!!!" });
        }
        #endregion

        #region -- List of Product Mapping --
        [HttpGet]
        [SwaggerOperation(
                          Summary = "Get List of all Product Mapping Type",
                          Description = "Get List of all Product Mapping Type",
                          OperationId = "ListProductMappingType",
                          Tags = new[] { "Product Mapping" }
                      )]

        [SwaggerResponse(200, "Product Mapping Type found"/*,typeof(IEnumerable<Vendor>)*/)]
        [SwaggerResponse(204, "Product Mapping Type not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult List_ProductMappingMaster(string Type)
        {
            var lstData = MasterSetupLogic.List_ProductMappingMaster(MSSQLConnection, Type);

            if (lstData != null && lstData.Count() > 0)
            {
                return Ok(new { success = 1, message = "ProductMappingType list", data = lstData });
            }
            else if (lstData == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- Get Product Mapping by ID --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get Product Mapping by ID",
                         Description = "Get Product Mapping by ID",
                         OperationId = "GetProductMappingByID",
                         Tags = new[] { "Product Mapping Setup" }
                     )]
        [SwaggerResponse(201, "Product found", typeof(Divisionwise_ProductEntity))]
        [SwaggerResponse(204, "Product not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public IActionResult GetByID_ProductMappingMaster([FromQuery, SwaggerParameter("Product's ID", Required = true)] Int32 ProductID)
        {
            var Product = MasterSetupLogic.GetByID_ProductMappingMaster(MSSQLConnection, ProductID);

            if (Product.Count > 0)
            {
                return Ok(new { success = 1, message = "Product found", data = Product });
            }
            else if (Product.Count == 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion


        #endregion

        #region Division wise Product Search Fields
        [HttpGet]
        [SwaggerOperation(
                        Summary = "Division Product ",
                        Description = "Division Product ",
                        OperationId = "Division Product",
                        Tags = new[] { "Product" }
                    )]

        [SwaggerResponse(200, "Division Product")]
        [SwaggerResponse(204, "Division Product ", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]

        public async Task<IActionResult> DivisionProduct_SearchFields()
        {
            var records = await this.masterSetupLogic.DivisionProduct_SearchFields();
            if (records != null && records.Length > 0)
            {
                return Ok(new { success = 1, message = "Division Product", data = new { divisions = records[0], depots = records[1], products = records[2], packunits = records[3] } });
            }
            else if (records.Count() <= 0)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- Divisionwise Product ---
        #region Product Type ---

        #region -- List of Product --
        [HttpGet]
        [SwaggerOperation(
                          Summary = "Get List of all Product Type",
                          Description = "Get List of all Product Type",
                          OperationId = "ListProductType",
                          Tags = new[] { "Product" }
                      )]

        [SwaggerResponse(200, "Product Type found"/*,typeof(IEnumerable<Vendor>)*/)]
        [SwaggerResponse(204, "Product Type not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> List_Divisionwise_Product(string Division, string DepotName, string Product, string PackUnit)
        {
            var lstData = await this.masterSetupLogic.List_Divisionwise_Product(Division, DepotName, Product, PackUnit);

            if (lstData != null && lstData.Count() > 0)
            {
                return Ok(new { success = 1, message = "ProductType list", data = lstData });
            }
            else if (lstData == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region -- Add New Product --
        [HttpPost]

        [SwaggerOperation(
                   Summary = "Add New Product",
                   Description = "Add New Product",
                   OperationId = "AddProduct",
                   Tags = new[] { "Product Setup" }
               )]
        [SwaggerResponse(201, "Product Created", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> AddNew_Divisionwise_Product([FromBody, SwaggerParameter("Add Product's Details", Required = true)] Divisionwise_ProductEntity Product)
        {
            var result = await this.masterSetupLogic.AddNew_Divisionwise_Product(Product);
            if (result == true)
            {
                return Ok(new { success = 1, message = "Product Created successfully." });
            }
            else
            {
                return BadRequest(new { success = 0, message = "Product not created due to internal error." });
            }
        }
        #endregion

        #region -- Update New Product --
        [HttpPost]

        [SwaggerOperation(
                          Summary = "Update exsisting Product",
                          Description = "Update exsisting Product",
                          OperationId = "UpdateProduct",
                          Tags = new[] { "Product Setup" }
                      )]
        [SwaggerResponse(201, "Product Updated", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> Update_Divisionwise_Product([FromBody, SwaggerParameter("Update exsisting Product's Details", Required = true)] Divisionwise_ProductEntity Product)
        {
            var applicationUser = await this.GetCurrentUser();
            var result = await this.masterSetupLogic.Update_Divisionwise_Product(Product, applicationUser);

            if (result == true)
            {
                return Ok(new { success = 1, message = "Product update successfully." });
            }
            else
            {
                return BadRequest(new { success = 0, message = "Product not update due to internal error." });
            }
        }
        #endregion

        #region -- Get by ID --
        [HttpGet]

        [SwaggerOperation(
                         Summary = "Get Product by ID",
                         Description = "Get Product by ID",
                         OperationId = "GetProductByID",
                         Tags = new[] { "Product Setup" }
                     )]
        [SwaggerResponse(201, "Product found", typeof(Divisionwise_ProductEntity))]
        [SwaggerResponse(204, "Product not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> GetByID_Divisionwise_Product([FromQuery, SwaggerParameter("Product's ID", Required = true)] Int32 ProductID)
        {
            var Product = await this.masterSetupLogic.GetByID_Divisionwise_Product(ProductID);

            if (Product != null)
            {
                return Ok(new { success = 1, message = "Product found", data = Product });
            }
            else if (Product == null)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        #endregion

        #region -- Delete New Product --
        [HttpGet]

        [SwaggerOperation(
                            Summary = "Delete Product",
                            Description = "Delete Product",
                            OperationId = "DeleteProduct",
                            Tags = new[] { "Product" }
                        )]
        [SwaggerResponse(201, "Product Deleted", typeof(Divisionwise_ProductEntity))]
        [SwaggerResponse(204, "Product not found", typeof(string))]
        [SwaggerResponse(400, "Bad Request", typeof(string))]
        public async Task<IActionResult> Delete_Divisionwise_Product([FromQuery, SwaggerParameter("Product's ID", Required = true)] Int32 ProductID)
        {
            var Product = await this.masterSetupLogic.Delete_Divisionwise_Product(ProductID);
            if (Product == true)
            {
                return Ok(new { success = 1, message = "Product Deleted" });
            }
            else
            {
                return BadRequest();
            }
        }


        #endregion
        #endregion
        #endregion
    }
}
