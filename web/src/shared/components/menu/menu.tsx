import { Menu as ReactMenu, MenuProps } from "@szhsin/react-menu";
import "@szhsin/react-menu/dist/index.css";
import styled from "styled-components";

import { BLACK, TABLE_HOVER, WHITE } from "shared/constants";

export { MenuItem } from "@szhsin/react-menu";

const StyledMenu = styled(ReactMenu)`
  ul {
    color: ${WHITE};
    background-color: ${BLACK};
  }

  li.szh-menu__item--hover {
    background-color: ${TABLE_HOVER};
  }
`;

export const Menu = ({ children, ...props }: MenuProps) => {
  return <StyledMenu {...props}>{children}</StyledMenu>;
};

Menu.displayName = "Menu";
