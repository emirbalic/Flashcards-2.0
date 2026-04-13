using Microsoft.AspNetCore.Mvc;
using VocabularyTrainer.Service.Services;
using VocabularyTrainer.Data.Models;
using VocabularyTrainer.Contracts.Flashcards;

namespace VocabularyTrainer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlashcardsController : ControllerBase
    {
        private readonly FlashcardService _flashcardService;
        private readonly ILogger<FlashcardsController> _logger;

        // Inject logger into the constructor
        public FlashcardsController(FlashcardService flashcardService, ILogger<FlashcardsController> logger)
        {
            _flashcardService = flashcardService;
            _logger = logger;
        }
        // // [HttpGet]
        // // public async Task<ActionResult<List<Flashcard>>> GetFlashcards()
        // // {
        // //     var flashcards = await _flashcardService.GetAllFlashcardsAsync();
        // //     return Ok(flashcards);
        // // }
        //
        // [HttpGet]
        // public async Task<ActionResult<List<Flashcard>>> GetFlashcards([FromQuery] FlashcardQueryParams queryParams)
        // {
        //     var flashcards = await _flashcardService.GetFlashcardsAsync(queryParams);
        //     return Ok(flashcards);
        // }
        
        [HttpGet]
        public async Task<ActionResult<PagedResult<Flashcard>>> GetFlashcards([FromQuery] FlashcardQueryParams queryParams)
        {
            var result = await _flashcardService.GetFlashcardsAsync(queryParams);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Flashcard>> GetFlashcard(int id)
        {
            var flashcard = await _flashcardService.GetFlashcardByIdAsync(id);
            if (flashcard == null)
                return NotFound();

            return Ok(flashcard);
        }

        [HttpPost]
        public async Task<ActionResult> CreateFlashcard([FromBody] Flashcard flashcard)
        {
            await _flashcardService.CreateFlashcardAsync(flashcard);
            return CreatedAtAction(nameof(GetFlashcard), new { id = flashcard.Id }, flashcard);
        }



        [HttpPost("bulk")]
        public async Task<IActionResult> UploadFlashcards([FromForm] IFormFile file)
        {
            _logger.LogInformation("Starting the file upload process...");

            if (file == null || file.Length == 0)
            {
                _logger.LogError("No file uploaded or file is empty.");
                return BadRequest("No file uploaded.");
            }

            // Log file details
            _logger.LogInformation("Processing file: {FileName}, Size: {FileSize} bytes", file.FileName, file.Length);

            var result = await _flashcardService.UploadFlashcardsAsync(file);

            if (!result.Success)
            {
                _logger.LogError("File processing failed: {ErrorMessage}", result.Message);
                return BadRequest(result.Message);
            }

            _logger.LogInformation("File processed successfully, {Count} flashcards uploaded.", result.Count);

            return Ok(new { message = "Flashcards uploaded successfully!", count = result.Count });
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateFlashcard(int id, [FromBody] Flashcard flashcard)
        {
            flashcard.Id = id;
            await _flashcardService.UpdateFlashcardAsync(flashcard);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlashcard(int id)
        {
            await _flashcardService.DeleteFlashcardAsync(id);
            return NoContent();
        }
    }
}
