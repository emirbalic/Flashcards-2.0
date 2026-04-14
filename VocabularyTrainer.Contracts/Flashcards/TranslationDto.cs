namespace VocabularyTrainer.Contracts.Flashcards;

public class TranslationDto
{
    public string LanguageCode { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string? ExampleSentence { get; set; }
}