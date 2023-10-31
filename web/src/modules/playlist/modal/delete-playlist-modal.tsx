import styled from "styled-components";
import { RED } from "shared/constants";
import { Text, SecondaryText, SubTitle } from "shared/components/typography";
import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { DetailedPlaylist } from "../playlist-types";

type DeletePlaylistModalViewProps = {
  onSubmit(event: React.FormEvent<HTMLFormElement>): Promise<void>;
  playlist: DetailedPlaylist | undefined;
  modalSaving: boolean;
  modalError?: string;
  onModalClose(): void;
};

export const DeletePlaylistModalView = ({
  onSubmit,
  playlist,
  modalSaving,
  modalError,
  onModalClose,
}: DeletePlaylistModalViewProps) => {
  return (
    <form {...{ onSubmit }} autoComplete="off">
      <ModalContainer>
        <SubTitle style={{ marginBottom: 8 }}>
          Remove playlist
        </SubTitle>
        <Text style={{ marginBottom: 8 }}>
          Are you sure you want to remove {playlist?.name} from monitoring?
          This will remove all skiiped track records for this playlist.
        </Text>
        <div className="rows">
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

DeletePlaylistModalView.displayName = "DeletePlaylistModalView";
