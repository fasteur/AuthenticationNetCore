
using Microsoft.EntityFrameworkCore;

namespace AuthenticationNetCore.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Classe> Classes { get; set; }
        public DbSet<StudentClasse> StudentClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentClasse>().HasKey(uc => new { uc.StudentId, uc.ClasseId });
            modelBuilder.Entity<Classe>().HasOne(c => c.Teacher).WithMany(t => t.Classes);
        }
    }
}