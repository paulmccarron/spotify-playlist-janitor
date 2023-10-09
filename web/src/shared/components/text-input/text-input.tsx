import React, { DetailedHTMLProps, InputHTMLAttributes } from "react";
import styled from "styled-components";

type TextInputProps = DetailedHTMLProps<
  InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
> & {
  label?: string;
};

export const TextInput = ({ label, ...props }: TextInputProps) => {
  return (
    <Container>
      <div className="input-container">
        <input {...props} />
        {label && (
          <label className={!!props.value ? "filled" : ""} htmlFor={props.id}>
            {label}
          </label>
        )}
      </div>
    </Container>
  );
};

const Container = styled.div`
  background-color: white;
  padding: 0.5rem 1rem 0.3rem 1rem;
  border-radius: 2rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;

  .input-container {
    position: relative;
    display: flex;
    flex-direction: column;
    width: 100%;
  }

  .input-container label {
    position: absolute;
    pointer-events: none;
    transform: translate(0, 8px) scale(1);
    transform-origin: top left;
    font-size: 1rem;
    font-weight: 600;
    line-height: 1;
    color: transparent;
  }

  .input-container .filled,
  .input-container:focus-within label {
    transform: translate(0, -3px) scale(0.65);
    font-size: 1rem;
    font-weight: 600;
    color: #757575;
  }

  .input-container input {
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
