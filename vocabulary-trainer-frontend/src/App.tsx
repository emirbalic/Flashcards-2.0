import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import FlashcardPage from './features/flashcards/FlashcardPage';
import FlashcardDetailsPage from './features/flashcards/FlashcardDetailsPage'; // Import the details page
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Navbar from './components/Navbar';
import FlashcardForm from './features/flashcards/components/FlashcardForm';

const queryClient = new QueryClient();

const App: React.FC = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <Navbar />
    <Routes>
      <Route path="/" element={<Navigate to="/flashcards" />} />
      <Route path="/flashcards" element={<FlashcardPage />} />
      <Route path="/flashcards/create" element={<FlashcardForm />} /> {/* Route for creating flashcard */}
      <Route path="/flashcards/:id/edit" element={<FlashcardForm />} /> {/* Route for editing flashcard */}
      <Route path="/flashcards/:id" element={<FlashcardDetailsPage />} /> {/* New route */}
    </Routes>
    </QueryClientProvider>
  );
};

export default App;
