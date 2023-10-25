import styled from "styled-components";
import { useHomeLogic } from "./use-home-logic";
import { BLACK, GREEN, RED } from "shared/constants";
import { SecondaryText, SubTitle, Text } from "shared/components/typography";
import { Modal } from "shared/components/modal";
import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { Select } from "shared/components/select";
import { AiOutlinePlusCircle } from "react-icons/ai";
import { TextInput } from "shared/components/text-input";
import { Tooltip } from "shared/components/tooltip";
import { VscQuestion } from "react-icons/vsc";
import { Toggle } from "shared/components/toggle";

export const Home = () => {
  const {
    monitoredPlaylists,
    unmonitoredPlaylists,
    loading,
    modalOpen,
    onModalOpen,
    onModalClose,
    onSubmit,
    onPlaylistChange,
    modalError,
    showSpotifyAuthModal,
  } = useHomeLogic();

  return (
    <PageContainer>
      {monitoredPlaylists && (
        <>
          {monitoredPlaylists.map((monitoredPlaylist) => (
            <div
              key={monitoredPlaylist.id}
              className="item"
              onClick={() =>
                alert(
                  `Naviagte to ${monitoredPlaylist.name} at route ${monitoredPlaylist.id}`
                )
              }
            >
              {monitoredPlaylist.image && (
                <img
                  alt={monitoredPlaylist.name}
                  width={130}
                  height={130}
                  src={monitoredPlaylist.image.url}
                />
              )}

              <SubTitle>{monitoredPlaylist.name}</SubTitle>
            </div>
          ))}
          <div className="item new" onClick={onModalOpen}>
            <AiOutlinePlusCircle />
          </div>
        </>
      )}
      <Modal
        {...{
          isOpen: modalOpen,
          onClose: onModalClose,
          label: "Select Playlist Modal",
        }}
      >
        <form {...{ onSubmit }} autoComplete="off">
          <ModalContainer>
            <SubTitle style={{ marginBottom: 8 }}>
              Select a playlist to monitor
            </SubTitle>
            <div className="rows">
              <Select
                {...{
                  label: "Select Playlist",
                  placeholder: "Select playlist to monitor...",
                  name: "playlist_select",
                  options:
                    unmonitoredPlaylists?.map((unmonitoredPlaylist) => ({
                      label: unmonitoredPlaylist.name,
                      value: unmonitoredPlaylist.id,
                    })) || [],
                  onChange: onPlaylistChange,
                  styles: {
                    menuPortal: (base: any) => ({ ...base, zIndex: 3 }),
                  },
                  menuPortalTarget: document.body,
                }}
              />
              <div className="row">
                <Text>Skip threshold (seconds):</Text>
                <TextInput
                  {...{
                    className: "number-input",
                    type: "number",
                    defaultValue: 10,
                    min: 0,
                    max: 999,
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
                <div className="auto-delete-input">
                  <TextInput
                    {...{
                      className: "number-input",
                      type: "number",
                      min: 0,
                      max: 999,
                    }}
                  />
                </div>
                <Text>skips</Text>
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
                  id: "cancel-modal-button-example",
                  "data-testid": "cancel-modal-button-example",
                  onClick: onModalClose,
                }}
              >
                Cancel
              </SecondaryButton>
              <PrimaryButton
                {...{
                  className: "primary",
                  style: { margin: "0px 4px" },
                  type: "submit",
                  id: "confirm-modal-button-example",
                  "data-testid": "confirm-modal-button-example",
                }}
              >
                Confirm
              </PrimaryButton>
            </div>
          </ModalContainer>
        </form>
      </Modal>
      <Modal
        {...{
          isOpen: showSpotifyAuthModal,
          onClose: () => { },
          label: "Spotify Auth Modal",
        }}
      >
        <ModalContainer>
          <SubTitle style={{ marginBottom: 8 }}>
            Spotify Authentication Error
          </SubTitle>
          <Text>The application has not been authenticated with your Spotify account. Please follow <a href={process.env.REACT_APP_API_URL} target="_blank">this</a> link and sign into Spotify, and <a href=".">reload</a> this page.</Text>
        </ModalContainer>
      </Modal>
    </PageContainer>
  );
};

const PageContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  align-content: flex-start;

  color: white;
  width: 100%;
  max-width: 1825px;
  paddingtop: 18px;

  .item {
    flex: 1 1 30%; /*grow | shrink | basis */
    display: flex;
    justify-content: space-evenly;
    align-items: center;
    margin: 8px;
    height: 160px;
    max-width: 33%;
    background-color: ${BLACK};

    border-radius: 15px;

    cursor: pointer;

    &:hover {
      transform: scale(1.01);
      box-shadow: 0 0 32px rgba(117, 117, 117, 0.2);
    }
  }

  .new {
    background-color: ${GREEN};

    svg {
      height: 6em;
      width: 6em;
    }
  }
`;

const ModalContainer = styled.div`

  width:360px;

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
    margin-right: 16px;
  }

  .auto-delete-input {
    margin-right: -10px;
  }

  .error {
    color: ${RED};
  }
`;

Home.displayName = "Home";
