using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using RecipeApi.Models;

namespace RecipeApi.Data
{
    //ApplicationDbContext: class quan ly ket noi va thao tac voi database
    //ke thua DbContext cua Entity Framework Core
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        //4 tables, 4 DbSet<> Properties
        //DbSet dai dien cho bang Recipes trong database
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<RecipeTag> RecipeTags { get; set; }
        public DbSet<Rating> Ratings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        //LK n-n recipe -tag
        //protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<RecipeTag>()
                .HasKey(rt => new{rt.RecipeId,rt.TagId});        //rt: recipetag
            //Khoa chinh

            //recipetag->recipe.  1-n
            modelBuilder.Entity<RecipeTag>()
                .HasOne(rt => rt.Recipe)   //1r->n rt
                .WithMany(r => r.RecipeTags)        //r:recipe
                .HasForeignKey(rt => rt.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            //1-n: Tag - RecipeTag
            modelBuilder.Entity<RecipeTag>()
                .HasOne(rt => rt.Tag) ////1t->n rt
                .WithMany(t => t.RecipeTags)
                .HasForeignKey(rt => rt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            //1-n : Recipe- Rating
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.Recipe)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

        
            modelBuilder.Entity<Rating>()
                .Property(r => r.Score)
                .HasAnnotation("Range",  new[] {1,5 });         //Rating (1-5) 

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Recipe>()
                .Property(r => r.Instructions)
                .IsRequired();

            modelBuilder.Entity<Tag>()
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
}
