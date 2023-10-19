import { HEADER_HEIGHT, HEADER_PADDING } from "shared/constants";
import styled from "styled-components";
import { Register as RegisterInitialise } from "modules/register";

export const Register = () => {
  return (
    <PageContainer>
      <RegisterInitialise />
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

Register.displayName = "Register";
