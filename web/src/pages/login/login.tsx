import styled from "styled-components";
import { Login as LoginInitialise } from "modules/login";
import { HEADER_HEIGHT, HEADER_PADDING } from "shared/constants";

export const Login = () => {
  return (
    <PageContainer>
      <LoginInitialise />
    </PageContainer>
  );
};

const PageContainer = styled.div`
  display: flex;
  justify-content: center;
  height: calc(
    100vh - ${HEADER_HEIGHT} - ${HEADER_PADDING} - ${HEADER_PADDING}
  );
  width: 400px;
  margin: auto;
`;

Login.displayName = "Login";
