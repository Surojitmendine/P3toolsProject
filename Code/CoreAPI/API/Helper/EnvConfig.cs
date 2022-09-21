using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace API.Helper
{
    interface IEnvDBConfig
    {
        void ConfigureDB(DbContextOptionsBuilder optionsBuilder);
    }
    public  class EnvDBConfig: IEnvDBConfig
    {
        private IConfiguration Configuration { get; }
       
        public EnvDBConfig()
        {

            Configuration = this.GetConfiguration();

        }
        
        private  enum DBProvider
        {
            MSSQL,
            MySQL,
            NotSpecified
        }

        private   DBProvider CurrentDbProvider
        {           
            get
            {
                
                string dbprovider= Configuration.GetValue<string>("ConnectionStrings:DBProvider");
                DBProvider dBProvider = dbprovider == "MSSQL" ? DBProvider.MSSQL : dbprovider == "MySQL" ? DBProvider.MySQL : DBProvider.NotSpecified;
                return dBProvider;
            }
        }

        private string CurrentConnectionString
        {
            get
            {

                string connectionstring = this.CurrentDbProvider == DBProvider.MSSQL ? Configuration.GetConnectionString("MSSQLConnection") :
                    Configuration.GetConnectionString("MySQLConnection");                
                return connectionstring;
            }
        }

        public void ConfigureDB(DbContextOptionsBuilder optionsBuilder)
        {         

            var connectionString =this.CurrentConnectionString;
            if (this.CurrentDbProvider == DBProvider.MSSQL)
            {
                optionsBuilder.UseSqlServer(connectionString)
                    .UseLazyLoadingProxies();
            }
            else if (this.CurrentDbProvider == DBProvider.MySQL)
            {
                optionsBuilder.UseMySql(connectionString, // replace with your Connection String
                    mySqlOptions =>
                    {
                        mySqlOptions.ServerVersion(new Version(8, 0, 18), ServerType.MySql); // replace with your Server Version and Type
                    });
            }
          
        }

        private IConfiguration GetConfiguration()
        {
           return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        }
    }
}
