import React from 'react';
import styled from 'styled-components';

type TextInputProps = {
    type?: string;
    placeholder?: string;
    value?: string | number;
    onChange(e: any): void;
}

export const TextInput = ({ type = 'text', placeholder = '', value = '', onChange }: TextInputProps) => {
    return (
        <Container>
            <input {...{type, placeholder, value, onChange}} />
        </Container>
    );
}

const Container = styled.div`
background-color: white;
padding: 0.4rem 1rem;
border-radius: 2rem;
display: flex;
align-items: center;
gap: 0.5rem;
input {
  font-size: 1rem;
  border: none;
  height: 2rem;
  width: 100%;
  &:focus {
    outline: none;
  }
}
`;