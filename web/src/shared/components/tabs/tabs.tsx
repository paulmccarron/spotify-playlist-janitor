import styled from "styled-components";
import { Tabs as ReactTabs, TabsProps } from "react-tabs";
import "react-tabs/style/react-tabs.css";

import { GREEN } from "shared/constants";

export { Tab, TabList, TabPanel } from "react-tabs";

const StyledTabs = styled(ReactTabs)`
  .react-tabs__tab-list {
    border-bottom: 2px solid #1ed760;
  }

  .react-tabs__tab {
    display: inline-block;
    border: 1px solid transparent;
    border-bottom: none;
    bottom: -1px;
    position: relative;
    list-style: none;
    padding: 6px 12px;
    cursor: pointer;
    color: ${GREEN};
    font-size: 1rem;
    font-weight: 600;
    min-width: 87px;
    text-align: center;
  }

  .react-tabs__tab--selected {
    background: ${GREEN};
    border-color: ${GREEN};
    color: black;
    border-radius: 15px 15px 0px 0px;
    font-size: 1rem;
    font-weight: 600;
    min-width: 87px;
    text-align: center;
  }

  .react-tabs__tab:focus:after {
    background: transparent;
  }
`;

export const Tabs = ({ children, ...props }: TabsProps) => {
  return <StyledTabs {...props}>{children}</StyledTabs>;
};

Tabs.displayName = "Tabs";
