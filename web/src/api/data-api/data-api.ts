import { AxiosRequestConfig } from "axios";
import { get, post } from "api/api";
import {
  AddDatabasePlaylistRequest,
  DatabasePlaylistResponse,
} from "./data-api-types";

export const getDatabasePlaylists = (config: AxiosRequestConfig) =>
  get<DatabasePlaylistResponse[]>("/data/playlists", config);

export const addDatabasePlaylist = (
  body: AddDatabasePlaylistRequest,
  config: AxiosRequestConfig
) => post("/data/playlists", body, config);
