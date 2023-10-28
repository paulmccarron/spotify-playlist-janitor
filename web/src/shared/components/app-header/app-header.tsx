import styled from "styled-components";

import { BLACK, GREEN, HEADER_PADDING } from "shared/constants";
import { SubTitle, Text } from "../typography";
import { useAppHeaderLogic } from "./use-app-header-logic";

const Header = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  width: 100%;
  padding: ${HEADER_PADDING} 0px;
  background-color: ${GREEN};
  color: ${BLACK};
  position: sticky;
  top: 0;
  z-index: 2;

  .header-item {
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0px 16px;
    cursor: pointer;
  }

  &:hover .sign-out {
    transform: scale(1.04);
  }
`;

export const AppHeader = () => {
  const { onHomeClick, onSignOutClick, loggedIn } = useAppHeaderLogic();
  return (
    <Header>
      <div className="header-item" onClick={onHomeClick}>
        <SubTitle {...{ id: "home", "data-testid": "home" }}>
          Spotify Playlist Janitor
        </SubTitle>
      </div>

      <div className="header-item sign-out" onClick={onSignOutClick}>
        {loggedIn && (
          <Text {...{ id: "sign-out", "data-testid": "sign-out" }}>
            Sign Out
          </Text>
        )}
      </div>
    </Header>
  );
};

AppHeader.displayName = "AppHeader";
