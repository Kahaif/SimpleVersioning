using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;

namespace SimpleVersioning.Data.SQLServer
{
    public class SqlServerContext: DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

       
    }
}
