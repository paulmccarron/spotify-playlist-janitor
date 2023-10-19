import styled from "styled-components";

import { GREEN, HEADER_PADDING } from "shared/constants";
import { SubTitle } from "../typography";
import { useAppHeaderLogic } from "./use-app-header-logic";

const Header = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  width: 100%;
  padding: ${HEADER_PADDING} 0px;
  background-color: ${GREEN};
  color: black;
  position: sticky;
  top: 0;
  z-index: 2;

  .header-item {
    width: 80;
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0px 16px;
    cursor: pointer;
  }
`;

export const AppHeader = () => {
  const { onHomeClick, onSignOutClick, loggedIn } = useAppHeaderLogic();
  return (
    <Header>
      <div className="header-item" onClick={onHomeClick}>
        <SubTitle>Spotify Playlist Janitor</SubTitle>
      </div>

      <div className="header-item" onClick={onSignOutClick}>
        {loggedIn && "Sign Out"}
      </div>
    </Header>
  );
};

AppHeader.displayName = "AppHeader";
