namespace VocabularyTrainer.Contracts.Flashcards;

public class FlashcardDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? WordType { get; set; }

    public string? ExampleSentence { get; set; }

    public List<FlashcardTranslationDto> Translations { get; set; } = new();
}