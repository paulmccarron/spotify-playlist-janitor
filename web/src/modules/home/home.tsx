import styled from "styled-components";
import { useHomeLogic } from "./use-home-logic";
import { GREEN } from "shared/constants";

export const Home = () => {
  const { monitoredPlaylists, unmonitoredPlaylists, loading } = useHomeLogic();
  return (
    <PageContainer>
      {monitoredPlaylists.map((monitoredPlaylist) => (
        <div className="item">
          {monitoredPlaylist.image && (
            <img
              alt={monitoredPlaylist.name}
              width={monitoredPlaylist.image.width}
              height={monitoredPlaylist.image.height}
              src={monitoredPlaylist.image.url}
            />
          )}

          <div>{monitoredPlaylist.name}</div>
        </div>
      ))}
      <div className="item"></div>
    </PageContainer>
  );
};

const PageContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  justify-content: center;
  align-content: flex-start;

  color: white;
  width: 100%;
  max-width: 1825px;

  .item {
    flex: 1 1 30%; /*grow | shrink | basis */
    background-color: ${GREEN};
    margin: 8px;
    height: 160px;
  }
`;

Home.displayName = "Home";
