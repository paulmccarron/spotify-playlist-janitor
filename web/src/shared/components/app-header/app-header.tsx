import React from 'react';
import styled from 'styled-components';

export const AppHeader = () => {
  return <Header>Spotify Playlist Janitor</Header>;
};

const Header = styled.div`
  width: 100%;
  height: 24px;
  background-color: #1ed760;
  color: black;
`;
