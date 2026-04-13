namespace VocabularyTrainer.Data.Models;

public class UserFlashcardProgress
{
    public int Id { get; set; }   // ✅ ADD THIS

    public int UserId { get; set; }

    public int FlashcardId { get; set; }

    public int FamiliarityLevel { get; set; }
}