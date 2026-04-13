import { useParams, useNavigate } from 'react-router-dom';
import { Box, Button, Card, Typography, CircularProgress } from '@mui/material';
import useFlashcard from './hooks/useFlashcard';
import { deleteData } from '../../api/apiHelpers'; // Import delete function


const FlashcardDetailsPage = () => {

    
    const { id } = useParams<{ id: string }>(); // Ensure ID is always a string
    const { data: flashcard, isLoading, error } = useFlashcard(id);
    const navigate = useNavigate();

if (isLoading) return <CircularProgress sx={{ display: 'block', margin: 'auto', mt: 4 }} />;
if (error) return <Typography color="error">Error loading flashcard details.</Typography>;


const handleDelete = async () => {
    if (!id) return;

    const isConfirmed = window.confirm('Are you sure you want to delete this flashcard? This action cannot be undone.');

    if (!isConfirmed) return;

    try {
      await deleteData(`/flashcards/${id}`);
      navigate('/flashcards'); // Redirect after deletion
    } catch (err) {
      console.error('Error deleting flashcard:', err);
    }
  };

  return (
    <Box
    //   display="flex"
    //   flexDirection="row"
    // flexDirection="column"
      alignItems="center"
      justifyContent="center"
    //   minHeight="100vh"
      padding={4}
    //   sx={{ textAlign: 'center' }}
    // sx={{ width: 1, height: "100vh" }}
    >
      <Card sx={{ padding: 6, maxWidth: 800, textAlign: 'center', boxShadow: 3 }}>
        <Typography variant="h4" fontWeight="bold" gutterBottom>
          Flashcard Details
        </Typography>
        <Typography variant="h5" color="primary" gutterBottom>
          {flashcard!.german}
        </Typography>
        <Typography variant="body1"><strong>English:</strong> {flashcard!.english}</Typography>
        <Typography variant="body1"><strong>Croatian:</strong> {flashcard!.croatian}</Typography>
        <Typography variant="body1"><strong>French:</strong> {flashcard!.french}</Typography>
        <Typography variant="body2" fontStyle="italic" mt={2}>
          "{flashcard!.exampleSentence}"
        </Typography>

        <Button 
          variant="contained" 
          color="secondary" 
          onClick={() => navigate('/flashcards')}
          sx={{ mt: 3 }}
        >
          Back to Flashcards
        </Button>

        {/* Update button to go to the form page */}
        <Button 
          variant="contained" 
          color="primary" 
          onClick={() => navigate(`/flashcards/${id}/edit`)} // Navigate to the edit page
          sx={{ mt: 3, ml: 2 }}
        >
          Edit Flashcard
        </Button>

        <Button 
          variant="contained" 
          color="error" 
          onClick={handleDelete} 
          sx={{ mt: 3, ml: 2 }}
        >
          Delete Flashcard
        </Button>
        
      </Card>
    </Box>
  );
};

export default FlashcardDetailsPage;
