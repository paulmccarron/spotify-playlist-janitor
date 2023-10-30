import { AxiosRequestConfig } from "axios";
import { get } from "api/api";
import { SpotifyPlaylistResponse } from "./spotify-api-types";

export const getSpotifyPlaylists = (config: AxiosRequestConfig) =>
  get<SpotifyPlaylistResponse[]>("/spotify/playlists", config);

export const getSpotifyPlaylist = (id: string, config: AxiosRequestConfig) =>
  get<SpotifyPlaylistResponse>(`/spotify/playlists/${id}`, config);
