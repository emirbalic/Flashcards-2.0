import { useQuery } from '@tanstack/react-query';
import { getFilteredFlashcards } from '../../../api/apiHelpers';
import { Flashcard } from '../../../types/Flashcard';
import { FlashcardQueryParams } from '../../../types/FlashcardQueryParams'

const useFlashcards = (params?: FlashcardQueryParams) => {
  const mergedParams: FlashcardQueryParams = {
    page: 1,
    pageSize: 10,
    sortDesc: false,
    ...params, // override defaults
  };

  const { data, isLoading, error } = useQuery<{ items: Flashcard[]; totalCount: number }, Error>({
    queryKey: ['flashcards', mergedParams],
    queryFn: () => getFilteredFlashcards(mergedParams),
    placeholderData: (previousData) => previousData, // 🔥 replacement
  });

  return {
    flashcards: data?.items || [],
    totalCount: data?.totalCount || 0,
    loading: isLoading,
    error: error instanceof Error ? error.message : null,
  };
};

export default useFlashcards;