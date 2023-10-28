import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { TextInput } from "shared/components/text-input";
import { SecondaryText, Text, Title } from "shared/components/typography";
import { GREEN, RED, WHITE } from "shared/constants";
import styled from "styled-components";
import { useLoginLogic } from "./use-login-logic";

export const Login = () => {
  const { disabled, onSubmit, error, onRegisterClick } = useLoginLogic();
  return (
    <PageContainer>
      <div className="row">
        <Title>Login</Title>
      </div>
      <div className="row">
        <Text>Log in with email/password:</Text>
      </div>
      <form {...{ onSubmit }} autoComplete="off">
        <div className="row">
          <TextInput
            {...{
              name: "email",
              label: "Email",
              placeholder: "Enter email...",
              id: "email-input",
              "data-testid": "email-input",
              disabled,
            }}
          />
        </div>
        <div className="row">
          <TextInput
            {...{
              name: "password",
              type: "password",
              label: "Password",
              placeholder: "Enter password...",
              id: "password-input",
              "data-testid": "password-input",
              disabled,
            }}
          />
        </div>
        {error && (
          <div
            {...{
              className: "row error",
              id: "submit-error",
              "data-testid": "submit-error",
            }}
          >
            <SecondaryText>{error}</SecondaryText>
          </div>
        )}
        <div className="row">
          <PrimaryButton
            {...{
              id: "login-button",
              "data-testid": "login-button",
              disabled,
            }}
          >
            Log In
          </PrimaryButton>
        </div>
      </form>
      <div>
        <SecondaryButton
          {...{
            id: "register-button",
            "data-testid": "register-button",
            onClick: onRegisterClick,
            disabled,
          }}
        >
          Register
        </SecondaryButton>
      </div>
    </PageContainer>
  );
};

const PageContainer = styled.div`
  display: flex;
  flex-direction: column;
  color: ${WHITE};
  justify-content: center;
  align-items: center;

  form {
    margin-bottom: 8px;
    border-bottom: 1px solid ${GREEN};
    width: 400px;
  }

  .row {
    margin-bottom: 8px;
    display: flex;
    justify-content: center;

    div {
      width: 260px;
      display: flex;
    }
  }

  .error {
    color: ${RED};
  }
`;

Login.displayName = "Login";
