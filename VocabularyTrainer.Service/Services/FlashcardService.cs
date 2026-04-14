// VocabularyTrainer.Service/Services/FlashcardService.cs

using VocabularyTrainer.DataAccess.Repositories;
using VocabularyTrainer.Data.Models;
using System.Text.Json;

using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

using Microsoft.AspNetCore.Http;
using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Service.Interfaces;

namespace VocabularyTrainer.Service.Services
{
    public class FlashcardService : IFlashcardService
    {
        private readonly FlashcardRepository _flashcardRepository;

        public FlashcardService(FlashcardRepository flashcardRepository)
        {
            _flashcardRepository = flashcardRepository;
        }

        // ---------------- EXISTING ----------------

        public async Task<PagedResult<Flashcard>> GetFlashcardsAsync(FlashcardQueryParams queryParams)
        {
            return await _flashcardRepository.GetFilteredFlashcardsAsync(queryParams);
        }

        public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
        {
            return await _flashcardRepository.GetByIdAsync(id);
        }

        public async Task CreateFlashcardAsync(Flashcard flashcard)
        {
            await _flashcardRepository.AddAsync(flashcard);
        }

        public async Task<(bool Success, string Message, int Count)> UploadFlashcardsAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (extension != ".csv" && extension != ".json")
                return (false, "Only CSV and JSON files are supported.", 0);

            using var streamReader = new StreamReader(file.OpenReadStream());

            var result = extension == ".json"
                ? await ProcessJsonAsync(await streamReader.ReadToEndAsync())
                : await ProcessCsvAsync(streamReader);

            return (result.Success, result.Message, result.Count);
        }

        public async Task<UploadResult> ProcessJsonAsync(string jsonContent)
        {
            try
            {
                var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(
                    jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (flashcards == null || !flashcards.Any())
                    return new UploadResult(false, "Invalid or empty JSON file.");

                await _flashcardRepository.AddFlashcardsAsync(flashcards);

                return new UploadResult(true, flashcards.Count);
            }
            catch (Exception ex)
            {
                return new UploadResult(false, $"Error processing JSON: {ex.Message}");
            }
        }

        public async Task<UploadResult> ProcessCsvAsync(StreamReader reader)
        {
            try
            {
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                    HeaderValidated = null,
                    MissingFieldFound = null
                });

                var flashcards = csv.GetRecords<Flashcard>().ToList();

                if (!flashcards.Any())
                    return new UploadResult(false, "CSV file is empty or invalid.");

                await _flashcardRepository.AddFlashcardsAsync(flashcards);

                return new UploadResult(true, flashcards.Count);
            }
            catch (Exception ex)
            {
                return new UploadResult(false, $"Error processing CSV: {ex.Message}");
            }
        }

        public async Task UpdateFlashcardAsync(Flashcard flashcard)
        {
            await _flashcardRepository.UpdateAsync(flashcard);
        }

        public async Task DeleteFlashcardAsync(int id)
        {
            await _flashcardRepository.DeleteAsync(id);
        }

        // ---------------- NEW (2.0) ----------------

        public async Task<Flashcard> CreateAsync(CreateFlashcardDto dto, int userId)
        {
            var flashcard = new Flashcard
            {
                UserId = userId,
                WordType = dto.WordType,
                Translations = dto.Translations.Select(t => new FlashcardTranslation
                {
                    LanguageCode = t.LanguageCode,
                    Text = t.Text,
                    ExampleSentence = t.ExampleSentence
                }).ToList()
            };

            await _flashcardRepository.AddAsync(flashcard);

            return flashcard;
        }

        public async Task<List<Flashcard>> GetAllAsync(int userId)
        {
            return await _flashcardRepository.GetAllByUserAsync(userId);
        }
    }
}