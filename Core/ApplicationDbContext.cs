﻿using Core.Data.EntryDbModels;
using Core.Data.EntryDbModels.Inquiry;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Type = Core.Data.EntryDbModels.Type;

namespace Core;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Type> Types { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<InquiryHeader> InquiryHeaders { get; set; }
    public DbSet<InquiryDetail> InquiryDetails { get; set; }
    
    
}