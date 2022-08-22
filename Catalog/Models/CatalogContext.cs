﻿using Microsoft.EntityFrameworkCore;

namespace Catalog.Models
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
    }
}
