using Microsoft.EntityFrameworkCore;
using SimpleVersioning.Models;
using System;

namespace SimpleVersioning.Data.Sql
{
    public class MariaDBServerContext: DbContext
    {
        public DbSet<File> Files { get; set; }
        public DbSet<FileProperty> FileProperties { get; set; }
        public DbSet<FileVersion> FileVersions { get; set; }
        public MariaDBServerContext(DbContextOptions<MariaDBServerContext> options) : base(options) 
            => _ = options ?? throw new ArgumentNullException(nameof(options));


        public MariaDBServerContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseMySql("Server=80.219.88.57;Port=3306;Database=SimpleVersioning;User=simple-versioning;Password=McdlMp$123;");
            base.OnConfiguring(optionsBuilder);
        }
        
    }
}
