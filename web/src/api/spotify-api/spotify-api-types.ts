export type SpotifyImage = {
  height: number;
  width: number;
  url: string;
};

export type SpotifyPlaylistResponse = {
  id: string;
  name: string;
  href: string;
  images: SpotifyImage[];
};
