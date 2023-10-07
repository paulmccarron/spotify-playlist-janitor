import React from 'react';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { HOME, CATALOGUE } from './shared/constants'
import { Home } from './pages/home'
import { Catalogue } from './pages/catalogue'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path={HOME} element={<Home />} />
        <Route path={CATALOGUE} element={<Catalogue />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
