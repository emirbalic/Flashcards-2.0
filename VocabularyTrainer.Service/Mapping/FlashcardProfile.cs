using AutoMapper;
using VocabularyTrainer.Contracts.Flashcards;
using VocabularyTrainer.Data.Models;

namespace VocabularyTrainer.Service.Mapping;

public class FlashcardProfile : Profile
{
    public FlashcardProfile()
    {
        CreateMap<Flashcard, FlashcardDto>();

        CreateMap<FlashcardTranslation, FlashcardTranslationDto>();
    }
}