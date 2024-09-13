import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Container, Typography, Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography as MuiTypography, AppBar, Toolbar, MenuItem, Select, FormControl, InputLabel } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import AddCafe from './AddCafe';

const Cafes = () => {
  const [cafes, setCafes] = useState([]);
  const [locations, setLocations] = useState([]);
  const [selectedLocationId, setSelectedLocationId] = useState('');
  const [showAddForm, setShowAddForm] = useState(false);
  const [selectedCafe, setSelectedCafe] = useState(null);
  const [showConfirmDialog, setShowConfirmDialog] = useState(false);
  const [cafeToDelete, setCafeToDelete] = useState(null);
  const [isEditing, setIsEditing] = useState(false); // Track if we are editing or adding

  const location = useLocation(); // Get the current location

  const [columnDefs] = useState([
    { 
      headerName: 'Logo', 
      field: 'Logo', 
      cellRenderer: (params) => {
        return params.value ? (
          <img
            src={`data:image/png;base64,${params.value}`}
            alt="Cafe Logo"
            style={{ width: '50px', height: '50px', borderRadius: '50%' }}
          />
        ) : (
          'No Logo'
        );
      }
    },
    { headerName: 'Name', field: 'CafeName', sortable: false, filter: false },
    { headerName: 'Description', field: 'Description', sortable: false, filter: false },
    { 
      headerName: 'Employee Count', 
      field: 'EmployeeCount', 
      sortable: false, 
      filter: false,
      width: 120  
    },
    { headerName: 'Location Name', field: 'LocationName', sortable: false, filter: false },
    {
      headerName: 'Actions',
      cellRenderer: (params) => (
        <div>
          <Button
            variant="contained"
            color="success"
            onClick={() => handleUpdateClick(params.data)}
            style={{ marginRight: '5px' }}
          >
            Update
          </Button>
          <Button
            variant="contained"
            color="error"
            onClick={() => handleDeleteClick(params.data)}
          >
            Delete
          </Button>
        </div>
      ),
    },
  ]);

  const fetchLocations = () => {
    axios
      .get('https://localhost:44388/api/Cafe/GetAllLocations')
      .then((response) => {
        setLocations(response.data);
      })
      .catch((error) => {
        console.error('Error fetching locations:', error);
      });
  };

  const fetchCafes = (locationId) => {
    const url = locationId
      ? `https://localhost:44388/api/Cafe/GetAllCafeDetails?locationId=${locationId}`
      : 'https://localhost:44388/api/Cafe/GetAllCafeDetails'; 
    axios
      .get(url)
      .then((response) => {
        setCafes(response.data);
      })
      .catch((error) => {
        console.error('Error fetching cafe details:', error);
      });
  };

  useEffect(() => {
    fetchLocations();
  }, []);

  useEffect(() => {
    fetchCafes(selectedLocationId);
  }, [selectedLocationId]);

  const handleLocationChange = (event) => {
    const newLocationId = event.target.value;
    setSelectedLocationId(newLocationId);
  };

  const handleAddCafeSuccess = () => {
    fetchCafes(selectedLocationId);
    setShowAddForm(false);
    setSelectedCafe(null);
    setIsEditing(false); // Reset editing state after success
  };

  const handleUpdateClick = (cafe) => {
    setSelectedCafe(cafe);
    setShowAddForm(true);
    setIsEditing(true); // Set editing state when updating
  };

  const handleDeleteClick = (cafe) => {
    setCafeToDelete(cafe);
    setShowConfirmDialog(true);
  };

  const handleConfirmDelete = () => {
    axios
      .delete('https://localhost:44388/api/Cafe/DeleteCafeDetails', {
        params: { cafeId: cafeToDelete.Id }
      })
      .then(() => {
        fetchCafes(selectedLocationId);
        setShowConfirmDialog(false);
      })
      .catch((error) => {
        console.error('Error deleting cafe:', error);
      });
  };

  const handleCancelDelete = () => {
    setShowConfirmDialog(false);
    setCafeToDelete(null);
  };

  const isActive = (path) => location.pathname === path;

  return (
    <>
      <AppBar position="static" sx={styles.appBar}>
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Cafe Management
          </Typography>
          <Button
            color="inherit"
            component={Link}
            to="/"
            sx={isActive('/') ? styles.activeButton : styles.button}
          >
            Home
          </Button>
          <Button
            color="inherit"
            component={Link}
            to="/cafes"
            sx={isActive('/cafes') ? styles.activeButton : styles.button}
          >
            Cafe
          </Button>
          <Button
            color="inherit"
            component={Link}
            to="/employees"
            sx={isActive('/employees') ? styles.activeButton : styles.button}
          >
            Employee
          </Button>
        </Toolbar>
      </AppBar>

      <Container style={{ marginTop: '30px' }}>
        {!showAddForm && (
          <>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
              <Typography variant="h4" gutterBottom>
                Cafes List
              </Typography>
              <Box display="flex" alignItems="center">
              <InputLabel  style={{ padding: '5px' }}>   Location </InputLabel> 
                <FormControl   style={{ marginRight: '20px' }}>
                
                  <Select  style={{ height: '35px' }}
                    value={selectedLocationId}
                    onChange={handleLocationChange}
                  >
                    <MenuItem value="">
                      <em>All Locations</em>
                    </MenuItem>
                    {locations.map((location) => (
                      <MenuItem key={location.Id} value={location.Id}>
                        {location.LocationName}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
                <Button
                  variant="contained"
                  color="primary"
                  onClick={() => {
                    setSelectedCafe(null);
                    setShowAddForm(true);
                    setIsEditing(false); // Set to adding mode
                  }}
                >
                  Add New Cafe
                </Button>
              </Box>
            </Box>

            <Box
              className="ag-theme-alpine"
              style={{ height: '500px', width: '100%', marginTop: '20px', overflowX: 'auto' }}
            >
              <AgGridReact
                rowData={cafes}
                columnDefs={columnDefs}
                pagination={true}
                paginationPageSize={10}
              />
            </Box>
          </>
        )}

        {showAddForm && (
          <AddCafe
            onSuccess={handleAddCafeSuccess}
            onCancel={() => setShowAddForm(false)}
            cafeData={selectedCafe}
            title={isEditing ? 'Update Cafe' : 'Add Cafe'}
          />
        )}

        <Dialog
          open={showConfirmDialog}
          onClose={handleCancelDelete}
        >
          <DialogTitle>Confirm Deletion</DialogTitle>
          <DialogContent>
            <MuiTypography variant="body1">
              Are you sure you want to delete this cafe?
            </MuiTypography>
          </DialogContent>
          <DialogActions>
            <Button onClick={handleCancelDelete} color="primary">
              Cancel
            </Button>
            <Button onClick={handleConfirmDelete} color="secondary">
              Delete
            </Button>
          </DialogActions>
        </Dialog>
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
};

export default Cafes;
