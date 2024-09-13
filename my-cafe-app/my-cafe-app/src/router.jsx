import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './components/Home';
import Cafes from './components/Cafes';
import Employee from './components/Employee';

const RouterConfig = () => (
  <Router>
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/cafes" element={<Cafes />} />
      <Route path="/employees" element={<Employee />} />
    </Routes>
  </Router>
);

export default RouterConfig;
