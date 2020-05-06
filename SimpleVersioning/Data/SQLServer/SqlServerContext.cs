using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;

namespace SimpleVersioning.Data.SQLServer
{
    public class SqlServerContext: DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<FileProperty> Properties { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }
       
    }
}
