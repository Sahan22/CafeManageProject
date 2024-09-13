import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container } from '@mui/material';
import CafeImage from './../assets/images/cafe-image.jpg'; // Import a suitable image

const Home = () => {
  const navigate = useNavigate(); // Initialize useNavigate hook
  const location = useLocation(); // Get the current location

  const goToCafes = () => {
    navigate('/cafes'); // Navigate to the Cafes page
  };

  // Determine the active route
  const isActive = (path) => location.pathname === path;

  return (
    <>
      <AppBar position="static" sx={styles.appBar}>
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Cafe Management Application
          </Typography>
          <Button
            color="inherit"
            onClick={() => navigate('/')}
            sx={isActive('/') ? styles.activeButton : styles.button}
          >
            Home
          </Button>
          <Button
            color="inherit"
            onClick={goToCafes}
            sx={isActive('/cafes') ? styles.activeButton : styles.button}
          >
            Cafe
          </Button>
          <Button
            color="inherit"
            onClick={() => navigate('/employees')}
            sx={isActive('/employees') ? styles.activeButton : styles.button}
          >
            Employee
          </Button>
        </Toolbar>
      </AppBar>

      <Container>
        <div style={styles.container}>
          <h1 style={styles.heading}>Home </h1>
          <p style={styles.paragraph}>Welcome to the Cafe Management System!</p>
          <img
            src={CafeImage}
            alt="Cafe Management"
            style={styles.image}
          />
        </div>
      </Container>
    </>
  );
};

const styles = {
  appBar: {
    borderRadius: '10px 10px 10px 10px',  
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
  },
  button: {
    marginLeft: '10px',
    transition: 'background-color 0.3s ease-in-out, color 0.3s ease-in-out',
  },
  activeButton: {
    backgroundColor: '#ffffff',  
    color: '#000000',  
    marginLeft: '10px',
  },
  container: {
    textAlign: 'center',
    marginTop: '20px',
    transition: 'all 0.3s ease-in-out',
  },
  heading: {
    fontSize: '2.5rem',
    marginBottom: '20px',
    color: '#333',
    transition: 'color 0.3s ease-in-out',
  },
  paragraph: {
    fontSize: '1.2rem',
    color: '#666',
    marginBottom: '30px',
  },
  image: {
    width: '100%',
    maxWidth: '600px',
    borderRadius: '8px',
    marginBottom: '20px',
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    transition: 'transform 0.3s ease-in-out',
  },
};

export default Home;
