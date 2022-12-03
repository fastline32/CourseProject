using Core.Data.EntryDbModels;
using Microsoft.EntityFrameworkCore;
using Type = Core.Data.EntryDbModels.Type;

namespace Core;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<Product> Products { get; set; }
    
}