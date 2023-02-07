using System;
using System.Collections.Generic;
using BlazorBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorBlog.Data;

public partial class DataContext : DbContext
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Article");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Title).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(20);

            entity.HasMany(d => d.Categories).WithMany(p => p.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticleCategory",
                    r => r.HasOne<Category>().WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ArticleCategories_Categories"),
                    l => l.HasOne<Article>().WithMany()
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_ArticleCategories_Articles"),
                    j =>
                    {
                        j.HasKey("ArticleId", "CategoryId");
                    });
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(20);
            entity.Property(e => e.Url).HasMaxLength(20);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => new { e.ArticleId, e.NameImage });

            entity.Property(e => e.NameImage).HasMaxLength(50);

            entity.HasOne(d => d.Article).WithMany(p => p.Images)
                .HasForeignKey(d => d.ArticleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Images_Articles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
