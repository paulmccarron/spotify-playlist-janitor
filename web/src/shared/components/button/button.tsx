import { ButtonHTMLAttributes, DetailedHTMLProps } from "react";
import styled from "styled-components";

import { GREEN } from "shared/constants";

type ButtonProps = DetailedHTMLProps<
  ButtonHTMLAttributes<HTMLButtonElement>,
  HTMLButtonElement
>;

const StyledButton = styled.button`
  border-radius: 5rem;
  height: 50px;
  min-width: 112px;
  color: black;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  border: none;

  &:hover {
    transform: scale(1.04);
  }
`;

const StyledPrimaryButton = styled(StyledButton)`
  background-color: ${GREEN};
`;

const StyledSecondaryButton = styled(StyledButton)`
  background-color: white;
`;

export const PrimaryButton = ({ children, ...rest }: ButtonProps) => {
  return <StyledPrimaryButton {...rest}>{children}</StyledPrimaryButton>;
};

PrimaryButton.displayName = "PrimaryButton";

export const SecondaryButton = ({ children, ...rest }: ButtonProps) => {
  return <StyledSecondaryButton {...rest}>{children}</StyledSecondaryButton>;
};

SecondaryButton.displayName = "SecondaryButton";
