using Microsoft.EntityFrameworkCore;
using VocabularyTrainer.Data.Models;

namespace VocabularyTrainer.Data.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Flashcard> Flashcards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sample Data (Optional)
            modelBuilder.Entity<Flashcard>().HasData(
                new Flashcard { Id = 1, German = "Haus", English = "House", Croatian = "Kuća", French = "Maison", ExampleSentence = "Das Haus ist groß." },
                new Flashcard { Id = 2, German = "Baum", English = "Tree", Croatian = "Drvo", French = "Arbre", ExampleSentence = "Der Baum ist hoch." }
            );
        }
    }
}
