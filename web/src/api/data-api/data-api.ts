import { AxiosRequestConfig } from "axios";
import { get } from "api/api";
import { DatabasePlaylistResponse  } from "./data-api-types";

export const getDatabasePlaylists = (config: AxiosRequestConfig) =>
get<DatabasePlaylistResponse[]>("/data/playlists", config);
