import { UserToken } from 'shared/types';
import { userState as mockUserState } from 'shared/mock-data/state';
import { VERSION, getItem, setItem, removeItem, clearAll, flushSessionStorage } from '../session-storage-service';

describe('Storage Service', () => {
  const spyConsoleWarn = jest.spyOn(window.console, 'warn').mockImplementation(() => {});

  const USER = 'USER_TOKEN';
  const user: UserToken = {...mockUserState};

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('should set the current version of the data and create the initial value for NGA store', () => {
    flushSessionStorage();
    expect(window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_VERSION')).toBe(VERSION);
    expect(window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_STORE')).toBe('{}');
  });

  it('should create the initial value for Spotify Playlist Janiotr store if none is found', () => {
    window.sessionStorage.removeItem('SPOTIFY_PLAYLIST_JANITOR_STORE');
    flushSessionStorage();
    expect(window.sessionStorage.getItem('SPOTIFY_PLAYLIST_JANITOR_STORE')).toBe('{}');
  });

  it('should set and get value when the setItem and getItem functions are called', () => {
    setItem(USER, user);
    expect(getItem(USER)).toStrictEqual(user);
  });

  it('should handle an error when the setItem function is called', () => {
    const ERROR = new Error('ERROR');
    const spyOnSetItem = jest.spyOn(Storage.prototype, 'setItem').mockImplementation(() => {
      throw ERROR;
    });
    setItem(USER, user);

    expect(spyConsoleWarn).toBeCalledWith(ERROR);
    spyOnSetItem.mockRestore();
  });

  it('should handle an error when the getItem function is called', () => {
    const ERROR = new Error('ERROR');
    const spyOnGetItem = jest.spyOn(Storage.prototype, 'getItem').mockImplementation(() => {
      throw ERROR;
    });
    getItem(USER);

    expect(spyConsoleWarn).toBeCalledWith(ERROR);
    spyOnGetItem.mockRestore();
  });

  it('should not retrieve a value when the getItem function is called after removeItem has been called', () => {
    setItem(USER, user);
    removeItem(USER);
    expect(getItem(USER)).not.toBe(user);
  });

  it('should handle an error when the removeItem function is called', () => {
    const ERROR = new Error('ERROR');
    const spyOnRemoveItem = jest.spyOn(Storage.prototype, 'getItem').mockImplementation(() => {
      throw ERROR;
    });
    removeItem(USER);

    expect(spyConsoleWarn).toBeCalledWith(ERROR);
    spyOnRemoveItem.mockRestore();
  });

  it('should clear all properties from the NGA store when the clearAll function is called', () => {
    setItem(USER, user);
    clearAll();
    expect(getItem(USER)).toBeUndefined();
  });

  it('should handle an error when the clearAll function is called', () => {
    const ERROR = new Error('ERROR');
    const spyOnRemoveItem = jest.spyOn(Storage.prototype, 'setItem').mockImplementation(() => {
      throw ERROR;
    });
    clearAll();

    expect(spyConsoleWarn).toBeCalledWith(ERROR);
    spyOnRemoveItem.mockRestore();
  });
});
