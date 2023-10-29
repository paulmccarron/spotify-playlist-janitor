import styled from "styled-components";
import { WHITE } from "shared/constants";

type PlaylistProps = {
    id?: string;
}

export const Playlist = ({ id }: PlaylistProps) => {
    return <PageContainer>{id}</PageContainer>
}

const PageContainer = styled.div`
  display: flex;
  flex-direction: column;
  color: ${WHITE};
  justify-content: center;
  align-items: center;
`;

Playlist.displayName = "Playlist";