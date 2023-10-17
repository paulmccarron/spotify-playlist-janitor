import React, { RefObject } from "react";
import { act, renderHook } from "@testing-library/react";

import { useTableSorting } from "../../hooks/use-table-sorting";
import { tableColumns, tableData } from "shared/mock-data/table-data";
import { SortOrder } from "../../table-types";

describe("<useTableSorting/>", () => {
  let result: RefObject<ReturnType<typeof useTableSorting>>;

  beforeEach(() => {
    ({ result } = renderHook(() =>
      useTableSorting({ data: tableData, columns: tableColumns })
    ));
  });

  it("should ignore default if no column sortByOrder", () => {
    const noSortColumns = tableColumns.map((column) => ({
      ...column,
      sortbyOrder: undefined,
    }));
    ({ result } = renderHook(() =>
      useTableSorting({ data: tableData, columns: noSortColumns })
    ));

    const outTableData = result.current?.[0];

    expect(outTableData).toStrictEqual(tableData);
  });

  it("should default sort tableData by string column sortByOrder", () => {
    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(({ title: a }, { title: b }) =>
      b.localeCompare(a)
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });

  it("should default sort tableData by number column sortByOrder", () => {
    const numberSortColumns = tableColumns.map((column) => ({
      ...column,
      sortbyOrder:
        column.accessor === "length" ? ("asc" as SortOrder) : undefined,
    }));
    ({ result } = renderHook(() =>
      useTableSorting({ data: tableData, columns: numberSortColumns })
    ));
    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(({ length: a }, { length: b }) =>
      a.toString().localeCompare(b.toString())
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });

  it("should default sort tableData by Date column sortByOrder", () => {
    const dateSortColumns = tableColumns.map((column) => ({
      ...column,
      sortbyOrder:
        column.accessor === "date_skipped" ? ("asc" as SortOrder) : undefined,
    }));
    ({ result } = renderHook(() =>
      useTableSorting({ data: tableData, columns: dateSortColumns })
    ));
    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(
      ({ date_skipped: a }, { date_skipped: b }) => (a > b ? 1 : -1)
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });

  it("should sort tableData with handleSorting by string field", () => {
    act(() => {
      result.current?.[1]("artist", "asc");
    });

    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(({ artist: a }, { artist: b }) =>
      a.localeCompare(b)
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });

  it("should sort tableData with handleSorting by number field", () => {
    act(() => {
      result.current?.[1]("length", "asc");
    });

    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(({ length: a }, { length: b }) =>
      a.toString().localeCompare(b.toString())
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });

  it("should sort tableData with handleSorting by Date field", () => {
    act(() => {
      result.current?.[1]("date_skipped", "asc");
    });

    const outTableData = result.current?.[0];

    const expectedSort = tableData.sort(
      ({ date_skipped: a }, { date_skipped: b }) => (a > b ? 1 : -1)
    );

    expect(outTableData).toStrictEqual(expectedSort);
  });
});
