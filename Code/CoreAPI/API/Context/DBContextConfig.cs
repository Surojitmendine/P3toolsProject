using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using API.Helper;

namespace API.Context
{
    public partial class DBContext: DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //https://stackoverflow.com/a/48484797
            if (!optionsBuilder.IsConfigured)
            {
                EnvDBConfig envConfig = new EnvDBConfig();

                envConfig.ConfigureDB(optionsBuilder);
            }
        }
    }
}
