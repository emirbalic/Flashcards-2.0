import React, { useState } from 'react';
import { Box, Card, Typography, Button } from '@mui/material';
import { Flashcard as FlashcardType } from '../../../types/Flashcard';
import { Link } from 'react-router-dom';
import { useLanguageStore } from '../../../store/languageStore'; // Importing the Zustand hook

// const cardStyles = {
//   root: {
//     position: 'relative',
//     height: '300px', // Adjust height as needed
//     cursor: 'pointer',
//     display: 'flex',
//     flexDirection: 'column',
//     justifyContent: 'space-between', // Push content apart
//     padding: '16px', // Add padding for spacing
//   },
//   cardContent: {
//     flexGrow: 1, // Allow text to expand
//     display: 'flex',
//     alignItems: 'center',
//     justifyContent: 'center',
//     position: 'relative',
//   },
//   text: {
//     position: 'absolute',
//     width: '100%',
//     height: '100%',
//     display: 'flex',
//     alignItems: 'center',
//     justifyContent: 'center',
//     opacity: 0,
//     transition: 'opacity 0.5s ease-in-out',
//   },
//   visible: {
//     opacity: 1,
//   },
//   buttonContainer: {
//     display: 'flex',
//     justifyContent: 'center',
//     marginTop: '8px', // Space from the card content
//   },
// };

interface FlashcardProps {
  flashcard: FlashcardType;
}

const Flashcard: React.FC<FlashcardProps> = ({ flashcard }) => {
  const [isFlipped, setIsFlipped] = useState(false);

  // Get languages from Zustand store
  const { fromLanguage, toLanguage } = useLanguageStore();

  const handleCardClick = () => {
    setIsFlipped((prevState) => !prevState);
  };

  const getLanguageContent = (language: string) => {
    // Return the content for the selected language
    if (language === 'german') {
      return flashcard.german; // Assuming german is available
    } else if (language === 'english') {
      return flashcard.english; // Assuming english is available
    } else if (language === 'french' && flashcard.french) {
      return flashcard.french; // Handle french language if available
    } else if (language === 'croatian' && flashcard.croatian) {
      return flashcard.croatian; // Handle croatian language if available
    }
  
    // Fallback: Return a message if no matching language is found
    return `Content not available in ${language}`;
  };


  return (
      <Card
          onClick={handleCardClick}
          sx={{
            height: 220,
            display: "flex",
            flexDirection: "column",
            justifyContent: "space-between",
            p: 2,
            borderRadius: 3,
            boxShadow: 2,
            transition: "all 0.2s ease",
            cursor: "pointer",
            "&:hover": {
              boxShadow: 6,
              transform: "translateY(-2px)",
            },
          }}
      >
        <Box
            sx={{
              flexGrow: 1,
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              position: "relative",
              textAlign: "center",
            }}
        >
          <Typography
              variant="h6"
              sx={{
                position: "absolute",
                opacity: isFlipped ? 0 : 1,
                transition: "opacity 0.3s",
              }}
          >
            {getLanguageContent(fromLanguage)}
          </Typography>

          <Typography
              variant="h6"
              sx={{
                position: "absolute",
                opacity: isFlipped ? 1 : 0,
                transition: "opacity 0.3s",
              }}
          >
            {getLanguageContent(toLanguage)}
          </Typography>
        </Box>

        <Box display="flex" justifyContent="center">
          <Link
              to={`/flashcards/${flashcard.id}`}
              onClick={(e) => e.stopPropagation()}
              style={{ textDecoration: "none" }}
          >
            <Button variant="outlined" size="small">
              Details
            </Button>
          </Link>
        </Box>
      </Card>
  );
};

export default Flashcard;
