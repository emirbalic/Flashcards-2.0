using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Data.Models;

namespace VocabularyTrainer.Service.Interfaces;

public interface IFlashcardService
{
    Task<Flashcard> CreateAsync(CreateFlashcardDto dto, int userId);
    Task<List<Flashcard>> GetAllAsync(int userId);
}