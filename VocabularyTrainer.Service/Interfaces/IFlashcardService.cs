using VocabularyTrainer.Contracts.Common;
using VocabularyTrainer.Contracts.Flashcards;

namespace VocabularyTrainer.Service.Interfaces;

public interface IFlashcardService
{
    Task<Flashcard> CreateAsync(CreateFlashcardDto dto, int userId);

    Task<List<FlashcardDto>> GetAllAsync(int userId);

    Task<PagedResult<FlashcardDto>> GetFlashcardsAsync(FlashcardQueryParams queryParams);
}