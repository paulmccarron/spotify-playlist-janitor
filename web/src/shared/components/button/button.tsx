import React, { ButtonHTMLAttributes, DetailedHTMLProps } from "react";
import styled from "styled-components";

type ButtonProps = DetailedHTMLProps<
  ButtonHTMLAttributes<HTMLButtonElement>,
  HTMLButtonElement
>;

export const Button = ({ children, ...rest }: ButtonProps) => {
  return (
    <Container>
      <button {...rest}>{children}</button>
    </Container>
  );
};

const Container = styled.div`
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  img {
    height: 50vh;
  }
  button {
    border-radius: 5rem;
    height: 50px;
    width: 112px;
    color: black;
    font-size: 1rem;
    font-weight: 600;
    cursor: pointer;
    border: none;
    &:hover {
      transform: scale(1.04);
    }
  }
  button.primary {
    background-color: #1ed760;
  }
  button.secondary {
    background-color: white;
  }
`;
