import axios, { AxiosRequestConfig } from 'axios';
import { axiosInstance, get, post, put } from '../api';

describe('get', () => {
  let axiosGetSpy: jest.SpyInstance;

  beforeEach(() => {
    axios.create.mockReturnThis();
    axiosGetSpy = jest.spyOn(axiosInstance, 'get');
  });

  afterEach(() => {
    axiosGetSpy.mockRestore();
  });

  it('should call the axios get method', () => {
    const URL = 'URL';
    const OPTIONS: AxiosRequestConfig = {
      headers: {
        someHeader: 'some header',
      },
    };

    get(URL, OPTIONS);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(1, URL, OPTIONS);
  });

  it('should call the axios get method with empty config object when none is passed', () => {
    const URL = 'URL';

    get(URL);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(1, URL, {});
  });
});

describe('post', () => {
  let axiosGetSpy: jest.SpyInstance;

  beforeAll(() => {
    axiosGetSpy = jest.spyOn(axiosInstance, 'post');
  });

  afterAll(() => {
    axiosGetSpy.mockRestore();
  });

  it('should call the axios post method', () => {
    const URL = 'URL';
    const OPTIONS: AxiosRequestConfig = {
      headers: {
        someHeader: 'some header',
      },
    };

    const BODY = {
      someData: 'some data',
    };

    post(URL, BODY, OPTIONS);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(1, URL, BODY, OPTIONS);
  });

  it('should call the axios post method with empty config object when none is passed', () => {
    const URL = 'URL';

    const BODY = {
      someData: 'some data',
    };

    post(URL, BODY);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(2, URL, BODY, {});
  });
});

describe('put', () => {
  let axiosGetSpy: jest.SpyInstance;

  beforeAll(() => {
    axiosGetSpy = jest.spyOn(axiosInstance, 'put');
  });

  afterAll(() => {
    axiosGetSpy.mockRestore();
  });

  it('should call the axios put method', () => {
    const URL = 'URL';
    const OPTIONS: AxiosRequestConfig = {
      headers: {
        someHeader: 'some header',
      },
    };

    const BODY = {
      someData: 'some data',
    };

    put(URL, BODY, OPTIONS);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(1, URL, BODY, OPTIONS);
  });

  it('should call the axios put method with empty config object when none is passed', () => {
    const URL = 'URL';

    const BODY = {
      someData: 'some data',
    };

    put(URL, BODY);
    expect(axiosGetSpy).toHaveBeenNthCalledWith(2, URL, BODY, {});
  });
});
