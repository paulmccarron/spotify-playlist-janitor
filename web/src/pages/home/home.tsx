import React from "react";
import styled from "styled-components";

export const Home = () => {
  return (
    <Content>
      <>API URL: {process.env.REACT_APP_API_URL}</>
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  color: white;
`;
