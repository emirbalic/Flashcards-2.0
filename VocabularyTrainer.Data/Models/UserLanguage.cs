namespace VocabularyTrainer.Data.Models;

public class UserLanguage
{
    public int Id { get; set; } 

    public int UserId { get; set; }

    public string LanguageCode { get; set; } = string.Empty;
}