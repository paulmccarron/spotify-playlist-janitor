import styled from "styled-components";
import { HEADER_HEIGHT, HEADER_PADDING } from "shared/constants";
import { Home as HomeInitialise } from "modules/home";

export const Home = () => {
  return (
    <Content>
      <HomeInitialise />
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  justify-content: center;
  height: calc(
    100vh - ${HEADER_HEIGHT} - ${HEADER_PADDING} - ${HEADER_PADDING}
  );
`;

Home.displayName = "Home";
