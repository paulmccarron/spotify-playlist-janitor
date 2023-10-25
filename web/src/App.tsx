import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { RecoilRoot } from "recoil";
import Modal from "react-modal";
import styled from "styled-components";

import { HOME, CATALOGUE, LOGIN, REGISTER } from "shared/constants";
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
                <AuthProvider shouldBeAuthorised={true} redirectPath={LOGIN}>
                  <Home />
                </AuthProvider>
              }
            />
            <Route
              path={CATALOGUE}
              element={
                <AuthProvider shouldBeAuthorised={true} redirectPath={LOGIN}>
                  <Catalogue />
                </AuthProvider>
              }
            />
            <Route
              path={LOGIN}
              element={
                <AuthProvider shouldBeAuthorised={false} redirectPath={HOME}>
                  <Login />
                </AuthProvider>
              }
            />
            <Route
              path={REGISTER}
              element={
                <AuthProvider shouldBeAuthorised={false} redirectPath={HOME}>
                  <Register />
                </AuthProvider>
              }
            />
          </Routes>
        </Router>
      </Container>
    </RecoilRoot>
  );
}

const Container = styled.div`
  display: block;
`;

export default App;
