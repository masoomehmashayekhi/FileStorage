using FileStorage.Layers.L01_Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace FileStorage.Layers.L02_DataLayer
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext()
        {
        }
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options)
: base(options)
        {
        }

        public virtual DbSet<ClientEntity> Clients { set; get; }
        public virtual DbSet<FileEntity> Files { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(
           @"Data Source=.;Initial Catalog=FileStorage;User id=sa; Password=Aa123456;");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<ClientEntity>(entity =>
            {
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.IPAddress).IsRequired();
                entity.HasIndex(b => b.IPAddress);
                entity.HasIndex(b => b.Token);

            });
            modelBuilder.Entity<FileEntity>(entity =>
            {
                entity.HasOne(ut => ut.client)
                      .WithMany(u => u.Files)
                      .HasForeignKey(ut => ut.ClientId);
                entity.HasIndex(ut => ut.Id);
            });
        }
    }
}
