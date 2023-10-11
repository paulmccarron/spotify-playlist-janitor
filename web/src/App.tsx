import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { HOME, CATALOGUE } from "./shared/constants";
import { Home } from "./pages/home";
import { Catalogue } from "./pages/catalogue";
import styled from "styled-components";
import { AppHeader } from "./shared/components/app-header";
import Modal from "react-modal";

Modal.setAppElement('#root');

function App() {
  return (
    <Container>
      <AppHeader />
      <Router>
        <Routes>
          <Route path={HOME} element={<Home />} />
          <Route path={CATALOGUE} element={<Catalogue />} />
        </Routes>
      </Router>
    </Container>
  );
}

const Container = styled.div`
  width: 100vw;
  height: 100vh;
  background: linear-gradient(transparent, rgba(0, 0, 0, 1));
  background-color: #121212;
  display: block;
  overflow-x: hidden;
`;

export default App;
