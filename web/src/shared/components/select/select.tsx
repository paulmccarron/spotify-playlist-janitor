import React from "react";
import styled from "styled-components";

type SelectProps = {
  value: string;
  placeholder?: string;
  options: { label: string; value: string }[];
  onChange(e: any): void;
};

export const Select = ({
  value = "",
  placeholder = "",
  options = [],
  onChange,
}: SelectProps) => {
  var mappedOptions = options.map((option) => (
    <option value={option.value}>{option.label}</option>
  ));
  return (
    <Container>
      <select {...{ value, placeholder, onChange }}>{mappedOptions}</select>
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
  select {
    background-color: white;
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
