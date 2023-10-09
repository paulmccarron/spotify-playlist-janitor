import React from "react";
import styled from "styled-components";
import { default as ReactSelect } from "react-select";

type SelectProps = {
  value?: { label: string; value: string };
  placeholder?: string;
  options: { label: string; value: string }[];
  onChange(e: any): void;
};

export const Select = ({
  value,
  placeholder = "",
  options = [],
  onChange,
}: SelectProps) => {
  return (
    <Container>
      <StyledSelect
        className="basic-select"
        classNamePrefix="select"
        defaultValue={undefined}
        isClearable={true}
        isSearchable={true}
        name="color"
        options={options}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
      />
    </Container>
  );
};

const Container = styled.div`
  background-color: white;
  padding: 0.2rem 1rem;
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

  .basic-select {
    width: 100%;
  }
`;

const StyledSelect = styled(ReactSelect)`
  .select__control {
    height: 40px;
    width: 100%;
    border: none;
    border-radius: 0;
    cursor: pointer;
  }

  .select__value-container {
    font-size: 1rem;
    font-weight: 600;
    padding: 2px 0px;
  }

  .select__control:hover {
    border-color: transparent;
  }

  .select__control--is-focused {
    box-shadow: none;
    outline: none;
  }

  .select__single-value {
    color: black;
    font-weight: 600;
  }

  .select__indicator {
    color: #808080;
  }

  .select__indicator:hover {
    color: #3c3d3e;
  }

  .select__indicator-separator {
    display: none;
  }

  .select__menu {
    color: #3c3d3e;
    font-weight: 600;
  }
`;
