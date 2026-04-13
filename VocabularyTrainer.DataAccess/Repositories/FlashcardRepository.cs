// VocabularyTrainer.DataAccess/Repositories/FlashcardRepository.cs

using VocabularyTrainer.Data.Data;
using VocabularyTrainer.Data.Models;
using Microsoft.EntityFrameworkCore;
using VocabularyTrainer.Contracts.Flashcards;
using System.Linq.Expressions;

namespace VocabularyTrainer.DataAccess.Repositories
{
    public class FlashcardRepository
    {
        private readonly AppDbContext _context;

        public FlashcardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Flashcard>> GetAllAsync()
        {
            return await _context.Flashcards.ToListAsync();
        }

        // public async Task<List<Flashcard>> GetFilteredFlashcardsAsync(FlashcardQueryParams query)
        public async Task<PagedResult<Flashcard>> GetFilteredFlashcardsAsync(FlashcardQueryParams query)
        {
            var flashcardsQuery = _context.Flashcards.AsQueryable();

            // Searching
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                flashcardsQuery = flashcardsQuery
                    .Where(f => f.German.Contains(query.Search) || f.English.Contains(query.Search));
            }

            // // Filtering (example: by category)
            // if (!string.IsNullOrWhiteSpace(query.Category))
            // {
            //     flashcardsQuery = flashcardsQuery.Where(f => f.Category == query.Category);
            // }

            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                flashcardsQuery = query.SortDesc
                    ? flashcardsQuery.OrderByDescendingDynamic(query.SortBy)
                    : flashcardsQuery.OrderByDynamic(query.SortBy);
            }
            
            var totalCount = await flashcardsQuery.CountAsync();

            // Pagination
            int skip = (query.Page - 1) * query.PageSize;
            var items = await flashcardsQuery
                .Skip(skip)
                .Take(query.PageSize)
                .ToListAsync();

            // Return structured result
            return new PagedResult<Flashcard>
            {
                Items = items,
                TotalCount = totalCount
            };
            // int skip = (query.Page - 1) * query.PageSize;
            // flashcardsQuery = flashcardsQuery.Skip(skip).Take(query.PageSize);
            //
            // return await flashcardsQuery.ToListAsync();
        }


        public async Task<Flashcard> GetByIdAsync(int id)
        {
            return await _context.Flashcards.FindAsync(id);
        }

        public async Task AddAsync(Flashcard flashcard)
        {
            await _context.Flashcards.AddAsync(flashcard);
            await _context.SaveChangesAsync();
        }

        public async Task AddFlashcardsAsync(List<Flashcard> flashcards)
        {
            await _context.Flashcards.AddRangeAsync(flashcards);
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
    }

    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, propertyName);
            var lambda = Expression.Lambda(property, param);
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), property.Type);
            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }

        public static IQueryable<T> OrderByDescendingDynamic<T>(this IQueryable<T> source, string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(param, propertyName);
            var lambda = Expression.Lambda(property, param);
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == "OrderByDescending" && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), property.Type);
            return (IQueryable<T>)method.Invoke(null, new object[] { source, lambda })!;
        }
    }
}