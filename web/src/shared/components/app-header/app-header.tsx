import React from "react";
import styled from "styled-components";
import { SubTitle } from "../typography";

const Header = styled.div`
  width: 100%;
  padding: 2px 0px;
  background-color: #1ed760;
  color: black;
  position: sticky;
  top: 0;
  z-index: 2;
`;

export const AppHeader = () => {
  return (
    <Header>
      <SubTitle>Spotify Playlist Janitor</SubTitle>
    </Header>
  );
};

AppHeader.displayName = "AppHeader";
