import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Box, TextField, Button, InputLabel, FormControl, MenuItem, Select, Typography as MuiTypography, RadioGroup, FormControlLabel, Radio } from '@mui/material';

const AddEmployee = ({ onSuccess, onCancel, employeeData, mode }) => {
  const [employee, setEmployee] = useState({
    EMP_Id: '',
    EMP_Name: '', 
    EMP_PH: '',
    EMP_Address: '',
    EMP_Gender: '',   
    CAF_Id: '',
  });

  const [cafes, setCafes] = useState([]);
  const [errors, setErrors] = useState({
    EMP_Name: '',
    EMP_Address: '',
    EMP_PH: '',
  });

  // Fetch cafes and set employee data if provided
  useEffect(() => {
    axios
      .get('https://localhost:44388/api/Cafe/GetAllCafeDropdown?locationId=')
      .then((response) => {
        setCafes(response.data);
      })
      .catch((error) => {
        console.error('Error fetching cafes:', error);
      });

    if (employeeData) {
      setEmployee({
        EMP_Id: employeeData.EMP_Id || '',
        EMP_Name: employeeData.EMP_Name || '', 
        EMP_PH: employeeData.EMP_PH || '',
        EMP_Address: employeeData.EMP_Address || '',
        EMP_Gender: employeeData.EMP_Gender || '',  
        CAF_Id: employeeData.CAF_Id || '',
      });
    }
  }, [employeeData]);

  // Validate employee input
  const validate = () => {
    let valid = true;
    let newErrors = { EMP_Name: '', EMP_Address: '', EMP_PH: '' };

    if (employee.EMP_Name.trim() === '') {
      newErrors.EMP_Name = 'Name is required';
      valid = false;
    }
    if (employee.EMP_Address.trim() === '') {
      newErrors.EMP_Address = 'Address is required';
      valid = false;
    }
    if (employee.EMP_PH.trim() === '') {
      newErrors.EMP_PH = 'Phone is required';
      valid = false;
    }

    setErrors(newErrors);
    return valid;
  };

  // Save employee data (add or update)
  const handleSave = () => {
    if (!validate()) {
      return;
    }

    const apiUrl = mode === 'add'
      ? 'https://localhost:44388/api/Employee/InsertEmployeeDetails'
      : `https://localhost:44388/api/Employee/UpdateEmployeeDetails`;

    const method = mode === 'add' ? 'post' : 'put'; 

    axios({
      method: method,
      url: apiUrl,
      data: employee   
    })
    .then(() => {
      onSuccess();
    })
    .catch((error) => {
      console.error('Error saving employee:', error);
    });
  };

  // Handle input change for text fields
  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setEmployee((prevEmployee) => ({
      ...prevEmployee,
      [name]: value,
    }));
  };

  return (
    <Box component="form" noValidate autoComplete="off" style={{ padding: '20px', maxWidth: '600px', margin: '0 auto' }}>
      <MuiTypography variant="h4" style={{ marginBottom: '20px' }}>
        {mode === 'add' ? 'Add New Employee' : 'Update Employee'}
      </MuiTypography>

      <TextField
        label="Name"
        name="EMP_Name"
        value={employee.EMP_Name}
        onChange={handleInputChange}
        error={!!errors.EMP_Name}
        helperText={errors.EMP_Name}
        fullWidth
        margin="normal"
        InputProps={{ style: { height: '40px' } }} // Reduce input height
        InputLabelProps={{ style: { fontSize: '0.875rem' } }} // Smaller font size for label
      />
      <TextField
        label="Address"
        name="EMP_Address"
        value={employee.EMP_Address}
        onChange={handleInputChange}
        error={!!errors.EMP_Address}
        helperText={errors.EMP_Address}
        fullWidth
        margin="normal"
        InputProps={{ style: { height: '40px' } }} // Reduce input height
        InputLabelProps={{ style: { fontSize: '0.875rem' } }} // Smaller font size for label
      />
      <TextField
        label="Phone"
        name="EMP_PH"
        value={employee.EMP_PH}
        onChange={handleInputChange}
        error={!!errors.EMP_PH}
        helperText={errors.EMP_PH}
        fullWidth
        margin="normal"
        InputProps={{ style: { height: '40px' } }} // Reduce input height
        InputLabelProps={{ style: { fontSize: '0.875rem' } }} // Smaller font size for label
      />
      <InputLabel style={{ marginTop: '20px' }}>Gender</InputLabel>
      <FormControl component="fieldset" margin="normal">
        <RadioGroup
          row
          aria-label="gender"
          name="EMP_Gender"
          value={employee.EMP_Gender}
          onChange={handleInputChange}
        >
          <FormControlLabel value="Male" control={<Radio />} label="Male" />
          <FormControlLabel value="Female" control={<Radio />} label="Female" />
          <FormControlLabel value="Other" control={<Radio />} label="Other" />
        </RadioGroup>
      </FormControl>

      <FormControl fullWidth margin="normal">
        <InputLabel>Cafe</InputLabel>
        <Select
          name="CAF_Id"
          value={employee.CAF_Id}
          onChange={handleInputChange}
          style={{ height: '40px' }} // Reduce select height
        >
          {cafes.map((cafe) => (
            <MenuItem key={cafe.Id} value={cafe.Id}>
              {cafe.CafeName}
            </MenuItem>
          ))}
        </Select>
      </FormControl>

      <Box mt={2}>
        <Button variant="contained" color="primary" onClick={handleSave}>
          {mode === 'add' ? 'Add Employee' : 'Update Employee'}
        </Button>
        <Button variant="outlined" color="secondary" onClick={onCancel} style={{ marginLeft: '10px' }}>
          Cancel
        </Button>
      </Box>
    </Box>
  );
};

export default AddEmployee;
