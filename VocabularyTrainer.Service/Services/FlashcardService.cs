using AutoMapper;
using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Data.Models;
using VocabularyTrainer.DataAccess.Interfaces;
using VocabularyTrainer.Service.Interfaces;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using VocabularyTrainer.Contracts.Common;

namespace VocabularyTrainer.Service.Services
{
    public class FlashcardService : IFlashcardService
    {
        private readonly IFlashcardRepository _repository;
        private readonly IMapper _mapper;

        public FlashcardService(IFlashcardRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // ---------------- EXISTING ----------------

        // public async Task<PagedResult<Flashcard>> GetFlashcardsAsync(FlashcardQueryParams queryParams)
        // {
        //     return await _repository.GetFilteredFlashcardsAsync(queryParams);
        // }

        public async Task<PagedResult<FlashcardDto>> GetFlashcardsAsync(FlashcardQueryParams queryParams)
        {
            var result = await _repository.GetFilteredFlashcardsAsync(queryParams);

            return new PagedResult<FlashcardDto>
            {
                Items = _mapper.Map<List<FlashcardDto>>(result.Items),
                TotalCount = result.TotalCount,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
            };
        }

        public async Task<Flashcard?> GetFlashcardByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

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

            await _repository.AddAsync(flashcard);
            return flashcard;
        }

        public async Task UpdateFlashcardAsync(Flashcard flashcard)
        {
            await _repository.UpdateAsync(flashcard);
        }

        public async Task DeleteFlashcardAsync(int id)
        {
            await _repository.DeleteAsync(id);
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

                await _repository.AddFlashcardsAsync(flashcards);

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

                await _repository.AddFlashcardsAsync(flashcards);

                return new UploadResult(true, flashcards.Count);
            }
            catch (Exception ex)
            {
                return new UploadResult(false, $"Error processing CSV: {ex.Message}");
            }
        }

        // ---------------- NEW CLEAN READ (DTO-based) ----------------

        public async Task<List<FlashcardDto>> GetAllAsync(int userId)
        {
            var entities = await _repository.GetAllByUserAsync(userId);

            return _mapper.Map<List<FlashcardDto>>(entities);
        }

        //         public async Task<PagedResult<FlashcardDto>> GetFlashcardsAsync(FlashcardQueryParams queryParams)
        // {
        //     var result = await _repository.GetFilteredFlashcardsAsync(queryParams);

        //     return new PagedResult<FlashcardDto>
        //     {
        //         Items = _mapper.Map<List<FlashcardDto>>(result.Items),
        //         TotalCount = result.TotalCount,
        //         Page = queryParams.Page,
        //         PageSize = queryParams.PageSize
        //     };
    }

    // ---------------- NEW CREATE (still manual for now) ----------------

    // public async Task<Flashcard> CreateAsync(CreateFlashcardDto dto, int userId)
    // {
    //     var flashcard = new Flashcard
    //     {
    //         UserId = userId,
    //         WordType = dto.WordType,
    //         Translations = dto.Translations.Select(t => new FlashcardTranslation
    //         {
    //             LanguageCode = t.LanguageCode,
    //             Text = t.Text,
    //             ExampleSentence = t.ExampleSentence
    //         }).ToList()
    //     };

    //     await _repository.AddAsync(flashcard);

    //     return flashcard;
    // }
    // }
}