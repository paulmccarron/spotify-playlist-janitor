import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { TextInput } from "shared/components/text-input";
import {
  SecondaryText,
  Text,
  Title,
} from "shared/components/typography";
import { GREEN, RED } from "shared/constants";
import styled from "styled-components";
import { useLoginLogic } from "./use-login-logic";

export const Login = () => {
  const { onSubmit, error, onRegisterClick } = useLoginLogic();
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
              "data-testid": "email-input",
            }}
          />
        </div>
        {error && (
          <div className="row error">
            <SecondaryText>{error}</SecondaryText>
          </div>
        )}
        <div className="row">
          <PrimaryButton
            {...{
              id: "login-button",
              "data-testid": "login-button",
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
  color: white;
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
