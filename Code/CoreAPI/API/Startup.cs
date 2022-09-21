using API.Context;
using API.Filters;
using API.Helper;
using API.IdentityModels;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Compression;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace API
{
    public class Startup
    {
        

        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
           
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Server Specific Configaration
            //https://stackoverflow.com/a/55196057/4336330
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            #endregion

            #region Connection
            
            /***
             * Connection String moved to Cotext file => OnConfiguring()
             */
            services.AddDbContext<DBContext>()/*(options => {                
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            })*/ ;

            services.AddDbContext<ApplicationDbContext>()/*(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")))*/;

            #endregion

            #region Logging

            services.AddScoped<LoggingFilter>();
            services.AddControllers(config =>
            {
                config.Filters.Add<LoggingFilter>();
            });
            #endregion


            #region  Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutomapperMappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            #endregion

            #region Asp >net Core Identity Configuaration
            services.AddIdentity<ApplicationUser, tbl_SYS_AspNet_Roles>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
            })

            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #endregion

            #region ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {

                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        AuthenticationType = "Bearer",
                        ValidIssuer = Configuration["Tokens:Issuer"],
                        ValidAudience = Configuration["Tokens:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            #endregion

            #region Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Api",
                    Version = "",
                    Description = "",/* @"This is a sample server Petstore server.
                                    You can find out more about Swagger at
                                    [http://swagger.io](http://swagger.io) or on [irc.freenode.net, #swagger](http://swagger.io/irc/).
                                    For this sample, you can use the api key `special-key` to test the authorization filters.

## Introduction     
This API is documented in **OpenAPI format** and is based on
[Petstore sample](http://petstore.swagger.io/) provided by [swagger.io](http://swagger.io) team.
It was **extended** to illustrate features of [generator-openapi-repo](https://github.com/Rebilly/generator-openapi-repo)
tool and [ReDoc](https://github.com/Redocly/redoc) documentation. In addition to standard
OpenAPI syntax we use a few [vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).

## OpenAPI Specification
This API is documented in **OpenAPI format** and is based on
[Petstore sample](http://petstore.swagger.io/) provided by [swagger.io](http://swagger.io) team.
It was **extended** to illustrate features of [generator-openapi-repo](https://github.com/Rebilly/generator-openapi-repo)
tool and [ReDoc](https://github.com/Redocly/redoc) documentation. In addition to standard
OpenAPI syntax we use a few [vendor extensions](https://github.com/Redocly/redoc/blob/master/docs/redoc-vendor-extensions.md).

## Cross-Origin Resource Sharing
This API features Cross-Origin Resource Sharing (CORS) implemented in compliance with  [W3C spec](https://www.w3.org/TR/cors/).
And that allows cross-domain communication from the browser.
All responses have a wildcard same-origin which makes them completely public and accessible to everyone, including any code on any site.

## Authentication
Petstore offers two forms of authentication:
- George Washington
- John Adams
- Thomas Jefferson

OAuth2 - an open protocol to allow secure authorization in a simple
and standard method from web, mobile and desktop applications.",*/

                });

                c.CustomOperationIds(apiDesc =>
                {
                    return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
                });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}");
                c.DocumentFilter<XLogoDocumentFilter>();
                //c.DocumentFilter<XTaggroupDoumentFilter>();

                c.ExampleFilters();
                c.EnableAnnotations();
                c.OperationFilter<AddResponseHeadersFilter>();
            });

            services.AddSwaggerExamplesFromAssemblyOf<RespExample>();
            #endregion
            //services.AddCors(option =>
            //{
            //    option.AddDefaultPolicy(policy =>
            //    {
            //        policy.AllowAnyOrigin();
            //        policy.AllowAnyHeader();
            //        policy.AllowAnyMethod();

            //    });                

            //});

            //services.AddCors();

            #region Register Background Service
            services.AddHostedService<TimedHostedService>();
            #endregion

            /*services.AddResponseCaching(optiins =>
            {

            });*/

            services.AddMemoryCache();

            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });


            services.AddMvc()               
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;                

            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseResponseCompression();

            #region Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api-dev";

            });
            app.UseReDoc(c =>
            {
                c.SpecUrl = "/swagger/v1/swagger.json";
                c.DocumentTitle = "Your API name";
                c.HideDownloadButton();
                c.HideHostname();
                //c.ExpandResponses("200,201");
                //c.RequiredPropsFirst();
                //c.NoAutoAuth();
                //c.PathInMiddlePanel();
                //c.HideLoading();
                //c.NativeScrollbars();
                //c.DisableSearch();
                //c.OnlyRequiredInSamples();
                //c.SortPropsAlphabetically();


            });
            #endregion

            #region Migrate Startup.Configure 
            app.UseStaticFiles();
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;

                var result = JsonConvert.SerializeObject(new { error = exception.Message, stacktrace = exception.StackTrace, innerexception = exception.InnerException });
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(result);
            }));
            app.UseRouting();
            app.UseCors(options =>
            {

                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });
            //app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            /*
             https://docs.microsoft.com/en-us/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio#migrate-startupconfigure
             follow Warning message
             */

            #endregion

        }
    }



    public class XLogoDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // need to check if extension already exists, otherwise swagger 
            // tries to re-add it and results in error  

            if (!swaggerDoc.Info.Extensions.ContainsKey("x-logo"))
                swaggerDoc.Info.Extensions.Add("x-logo", new OpenApiObject
            {
                {"url", new OpenApiString("http://ec2-18-218-30-23.us-east-2.compute.amazonaws.com:82/petrosoft.png")},
                {"backgroundColor", new OpenApiString("#FFFFFF")},
                {"altText", new OpenApiString("PetStore Logo")}
            });


        }

    }

    public class XTaggroupDoumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // need to check if extension already exists, otherwise swagger 
            // tries to re-add it and results in error  

            if (!swaggerDoc.Info.Extensions.ContainsKey("x-tagGroups"))
                swaggerDoc.Info.Extensions.Add("x-tagGroups", new OpenApiObject
            {
                {"name", new OpenApiString("x-tagGroups")},

            });


        }

    }


}


