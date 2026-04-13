using VocabularyTrainer.Data.Models;

public class Flashcard
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? WordType { get; set; }

    public string? ExampleSentence { get; set; }

    public ICollection<FlashcardTranslation> Translations { get; set; } = new List<FlashcardTranslation>();
}