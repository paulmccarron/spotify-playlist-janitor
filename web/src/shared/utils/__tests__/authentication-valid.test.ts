import { isAuthDataValid } from '../authentication-valid';

describe('isAuthDataValid', () => {
  const RealDate = Date.now;
  const token = 'token';

  beforeEach(() => {
    global.Date.now = jest.fn(() => new Date('2021-08-08T10:20:30Z').getTime());
  });

  afterEach(() => {
    jest.clearAllMocks();
    global.Date.now = RealDate;
  });

  it('should return true token is present and date is in the future', () => {
    const testDate = new Date('2021-08-08T12:20:30Z');
    const testDateime = Math.floor(new Date(testDate).getTime() / 1000);
    expect(isAuthDataValid(token, testDateime)).toBe(true);
  });

  it('should return false token is present and date is in the past', () => {
    const testDate = new Date('2021-08-08T08:20:30Z');
    const testDateime = Math.floor(new Date(testDate).getTime() / 1000);
    expect(isAuthDataValid(token, testDateime)).toBe(false);
  });

  it('should return false token is absent and date is in the future', () => {
    const testDate = new Date('2021-08-08T12:20:30Z');
    const testDateime = Math.floor(new Date(testDate).getTime() / 1000);
    expect(isAuthDataValid(undefined, testDateime)).toBe(false);
  });
});
