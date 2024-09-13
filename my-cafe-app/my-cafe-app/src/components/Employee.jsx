import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { AgGridReact } from 'ag-grid-react';
import 'ag-grid-community/styles/ag-grid.css';
import 'ag-grid-community/styles/ag-theme-alpine.css';
import { Container, Typography, Box, Button, Dialog, DialogActions, DialogContent, DialogTitle, Typography as MuiTypography, AppBar, Toolbar, FormControl, InputLabel, Select, MenuItem } from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import AddEmployee from './AddEmployee';

const Employees = () => {
  const [employees, setEmployees] = useState([]);
  const [cafes, setCafes] = useState([]);
  const [selectedCafeId, setSelectedCafeId] = useState('');
  const [showAddForm, setShowAddForm] = useState(false);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [showConfirmDialog, setShowConfirmDialog] = useState(false);
  const [employeeToDelete, setEmployeeToDelete] = useState(null);
  const [formMode, setFormMode] = useState('add');

  const location = useLocation(); // Get the current location

  const [columnDefs] = useState([
    { headerName: 'ID', field: 'EMP_Id', sortable: false, filter: false, width: 80 },
    { headerName: 'Name', field: 'EMP_Name', sortable: false, filter: false },
    { headerName: 'Email Address', field: 'EMP_Address', sortable: false, filter: false },
    { headerName: 'Phone', field: 'EMP_PH', sortable: false, filter: false, width: 120 },
    { headerName: 'Days Worked', field: 'DaysOfWork', sortable: false, filter: false, width: 120 },
    { headerName: 'Cafe Name', field: 'CAF_NAME', sortable: false, filter: false },
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

  const fetchCafes = () => {
    axios
      .get('https://localhost:44388/api/Cafe/GetAllCafeDetails?locationId=')
      .then((response) => {
        setCafes(response.data);
      })
      .catch((error) => {
        console.error('Error fetching cafes:', error);
      });
  };

  const fetchEmployees = (cafeId) => {
    const url = cafeId
      ? `https://localhost:44388/api/Employee/GetAllEmolyees?cafeId=${cafeId}`
      : 'https://localhost:44388/api/Employee/GetAllEmolyees';
    axios
      .get(url)
      .then((response) => {
        setEmployees(response.data);
      })
      .catch((error) => {
        console.error('Error fetching employees:', error);
      });
  };

  useEffect(() => {
    fetchCafes();
  }, []);

  useEffect(() => {
    fetchEmployees(selectedCafeId);
  }, [selectedCafeId]);

  const handleCafeChange = (event) => {
    setSelectedCafeId(event.target.value);
  };

  const handleAddEmployeeSuccess = () => {
    fetchEmployees(selectedCafeId);
    setShowAddForm(false);
    setSelectedEmployee(null);
  };

  const handleUpdateClick = (employee) => {
    setSelectedEmployee(employee);
    setFormMode('update');
    setShowAddForm(true);
  };

  const handleAddClick = () => {
    setSelectedEmployee(null);
    setFormMode('add');
    setShowAddForm(true);
  };

  const handleDeleteClick = (employee) => {
    setEmployeeToDelete(employee);
    setShowConfirmDialog(true);
  };

  const handleConfirmDelete = () => {
    axios
      .delete('https://localhost:44388/api/Employee/DeleteEmployeeDetails', {
        params: { employeeId: employeeToDelete.EMP_Id }
      })
      .then(() => {
        fetchEmployees(selectedCafeId);
        setShowConfirmDialog(false);
        setEmployeeToDelete(null);
      })
      .catch((error) => {
        console.error('Error deleting employee:', error);
      });
  };

  const handleCancelDelete = () => {
    setShowConfirmDialog(false);
    setEmployeeToDelete(null);
  };

  const isActive = (path) => location.pathname === path;

  return (
    <>
      <AppBar position="static" sx={styles.appBar}>
        <Toolbar>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            Employee Management
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
        {showAddForm ? (
          <AddEmployee
            onSuccess={handleAddEmployeeSuccess}
            onCancel={() => setShowAddForm(false)}
            employeeData={selectedEmployee}
            mode={formMode}
          />
        ) : (
          <>
            <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
              <Typography variant="h4" >Employees List</Typography>
              <Box display="flex" alignItems="center">
                <InputLabel style={{ padding: '5px' }}> Select Cafe</InputLabel>
                <FormControl style={{ marginRight: '20px' }}>

                  <Select  style={{ height: '35px' }}
                    value={selectedCafeId}
                    onChange={handleCafeChange}
                  >
                    <MenuItem value="">
                      <em>All Cafes</em>
                    </MenuItem>
                    {cafes.map((cafe) => (
                      <MenuItem key={cafe.Id} value={cafe.Id}>
                        {cafe.CafeName}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
                <Button
                  variant="contained"
                  color="primary"
                  onClick={handleAddClick}
                >
                  Add New Employee
                </Button>
              </Box>
            </Box>

            <Box
              className="ag-theme-alpine"
              style={{ height: '500px', width: '100%', marginTop: '20px' }}
            >
              <AgGridReact
                rowData={employees}
                columnDefs={columnDefs}
                pagination={true}
                paginationPageSize={10}
              />
            </Box>
          </>
        )}

        <Dialog open={showConfirmDialog} onClose={handleCancelDelete}>
          <DialogTitle>Confirm Deletion</DialogTitle>
          <DialogContent>
            <MuiTypography variant="body1">
              Are you sure you want to delete this employee?
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

export default Employees;
