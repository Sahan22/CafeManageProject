// src/api.js
const API_BASE_URL = 'http://localhost:44388/api'; // Adjust based on your API base URL

export const getAllCafeDetails = async (locationId) => {
  const response = await fetch(`${API_BASE_URL}/Cafe/GetAllCafeDetails?locationId=${locationId}`);
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
};

export const insertCafeDetails = async (cafeDetails) => {
  const response = await fetch(`${API_BASE_URL}/Cafe/InsertCafeDetails`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(cafeDetails),
  });
  if (!response.ok) {
    throw new Error('Network response was not ok');
  }
  return response.json();
};
