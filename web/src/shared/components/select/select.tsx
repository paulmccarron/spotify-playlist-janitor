import styled from "styled-components";
import { default as ReactSelect } from "react-select";

import { BLACK, INPUT_LABEL, SELECT_INDICATOR, SELECT_MENU, WHITE } from "shared/constants";

type SelectProps = {
  value?: { label: string; value: string };
  label?: string;
  placeholder?: string;
  name?: string;
  options: { label: string; value: string }[];
  onChange?(e: any): void;
};

export const Select = ({
  value,
  label,
  placeholder = "",
  name = "select",
  options = [],
  onChange,
  ...props
}: SelectProps) => {
  return (
    <Container>
      {label && (
        <label
          className={!!value ? "filled" : ""}
          id="aria-label"
          htmlFor="aria-example-input"
        >
          {label}
        </label>
      )}
      <ReactSelect
        {...props}
        aria-labelledby="aria-label"
        inputId="aria-example-input"
        className="basic-select"
        classNamePrefix="select"
        defaultValue={undefined}
        isClearable={true}
        isSearchable={true}
        name={name}
        options={options}
        placeholder={placeholder}
        value={value}
        onChange={onChange}
      />
    </Container>
  );
};

const Container = styled.div`
  position: relative;
  background-color: ${WHITE};
  padding: 0.2rem 1rem;
  border-radius: 2rem;

  label {
    position: absolute;
    pointer-events: none;
    transform-origin: top left;
    transform: translate(0, 0.5px) scale(0.65);
    font-size: 1rem;
    font-weight: 600;
    line-height: 1;
    z-index: 1;
    color: transparent;
  }

  .filled {
    color: ${INPUT_LABEL};
  }

  select {
    background-color: ${WHITE};
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

  .select__control {
    height: 40px;
    width: 100%;
    padding-top: 2px;
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
    color: ${BLACK};
    font-weight: 600;
  }

  .select__indicator {
    color: ${SELECT_INDICATOR};
  }

  .select__indicator:hover {
    color: ${SELECT_MENU};
  }

  .select__indicator-separator {
    display: none;
  }

  .select__menu {
    color: ${SELECT_MENU};
    font-weight: 600;
  }
`;

Select.displayName = "Select";
