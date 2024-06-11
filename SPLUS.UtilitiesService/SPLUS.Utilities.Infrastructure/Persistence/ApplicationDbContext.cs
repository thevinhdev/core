﻿using Microsoft.EntityFrameworkCore;
using SPLUS.Utilities.Domain.Entities;
using SPLUS.Utilities.Infrastructure.Contracts;
using SPLUS.Utilities.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPLUS.Utilities.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public string TenantId { get; set; }
        private readonly ITenantService _tenantService;

        public ApplicationDbContext(DbContextOptions options, ITenantService tenantService) : base(options)
        {
            _tenantService = tenantService;
            TenantId = _tenantService.GetTenant()?.TID;
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Shoe> Shoes { get; set; }
        public DbSet<RequestCleaningService> RequestCleaningService { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Product>().HasQueryFilter(a => a.TenantId == TenantId);
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantConnectionString = _tenantService.GetConnectionString();
            if (!string.IsNullOrEmpty(tenantConnectionString))
            {
                var DBProvider = _tenantService.GetDatabaseProvider();
                if (DBProvider.ToLower() == "sqlserver")
                {
                    optionsBuilder.UseSqlServer(_tenantService.GetConnectionString());
                }
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IMustHaveTenant>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        entry.Entity.TenantId = TenantId;
                        break;
                }
            }
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
