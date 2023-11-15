import { PrimaryButton } from "shared/components/button";
import { TextInput } from "shared/components/text-input";
import { SecondaryText, Text, Title } from "shared/components/typography";
import { RED, WHITE } from "shared/constants";
import styled from "styled-components";
import { useRegisterLogic } from "./use-register-logic";
import { Tooltip } from "shared/components/tooltip";
import { VscQuestion } from "react-icons/vsc";

export const Register = () => {
  const { disabled, onSubmit, error } = useRegisterLogic();
  return (
    <PageContainer>
      <div className="row">
        <Title>Register</Title>
      </div>
      <div className="row">
        <Text>Register with email/password:</Text>
      </div>
      <form {...{ onSubmit }} autoComplete="off">
        <div className="row">
          <TextInput
            {...{
              name: "email",
              label: "User Email",
              placeholder: "Enter user email...",
              id: "email-input",
              "data-testid": "email-input",
              disabled,
            }}
          />
          <span className="tooltip" />
        </div>
        <div className="row">
          <TextInput
            {...{
              name: "spotifyEmail",
              label: "Spotify Email",
              placeholder: "Enter Spotify email...",
              id: "spotify-email-input",
              "data-testid": "spotify-email-input",
              disabled,
            }}
          />
          <span className="tooltip">
            <Tooltip
              content={
                <>
                  The email address for the Spotify account you would like to
                  track with this user. <br /> If left blank it will default to
                  the user email address.
                </>
              }
              dataTooltipId="spotify-email-tooltip"
            >
              <VscQuestion />
            </Tooltip>
          </span>
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
          <span className="tooltip" />
        </div>
        <div className="row">
          <TextInput
            {...{
              name: "passwordConfirm",
              type: "password",
              label: "Confirm Password",
              placeholder: "Confirm password...",
              id: "password-confirm-input",
              "data-testid": "password-confirm-input",
              disabled,
            }}
          />
          <span className="tooltip" />
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
              id: "register-button",
              "data-testid": "register-button",
              disabled,
            }}
          >
            Register
          </PrimaryButton>
        </div>
      </form>
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
    width: 400px;
  }

  .row {
    margin-bottom: 8px;
    display: flex;
    justify-content: center;
    align-items: center;

    div {
      width: 260px;
      display: flex;
    }
  }

  .tooltip {
    width: 0px;
    margin-left: 8px;
  }

  .error {
    color: ${RED};
  }
`;

Register.displayName = "Register";
