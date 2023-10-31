export type Image = {
  height: number;
  width: number;
  url: string;
};

export type Playlist = {
  id: string;
  name: string;
  href: string;
  image?: Image;
};
