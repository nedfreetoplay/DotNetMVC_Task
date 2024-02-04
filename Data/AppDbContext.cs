using Microsoft.EntityFrameworkCore;

namespace DotNetMVC_Task.Data;

public class AppDbContext: DbContext {
    public DbSet<Url> Urls { get; set; }

    protected readonly IConfiguration Configuration;

    public AppDbContext(IConfiguration configuration) {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseSerialColumns();
    }
}
