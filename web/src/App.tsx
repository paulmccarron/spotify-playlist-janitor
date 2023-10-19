import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { RecoilRoot } from "recoil";
import Modal from "react-modal";
import styled from "styled-components";

import { HOME, CATALOGUE, LOGIN, BACKGROUND, REGISTER } from "shared/constants";
import { AppHeader } from "shared/components/app-header";
import { Home } from "pages/home";
import { Catalogue } from "pages/catalogue";
import { Login } from "pages/login";
import { Register } from "pages/register";
import { AuthProvider } from "shared/components/auth-provider";

function App() {
  Modal.setAppElement("#root");
  return (
    <RecoilRoot>
      <Container>
        <Router>
          <AppHeader />
          <Routes>
            <Route
              path={HOME}
              element={
                <AuthProvider>
                  <Home />
                </AuthProvider>
              }
            />
            <Route
              path={CATALOGUE}
              element={
                <AuthProvider>
                  <Catalogue />
                </AuthProvider>
              }
            />
            <Route path={LOGIN} element={<Login />} />
            <Route path={REGISTER} element={<Register />} />
          </Routes>
        </Router>
      </Container>
    </RecoilRoot>
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
