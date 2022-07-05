using CodingGirls_project.Models;
using Microsoft.EntityFrameworkCore;


namespace CodingGirls_project.Contexts
{
    public class SchoolContext : DbContext
    {
       //Padrão Entity
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        }
        public DbSet<Course> Course { get; set; }

        public DbSet<Student> Student { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>().ToTable("student");
            modelBuilder.Entity<Student>()
               .HasOne(e => e.Course) // Um aluno tem um curso
               .WithMany(e => e.Students) // Um curso tem vários alunos
               .HasForeignKey(e => e.CourseId)
              ; //fk

            modelBuilder.Entity<Course>().ToTable("course");
            modelBuilder.Entity<Course>()
                .HasMany(e => e.Students)
                .WithOne(e => e.Course)
                ;

            modelBuilder.Entity<Course>()
                .Navigation(c => c.Students)
                .UsePropertyAccessMode(PropertyAccessMode.Property);

        } 

    }


}

