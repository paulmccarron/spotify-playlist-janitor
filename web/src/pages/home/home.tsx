import React from "react";
import { HEADER_HEIGHT, HEADER_PADDING } from "shared/constants";
import styled from "styled-components";

export const Home = () => {
  return (
    <Content>
      <>Home Page</>
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
  height: calc(
    100vh - ${HEADER_HEIGHT} - ${HEADER_PADDING} - ${HEADER_PADDING}
  );
`;

Home.displayName = "Home";
