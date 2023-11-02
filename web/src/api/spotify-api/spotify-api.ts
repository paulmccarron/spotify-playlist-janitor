import { AxiosRequestConfig } from "axios";
import { deleteRequest, get } from "api/api";
import {
  SpotifyPlaylistResponse,
  SpotifyTrackResponse,
} from "./spotify-api-types";

export const getSpotifyPlaylists = (config: AxiosRequestConfig) =>
  get<SpotifyPlaylistResponse[]>("/spotify/playlists", config);

export const getSpotifyPlaylist = (id: string, config: AxiosRequestConfig) =>
  get<SpotifyPlaylistResponse>(`/spotify/playlists/${id}`, config);

export const getSpotifyPlaylistTracks = (
  id: string,
  config: AxiosRequestConfig
) => get<SpotifyTrackResponse>(`/spotify/playlists/${id}/tracks`, config);

export const deleteSpotifyPlaylistTracks = (
  id: string,
  trackIds: string[],
  config: AxiosRequestConfig
) =>
  deleteRequest(
    `/spotify/playlists/${id}/tracks${
      trackIds.length === 0
        ? ""
        : `?${trackIds.map((trackId) => `trackIds[]=${trackId}`).join("&")}`
    }`,
    config
  );
