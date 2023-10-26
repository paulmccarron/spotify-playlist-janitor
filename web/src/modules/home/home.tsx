import styled from "styled-components";
import { AiOutlinePlusCircle } from "react-icons/ai";

import { BLACK, GREEN, RED } from "shared/constants";
import { SubTitle, Text } from "shared/components/typography";
import { Modal } from "shared/components/modal";

import { useHomeLogic } from "./use-home-logic";
import { AddPlaylistModalView } from "./modal/add-playlist-modal";

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
    modalSaving,
    showSpotifyAuthModal,
  } = useHomeLogic();

  return (
    <PageContainer>
      {monitoredPlaylists && (
        <>
          {monitoredPlaylists.map((monitoredPlaylist, index) => (
            <div
              key={monitoredPlaylist.id}
              className="item"
              onClick={() =>
                alert(
                  `Naviagte to ${monitoredPlaylist.name} at route ${monitoredPlaylist.id}`
                )
              }
              id={`playlist-item-${index}`}
              data-testid={`playlist-item-${index}`}
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
          <div
            className="item new"
            onClick={onModalOpen}
            id={`add-playlist-item`}
            data-testid={`add-playlist-item`}
          >
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
        <AddPlaylistModalView
          {...{
            onSubmit,
            unmonitoredPlaylists,
            onPlaylistChange,
            modalSaving,
            modalError,
            onModalClose,
          }}
        />
      </Modal>
      <Modal
        {...{
          isOpen: showSpotifyAuthModal,
          onClose: () => {},
          label: "Spotify Auth Modal",
        }}
      >
        <ModalContainer>
          <SubTitle style={{ marginBottom: 8 }}>
            Spotify Authentication Error
          </SubTitle>
          <Text>
            The application has not been authenticated with your Spotify
            account. Please follow{" "}
            <a
              href={process.env.REACT_APP_API_URL}
              target="_blank"
              rel="noreferrer"
            >
              this
            </a>{" "}
            link and sign into Spotify, and <a href=".">reload</a> this page.
          </Text>
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
    margin-right: 16px;
  }

  .auto-delete-input {
    margin-right: -10px;
  }

  .auto-delete-post-text {
    margin-right: 10px;
  }

  .error {
    color: ${RED};
  }
`;

Home.displayName = "Home";
