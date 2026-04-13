using System.ComponentModel.DataAnnotations;

namespace VocabularyTrainer.Data.Models
{
    public class Flashcard
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string German { get; set; } = string.Empty;

        public string English { get; set; } = string.Empty;
        public string Croatian { get; set; } = string.Empty;
        public string French { get; set; } = string.Empty;

        public string ExampleSentence { get; set; } = string.Empty;
    }
}
