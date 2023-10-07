import React from 'react';
import styled from 'styled-components';

type TextInputProps = {
    type?: string;
    placeholder?: string;
}

export const TextInput = ({ type = 'text', placeholder = '' }: TextInputProps) => {
    return (
        <Container>
            <input {...{type, placeholder}} />
        </Container>
    );
}

const Container = styled.div`
background-color: white;
width: 30%;
padding: 0.4rem 1rem;
border-radius: 2rem;
display: flex;
align-items: center;
gap: 0.5rem;
input {
  border: none;
  height: 2rem;
  width: 100%;
  &:focus {
    outline: none;
  }
}
`;