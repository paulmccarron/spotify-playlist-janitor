import { Menu as ReactMenu, MenuProps } from "@szhsin/react-menu";
import "@szhsin/react-menu/dist/index.css";
import styled from "styled-components";

export const Menu = ({ children, ...props }: MenuProps) => {
  return <StyledMenu {...props}>{children}</StyledMenu>;
};

const StyledMenu = styled(ReactMenu)`
  ul {
    color: white;
    background-color: black;
  }

  li.szh-menu__item--hover {
    background-color: #232323;
  }
`;

export { MenuItem } from "@szhsin/react-menu";
