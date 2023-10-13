import { render } from "@testing-library/react";
import { tableData, tableColumns } from "shared/mock-data/table-data";

import { Table } from "../table";

describe("<Table />", () => {
  let container: HTMLElement;
  let props: any;

  beforeEach(() => {
    props = {
      caption: "Table caption.",
      data: tableData,
      columns: tableColumns,
    };
    ({ container } = render(<Table {...props} />));
  });

  it("should render Table component", () => {
    expect(container.firstChild).toMatchSnapshot();
  });
});
