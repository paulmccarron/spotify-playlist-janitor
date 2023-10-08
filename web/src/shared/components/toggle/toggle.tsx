import React from 'react';
import styled from 'styled-components';

type ToggleProps = {
    label?: string;
    checked: boolean;
    onChange(e: any): void;
}

export const Toggle = ({ label = '', checked = false, onChange }: ToggleProps) => {
    return (
        <Label>
            <span>{label}</span>
            <Input type="checkbox" checked={checked} onChange={onChange}/>
            <Switch />
        </Label>
    );
}

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
background: #b3b3b3;
border-radius: 24px;
transition: 300ms all;

&:before {
  transition: 300ms all;
  content: "";
  position: absolute;
  width: 20px;
  height: 20px;
  border-radius: 24px;
  top: 50%;
  left: 2px;
  background: white;
  transform: translate(0, -50%);
}
`;

const Input = styled.input`
opacity: 0;
position: absolute;

&:checked + ${Switch} {
  background: #1ed760;

  &:before {
    transform: translate(18px, -50%);
  }
}
`;
