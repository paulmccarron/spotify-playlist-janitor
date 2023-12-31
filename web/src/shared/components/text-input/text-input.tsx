import { DetailedHTMLProps, InputHTMLAttributes } from "react";
import styled from "styled-components";

import { DISABLED, INPUT_LABEL, WHITE } from "shared/constants";

type TextInputVaraint = "normal" | "boxed";

type TextInputProps = DetailedHTMLProps<
  InputHTMLAttributes<HTMLInputElement>,
  HTMLInputElement
> & {
  label?: string;
  varaint?: TextInputVaraint;
};

export const TextInput = ({ label, varaint = "normal", ...props }: TextInputProps) => {
  return (
    <StyledTextInput disabled={props.disabled} varaint={varaint}>
      <div className="input-container">
        <input {...props} />
        {label && (
          <label className={!!props.value ? "filled" : ""} htmlFor={props.id}>
            {label}
          </label>
        )}
      </div>
    </StyledTextInput>
  );
};

TextInput.displayName = "TextInput";

const StyledTextInput = styled.div<{ disabled?: boolean, varaint?: TextInputVaraint }>`
  background-color: ${(props) => (props.disabled ? DISABLED : WHITE)};
  padding: ${(props) => (props.varaint === "normal" ? "0.5rem 1rem 0.3rem 1rem" : "0.5rem 0.5rem 0.3rem 0.3rem")};
  border-radius: ${(props) => (props.varaint === "normal" ? "2rem" : "0.3rem")};

  &:disabled {
    opacity: 1;
  }

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
    color: ${INPUT_LABEL};
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
