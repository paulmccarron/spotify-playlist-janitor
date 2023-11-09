type Image = {
  height: number;
  width: number;
  url: string;
};

export type SpotifyPlaylistResponse = {
  id: string;
  name: string;
  href: string;
  images: Image[];
};

export type DeleteSpotifyTracksParams = {
  trackIds: string[];
};

type SpotifyTrackArtist = {
  id: string;
  name: string;
  href: string;
}

type SpotifyTrackAlbum = {
  id: string;
  name: string;
  href: string;
  images: Image[];
}

type SpotifyTrack = {
  id: string;
  name: string;
  artists: SpotifyTrackArtist[];
  album: SpotifyTrackAlbum;
  duration: number;
}

export type SpotifyTrackResponse = SpotifyTrack[];
