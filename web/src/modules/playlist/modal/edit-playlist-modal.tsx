import styled from "styled-components";
import { RED } from "shared/constants";
import { SecondaryText, SubTitle, Text } from "shared/components/typography";
import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { TextInput } from "shared/components/text-input";
import { Tooltip } from "shared/components/tooltip";
import { VscQuestion } from "react-icons/vsc";
import { Toggle } from "shared/components/toggle";
import { DetailedPlaylist } from "../playlist-types";

type EditPlaylistModalViewProps = {
  onSubmit(event: React.FormEvent<HTMLFormElement>): Promise<void>;
  playlist: DetailedPlaylist | undefined;
  modalSaving: boolean;
  modalError?: string;
  onModalClose(): void;
};

export const EditPlaylistModalView = ({
  onSubmit,
  playlist,
  modalSaving,
  modalError,
  onModalClose,
}: EditPlaylistModalViewProps) => {
  return (
    <form {...{ onSubmit }} autoComplete="off">
      <ModalContainer>
        <SubTitle style={{ marginBottom: 8 }}>
          Edit playlist details
        </SubTitle>
        <div className="rows">
          <div className="row">
            <Text>Skip threshold (seconds):</Text>
            <TextInput
              {...{
                className: "number-input",
                id: "skip-threshold-input",
                "data-testid": "skip-threshold-input",
                type: "number",
                defaultValue: playlist?.skipThreshold,
                min: 1,
                max: 999,
                varaint: "boxed",
                disabled: modalSaving,
              }}
            />
            <Tooltip
              content={
                "The track progress before which a change will be counted as a skip."
              }
              dataTooltipId="skip-threshold-tooltip"
            >
              <VscQuestion />
            </Tooltip>
          </div>
          <div className="row">
            <div className="toggle-input">
              <Toggle
                {...{
                  label: "Ignore initial skips",
                  id: "ignore-intial-skips-toggle",
                  "data-testid": "ignore-intial-skips-toggle",
                  disabled: modalSaving,
                  defaultChecked: playlist?.ignoreInitialSkips
                }}
              />
            </div>
            <Tooltip
              content={
                "Ignore any skips that occur when listening first begins until a song has exceeded the Skip Threshold, any skip after that will be counted."
              }
              dataTooltipId="initial-skips-tooltip"
            >
              <VscQuestion />
            </Tooltip>
          </div>
          <div className="row">
            <Text>Auto-delete tracks after:</Text>
              <TextInput
                {...{
                  className: "number-input",
                  id: "auto-delete-input",
                  "data-testid": "auto-delete-input",
                  type: "number",
                  min: 0,
                  max: 999,
                  varaint: "boxed",
                  disabled: modalSaving,
                  defaultValue: playlist?.autoCleanupLimit
                }}
              />
            <Text className="auto-delete-post-text">skips</Text>
            <Tooltip
              content={
                "Can be left blank to prevent automatic deletion of any tracks."
              }
              dataTooltipId="aut-delete-tooltip"
            >
              <VscQuestion />
            </Tooltip>
          </div>
          {modalError && (
            <div
              {...{
                className: "row error",
                id: "modal-error",
                "data-testid": "modal-error",
              }}
            >
              <SecondaryText>{modalError}</SecondaryText>
            </div>
          )}
        </div>
        <div
          style={{
            display: "flex",
            flexDirection: "row",
            justifyContent: "flex-end",
          }}
        >
          <SecondaryButton
            {...{
              className: "secondary",
              style: { margin: "0px 4px" },
              type: "button",
              id: "cancel-modal-button",
              "data-testid": "cancel-modal-button",
              onClick: onModalClose,
              disabled: modalSaving,
            }}
          >
            Cancel
          </SecondaryButton>
          <PrimaryButton
            {...{
              className: "primary",
              style: { margin: "0px 4px" },
              type: "submit",
              id: "confirm-modal-button",
              "data-testid": "confirm-modal-button",
              disabled: modalSaving,
            }}
          >
            Confirm
          </PrimaryButton>
        </div>
      </ModalContainer>
    </form>
  );
};

const ModalContainer = styled.div`
  width: 360px;

  .rows {
    margin-bottom: 8px;
  }

  .row {
    display: flex;
    flex-direction: row;
    align-items: center;
    margin-bottom: 8px;
  }

  .number-input {
    width: 55px !important;
  }

  .toggle-input {
    margin-right: 10px;
  }

  .auto-delete-post-text {
    margin-right: 10px;
  }

  .error {
    color: ${RED};
  }
`;

EditPlaylistModalView.displayName = "EditPlaylistModalView";
