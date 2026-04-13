// src/api/axiosInstance.ts
import axios from 'axios';

// Set the base URL for the API
const axiosInstance = axios.create({
  baseURL: 'http://localhost:5287/api/', // Updated with the correct backend URL
  headers: {
    'Content-Type': 'application/json',
  },
});

export default axiosInstance;
