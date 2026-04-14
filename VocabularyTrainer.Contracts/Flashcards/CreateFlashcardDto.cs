namespace VocabularyTrainer.Contracts.Flashcards;

public class CreateFlashcardDto
{
    public string? WordType { get; set; }

    public List<TranslationDto> Translations { get; set; } = new();
}