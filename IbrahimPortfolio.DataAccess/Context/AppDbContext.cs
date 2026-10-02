using IbrahimPortfolio.Entity.Entities;
using Microsoft.EntityFrameworkCore;

namespace IbrahimPortfolio.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<SocialMedia> SocialMedias { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}