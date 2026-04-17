using VocabularyTrainer.Contracts.Common;
using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Data.Models;

namespace VocabularyTrainer.DataAccess.Interfaces;

public interface IFlashcardRepository
{
    Task AddAsync(Flashcard flashcard);
    Task AddFlashcardsAsync(List<Flashcard> flashcards);

    Task<Flashcard?> GetByIdAsync(int id);
    Task<PagedResult<Flashcard>> GetFilteredFlashcardsAsync(FlashcardQueryParams queryParams);
    
    Task<List<Flashcard>> GetAllByUserAsync(int userId);

    Task UpdateAsync(Flashcard flashcard);
    Task DeleteAsync(int id);
}