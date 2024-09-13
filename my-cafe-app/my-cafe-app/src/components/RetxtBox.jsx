// RetxtBox.js
import React from 'react';
import { TextField } from '@mui/material';

const RetxtBox = ({ name, label, value, onChange, error, helperText, ...props }) => {
  return (
    <TextField
      name={name}
      label={label}
      value={value}
      onChange={onChange}
      fullWidth
      margin="normal"
      error={Boolean(error)}
      helperText={helperText}
      {...props}
    />
  );
};

export default RetxtBox;
