import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Box, Button, MenuItem, Select, InputLabel, FormControl, Typography } from '@mui/material';
import RetxtBox from './RetxtBox';

const AddCafe = ({ onSuccess, onCancel, cafeData }) => {
  const [cafe, setCafe] = useState({
    Id: 0,
    CafeName: '',
    Description: '',
    Logo: null,
    FK_LocationId: '',
  });
  const [locations, setLocations] = useState([]);
  const [newLocation, setNewLocation] = useState('');
  const [previewImage, setPreviewImage] = useState(null);
  const [errors, setErrors] = useState({});

  // Fetch locations and set initial cafe data if provided
  useEffect(() => {
    axios
      .get('https://localhost:44388/api/Cafe/GetAllLocations')
      .then((response) => {
        setLocations(response.data);
      })
      .catch((error) => {
        console.error('Error fetching locations:', error);
      });

    if (cafeData) {
      setCafe({
        Id: cafeData.Id || 0,
        CafeName: cafeData.CafeName || '',
        Description: cafeData.Description || '',
        Logo: null,
        FK_LocationId: cafeData.FK_LocationId || '',
      });
      // Set preview image URL if Logo exists
      if (cafeData.Logo) {
        setPreviewImage(`data:image/png;base64,${cafeData.Logo}`);
      }
    }
  }, [cafeData]);

  // Validate cafe data
  const validate = () => {
    const newErrors = {};
    if (cafe.CafeName.length < 6 || cafe.CafeName.length > 10) {
      newErrors.CafeName = 'Cafe Name must be between 6 and 10 characters.';
    }
    if (cafe.Description.length > 256) {
      newErrors.Description = 'Description cannot exceed 256 characters.';
    }
    if (cafe.Logo && cafe.Logo.size > 2 * 1024 * 1024) { // 2 MB limit
      newErrors.Logo = 'Logo must be less than 2 MB.';
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  // Handle input change for text fields
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setCafe((prevCafe) => ({
      ...prevCafe,
      [name]: value,
    }));
  };

  // Handle file input change and image preview
  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (file.size > 2 * 1024 * 1024) { // 2 MB limit
        setErrors((prevErrors) => ({
          ...prevErrors,
          Logo: 'Logo must be less than 2 MB.',
        }));
        return;
      }
      setCafe((prevCafe) => ({
        ...prevCafe,
        Logo: file,
      }));
      setPreviewImage(URL.createObjectURL(file));
      setErrors((prevErrors) => ({
        ...prevErrors,
        Logo: '',
      }));
    }
  };

  // Handle location dropdown change
  const handleLocationChange = (e) => {
    setCafe((prevCafe) => ({
      ...prevCafe,
      FK_LocationId: e.target.value,
    }));
    if (e.target.value !== 'new') {
      setNewLocation('');
    }
  };

  // Handle new location input change
  const handleNewLocationChange = (e) => {
    setNewLocation(e.target.value);
  };

  // Handle form submission
  const handleSubmit = (e) => {
    e.preventDefault();

    if (!validate()) {
      return;
    }

    // Define the API URL and method based on whether we're updating or adding
    const apiUrl = cafeData ? 'https://localhost:44388/api/Cafe/UpdateCafeDetails' : 'https://localhost:44388/api/Cafe/InsertCafeDetails';
    const method = cafeData ? 'PUT' : 'POST';

    // Create a FormData instance
    const formData = new FormData();

    // Append existing cafe details to FormData
    if (cafe.Id) {
      formData.append('Id', cafe.Id);
    }
    formData.append('CafeName', cafe.CafeName);
    formData.append('Description', cafe.Description);

    // Append location ID or new location to FormData
    if (cafe.FK_LocationId === 'new' && newLocation) {
      formData.append('LocationName', newLocation);
    } else {
      formData.append('FK_LocationId', cafe.FK_LocationId);
    }

    // Append logo if it exists
    if (cafe.Logo) {
      formData.append('Logo', cafe.Logo);
    }

    // Submit the form data
    axios({
      method,
      url: apiUrl,
      data: formData,
    })
      .then((response) => {
        console.log('Cafe saved:', response.data);
        onSuccess();
      })
      .catch((error) => {
        console.error('Error saving cafe:', error);
      });
  };

  return (
    <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
      <Typography variant="h4" component="h1" gutterBottom>
        {cafeData ? 'Update Cafe' : 'Add Cafe'}
      </Typography>
      <RetxtBox
        name="CafeName"
        label="Cafe Name"
        value={cafe.CafeName}
        onChange={handleInputChange}
        error={errors.CafeName}
        helperText={errors.CafeName}
        inputProps={{ maxLength: 10 }}
        sx={{ mb: 2, '& .MuiInputBase-root': { height: 40 } }} // Adjust height
      />
      <RetxtBox
        name="Description"
        label="Description"
        value={cafe.Description}
        onChange={handleInputChange}
        error={errors.Description}
        helperText={errors.Description}
        inputProps={{ maxLength: 256 }}
        sx={{ mb: 2, '& .MuiInputBase-root': { height: 40 } }} // Adjust height
      />
      <input
        type="file"
        accept="image/*"
        onChange={handleFileChange}
        style={{ marginTop: '10px', padding: '10px', height: '40px' }} // Adjust height
      />
      {errors.Logo && <div style={{ color: 'red', marginTop: '10px' }}>{errors.Logo}</div>}
      {previewImage && (
        <img
          src={previewImage}
          alt="Cafe Logo Preview"
          style={{ width: '100px', height: '100px', borderRadius: '50%', marginTop: '10px' }}
        />
      )}
      <FormControl fullWidth margin="normal">
        <InputLabel>Location</InputLabel>
        <Select
          value={cafe.FK_LocationId || ''}
          onChange={handleLocationChange}
          name="FK_LocationId"
          sx={{ height: 40 }} // Adjust height
        >
          {locations.map((location) => (
            <MenuItem key={location.Id} value={location.Id}>
              {location.LocationName}
            </MenuItem>
          ))}
          <MenuItem value="new">Add New Location</MenuItem>
        </Select>
        {cafe.FK_LocationId === 'new' && (
          <RetxtBox
            label="New Location Name"
            value={newLocation}
            onChange={handleNewLocationChange}
            required
            sx={{ mt: 2, mb: 2, '& .MuiInputBase-root': { height: 40 } }} // Adjust height
          />
        )}
      </FormControl>
      <Box mt={2}>
        <Button
          type="submit"
          variant="contained"
          color="primary"
          sx={{ mr: 1 }} // Margin Right
        >
          {cafeData ? 'Update Cafe' : 'Add Cafe'}
        </Button>
        <Button
          variant="outlined"
          color="secondary"
          onClick={onCancel}
        >
          Cancel
        </Button>
      </Box>
    </Box>
  );
};

export default AddCafe;
