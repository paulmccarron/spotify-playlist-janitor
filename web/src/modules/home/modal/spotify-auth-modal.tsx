import styled from "styled-components";
import { SubTitle, Text } from "shared/components/typography";

export const SpotifyAuthModalView = () => {
  return (
    <ModalContainer>
      <SubTitle style={{ marginBottom: 8 }}>
        Spotify Authentication Error
      </SubTitle>
      <Text>
        The application has not been authenticated with your Spotify account.
        Please follow{" "}
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
  );
};

const ModalContainer = styled.div`
  width: 360px;
`;

SpotifyAuthModalView.displayName = "SpotifyAuthModalView";
