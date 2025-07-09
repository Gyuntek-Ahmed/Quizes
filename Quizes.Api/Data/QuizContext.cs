using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizes.Api.Data.Entities;
using Quizes.Shared;

namespace Quizes.Api.Data
{
    public class QuizContext : DbContext
    {
        public IPasswordHasher<User> _passwordHasher { get; }

        public QuizContext(DbContextOptions<QuizContext> options, IPasswordHasher<User> passwordHasher) : base(options)
        {
            _passwordHasher = passwordHasher;
        }

        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Option> Options { get; set; } = null!;

        public DbSet<Question> Questions { get; set; } = null!;

        public DbSet<Quiz> Quizzes { get; set; } = null!;

        public DbSet<StudentQuiz> StudentQuizzes { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var admin = new User
            {
                Name = "Gyuntek Ahmed",
                Email = "admin@mail.com",
                Phone = "0893794549",
                Role = nameof(UserRole.Admin),
                IsApproved = true,
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, "123456");

            modelBuilder
                .Entity<User>()
                .HasData(admin);
        }
    }
}
