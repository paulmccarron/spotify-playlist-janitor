import styled from "styled-components";

import { GREEN, TOGGLE_BACKGROUND, WHITE } from "shared/constants";
import { DetailedHTMLProps, InputHTMLAttributes } from "react";

// type ToggleProps = {
//   label?: string;
//   checked: boolean;
//   onChange(e: any): void;
// };

type ToggleProps = DetailedHTMLProps<
  InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
> & {
  label?: string;
};

const Label = styled.label`
  display: flex;
  align-items: center;
  gap: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
`;

const Switch = styled.div`
  position: relative;
  width: 42px;
  height: 24px;
  background: ${TOGGLE_BACKGROUND};
  border-radius: 24px;
  transition: 150ms all;

  &:before {
    transition: 150ms all;
    content: "";
    position: absolute;
    width: 20px;
    height: 20px;
    border-radius: 24px;
    top: 50%;
    left: 2px;
    background: ${WHITE};
    transform: translate(0, -50%);
  }
`;

const Input = styled.input`
  opacity: 0;
  position: absolute;

  &:checked + ${Switch} {
    background: ${GREEN};

    &:before {
      transform: translate(18px, -50%);
    }
  }
`;

export const Toggle = ({ label = "", ...props }: ToggleProps) => {
  return (
    <Label>
      <span>{label}</span>
      <Input type="checkbox" {...props} />
      <Switch />
    </Label>
  );
};

Toggle.displayName = "Toggle";
