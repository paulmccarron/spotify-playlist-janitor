import styled from "styled-components";
import {
    SKELETON_PLAYLIST_BASE,
    SKELETON_HIGHLIGHT,
    WHITE,
} from "shared/constants";
import { usePlaylistLogic } from "./use-playlist-logic";
import { Menu, MenuItem } from "shared/components/menu";
import { VscEdit, VscEllipsis, VscLinkExternal, VscTrash } from "react-icons/vsc";
import { SecondaryText, Text, Title } from "shared/components/typography";
import { Skeleton, SkeletonTheme } from "shared/components/skeleton";
import { Modal } from "shared/components/modal";
import { EditPlaylistModalView } from "./modal";
import { DeletePlaylistModalView } from "./modal/delete-playlist-modal";
import { PlaylistTabs } from "./tabs/playlist-tabs";


type PlaylistProps = {
    id: string;
};

const skeleton = <SkeletonTheme
    baseColor={SKELETON_PLAYLIST_BASE}
    highlightColor={SKELETON_HIGHLIGHT}
    height="100%"
>
    <Skeleton />
</SkeletonTheme>;

export const Playlist = ({ id }: PlaylistProps) => {
    const {
        loading,
        notFound,
        playlist,
        editOpen,
        onEditOpen,
        onEditClose,
        onEditSubmit,
        editError,
        editSaving,
        deleting,
        deleteOpen,
        onDeleteOpen,
        onDeleteClose,
        onDeleteSubmit,
        deleteError,
    } = usePlaylistLogic({ id });

    if (notFound) {
        return <PageContainer>
            <Text {...{
                id: "not-found",
                "data-testid": "not-found",
            }}>
                Not Found
            </Text>
        </PageContainer>;
    }

    return (
        <PageContainer>
            <div className="header">
                <div className="playlist">
                    <div className="playlist-image" id="playlist-image">
                        {loading ?
                            skeleton
                            : (
                                <img
                                    width={240}
                                    height={240}
                                    src={playlist?.image?.url}
                                    alt={playlist?.name}
                                />
                            )}
                    </div>
                    <div className="playlist-details">
                        <Title className={`playlist-title${loading ? "-loading" : ""}`}>
                            {!!playlist
                                ?
                                <>
                                    {playlist?.name}
                                    <a className="icon-link"
                                        href={playlist?.href}
                                        target="_blank"
                                        rel="noreferrer">
                                        <VscLinkExternal style={{ cursor: "pointer", fontSize: 28 }} />
                                    </a>
                                </>
                                :
                                skeleton
                            }
                        </Title>
                        <div
                            className={`playlist-details-item${loading ? "-loading" : ""}`}
                        >
                            {playlist?.skipThreshold ? (
                                <>
                                    <SecondaryText>Skip Threshold:&nbsp;</SecondaryText>
                                    <Text>{playlist?.skipThreshold} s</Text>
                                </>
                            ) : skeleton}
                        </div>

                        <div
                            className={`playlist-details-item${loading ? "-loading" : ""}`}
                        >
                            {playlist?.skipThreshold ? (
                                <>
                                    <SecondaryText>Ignore initial skips:&nbsp;</SecondaryText>
                                    <Text>{playlist?.ignoreInitialSkips ? "True" : "False"}</Text>
                                </>
                            ) : skeleton}
                        </div>

                        <div
                            className={`playlist-details-item${loading ? "-loading" : ""}`}
                        >
                            {playlist ? (
                                <>
                                    <SecondaryText>Auto-delete after:&nbsp;</SecondaryText>
                                    {playlist?.autoCleanupLimit ? (
                                        <>
                                            <Text>{playlist?.autoCleanupLimit}&nbsp;</Text>
                                            <SecondaryText>skips</SecondaryText>
                                        </>
                                    ) : (
                                        <Text>No auto-delete</Text>
                                    )}
                                </>
                            ) : skeleton}
                        </div>
                    </div>
                </div>
                <div className="playlist-menu">
                    <Menu
                        menuButton={
                            <div>
                                <VscEllipsis style={{ cursor: "pointer", fontSize: 28 }} />
                            </div>
                        }
                    >
                        <MenuItem {...{ onClick: onEditOpen }}>
                            <VscEdit style={{ cursor: "pointer", marginRight: 8 }} />
                            Edit
                        </MenuItem>
                        <MenuItem {...{ onClick: onDeleteOpen }}>
                            <VscTrash style={{ cursor: "pointer", marginRight: 8 }} />
                            Delete
                        </MenuItem>
                    </Menu>
                </div>
            </div>

            <div className="content">
                <PlaylistTabs {...{ id, loading }} />
            </div>

            <Modal
                {...{
                    isOpen: editOpen,
                    onClose: onEditClose,
                    label: "Edit Playlist Modal",
                }}
            >
                <EditPlaylistModalView
                    {...{
                        onSubmit: onEditSubmit,
                        playlist,
                        modalSaving: editSaving,
                        modalError: editError,
                        onModalClose: onEditClose,
                    }}
                />
            </Modal>
            <Modal
                {...{
                    isOpen: deleteOpen,
                    onClose: onDeleteClose,
                    label: "Delete Playlist Modal",
                }}
            >
                <DeletePlaylistModalView
                    {...{
                        onSubmit: onDeleteSubmit,
                        playlist,
                        modalSaving: deleting,
                        modalError: deleteError,
                        onModalClose: onDeleteClose,
                    }}
                />
            </Modal>
        </PageContainer>
    );
};

const PageContainer = styled.div`
  display: flex;
  flex-direction: column;
  color: ${WHITE};
  align-items: center;

  width: 100%;
  max-width: 1825px;
  padding-top: 16px;

  .header {
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    width: 100%;

    margin-bottom: 24px;
  }

  .playlist {
    display: flex;
    flex-direction: row;
  }

  .playlist-image {
    width: 240px;
    height: 240px;
  }

  .playlist-details {
    padding-left: 32px;

    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;

    width: 400px;
  }

  .playlist-title {
    margin-bottom: 24px;
  }

  .playlist-title-loading {
    height: 42px;
    width: 360px;
    margin-bottom: 24px;
  }

  .playlist-details-item {
    margin-bottom: 12px;

    display: flex;
  }

  .playlist-details-item-loading {
    height: 22px;
    width: 180px;
    margin-bottom: 12px;
  }

  .content {
    width: 100%;
  }

  .icon-link{
    color: ${WHITE};
    margin-left: 12px;

    svg {
        margin-bottom: -3px;
    }
  }
`;

Playlist.displayName = "Playlist";
