// src/features/flashcards/FlashcardPage.tsx
import React, {useState} from 'react';
import {Alert, Box, CircularProgress, TextField, Pagination, Select, MenuItem, Button} from '@mui/material';
import Flashcard from './components/Flashcard.tsx';
import useFlashcards from './hooks/useFlashcards.ts';

const FlashcardPage: React.FC = () => {
    const [search, setSearch] = useState("");
    const [page, setPage] = useState(1);
    const [sortBy, setSortBy] = useState<string | undefined>();
    const [sortDesc, setSortDesc] = useState(false);

    const {flashcards, totalCount, loading, error} = useFlashcards({
        search,
        page,
        sortBy,
        sortDesc,
    });

    if (error) return <Alert severity="error">{error}</Alert>;

    return (
        <Box
            px={2}
            py={3}
            sx={{
                minHeight: "100vh",
                maxWidth: "1200px",
                margin: "0 auto",
                width: "100%",
            }}
        >
            {/* Top bar */}
            <Box
                display="flex"
                flexDirection={{ xs: "column", sm: "row" }}
                gap={2}
                alignItems={{ sm: "center" }}
                justifyContent="space-between"
                mb={3}
            >
                <TextField
                    label="Search"
                    variant="outlined"
                    fullWidth
                    value={search}
                    onChange={(e) => {
                        setSearch(e.target.value);
                        setPage(1);
                    }}
                />

                {/* Loader stays stable */}
                <Box sx={{ height: 36, display: "flex", alignItems: "center" }}>
                    {loading && <CircularProgress size={20} />}
                </Box>
                <Select
                    value={sortBy || ""}
                    onChange={(e) => {
                        const value = e.target.value || undefined;
                        setSortBy(value);
                        setPage(1);
                    }}
                    size="small"
                    sx={{ minWidth: 160 }}
                >
                    <MenuItem value="">Default</MenuItem>
                    <MenuItem value="german">German</MenuItem>
                    <MenuItem value="english">English</MenuItem>
                </Select>
                <Button
                    onClick={() => setSortDesc((prev) => !prev)}
                    size="small"
                >
                    {sortDesc ? "↓ Desc" : "↑ Asc"}
                </Button>
            </Box>

            {/* Grid */}
            <Box
                sx={{
                    display: "grid",
                    gridTemplateColumns: {
                        xs: "1fr",
                        sm: "repeat(2, 1fr)",
                        md: "repeat(3, 1fr)",
                        lg: "repeat(4, 1fr)",
                    },
                    gap: 3,
                }}
            >
                {flashcards.map((flashcard) => (
                    <Flashcard key={flashcard.id} flashcard={flashcard} />
                ))}
            </Box>

            {/* Pagination */}
            <Box
                display="flex"
                justifyContent="center"
                mt={4}
            >
                <Pagination
                    count={Math.ceil(totalCount / 10)}
                    page={page}
                    onChange={(_, value) => setPage(value)}
                    color="primary"
                    size="large"
                    shape="rounded"
                />
            </Box>

        </Box>
    );
};

export default FlashcardPage;
