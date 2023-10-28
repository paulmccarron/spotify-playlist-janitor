import { useCallback } from "react";
import styled from "styled-components";
import { AiOutlinePlusCircle } from "react-icons/ai";

import { BLACK, GREEN, GREEN_DISABLED, RED } from "shared/constants";
import { SubTitle, Text } from "shared/components/typography";
import { Modal } from "shared/components/modal";

import { useHomeLogic } from "./use-home-logic";
import { AddPlaylistModalView } from "./modal/add-playlist-modal";
import { Skeleton, SkeletonTheme } from "shared/components/skeleton";
import { SpotifyAuthModalView } from "./modal";

export const Home = () => {
  const {
    monitoredPlaylists,
    unmonitoredPlaylists,
    loading,
    loadingSkeletons,
    onPlaylistClick,
    modalOpen,
    onModalOpen,
    onModalClose,
    onSubmit,
    onPlaylistChange,
    modalError,
    modalSaving,
    showSpotifyAuthModal,
  } = useHomeLogic();

  const addButton = useCallback(({ disabled }: { disabled: boolean }) =>
    <div
      className={`item new ${disabled ? "disabled" : ""}`}
      onClick={onModalOpen}
      id={`add-playlist-item`}
      data-testid={`add-playlist-item`}
    >
      <AiOutlinePlusCircle />
    </div>
    , [onModalOpen])

  return (
    <PageContainer>
      {loading &&
        <>
          {!loadingSkeletons && <div data-testid="empty-loading" />}
          {loadingSkeletons && loadingSkeletons?.map(loadingSkeleton =>
            <div
              key={loadingSkeleton}
              className="skelton"
              id={`playlist-item-skeleton-${loadingSkeleton}`}
              data-testid={`playlist-item-skeleton-${loadingSkeleton}`}
            >
              <SkeletonTheme baseColor="#020202" highlightColor="#444" height="100%" borderRadius={15}>
                <Skeleton />
              </SkeletonTheme>
            </div>
          )}
          {addButton({ disabled: true })}
        </>
      }
      {!loading && monitoredPlaylists && (
        <>
          {monitoredPlaylists.map((monitoredPlaylist, index) => (
            <div
              key={monitoredPlaylist.id}
              className="item"
              onClick={() => onPlaylistClick(monitoredPlaylist.id)}
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
          {addButton({ disabled: false })}
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
          onClose: () => { },
          label: "Spotify Auth Modal",
        }}
      >
        <SpotifyAuthModalView/>
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

  .skelton {
    flex: 1 1 30%; /*grow | shrink | basis */
    margin: 8px;
    height: 160px;
    max-width: 33%;
    border-radius: 15px;
  }

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

  .disabled {
    background-color: ${GREEN_DISABLED};
    pointer-events: none;
    cursor: default;

    &:hover {
      transform: none;
      box-shadow: none;
    }
  }
`;

Home.displayName = "Home";
