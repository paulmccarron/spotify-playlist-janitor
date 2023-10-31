import { AxiosRequestConfig } from "axios";
import { get, post, put, deleteRequest } from "api/api";
import {
  AddDatabasePlaylistRequest,
  DatabasePlaylistResponse,
  UpdateDatabasePlaylistRequest,
} from "./data-api-types";

export const getDatabasePlaylists = (config: AxiosRequestConfig) =>
  get<DatabasePlaylistResponse[]>("/data/playlists", config);

export const getDatabasePlaylist = (id: string, config: AxiosRequestConfig) =>
  get<DatabasePlaylistResponse>(`/data/playlists/${id}`, config);

export const addDatabasePlaylist = (
  body: AddDatabasePlaylistRequest,
  config: AxiosRequestConfig
) => post("/data/playlists", body, config);

export const updateDatabasePlaylist = (
  id: string,
  body: UpdateDatabasePlaylistRequest,
  config: AxiosRequestConfig
) => put(`/data/playlists/${id}`, body, config);

export const deleteDatabasePlaylist = (
  id: string,
  config: AxiosRequestConfig
) => deleteRequest(`/data/playlists/${id}`, config);
