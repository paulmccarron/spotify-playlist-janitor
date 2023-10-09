import React, { DetailedHTMLProps, InputHTMLAttributes } from "react";
import styled from "styled-components";

type TextInputProps = DetailedHTMLProps<
  InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
>;

export const TextInput = ({ ...props }: TextInputProps) => {
  return (
    <Container>
      <input {...props} />
    </Container>
  );
};

const Container = styled.div`
  background-color: white;
  padding: 0.4rem 1rem;
  border-radius: 2rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  input {
    font-size: 1rem;
    font-weight: 600;
    border: none;
    height: 2rem;
    width: 100%;
    &:focus {
      outline: none;
    }
  }
`;
