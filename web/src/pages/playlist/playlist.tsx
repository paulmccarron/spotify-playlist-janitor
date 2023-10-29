import { useParams } from "react-router-dom";
import styled from "styled-components";
import { Playlist as PlaylistInitialise } from "modules/playlist";
import { HEADER_HEIGHT, HEADER_PADDING } from "shared/constants";

export const Playlist = () => {
  let { id } = useParams();
  return (
    <Content>
      <PlaylistInitialise {...{ id }} />
    </Content>
  );
};

const Content = styled.div`
  display: flex;
  justify-content: center;
  height: calc(
    100vh - ${HEADER_HEIGHT} - ${HEADER_PADDING} - ${HEADER_PADDING}
  );
`;

Playlist.displayName = "Playlist";
