namespace VocabularyTrainer.Contracts.Flashcards;

public class FlashcardQueryParams
{
    public string? Search { get; set; }
    public string? SortBy { get; set; } //= "CreatedAt"; // default sort
    public bool SortDesc { get; set; } = false;

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    // Optional: filter by category, tags, etc.
    // public string? Category { get; set; }
}