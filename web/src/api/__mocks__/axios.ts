export default {
  get: jest.fn(() => Promise.resolve({ data: {} })),
  post: jest.fn(() => Promise.resolve({ data: {} })),
  put: jest.fn(() => Promise.resolve({ data: {} })),
  delete: jest.fn(() => Promise.resolve({ data: {} })),

  create: jest.fn(() => ({
    get: jest.fn(() => Promise.resolve({ data: {} })),
    post: jest.fn(() => Promise.resolve({ data: {} })),
    put: jest.fn(() => Promise.resolve({ data: {} })),
    delete: jest.fn(() => Promise.resolve({ data: {} })),
  })),

  CancelToken: {
    source: () => ({
      cancel: jest.fn(),
    }),
  },

  isCancel: jest.fn(error => error instanceof MockIsCancel),

  isAxiosError: jest.fn(() => true),
};

export class MockIsCancel {}
