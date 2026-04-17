namespace VocabularyTrainer.Contracts.Flashcards;

public class FlashcardTranslationDto
{
    public int Id { get; set; }

    public string LanguageCode { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string? ExampleSentence { get; set; }
}