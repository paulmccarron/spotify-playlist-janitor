import axios, { AxiosRequestConfig } from 'axios';
import { axiosInstance, get, post, put, deleteRequest } from '../api';

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
  let axiosPostSpy: jest.SpyInstance;

  beforeAll(() => {
    axiosPostSpy = jest.spyOn(axiosInstance, 'post');
  });

  afterAll(() => {
    axiosPostSpy.mockRestore();
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
    expect(axiosPostSpy).toHaveBeenNthCalledWith(1, URL, BODY, OPTIONS);
  });

  it('should call the axios post method with empty config object when none is passed', () => {
    const URL = 'URL';

    const BODY = {
      someData: 'some data',
    };

    post(URL, BODY);
    expect(axiosPostSpy).toHaveBeenNthCalledWith(2, URL, BODY, {});
  });
});

describe('put', () => {
  let axiosPutSpy: jest.SpyInstance;

  beforeAll(() => {
    axiosPutSpy = jest.spyOn(axiosInstance, 'put');
  });

  afterAll(() => {
    axiosPutSpy.mockRestore();
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
    expect(axiosPutSpy).toHaveBeenNthCalledWith(1, URL, BODY, OPTIONS);
  });

  it('should call the axios put method with empty config object when none is passed', () => {
    const URL = 'URL';

    const BODY = {
      someData: 'some data',
    };

    put(URL, BODY);
    expect(axiosPutSpy).toHaveBeenNthCalledWith(2, URL, BODY, {});
  });
});

describe('deleteRequest', () => {
  let axiosDeleteSpy: jest.SpyInstance;

  beforeEach(() => {
    axios.create.mockReturnThis();
    axiosDeleteSpy = jest.spyOn(axiosInstance, 'delete');
  });

  afterEach(() => {
    axiosDeleteSpy.mockRestore();
  });

  it('should call the axios delete method', () => {
    const URL = 'URL';
    const OPTIONS: AxiosRequestConfig = {
      headers: {
        someHeader: 'some header',
      },
    };

    deleteRequest(URL, OPTIONS);
    expect(axiosDeleteSpy).toHaveBeenNthCalledWith(1, URL, OPTIONS);
  });

  it('should call the axios get method with empty config object when none is passed', () => {
    const URL = 'URL';

    deleteRequest(URL);
    expect(axiosDeleteSpy).toHaveBeenNthCalledWith(1, URL, {});
  });
});