import { UserToken } from 'shared/types';

export const VERSION = '1.0.0';

export type Store = {
  SPOTIFY_PLAYLIST_JANITOR_STORE: {
    USER_TOKEN?: UserToken;
  };
};

export type StoreKey = keyof Store['SPOTIFY_PLAYLIST_JANITOR_STORE'];

function getData() {
  return JSON.parse(window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_STORE') ?? '{}') as Store['SPOTIFY_PLAYLIST_JANITOR_STORE'];
}

function setData(data: any) {
  window.sessionStorage.setItem('SPOTIFY_PLAYLIST_JANITOR_STORE', JSON.stringify(data));
}

export function getItem(key: 'USER_TOKEN'): Store['SPOTIFY_PLAYLIST_JANITOR_STORE']['USER_TOKEN'];
export function getItem(key: StoreKey) {
  try {
    const currentData = getData();
    return currentData[key];
  } catch (error) {
    console.warn(error);
  }
}

export function setItem(key: StoreKey, value: any) {
  try {
    const currentData = getData();
    currentData[key] = value;
    setData(currentData);
  } catch (error) {
    console.warn(error);
  }
}

export function removeItem(key: StoreKey) {
  try {
    const currentData = getData();
    delete currentData[key];
    setData(currentData);
  } catch (error) {
    console.warn(error);
  }
}

export function clearAll() {
  try {
    window.sessionStorage.setItem('SPOTIFY_PLAYLIST_JANITOR_STORE', JSON.stringify({}));
  } catch (error) {
    console.warn(error);
  }
}

export function flushSessionStorage() {
  const savedVersion = window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_VERSION');

  if (savedVersion !== VERSION) {
    window.sessionStorage.setItem('SPOTIFY_PLAYLIST_JANITOR_VERSION', VERSION);
    window.sessionStorage.removeItem('SPOTIFY_PLAYLIST_JANITOR_STORE');
    window.sessionStorage.setItem('SPOTIFY_PLAYLIST_JANITOR_STORE', JSON.stringify({}));
  } else if (window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_STORE') === null) {
    window.sessionStorage.setItem('SPOTIFY_PLAYLIST_JANITOR_STORE', JSON.stringify({}));
  }
}
