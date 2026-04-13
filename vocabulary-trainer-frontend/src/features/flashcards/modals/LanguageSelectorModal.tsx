// src/features/flashcards/modals/LanguageSelectorModal.tsx
import React, { useState } from 'react';
import { Modal, Box, Typography, Button, FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { useLanguageStore } from '../../../store/languageStore';

interface LanguageSelectorModalProps {
  open: boolean;
  handleClose: () => void;
}

const LanguageSelectorModal: React.FC<LanguageSelectorModalProps> = ({ open, handleClose }) => {
  // Get languages and the setters from Zustand
  const { fromLanguage, toLanguage, setFromLanguage, setToLanguage } = useLanguageStore();
  
  // Local state for dropdown selections
  const [selectedFromLanguage, setSelectedFromLanguage] = useState(fromLanguage);
  const [selectedToLanguage, setSelectedToLanguage] = useState(toLanguage);
  const [error, setError] = useState<string | null>(null);
  

  // Handle form submission
  const handleSubmit = () => {
    // Validation check
    if (selectedFromLanguage === selectedToLanguage) {
      setError("Please select different languages for 'From' and 'To'.");
      return;
    }

    setFromLanguage(selectedFromLanguage);
    setToLanguage(selectedToLanguage);
    handleClose(); // Close the modal after submission
  };

  return (
    <Modal open={open} onClose={handleClose}>
      <Box sx={modalStyle}>
        <Typography variant="h6" gutterBottom>
          Select Languages
        </Typography>

        <FormControl fullWidth margin="normal">
          <InputLabel>From Language</InputLabel>
          <Select
            value={selectedFromLanguage}
            onChange={(e) => setSelectedFromLanguage(e.target.value)}
            label="From Language"
          >
            <MenuItem value="german">German</MenuItem>
            <MenuItem value="english">English</MenuItem>
            <MenuItem value="croatian">Croatian</MenuItem>
            <MenuItem value="french">French</MenuItem>
            {/* Add more languages as needed */}
          </Select>
        </FormControl>

        <FormControl fullWidth margin="normal">
          <InputLabel>To Language</InputLabel>
          <Select
            value={selectedToLanguage}
            onChange={(e) => setSelectedToLanguage(e.target.value)}
            label="To Language"
          >
            <MenuItem value="german">German</MenuItem>
            <MenuItem value="english">English</MenuItem>
            <MenuItem value="croatian">Croatian</MenuItem>
            <MenuItem value="french">French</MenuItem>
            {/* Add more languages as needed */}
          </Select>
        </FormControl>

        {error && (
          <Typography color="error" sx={{ marginTop: 2 }}>
            {error}
          </Typography>
        )}


        <Box sx={{ display: 'flex', justifyContent: 'space-between', marginTop: '16px' }}>
          <Button
            variant="contained"
            color="primary"
            onClick={handleSubmit}
          >
            Save
          </Button>

          <Button
            variant="outlined"
            color="secondary"
            onClick={handleClose}
          >
            Cancel
          </Button>
        </Box>
      </Box>
    </Modal>
  );
};

const modalStyle = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  p: 4,
};

export default LanguageSelectorModal;
