using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;

namespace SimpleVersioning.Data.Sql
{
    public class SqlServerContext: DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<FileProperty> FileProperties { get; set; }

        public DbSet<FileVersion> FileVersions { get; set; }
        public SqlServerContext(DbContextOptions<SqlServerContext> options) : base(options) { }
        
        public SqlServerContext() : base() 
        { 
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=PC-Kevin\SQLSERVER;Database=SimpleVersioning;MultipleActiveResultSets=true;Integrated Security=True;");
            
        }
        
    }
}
