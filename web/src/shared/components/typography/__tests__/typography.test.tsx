import { render } from "@testing-library/react";

import { Title, SubTitle, Text, SecondaryText, SubText } from "..";

describe("<Title />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      id: "title-test",
      "data-testid": "title-test-data-testid",
    };
    ({ container } = render(<Title {...props}>Title Text</Title>));
  });

  it("should render Title component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});

describe("<SubTitle />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      id: "subtitle-test",
      "data-testid": "subtitle-test-data-testid",
    };
    ({ container } = render(<SubTitle {...props}>Subtitle Text</SubTitle>));
  });

  it("should render SubTitle component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});

describe("<Text />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      id: "text-test",
      "data-testid": "text-test-data-testid",
    };
    ({ container } = render(<Text {...props}>Text</Text>));
  });

  it("should render Text component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});

describe("<SecondaryText />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      id: "secondary-text-test",
      "data-testid": "secondary-text-test-data-testid",
    };
    ({ container } = render(<SecondaryText {...props}>Secondary Text</SecondaryText>));
  });

  it("should render SecondaryText component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});

describe("<SubText />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      id: "sub-text-test",
      "data-testid": "sub-text-test-data-testid",
    };
    ({ container } = render(<SubText {...props}>Sub Text</SubText>));
  });

  it("should render SubText component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});