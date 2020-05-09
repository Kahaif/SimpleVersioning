using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;

namespace SimpleVersioning.Data.Sql
{
    public class SqlServerContext: DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<FileProperty> Properties { get; set; }
        public DbSet<Configuration> Configurations { get; set; }

        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }
        /*
        public SqlServerContext() : base() 
        { 
        })*/

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=(localdb)\\SQLSERVER;Database=SimpleVersioning;Trusted_Connection=True;MultipleActiveResultSets=true;User ID=TestAccess;Password=test_password;");
            base.OnConfiguring(optionsBuilder);
        }
        */
    }
}
