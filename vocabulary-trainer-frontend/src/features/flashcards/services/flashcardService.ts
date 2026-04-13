// src/features/flashcards/services/flashcardService.ts
import { getData, postData, updateData, deleteData } from '../../../api/apiHelpers';
import { Flashcard } from '../../../types/Flashcard';

export const fetchFlashcards = async () => {
  // console.log('is ity');
  
  return await getData('flashcards');
};

export const getFlashcardById = async (id: string): Promise<Flashcard> => {
  return getData<Flashcard>(`/flashcards/${id}`);
};

export const createFlashcard = async (data: { word: string; translation: string; example: string }) => {
  return await postData('flashcards', data);
};

export const updateFlashcard = async (id: string, data: { word: string; translation: string; example: string }) => {
  return await updateData(`flashcards/${id}`, data);
};

export const deleteFlashcard = async (id: string) => {
  return await deleteData(`flashcards/${id}`);
};
