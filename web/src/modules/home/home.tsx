import styled from "styled-components";
import { useHomeLogic } from "./use-home-logic";
import { BLACK, GREEN } from "shared/constants";
import { SubTitle } from "shared/components/typography";

export const Home = () => {
  const { monitoredPlaylists, unmonitoredPlaylists, loading } = useHomeLogic();
  return (
    <PageContainer>
      {monitoredPlaylists.map((monitoredPlaylist) => (
        <div className="item" onClick={() => alert(`Naviagte to ${monitoredPlaylist.name} at route ${monitoredPlaylist.id}`)}>
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
      <div className="item new" onClick={() => alert(`Open modal`)}></div>
    </PageContainer>
  );
};

const PageContainer = styled.div`
  display: flex;
  flex-wrap: wrap;
  align-items: center;
//   justify-content: center;
  align-content: flex-start;

  color: white;
  width: 100%;
  max-width: 1825px;

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
`;

Home.displayName = "Home";
