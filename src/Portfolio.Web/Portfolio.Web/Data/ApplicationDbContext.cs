using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Portfolio.Web.Data;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class HeroStat
{
    public int Id { get; set; }
    [Required, MaxLength(100)]
    public string Value { get; set; } = string.Empty;
    [Required, MaxLength(200)]
    public string Label { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Color { get; set; } = "Primary";
    public int SortOrder { get; set; }
}

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<HeroStat> HeroStats => Set<HeroStat>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<HeroStat>().HasData(
            new HeroStat { Id = 1, Value = "30+",    Label = "Years in .NET",          Color = "Primary",   SortOrder = 1 },
            new HeroStat { Id = 2, Value = "AI",     Label = "First Approach",         Color = "Secondary", SortOrder = 2 },
            new HeroStat { Id = 3, Value = "SecOps", Label = "Security Built In",      Color = "Error",     SortOrder = 3 },
            new HeroStat { Id = 4, Value = "TDD/BDD",Label = "Test-Focused Developer", Color = "Success",   SortOrder = 4 }
        );
    }
}
