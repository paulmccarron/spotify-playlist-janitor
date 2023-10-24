import styled from "styled-components";
import { useHomeLogic } from "./use-home-logic";
import { BLACK, GREEN } from "shared/constants";
import { SubTitle, Text } from "shared/components/typography";
import { Modal } from "shared/components/modal";
import { PrimaryButton, SecondaryButton } from "shared/components/button";
import { Select } from "shared/components/select";

export const Home = () => {
  const { monitoredPlaylists, unmonitoredPlaylists, loading, modalOpen, onModalOpen, onModalClose, onSubmit } = useHomeLogic();
  return (
    <PageContainer>
      {monitoredPlaylists &&
        <>
          {monitoredPlaylists.map(monitoredPlaylist => (
            <div key={monitoredPlaylist.id} className="item" onClick={() => alert(`Naviagte to ${monitoredPlaylist.name} at route ${monitoredPlaylist.id}`)}>
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
          <div className="item new" onClick={onModalOpen}></div>
        </>}
      <Modal
        {...{
          isOpen: modalOpen,
          onClose: onModalClose,
          label: "Test Label",
        }}
      >
        <div className="modal-content">
          <SubTitle style={{ marginBottom: 8 }}>Select a playlist to monitor</SubTitle>
          {/* <Text style={{ marginBottom: 8 }}>Modal Content 2</Text> */}
          <form {...{ onSubmit }} autoComplete="off">
            <div>
              <Select
                {...{
                  label: "Select Playlist",
                  placeholder: "Select playlist to monitor...",
                  name: "playlist_select",
                  options: unmonitoredPlaylists?.map(unmonitoredPlaylist => ({ label: unmonitoredPlaylist.name, value: unmonitoredPlaylist.id })) || [],
                }}
              />
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
                  type: 'button',
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
                  type: 'submit',
                  id: "confirm-modal-button-example",
                  "data-testid": "confirm-modal-button-example",
                }}
              >
                Confirm
              </PrimaryButton>
            </div>
          </form>
        </div>
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
  paddingTop: 18px;

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
      transform: scale(1.04);
    }
  }

  .new {
    background-color: ${GREEN};
  }

  .modal-content{

  }
`;

Home.displayName = "Home";
