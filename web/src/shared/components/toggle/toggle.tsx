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

// const Container = styled.div`
// background-color: white;
// padding: 0.4rem 1rem;
// border-radius: 2rem;
// display: flex;
// align-items: center;
// gap: 0.5rem;
// input {
//   font-size: 1rem;
//   font-weight: 600;
//   border: none;
//   height: 2rem;
//   width: 100%;
//   &:focus {
//     outline: none;
//   }
// }
// `;

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
width: 60px;
height: 28px;
background: #b3b3b3;
border-radius: 32px;
padding: 4px;
transition: 300ms all;

&:before {
  transition: 300ms all;
  content: "";
  position: absolute;
  width: 28px;
  height: 28px;
  border-radius: 35px;
  top: 50%;
  left: 4px;
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
    transform: translate(32px, -50%);
  }
}
`;
