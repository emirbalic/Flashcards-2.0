import React, { useState, useEffect } from 'react';
import { Box, Button, TextField } from '@mui/material';
import { postData, updateData, getData } from '../../../api/apiHelpers';
import { Flashcard } from '../../../types/Flashcard';
import { useParams, useNavigate } from 'react-router-dom';

const FlashcardForm = () => {
  const { id } = useParams<{ id: string }>(); // Get the id from URL params (if present)
  const [flashcard, setFlashcard] = useState<Flashcard>({
    id: 0, // ID will be assigned by the backend for new flashcards
    german: '',
    english: '',
    croatian: '',
    french: '',
    exampleSentence: '',
  });
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // If editing, fetch the flashcard data
  useEffect(() => {
    if (id) {
      const fetchFlashcard = async () => {
        try {
          const response = await getData<Flashcard>(`/flashcards/${id}`);
          setFlashcard(response);
        } catch (err) {
          setError('Error fetching flashcard');
          console.error(err);
        }
      };

      fetchFlashcard();
    }
  }, [id]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFlashcard({
      ...flashcard,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    try {
      if (id) {
        // If `id` exists, update the flashcard
        const updatedFlashcard = await updateData<Flashcard, Flashcard>(`/flashcards/${id}`, flashcard);
        console.log('Flashcard updated:', updatedFlashcard);
      } else {
        // If `id` doesn't exist, create a new flashcard
        const createdFlashcard = await postData<Flashcard, Flashcard>('/flashcards', flashcard);
        console.log('Flashcard created:', createdFlashcard);
      }
      navigate('/flashcards'); // Redirect after success
    } catch (err) {
      setError('Error submitting flashcard');
      console.error(err);
    }
  };

  return (
    <Box sx={{ maxWidth: 600, margin: '0 auto', padding: 4 }}>
      <form onSubmit={handleSubmit}>
        {error && <p style={{ color: 'red' }}>{error}</p>}
        <TextField
          label="German"
          name="german"
          value={flashcard.german}
          onChange={handleChange}
          fullWidth
          required
          margin="normal"
        />
        <TextField
          label="English"
          name="english"
          value={flashcard.english}
          onChange={handleChange}
          fullWidth
          required
          margin="normal"
        />
        <TextField
          label="Croatian"
          name="croatian"
          value={flashcard.croatian}
          onChange={handleChange}
          fullWidth
          required
          margin="normal"
        />
        <TextField
          label="French"
          name="french"
          value={flashcard.french}
          onChange={handleChange}
          fullWidth
          required
          margin="normal"
        />
        <TextField
          label="Example Sentence"
          name="exampleSentence"
          value={flashcard.exampleSentence}
          onChange={handleChange}
          fullWidth
          required
          margin="normal"
        />
        <Button type="submit" variant="contained" color="primary" fullWidth sx={{ mt: 3 }}>
          {id ? 'Update Flashcard' : 'Create Flashcard'}
        </Button>
      </form>
    </Box>
  );
};

export default FlashcardForm;
