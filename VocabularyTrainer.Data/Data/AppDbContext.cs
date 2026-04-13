using Microsoft.EntityFrameworkCore;
using VocabularyTrainer.Data.Models;

namespace VocabularyTrainer.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardTranslation> FlashcardTranslations { get; set; }
        public DbSet<UserLanguage> UserLanguages { get; set; }
        public DbSet<UserFlashcardProgress> UserFlashcardProgresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flashcard>()
                .HasMany(f => f.Translations)
                .WithOne(t => t.Flashcard)
                .HasForeignKey(t => t.FlashcardId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
