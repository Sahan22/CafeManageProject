import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const App = () => {
  const navigate = useNavigate();

  useEffect(() => {
    // Navigate to /cafes on initial load
    navigate('/cafes');
  }, [navigate]);

  return (
    <div>
      <h1>Welcome to the Cafe App</h1>
      {/* Other content */}
    </div>
  );
};

export default App;
