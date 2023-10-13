import { render } from "@testing-library/react";
import { tableColumns, tableData } from "shared/mock-data/table-data";

import { TableBody } from "../table-body";

describe("<TableBody />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      columns: tableColumns,
      tableData,
    };

    ({ container } = render(<TableBody {...props} />));
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it("should render TableBody component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
