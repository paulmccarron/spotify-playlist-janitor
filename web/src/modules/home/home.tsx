import styled from "styled-components";
import { useHomeLogic } from "./use-home-logic";
import { BLACK, GREEN } from "shared/constants";
import { SubTitle, Text } from "shared/components/typography";
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
          label: "Test Label",
        }}
      >
        <ModalContainer>
          <SubTitle style={{ marginBottom: 8 }}>
            Select a playlist to monitor
          </SubTitle>
          <form {...{ onSubmit }} autoComplete="off">
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
                      className: "toggle-input",
                      label: "Toggle Example",
                      id: "test-toggle",
                      "data-testid": "test-toggle",
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
                    type: "number",
                  }}
                />
                <Text>skips</Text>
              </div>
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
          </form>
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
  .rows {
    margin-bottom: 16px;
  }

  .row {
    display: flex;
    flex-direction: row;
    align-items: center;
  }

  .number-input {
    width: 36px !important;
  }

  .toggle-input {
    margin-right: 24px;
  }
`;

Home.displayName = "Home";
