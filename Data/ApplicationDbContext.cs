using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using quill.Entities;

namespace quill.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Board> Boards { get; set; }
    public DbSet<QuillList> QLists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(n => n.Boards)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Board>()
            .HasMany(n => n.QLists)
            .WithOne(i => i.Board)
            .HasForeignKey(i => i.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Board>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<QuillList>()
            .HasMany(n => n.Items)
            .WithOne(i => i.QList)
            .HasForeignKey(i => i.ListId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<QuillList>()
            .HasKey(x => x.Id);
        modelBuilder.Entity<ListItem>()
            .HasKey(x => x.Id);
        base.OnModelCreating(modelBuilder);
    }
}