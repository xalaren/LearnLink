﻿using CoursesPrototype.Adapter.EFConfigurations;
using CoursesPrototype.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoursesPrototype.Adapter.EFContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Credentials> Credentials { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsersEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CredentialsEntityTypeConfiguration());
        }
    }
}
