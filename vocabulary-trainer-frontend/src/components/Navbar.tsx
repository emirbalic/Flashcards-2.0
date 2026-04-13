import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Box,
  Badge,
  IconButton,
  Drawer,
  List,
  ListItem,
  ListItemText,
  useTheme,
  useMediaQuery, ListItemButton,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import {Link} from "react-router-dom";
import {useState} from "react";
import FileUploadModal from "../features/flashcards/modals/FileUploadModal";
import LanguageSelectorModal from "../features/flashcards/modals/LanguageSelectorModal";
import {useLanguageStore} from "../store/languageStore"; // Import Zustand store

const Navbar = () => {
  const [openFileUploadModal, setOpenFileUploadModal] = useState(false);
  const [openLanguageModal, setOpenLanguageModal] = useState(false);
  const [drawerOpen, setDrawerOpen] = useState(false);

  // Get selected languages from Zustand store
  const {fromLanguage, toLanguage} = useLanguageStore();

  // Open/close modal handlers
  const handleFileUploadModalOpen = () => setOpenFileUploadModal(true);
  const handleFileUploadModalClose = () => setOpenFileUploadModal(false);
  const handleLanguageModalOpen = () => setOpenLanguageModal(true);
  const handleLanguageModalClose = () => setOpenLanguageModal(false);

  // Responsive drawer toggle
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

  const toggleDrawer = (open: boolean) => (
      event: React.KeyboardEvent | React.MouseEvent
  ) => {
      if (
          event.type === "keydown" &&
          ((event as React.KeyboardEvent).key === "Tab" ||
              (event as React.KeyboardEvent).key === "Shift")
      ) {
          return;
      }
      setDrawerOpen(open);
  };

  // Nav links including modal openers
  const navLinks = [
      {label: "Flashcards", to: "/flashcards", onClick: undefined},
      {label: "Login", to: "#", onClick: undefined},
      {label: "New", to: "/flashcards/create", onClick: undefined},
      {
          label: "Bulk Upload",
          to: "#",
          onClick: (e: React.MouseEvent) => {
              e.preventDefault();
              setDrawerOpen(false);
              handleFileUploadModalOpen();
          },
      },
      {
          label: "Language Selector",
          to: "#",
          onClick: (e: React.MouseEvent) => {
              e.preventDefault();
              setDrawerOpen(false);
              handleLanguageModalOpen();
          },
      },
  ];

  const drawerList = (
      <Box
          sx={{width: 250}}
          role="presentation"
          onClick={toggleDrawer(false)}
          onKeyDown={toggleDrawer(false)}
      >
          <List>
              {navLinks.map(({label, to, onClick}) => (
                  <ListItem key={label} disablePadding>
                      {onClick ? (
                          // If onClick is provided, render a normal button with click handler
                          <ListItemButton component="a" href={to} onClick={onClick}>
                              <ListItemText primary={label}/>
                          </ListItemButton>
                      ) : (
                          // Else render a Link (react-router) wrapped ListItemButton
                          <ListItemButton component={Link} to={to}>
                              <ListItemText primary={label}/>
                          </ListItemButton>
                      )}
                  </ListItem>
              ))}
          </List>
      </Box>
  );

  return (
      <>
          <AppBar position="fixed">
              <Toolbar>
                  <Typography variant="h6" sx={{flexGrow: 1}}>
                      Flashcards App
                  </Typography>

                  {isMobile ? (
                      <>
                          <IconButton
                              color="inherit"
                              edge="start"
                              onClick={toggleDrawer(true)}
                              aria-label="menu"
                              sx={{mr: 2}}
                          >
                              <MenuIcon/>
                          </IconButton>
                          <Drawer anchor="left" open={drawerOpen} onClose={toggleDrawer(false)}>
                              {drawerList}
                          </Drawer>
                      </>
                  ) : (
                      <Box>
                          <Button color="inherit" component={Link} to="/flashcards">
                              Flashcards
                          </Button>
                          <Button color="inherit" component="a" href="#">
                              Login
                          </Button>
                          <Button color="inherit" component="a" href="/flashcards/create">
                              New
                          </Button>
                          <Button color="inherit" onClick={handleFileUploadModalOpen}>
                              Bulk Upload
                          </Button>

                          <Badge
                              badgeContent={`${fromLanguage} → ${toLanguage}`}
                              anchorOrigin={{vertical: "bottom", horizontal: "left"}}
                              color="secondary"
                              sx={{marginLeft: 2, fontWeight: "bold", fontSize: "1rem"}}
                          >
                              <Button color="inherit" onClick={handleLanguageModalOpen}>
                                  Language Selector
                              </Button>
                          </Badge>
                      </Box>
                  )}
              </Toolbar>
          </AppBar>
          <Toolbar />

          {/* File Upload Modal */}
          <FileUploadModal open={openFileUploadModal} handleClose={handleFileUploadModalClose}/>

          {/* Language Selector Modal */}
          <LanguageSelectorModal
              open={openLanguageModal}
              handleClose={handleLanguageModalClose}
          />
      </>
  );
};

export default Navbar;
