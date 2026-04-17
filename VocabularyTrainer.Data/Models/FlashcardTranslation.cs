using System.Text.Json.Serialization;

namespace VocabularyTrainer.Data.Models;

public class FlashcardTranslation
{
    public int Id { get; set; }

    public int FlashcardId { get; set; }
    [JsonIgnore]
    public Flashcard Flashcard { get; set; } = null!;

    public string LanguageCode { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;
    public string? ExampleSentence { get; set; }
}