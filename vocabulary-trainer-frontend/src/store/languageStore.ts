import { create } from "zustand";
import { persist, createJSONStorage } from "zustand/middleware";

interface LanguageStore {
  fromLanguage: string;
  toLanguage: string;
  setFromLanguage: (language: string) => void;
  setToLanguage: (language: string) => void;
}

export const useLanguageStore = create(
  persist<LanguageStore>(
    (set) => ({
      fromLanguage: "german", // Default language
      toLanguage: "english", // Default language
      setFromLanguage: (language) => set({ fromLanguage: language }),
      setToLanguage: (language) => set({ toLanguage: language }),
    }),
    {
      name: "language-preferences", // Key for localStorage
      storage: createJSONStorage(() => localStorage), // Use JSON storage for localStorage
    }
  )
);
