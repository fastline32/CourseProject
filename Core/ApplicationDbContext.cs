using Core.Data.EntryDbModels;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
}