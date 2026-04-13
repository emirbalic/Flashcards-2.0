// src/features/flashcards/components/FileUploadModal.tsx

import React, { useState } from "react";
import { Modal, Box, Typography, Button, TextField } from "@mui/material";
import { useLocation, useNavigate } from "react-router-dom";
// import axios from "axios";
import { uploadBulkFlashcards } from "../../../api/apiHelpers";

// Define types for props
interface FileUploadModalProps {
  open: boolean;
  handleClose: () => void;
}

const FileUploadModal: React.FC<FileUploadModalProps> = ({
  open,
  handleClose,
}) => {
  // Define file state type as File | null
  const [file, setFile] = useState<File | null>(null);
  const [message, setMessage] = useState<string>("");
  const [isLoading, setIsLoading] = useState<boolean>(false); // Track loading state
  const navigate = useNavigate(); // Hook to navigate
  const location = useLocation();

  // Correct type for file change event
  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    // Check if files are available and not null
    const selectedFile = e.target.files ? e.target.files[0] : null;
    if (selectedFile) {
      setFile(selectedFile); // Set the selected file
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!file) {
      setMessage("Please select a file");
      return;
    }

    const formData = new FormData();
    formData.append("file", file);
    setIsLoading(true); // Start loading before the upload

    try {
      
      const response = await uploadBulkFlashcards(file);
      if (response) {
        setMessage('Upload successful!');
        setFile(null); // Clear the file input after successful upload
        // navigate('/flashcards'); 
        handleClose(); // Close modal
        if (location.pathname !== "/flashcards") {
          navigate("/flashcards"); // Redirect only if not already on the page
        }
      }
    } catch (error) {
      setMessage(`Error: ${error || "Something went wrong"}`);
    } finally {
      setIsLoading(false); // Stop loading when the upload completes (success or failure)
    }
  };

  return (
    <Modal open={open} onClose={handleClose}>
      <Box sx={modalStyle}>
        <Typography variant="h6" gutterBottom>
          Bulk Upload Flashcards
        </Typography>
        <form onSubmit={handleSubmit}>
          <TextField
            type="file"
            onChange={handleFileChange}
            fullWidth
            margin="normal"
            variant="outlined"
          />
          <Button
            variant="contained"
            color="primary"
            type="submit"
            disabled={isLoading}
          >
            {isLoading ? "Uploading..." : "Upload"}
          </Button>
        </form>
        {message && (
          <Typography color="error" mt={2}>
            {message}
          </Typography>
        )}
        {/* Close Button */}
        <Button
          onClick={handleClose}
          variant="outlined"
          color="secondary"
          sx={{ mt: 2 }}
          disabled={isLoading}
        >
          Close
        </Button>
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

export default FileUploadModal;
