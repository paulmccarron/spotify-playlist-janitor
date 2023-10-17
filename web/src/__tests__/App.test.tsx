import React from "react";
import { render } from "@testing-library/react";
import App from "../App";

jest.mock("react-modal");

describe("<App />", () => {

  beforeAll(() => {
    process.env.LOCAL_ENVIRONMENT = 'https://localhost:5001';
  });
  
  it("should render the App", async () => {
    const { container } = render(<App />);
    expect(container.firstChild).toMatchSnapshot();
  });
});
