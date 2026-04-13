import { useQuery } from '@tanstack/react-query';
import { getFlashcardById } from '../services/flashcardService';
import { Flashcard } from '../../../types/Flashcard';

const useFlashcard = (id?: string) => {
  return useQuery<Flashcard, Error>({
    queryKey: ['flashcard', id],
    queryFn: () => getFlashcardById(id!),
    enabled: Boolean(id), // Only fetch if id exists
  });
};

export default useFlashcard;
