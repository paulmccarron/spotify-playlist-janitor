import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Modal from "react-modal";
import styled from "styled-components";

import { HOME, CATALOGUE, LOGIN, BACKGROUND, REGISTER } from "shared/constants";
import { AppHeader } from "shared/components/app-header";

import { Home } from "pages/home";
import { Catalogue } from "pages/catalogue";
import { Login } from "pages/login";
import { Register } from "pages/register";

function App() {
  Modal.setAppElement("#root");
  return (
    <Container>
      <AppHeader />
      <Router>
        <Routes>
          <Route path={HOME} element={<Home />} />
          <Route path={CATALOGUE} element={<Catalogue />} />
          <Route path={LOGIN} element={<Login />} />
          <Route path={REGISTER} element={<Register />} />
        </Routes>
      </Router>
      <AppHeader />
    </Container>
  );
}

const Container = styled.div`
  width: 100vw;
  height: 100vh;
  background: linear-gradient(transparent, rgba(0, 0, 0, 1));
  background-color: ${BACKGROUND};
  display: block;
  overflow-x: hidden;
`;

export default App;
