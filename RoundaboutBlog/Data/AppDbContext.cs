using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RoundaboutBlog.Entities;

namespace RoundaboutBlog.Data;

public class AppDbContext: IdentityDbContext<AppUser>
{
    public DbSet<Post> Posts { get; set; }
    
    public DbSet<Comment> Comments { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Post>().Property(p => p.PostId).UseIdentityAlwaysColumn();
        modelBuilder.Entity<Comment>().Property(c => c.CommentId).UseIdentityAlwaysColumn();

        if (Database.IsNpgsql())
        {
            modelBuilder.Entity<Post>().Property(p => p.CreatedAt).HasDefaultValueSql("now()");
            modelBuilder.Entity<Comment>().Property(c => c.CreatedAt).HasDefaultValueSql("now()");
        }
        else if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            modelBuilder.Entity<Post>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            modelBuilder.Entity<Comment>().Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}