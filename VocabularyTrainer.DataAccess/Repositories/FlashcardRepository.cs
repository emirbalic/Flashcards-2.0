using Microsoft.EntityFrameworkCore;
using VocabularyTrainer.Data.Data;
using VocabularyTrainer.Data.Models;
using VocabularyTrainer.DataAccess.Interfaces;
using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Contracts.Common;

namespace VocabularyTrainer.DataAccess.Repositories
{
    public class FlashcardRepository : IFlashcardRepository
    {
        private readonly AppDbContext _context;

        public FlashcardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Flashcard>> GetFilteredFlashcardsAsync(FlashcardQueryParams queryParams)
        {
            var query = _context.Flashcards
                .Include(f => f.Translations)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();

                query = query.Where(f =>
                    f.Translations.Any(t => t.Text.ToLower().Contains(search))
                );
            }
            

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<Flashcard>
            {
                Items = items,
                TotalCount = totalCount,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<Flashcard?> GetByIdAsync(int id)
        {
            return await _context.Flashcards
                .Include(f => f.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddAsync(Flashcard flashcard)
        {
            _context.Flashcards.Add(flashcard);
            await _context.SaveChangesAsync();
        }

        public async Task AddFlashcardsAsync(List<Flashcard> flashcards)
        {
            _context.Flashcards.AddRange(flashcards);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Flashcard flashcard)
        {
            _context.Flashcards.Update(flashcard);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var flashcard = await _context.Flashcards.FindAsync(id);
            if (flashcard != null)
            {
                _context.Flashcards.Remove(flashcard);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Flashcard>> GetAllByUserAsync(int userId)
        {
            return await _context.Flashcards
                .Where(f => f.UserId == userId)
                .Include(f => f.Translations)
                .ToListAsync();
        }
    }
}